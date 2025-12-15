using Contracts.Data_Transfer_Objects;
using AppLogic.Interfaces;
using DataManagement;
using DataManagement.Interfaces;


namespace AppLogic.Services;


public class HarvestSuggestion : IHarvestSuggestion
{
    //Initialisierung der Liste und der Datenbankverbindung bei Erstellung des Objektes
    private readonly List<HarvestUploadDto> _harvestSuggestionsList = new();
    private readonly IHarvestDbs _harvestRepo;
   
    public HarvestSuggestion(IHarvestDbs harvestRepo, ProfileDto contentReceiver, int preloadCount)
    {
        _harvestRepo = harvestRepo;
        
        //Aufbereitung der ProfileId und der dazugehörigen TagIds
        var profileId = contentReceiver.ProfileId;
        var tagIds = contentReceiver.PreferenceDtos
            .Select(p => p.TagId)
            .Distinct()
            .ToList();
        
        //Übergabe der ProfileId und dem dazugehörigen Interessensprofil/TagIds an die Datenbank.
        _harvestSuggestionsList.AddRange(_harvestRepo.GetHarvestUploadRepo(profileId, tagIds, preloadCount));
    }
    
    public List<HarvestUploadDto> GetHarvestSuggestionList()
        => _harvestSuggestionsList;

//   Bin mir noch nicht sicher ob wir das überhaupt brauchen. 
//   public HarvestUploadDto GetHarvest(int uploadId)
//   {
//       var dto = GetById(uploadId);
//       if (dto is null)
//           throw new ArgumentException($"Upload {uploadId} existiert nicht!");
//       return dto;
//   }
//   
//   public HarvestUploadDto? GetById(int uploadId)
//       => _harvestSuggestionsList.FirstOrDefault(dto => dto.UploadId == uploadId);
//
//   public string GetUrl(int uploadId)
//       => GetHarvest(uploadId).ImageUrl;
//
//   public string GetDescription(int uploadId)
//       => GetHarvest(uploadId).Description;
//
//   public float GetWeight(int uploadId)
//       => GetHarvest(uploadId).WeightKg;
//
//   public float GetLength(int uploadId)
//       => GetHarvest(uploadId).LengthCm;
//
//   public float GetWidth(int uploadId)
//       => GetHarvest(uploadId).WidthCm; 
//
//   public DateTime GetDate(int uploadId)
//       => GetHarvest(uploadId).UploadDate;
}