using AppLogic.Logic.Data_Transfer_Objects;
using AppLogic.Logic.Interfaces;

namespace AppLogic.Logic.Services;

public class HarvestSuggestionManagement : IGetHarvestSuggestions, 
{

    private readonly IList<HarvestUploadDto> _harvestSuggestionsList;
    
    //WIE ÜBERGEBEN WIR DIE INTERESSEN?
    HarvestSuggestionManagement(int userId, int count)
    {
        _harvestSuggestionsList = new List<HarvestUploadDto>();

        while (count >= 0)
        {
            _harvestSuggestionsList.Add(GetHarvest());
        }

    }

    //NICHT FERTIG
    public HarvestUploadDto GetHarvest(int? uploadId)
    {
        var dto = _harvestSuggestionsList.Get
        
        if()
    }

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