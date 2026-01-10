using Contracts.Data_Transfer_Objects;
using AppLogic.Interfaces;
using DataManagement.Interfaces;
using Microsoft.Extensions.Logging;

namespace AppLogic.Services;

public class HarvestSuggestion : IHarvestSuggestion
{
    private readonly List<HarvestUploadDto> _harvestSuggestionsList = new();
    private readonly IHarvestDbs _harvestRepo;
    private readonly ILogger<HarvestSuggestion> _logger;
   
    public HarvestSuggestion(
        ILogger<HarvestSuggestion> logger,
        IHarvestDbs harvestRepo, 
        int profileId, 
        List<int> tagIds, 
        HashSet<int> alreadyRatedProfiles,
        int preloadCount)
    {
        _logger = logger;
        _logger.LogInformation(
            "HarvestSuggestion initialized (ProfileId={ProfileId}, TagCount={TagCount}, PreloadCount={PreloadCount})",
            profileId,
            tagIds?.Count ?? 0,
            preloadCount);
        
        //Zuweisung der IHarvestDbs
        _harvestRepo = harvestRepo;
        
        _logger.LogDebug(
            "Loading harvest suggestions from repository (AlreadyRatedProfiles={AlreadyRatedCount})",
            alreadyRatedProfiles?.Count ?? 0);
        
        //Übergabe der ProfileId und dem dazugehörigen Interessensprofil/TagIds an die Datenbank.
        _harvestSuggestionsList.AddRange(_harvestRepo.GetHarvestUploadRepo(
            profileId, 
            tagIds, 
            alreadyRatedProfiles, 
            preloadCount));
        
        _logger.LogInformation(
            "HarvestSuggestion loaded {Count} harvest uploads",
            _harvestSuggestionsList.Count);
    }
    
    public List<HarvestUploadDto> GetHarvestSuggestionList()
        => _harvestSuggestionsList;
}