using AppLogic.Logic.Data_Transfer_Objects;
using AppLogic.Logic.Interfaces;
using Infrastructure.Interfaces;

namespace AppLogic.Logic.Services;


public class HarvestSuggestionManagement : IGetHarvestSuggestions, IHarvestSuggestionRepository
{

    private readonly IList<HarvestUploadDto> _harvestSuggestionsList;
    
    //WIE ÜBERGEBEN WIR DIE INTERESSEN?
    HarvestSuggestionManagement(int userId, int count)
    {
        _harvestSuggestionsList = new List<HarvestUploadDto>();
        
        _harvestSuggestionsList.Add(new IHarvestDBS.);
        

    }

    //NICHT FERTIG
    public HarvestUploadDto GetHarvest(int? uploadId)
    {
        var dto = _harvestSuggestionsList.Get
            
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