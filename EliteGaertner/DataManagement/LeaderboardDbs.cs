using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

public class LeaderboardDbs : ILeaderBoardDbs
{
    private readonly EliteGaertnerDbContext _dbContext;
    private const int ReturnNumber = 5;

    public LeaderboardDbs(EliteGaertnerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<LeaderboardEntryDto> GetLeaderBoardEntries(int profileId, int? tagId, LeaderboardSearchGoal goal)
    {
        // Top-5 hängt NICHT vom eingeloggten User ab, daher hier keine ProfileId-Prüfung.
        if (!CheckParametersForTopList(tagId, goal))
            return Array.Empty<LeaderboardEntryDto>();

        return goal == LeaderboardSearchGoal.MostLikes
            ? GetMostLikes()
            : GetHarvestEntries(tagId!.Value, goal);
    }

    public LeaderboardEntryDto GetPersonalLeaderBoardEntry(int profileId, int? tagId, LeaderboardSearchGoal goal)
    {
        // Personal Entry hängt vom eingeloggten User ab -> ProfileId muss gültig sein.
        if (!CheckParametersForPersonal(profileId, tagId, goal))
            return new LeaderboardEntryDto();

        return goal == LeaderboardSearchGoal.MostLikes
            ? GetPersonalLikes(profileId)
            : GetPersonalHarvestEntry(profileId, tagId!.Value, goal);
    }

    public bool CheckParametersForTopList(int? tagId, LeaderboardSearchGoal goal)
    {
        //Überprüfe, ob goal gültig ist
        if (!Enum.IsDefined(typeof(LeaderboardSearchGoal), goal))
            return false;

        //TagId darf nur bei MostLikes null sein
        if (tagId is null)
            return goal == LeaderboardSearchGoal.MostLikes;

        //Überprüfe, ob tagId <= 0
        if (tagId <= 0)
            return false;

        //Tag muss existieren
        var tagExists = _dbContext.Tags
            .AsNoTracking()
            .Any(t => t.Tagid == tagId.Value);

        return tagExists;
    }

    public bool CheckParametersForPersonal(int profileId, int? tagId, LeaderboardSearchGoal goal)
    {
        //Erst die allgemeine Top-List-Validierung
        if (!CheckParametersForTopList(tagId, goal))
            return false;

        //ProfileId muss gültig sein
        if (profileId <= 0)
            return false;

        //Profil muss existieren
        var profileIdExists = _dbContext.Profiles
            .AsNoTracking()
            .Any(p => p.Profileid == profileId);

        return profileIdExists;
    }
    
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
            .Take(ReturnNumber)
            .ToList();
        
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
                // Tie-breaker: bei gleichem Wert gewinnt der alphabetisch höhere Username (Z → A)
                orderby b.Value descending, p.Username descending
                select new
                {
                    p.Profileid,
                    p.Username,
                    b.Value
                })
            .Take(ReturnNumber)
            .ToList();

        //Erstellt aus den vorherigen Queries 5 LeaderboardEntryDtos
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
    
    public LeaderboardEntryDto GetPersonalLikes(int profileId)
    {
        //Likes der Profile berechnen
        var likeCounts = _dbContext.Ratings
            .AsNoTracking()
            .Where(r => r.Profilerating)
            .GroupBy(r => r.Contentcreatorid)
            .Select(g => new
            {
                ProfileId = g.Key,
                Likes = (int?)g.Count() 
            });
        
        //Alle Profile + Like Anzahl (0 wenn keine Likes vorhanden)
        var all = (
                from p in _dbContext.Profiles.AsNoTracking()
                join lc in likeCounts on p.Profileid equals lc.ProfileId into lcj
                from lc in lcj.DefaultIfEmpty()
                select new
                {
                    p.Profileid,
                    p.Username,
                    Likes = lc.Likes ?? 0
                })
            .ToList();

        //Find das gesuchte Profil in der Tabelle
        var me = all.SingleOrDefault(p => p.Profileid == profileId);
        if (me == null)                       
            return new LeaderboardEntryDto(); 
        
        //Inhalt der Liste sortieren
        var ordered = all
            .OrderByDescending(x => x.Likes)
            .ThenBy(x => x.Username)
            .ToList();

        //Rang berechnen
        var rank = ordered.FindIndex(x => x.Profileid == profileId) + 1; 
        //Rang ungültig?
        if (rank <= 0)
            return new LeaderboardEntryDto();
        
        return new LeaderboardEntryDto
        {
            Rank = rank,
            ProfileId = me.Profileid,
            UserName = me.Username,
            Value = me.Likes
        };
    }

    public LeaderboardEntryDto GetPersonalHarvestEntry(int profileId, int tagId, LeaderboardSearchGoal goal)
    {
        // Prüfen, ob Profil überhaupt einen Upload mit diesem Tag hat
        var uploadsWithTagForMe = _dbContext.Harvestuploads
            .AsNoTracking()
            .Where(h => h.Profileid == profileId && h.Tags.Any(t => t.Tagid == tagId));

        // Falls der User gar keinen Upload mit dem Tag hat -> leeres DTO zurück
        if (!uploadsWithTagForMe.Any())
            return new LeaderboardEntryDto();

        //Persönlichen Bestwert je nach Goal bestimmen
        float myValue = goal switch
        {
            LeaderboardSearchGoal.Heaviest => (float)uploadsWithTagForMe.Max(h => h.Weightgramm),
            LeaderboardSearchGoal.Longest  => (float)uploadsWithTagForMe.Max(h => h.Lengthcm),
            LeaderboardSearchGoal.Widest   => (float)uploadsWithTagForMe.Max(h => h.Widthcm),
            _ => throw new ArgumentException("Ungültiges Goal für Personal Harvest-Leaderboard.", nameof(goal))
        };

        //Username laden
        var myUserName = _dbContext.Profiles
            .AsNoTracking()
            .Where(p => p.Profileid == profileId)
            .Select(p => p.Username)
            .SingleOrDefault();

        if (myUserName == null)
            return new LeaderboardEntryDto();

        //Alle Creator, die den Tag haben, mit ihrem jeweiligen Bestwert
        var uploadsWithTag = _dbContext.Harvestuploads
            .AsNoTracking()
            .Where(h => h.Tags.Any(t => t.Tagid == tagId));
        
        //"Bestes" Upload pro User
        var bestPerCreator = goal switch
        {
            LeaderboardSearchGoal.Heaviest => uploadsWithTag
                .GroupBy(h => h.Profileid)
                .Select(g => new { ProfileId = g.Key, Value = (float)g.Max(h => h.Weightgramm) }),

            LeaderboardSearchGoal.Longest => uploadsWithTag
                .GroupBy(h => h.Profileid)
                .Select(g => new { ProfileId = g.Key, Value = (float)g.Max(h => h.Lengthcm) }),

            LeaderboardSearchGoal.Widest => uploadsWithTag
                .GroupBy(h => h.Profileid)
                .Select(g => new { ProfileId = g.Key, Value = (float)g.Max(h => h.Widthcm) }),

            _ => throw new ArgumentException("Ungültiges Goal für Harvest-Leaderboard.", nameof(goal))
        };

        //Mit Profil-Tabelle joinen -> für den Usernamen
        var ordered = (
                from b in bestPerCreator
                join p in _dbContext.Profiles.AsNoTracking()
                    on b.ProfileId equals p.Profileid
                // Tie-breaker: bei gleichem Wert gewinnt der alphabetisch höhere Username (Z → A)
                orderby b.Value descending, p.Username descending
                select new { p.Profileid, p.Username, b.Value })
            .ToList();

        //Rang berechnen 
        var rank = ordered.FindIndex(x => x.Profileid == profileId) + 1;
        //Rang ungültig?
        if (rank <= 0)
            return new LeaderboardEntryDto();

        return new LeaderboardEntryDto
        {
            Rank = rank,
            ProfileId = profileId,
            UserName = myUserName,
            Value = myValue
        };
    }
}