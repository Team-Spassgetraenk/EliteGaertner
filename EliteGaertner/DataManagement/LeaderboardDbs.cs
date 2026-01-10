using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataManagement;

public class LeaderboardDbs : ILeaderBoardDbs
{
    private readonly EliteGaertnerDbContext _dbContext;
    private readonly ILogger<LeaderboardDbs> _logger;
    private const int ReturnNumber = 5;

    public LeaderboardDbs(EliteGaertnerDbContext dbContext, ILogger<LeaderboardDbs> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _logger.LogInformation("LeaderboardDbs created.");
    }

    public IEnumerable<LeaderboardEntryDto> GetLeaderBoardEntries(int profileId, int? tagId, LeaderboardSearchGoal goal)
    {
        _logger.LogInformation("GetLeaderBoardEntries called. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
        
        //Top-5 hängt nicht vom eingeloggten User ab, daher hier keine ProfileId-Prüfung.
        if (!CheckParametersForTopList(tagId, goal))
        {
            _logger.LogWarning("GetLeaderBoardEntries: invalid parameters. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
            return Array.Empty<LeaderboardEntryDto>();
        }

        try
        {
            var result = goal == LeaderboardSearchGoal.MostLikes
                ? GetMostLikes()
                : GetHarvestEntries(tagId!.Value, goal);

            var count = result is ICollection<LeaderboardEntryDto> c ? c.Count : result.Count();
            
            _logger.LogInformation("GetLeaderBoardEntries finished. returnedCount={Count}", count);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetLeaderBoardEntries failed. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
            throw;
        }
    }

    public LeaderboardEntryDto GetPersonalLeaderBoardEntry(int profileId, int? tagId, LeaderboardSearchGoal goal)
    {
        _logger.LogInformation("GetPersonalLeaderBoardEntry called. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
        
        // Personal Entry hängt vom eingeloggten User ab -> ProfileId muss gültig sein.
        if (!CheckParametersForPersonal(profileId, tagId, goal))
        {
            _logger.LogWarning("GetPersonalLeaderBoardEntry: invalid parameters. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
            return new LeaderboardEntryDto();
        }

        try
        {
            var result = goal == LeaderboardSearchGoal.MostLikes
                ? GetPersonalLikes(profileId)
                : GetPersonalHarvestEntry(profileId, tagId!.Value, goal);

            _logger.LogInformation("GetPersonalLeaderBoardEntry finished. rank={Rank}, value={Value}", result.Rank, result.Value);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPersonalLeaderBoardEntry failed. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
            throw;
        }
    }

    public bool CheckParametersForTopList(int? tagId, LeaderboardSearchGoal goal)
    {
        _logger.LogDebug("CheckParametersForTopList called. tagId={TagId}, goal={Goal}", tagId, goal);
        
        //Überprüfe, ob goal gültig ist
        if (!Enum.IsDefined(typeof(LeaderboardSearchGoal), goal))
        {
            _logger.LogDebug("CheckParametersForTopList: goal invalid. goal={Goal}", goal);
            return false;
        }

        //TagId darf nur bei MostLikes null sein
        if (tagId is null)
            return goal == LeaderboardSearchGoal.MostLikes;

        //Überprüfe, ob tagId <= 0
        if (tagId <= 0)
        {
            _logger.LogDebug("CheckParametersForTopList: tagId invalid. tagId={TagId}", tagId);
            return false;
        }

        //Tag muss existieren
        var tagExists = _dbContext.Tags
            .AsNoTracking()
            .Any(t => t.Tagid == tagId.Value);

        _logger.LogDebug("CheckParametersForTopList finished. tagId={TagId}, goal={Goal}, tagExists={TagExists}", tagId, goal, tagExists);
        return tagExists;
    }

    public bool CheckParametersForPersonal(int profileId, int? tagId, LeaderboardSearchGoal goal)
    {
        _logger.LogDebug("CheckParametersForPersonal called. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
        
        //Erst die allgemeine Top-List-Validierung
        if (!CheckParametersForTopList(tagId, goal))
            return false;

        //ProfileId muss gültig sein
        if (profileId <= 0)
        {
            _logger.LogDebug("CheckParametersForPersonal: profileId invalid. profileId={ProfileId}", profileId);
            return false;
        }

        //Profil muss existieren
        var profileIdExists = _dbContext.Profiles
            .AsNoTracking()
            .Any(p => p.Profileid == profileId);

        _logger.LogDebug("CheckParametersForPersonal finished. profileId={ProfileId}, tagId={TagId}, goal={Goal}, profileExists={ProfileExists}", profileId, tagId, goal, profileIdExists);
        return profileIdExists;
    }
    
    public IEnumerable<LeaderboardEntryDto> GetMostLikes()
    {
        _logger.LogDebug("GetMostLikes called. ReturnNumber={ReturnNumber}", ReturnNumber);
        
        try
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
            
            _logger.LogInformation("GetMostLikes finished. returnedCount={Count}", result.Count);
            
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetMostLikes failed.");
            throw;
        }
    }

    public IEnumerable<LeaderboardEntryDto> GetHarvestEntries(int tagId, LeaderboardSearchGoal goal)
    {
        _logger.LogDebug("GetHarvestEntries called. tagId={TagId}, goal={Goal}, ReturnNumber={ReturnNumber}", tagId, goal, ReturnNumber);
        
        try
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

            _logger.LogInformation("GetHarvestEntries finished. returnedCount={Count}", result.Count);
            
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetHarvestEntries failed. tagId={TagId}, goal={Goal}", tagId, goal);
            throw;
        }
    }
    
    public LeaderboardEntryDto GetPersonalLikes(int profileId)
    {
        _logger.LogDebug("GetPersonalLikes called. profileId={ProfileId}", profileId);
        
        try
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
            {
                _logger.LogWarning("GetPersonalLikes: profile not found in computed list. profileId={ProfileId}", profileId);
                return new LeaderboardEntryDto(); 
            }
            
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
            
            _logger.LogInformation("GetPersonalLikes finished. profileId={ProfileId}, rank={Rank}, likes={Likes}", profileId, rank, me.Likes);
            
            return new LeaderboardEntryDto
            {
                Rank = rank,
                ProfileId = me.Profileid,
                UserName = me.Username,
                Value = me.Likes
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPersonalLikes failed. profileId={ProfileId}", profileId);
            throw;
        }
    }

    public LeaderboardEntryDto GetPersonalHarvestEntry(int profileId, int tagId, LeaderboardSearchGoal goal)
    {
        _logger.LogDebug("GetPersonalHarvestEntry called. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
        
        try
        {
            // Prüfen, ob Profil überhaupt einen Upload mit diesem Tag hat
            var uploadsWithTagForMe = _dbContext.Harvestuploads
                .AsNoTracking()
                .Where(h => h.Profileid == profileId && h.Tags.Any(t => t.Tagid == tagId));

            // Falls der User gar keinen Upload mit dem Tag hat -> leeres DTO zurück
            if (!uploadsWithTagForMe.Any())
            {
                _logger.LogInformation("GetPersonalHarvestEntry: no uploads for user with tag. profileId={ProfileId}, tagId={TagId}", profileId, tagId);
                return new LeaderboardEntryDto();
            }

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
            {
                _logger.LogWarning("GetPersonalHarvestEntry: username not found. profileId={ProfileId}", profileId);
                return new LeaderboardEntryDto();
            }

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

            _logger.LogInformation("GetPersonalHarvestEntry finished. profileId={ProfileId}, rank={Rank}, value={Value}", profileId, rank, myValue);
            
            return new LeaderboardEntryDto
            {
                Rank = rank,
                ProfileId = profileId,
                UserName = myUserName,
                Value = myValue
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetPersonalHarvestEntry failed. profileId={ProfileId}, tagId={TagId}, goal={Goal}", profileId, tagId, goal);
            throw;
        }
    }
}