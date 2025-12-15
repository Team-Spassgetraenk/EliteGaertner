using System.Runtime.CompilerServices;
using Contracts.Data_Transfer_Objects;
using DataManagement.Entities;
using DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataManagement;

public class ManagementDbs : IHarvestDbs, IMatchesDbs, IPreferenceDbs, IProfileDbs
{
    private readonly EliteGaertnerDbContext _dbContext = new();
    
    public IEnumerable<HarvestUploadDto> GetHarvestUploadsRepo(int profileId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount)
    {
        //Überprüfung ob ProfilId, TagIds vorhanden sind und PreloadCount > 0 ist
        //Falls nicht -> leere Liste zurückgeben
        if (profileId == null || tagIds.Count == 0 || preloadCount > 0)
            return new List<HarvestUploadDto>();

        //Query fragt nach passenden HarvestUploads und fügt sie der Liste hinzu
        var matchingHarvestUploads = _dbContext.Harvestuploads
            //Da es sich hier um eine Read Only Abfrage handelt, muss sich EF kein Objekt merken
            .AsNoTracking()
            //Filter alle HarvestUploads des Content Receivers heraus 
            .Where(h => h.Profileid != profileId)
            //Überprüfe alle Tags des Harvest Uploads und prüfe, ob es mind ein Tag gibt, dass mit den Interessen
            //des Content Receivers übereinstimmen 
            .Where(h => h.Tags.Any(t => tagIds.Contains(t.Tagid)))
            //Gruppiere die HarvestUploads die dem gleichen User gehören und nimm den neusten Upload 
            .GroupBy(h => h.Profileid)
            .Select(g => g.OrderByDescending(h => h.Uploaddate).First())
            //Neueste HarvestUploads sollen zuerst angezeigt werden
            .OrderByDescending(h => h.Uploaddate)
            //PreloadCount gibt an wie viele Ergebnisse maximal angezeigt werden sollen
            .Take(preloadCount)
            //Aus den Ergebnissen werden die HarvestUploadDtos erstellt
            .Select(h => new HarvestUploadDto()
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
            .ToList();

        return matchingHarvestUploads;
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
