using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using DataManagement;

namespace AppLogic.Services;

public class ProfileSuggestion : IProfileSuggestion
{
    
    private readonly IMatchesDbs _matchesDbs;
    private readonly IProfileDbs _profileDbs;
    private readonly IHarvestDbs _harvestDbs;
    private readonly Dictionary<PublicProfileDto, HarvestUploadDto> _userSuggestionList;
    private HashSet<int> _alreadyRatedProfiles;
    
    public ProfileSuggestion(IMatchesDbs matchesDbs, IProfileDbs profileDbs, IHarvestDbs harvestDbs, int profileId, List<int> tagIds, int preloadCount)
    {
        _matchesDbs = matchesDbs;
        _profileDbs = profileDbs;
        _harvestDbs = harvestDbs;
        _userSuggestionList = new Dictionary<PublicProfileDto, HarvestUploadDto>();
        _alreadyRatedProfiles = new HashSet<int>();
        LoadAlreadyRatedProfiles(profileId);
        CreateProfileSuggestions(profileId, CreateHarvestSuggestions(profileId, tagIds, preloadCount));
        
    }
    
    public List<HarvestUploadDto> CreateHarvestSuggestions(int profileId, List<int> tagIds, int preloadCount)
    {
        var harvestSuggestion = new HarvestSuggestion(_harvestDbs, profileId, tagIds, _alreadyRatedProfiles, preloadCount);
        return harvestSuggestion.GetHarvestSuggestionList();
    }

    public void CreateProfileSuggestions(int profileId, List<HarvestUploadDto> harvestSuggestions)
    {
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
    }
    
    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList()
        => _userSuggestionList;
}