using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

public class LeaderboardDbs : ILeaderBoardDbs
{
    private readonly EliteGaertnerDbContext _dbContext;

    public LeaderboardDbs(EliteGaertnerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<LeaderboardEntryDto> GetLeaderBoardEntries(int? tagId, LeaderboardSearchGoal goal)
    {
        //Überprüfe, ob goal gültig ist    
        if (!Enum.IsDefined(typeof(LeaderboardSearchGoal), goal))
            return Array.Empty<LeaderboardEntryDto>();

        if (tagId is null)
        {
            //Exception bei ungültiger Parameterübergabe
            if (goal != LeaderboardSearchGoal.MostLikes)
                throw new ArgumentException(
                    "TagId darf nur null sein, wenn man nach MostLikes filtert!",
                    nameof(tagId));
        }
        else
        {
            //Überprüfe, ob tagId >= 1 ist
            if (tagId <= 0)
                return Array.Empty<LeaderboardEntryDto>();
            
            //Datenbankabfrage, ob TagId existiert
            var tagExists = _dbContext.Tags
                .AsNoTracking()
                .Any(t => t.Tagid == tagId.Value);

            //Überprüfe, ob tagId in der Datenbank vorkommt. Falls nicht -> Gib leere Liste zurück 
            if (!tagExists) 
                return Array.Empty<LeaderboardEntryDto>();
        }

        return goal == LeaderboardSearchGoal.MostLikes
            ? GetMostLikes()
            : GetHarvestEntries(tagId!.Value, goal);
    }

    //TODO Kommentare fehlen
    public IEnumerable<LeaderboardEntryDto> GetMostLikes()
    {
        //Greif auf die Ratings-Tabelle zu
        var likeCounts = _dbContext.Ratings
            //EF muss nichts speichern, weil nur gelesen wird 
            .AsNoTracking()
            //Zeig mir alle Einträge mit positivem UserRating
            .Where(r => r.Profilerating)
            //Gruppiere diese nach den Usern, die die Likes erhalten haben
            .GroupBy(r => r.Contentcreatorid)
            //Speicher pro Gruppe, ProfileId und die Anzahl der Likes ab
            .Select(g => new
            {
                ProfileId = g.Key,
                Likes = g.Count()
            });

        //TODO Kommentare
        var result = (
                //FÜr jede Gruppe aus ProfilId und Anzahl der Likes..
                from lc in likeCounts
                //Führe ein Join mit der Profiles-Tabelle aus
                join p in _dbContext.Profiles.AsNoTracking()
                    on lc.ProfileId equals p.Profileid
                //Sortiere die Einträge nach Likes und Username     
                orderby lc.Likes descending, p.Username
                select new
                {
                    p.Profileid,
                    p.Username,
                    lc.Likes
                })
            .Take(10)
            .ToList();
        
        //TODO KOMMENTARE
        return result
            .Select((x, index) => new LeaderboardEntryDto()
            {
                Rank = index + 1,
                ProfileId = x.Profileid,
                UserName = x.Username,
                Value = x.Likes,
            })
            .ToList();
    }

    public IEnumerable<LeaderboardEntryDto> GetHarvestEntries(int tagId, LeaderboardSearchGoal goal)
    {

        //Zeigt mir alle Uploads mit dem Tag
        var uploadsWithTag = _dbContext.Harvestuploads
            .AsNoTracking()
            .Where(h => h.Tags.Any(t => t.Tagid == tagId));
        
        var bestPerCreator = goal switch
        {
            //Gruppiert nach User und sortiert nach der schwersten Ernte
            LeaderboardSearchGoal.Heaviest => uploadsWithTag
                .GroupBy(h => h.Profileid)
                .Select(g => new
                {
                    ProfileId = g.Key,
                    Value = (float)g.Max(h => h.Weightgramm)
                }),

            //Gruppiert nach User und sortiert nach der längsten Ernte
            LeaderboardSearchGoal.Longest => uploadsWithTag
                .GroupBy(h => h.Profileid)
                .Select(g => new
                {
                    ProfileId = g.Key,
                    Value = (float)g.Max(h => h.Lengthcm)
                }),

            //Gruppiert nach User und sortiert nach der breitesten Ernte
            LeaderboardSearchGoal.Widest => uploadsWithTag
                .GroupBy(h => h.Profileid)
                .Select(g => new
                {
                    ProfileId = g.Key,
                    Value = (float)g.Max(h => h.Widthcm)
                }),

            _ => throw new ArgumentException("Ungültiges Goal für Harvest-Leaderboard.", nameof(goal))
        };
        
        //Join mit Profile Tabelle
        var result = (
                from b in bestPerCreator
                join p in _dbContext.Profiles.AsNoTracking()
                    //Item1 = ProfileId, Item2 = Value
                    on b.ProfileId equals p.Profileid
                orderby b.Value descending, p.Username
                select new
                {
                    p.Profileid,
                    p.Username,
                    b.Value
                })
            .Take(10)
            .ToList();

        //Erstellt aus den vorherigen Queries 10 LeaderboardEntryDtos
        return result
            .Select((x, index) => new LeaderboardEntryDto()
            {
                Rank = index + 1,
                ProfileId = x.Profileid,
                UserName = x.Username,
                Value = x.Value
            })
            .ToList();
    }
}