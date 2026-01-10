using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using DataManagement;
using Microsoft.Extensions.Logging;

namespace AppLogic.Services;

public class ProfileSuggestion : IProfileSuggestion
{
    
    private readonly IMatchesDbs _matchesDbs;
    private readonly IProfileDbs _profileDbs;
    private readonly IHarvestDbs _harvestDbs;
    private readonly ILogger<ProfileSuggestion> _logger;
    private readonly ILogger<HarvestSuggestion> _harvestLogger;
    private readonly Dictionary<PublicProfileDto, HarvestUploadDto> _userSuggestionList;
    private HashSet<int> _alreadyRatedProfiles;
    
    public ProfileSuggestion(IMatchesDbs matchesDbs, IProfileDbs profileDbs, IHarvestDbs harvestDbs, int profileId, List<int> tagIds, int preloadCount, ILogger<ProfileSuggestion> logger, ILogger<HarvestSuggestion> harvestLogger)
    {
        _matchesDbs = matchesDbs;
        _profileDbs = profileDbs;
        _harvestDbs = harvestDbs;
        _logger = logger;
        _harvestLogger = harvestLogger;
        
        _logger.LogInformation("ProfileSuggestion created. profileId={ProfileId}, tagIdsCount={TagIdsCount}, preloadCount={PreloadCount}", profileId, tagIds?.Count ?? 0, preloadCount);
        
        _userSuggestionList = new Dictionary<PublicProfileDto, HarvestUploadDto>();
        _alreadyRatedProfiles = new HashSet<int>();
        
        LoadAlreadyRatedProfiles(profileId);
        _logger.LogInformation("Loaded already-rated profiles. profileId={ProfileId}, alreadyRatedCount={AlreadyRatedCount}", profileId, _alreadyRatedProfiles?.Count ?? 0);

        var harvestSuggestions = CreateHarvestSuggestions(profileId, tagIds, preloadCount);
        _logger.LogInformation("Loaded harvest suggestions. profileId={ProfileId}, harvestSuggestionsCount={HarvestSuggestionsCount}", profileId, harvestSuggestions?.Count ?? 0);

        CreateProfileSuggestions(profileId, harvestSuggestions);
        _logger.LogInformation("Created profile suggestions. profileId={ProfileId}, profileSuggestionsCount={ProfileSuggestionsCount}", profileId, _userSuggestionList.Count);
    }
    
    public List<HarvestUploadDto> CreateHarvestSuggestions(int profileId, List<int> tagIds, int preloadCount)
    {
        //Initialisiert HarvestSuggestion
        var harvestSuggestion = new HarvestSuggestion(_harvestLogger, _harvestDbs, profileId, tagIds, _alreadyRatedProfiles, preloadCount);
        
        //Gibt die HarvestSuggestionListe zurück
        var list = harvestSuggestion.GetHarvestSuggestionList();
        
        _logger.LogDebug("HarvestSuggestion list built. profileId={ProfileId}, resultCount={ResultCount}", profileId, list?.Count ?? 0);
        
        return list;
    }

    public void CreateProfileSuggestions(int profileId, List<HarvestUploadDto> harvestSuggestions)
    {
        _logger.LogDebug("Creating profile suggestions. profileId={ProfileId}, harvestSuggestionsCount={HarvestSuggestionsCount}", profileId, harvestSuggestions?.Count ?? 0);
        
        //Gehe durch jedes einzelne HarvestUpload durch
        foreach (var harvestUpload in harvestSuggestions)
        {
            //Falls der Receiver dieses Profil bereits bewertet hat -> überspringen
            if (_alreadyRatedProfiles.Contains(harvestUpload.ProfileId))
                continue;

            //Pro Profil nur einen Vorschlag zulassen (sonst entstehen Duplikate, wenn mehrere Uploads vom selben Profil kommen)
            var alreadySuggested = _userSuggestionList.Keys.Any(p => p.ProfileId == harvestUpload.ProfileId);
            
            if (alreadySuggested)
                continue;
            
            //Gebe dir für jedes HarvestUpload das passende ProfileDto zurück
            var targetProfile = _profileDbs.GetPublicProfile(harvestUpload.ProfileId);
            
            //Füge ProfileDto + HarvestUpload zur Dictionary 
            _userSuggestionList.Add(targetProfile, harvestUpload);
        }
    }

    public void LoadAlreadyRatedProfiles(int profileId)
    {
        _alreadyRatedProfiles = _matchesDbs.GetAlreadyRatedProfileIds(profileId);
        _logger.LogDebug("Already-rated profiles loaded from DB. profileId={ProfileId}, alreadyRatedCount={AlreadyRatedCount}", profileId, _alreadyRatedProfiles?.Count ?? 0);
    }
    
    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList()
        => _userSuggestionList;
}