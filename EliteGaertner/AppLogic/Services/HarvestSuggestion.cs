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
   
    public HarvestSuggestion(IHarvestDbs harvestRepo, 
        int profileId, 
        List<int> tagIds, 
        HashSet<int> alreadyRatedProfiles,
        int preloadCount)
    {
        //Zuweisung der IHarvestDbs
        _harvestRepo = harvestRepo;
        //Übergabe der ProfileId und dem dazugehörigen Interessensprofil/TagIds an die Datenbank.
        _harvestSuggestionsList.AddRange(_harvestRepo.GetHarvestUploadRepo(
            profileId, 
            tagIds, 
            alreadyRatedProfiles, 
            preloadCount));
    }
    
    public List<HarvestUploadDto> GetHarvestSuggestionList()
        => _harvestSuggestionsList;
}