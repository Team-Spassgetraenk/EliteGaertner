using Contracts.Data_Transfer_Objects;
using AppLogic.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;


namespace AppLogic.Services;


public class HarvestSuggestion : IHarvestSuggestion
{

    private readonly List<HarvestUploadDto> _harvestSuggestionsList;
    
    //WIE ÜBERGEBEN WIR DIE INTERESSEN?
    public HarvestSuggestion(int userId, int preloadCount, PreferenceDto userPreference)
    {
        _harvestSuggestionsList = new List<HarvestUploadDto>();
        //Das muss glaub ich in die PROGRAM.CS
        IHarvestDBS harvestRepo = new HarvestDBS();
        _harvestSuggestionsList.AddRange(harvestRepo.GetHarvestUploadRepo(userId, preloadCount, userPreference));
    }

    public List<HarvestUploadDto> GetHarvestSuggestionList()
        => _harvestSuggestionsList;

    public HarvestUploadDto GetHarvest(int uploadId)
    {
        var dto = GetById(uploadId);
        if (dto is null)
            throw new ArgumentException($"Upload {uploadId} existiert nicht!");
        return dto;
    }
    
    public HarvestUploadDto? GetById(int uploadId)
        => _harvestSuggestionsList.FirstOrDefault(dto => dto.UploadId == uploadId);

    public string GetUrl(int uploadId)
        => GetHarvest(uploadId).ImageUrl;

    public string GetDescription(int uploadId)
        => GetHarvest(uploadId).Description;

    public float GetWeight(int uploadId)
        => GetHarvest(uploadId).WeightKg;

    public float GetLength(int uploadId)
        => GetHarvest(uploadId).LengthCm;

    public float GetWidth(int uploadId)
        => GetHarvest(uploadId).WidthCm; 

    public DateTime GetDate(int uploadId)
        => GetHarvest(uploadId).UploadDate;
}