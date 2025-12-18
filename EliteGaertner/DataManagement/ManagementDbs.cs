using System.Runtime.CompilerServices;
using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

public class ManagementDbs : IHarvestDbs, IMatchesDbs, IPreferenceDbs, IProfileDbs
{
    private readonly EliteGaertnerDbContext _dbContext;

    public ManagementDbs(EliteGaertnerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public IEnumerable<HarvestUploadDto> GetHarvestUploadsRepo(int profileId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount)
    {
        //Überprüfung ob ProfilId, TagIds vorhanden sind und PreloadCount > 0 ist
        //Falls nicht -> leere Liste zurückgeben
        if (profileId <= 0 || tagIds is null || tagIds.Count == 0 || preloadCount <= 0)
            return new List<HarvestUploadDto>();

        // Query: hol pro fremdem Profil genau den neuesten Upload, der mindestens einen passenden Tag hat.
        // Wichtig: NICHT vorher .ToList() machen, sonst materialisierst du unnötig viele Entities/DTOs.

        // Subquery: pro ProfileId den maximalen UploadDate-Wert (nur für passende Uploads)
        
        //Hier bin ich in einen bekannten EF Core Query Translation Bug gelaufen.
        //Anscheinend mag er die Kombi aus GroupBy, Select(g => g.OrderByDescending(...).First()), Select(new Dto) nicht.
        //-> KeyNotFoundException: EmptyProjectionMember 
        //Deswegen musste ich die Query "aufsplitten"
        
        var latestPerProfile = _dbContext.Harvestuploads
            //Da es sich hier um eine Read Only Abfrage handelt, muss sich EF kein Objekt merken
            .AsNoTracking()
            //Filter alle HarvestUploads des Content Receivers heraus
            .Where(h => h.Profileid != profileId)
            //Überprüfe alle Tags des Harvest Uploads und prüfe, ob es mind ein Tag gibt, dass mit den Interessen
            //des Content Receivers übereinstimmen
            .Where(h => h.Tags.Any(t => tagIds.Contains(t.Tagid)))
            //Gruppiere die HarvestUploads die dem gleichen User gehören
            //Finde heraus welches dieser Uploads am aktuellsten ist und speicher dies in Objekten ab
            .GroupBy(h => h.Profileid)
            .Select(g => new
            {
                ProfileId = g.Key,
                MaxUploadDate = g.Max(x => x.Uploaddate)
            });

        // Hauptquery: Join auf (ProfileId, UploadDate) => genau diese neuesten Uploads
        // Dann sortieren + Take(preloadCount) und erst ganz am Ende materialisieren.
        var result = (
                from h in _dbContext.Harvestuploads.AsNoTracking()
                //Wir joinen jetzt das Ergebnis von latestPerProfile mit der HarvestUpload Tabelle
                //Somit bleiben nur noch die aktuellsten Uploads der User die mit unseren Interessen übereinstimmen
                join l in latestPerProfile
                    on new { ProfileId = h.Profileid, UploadDate = h.Uploaddate }
                    equals new { ProfileId = l.ProfileId, UploadDate = l.MaxUploadDate }
                //Redundante Absicherung um eigene Uploads zu vermeiden
                where h.Profileid != profileId
                //Überprüft nochmal ob der Tag im Upload vorhanden ist
                where h.Tags.Any(t => tagIds.Contains(t.Tagid))
                //Sortiert nach den aktuellsten Uploads
                orderby h.Uploaddate descending
                //Erstellt aus den Ergebnissen die benötigten Dtos
                select new HarvestUploadDto
                {
                    UploadId = h.Uploadid,
                    ImageUrl = h.Imageurl,
                    Description = h.Description,
                    WeightGram = h.Weightgramm,
                    WidthCm = h.Widthcm,
                    LengthCm = h.Lengthcm,
                    UploadDate = h.Uploaddate,
                    ProfileId = h.Profileid
                })
            //PreloadCount gibt die Anzahl der benötigten DTOs an
            .Take(preloadCount)
            .ToList();

        return result;
    }

    public MatchDto GetMatchInfo(ProfileDto contentReceiver, ProfileDto targetProfile)
    {
        throw new NotImplementedException();
    }

    public List<ProfileDto> GetSuccessfulMatches(ProfileDto contentReceiver)
    {
        throw new NotImplementedException();
    }

    public void SaveMatchInfo(MatchDto matchDto)
    {
        throw new NotImplementedException();
    }

    public PreferenceDto GetUserPreference(int userId)
    {
        throw new NotImplementedException();
    }

    public void SetUserPreference(int userId, PreferenceDto newUserPreference)
    {
        throw new NotImplementedException();
    }

    public ProfileDto GetProfile(int userId)
    {
        throw new NotImplementedException();
    }
}
