using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using DataManagement;

namespace AppLogic.Services;

public class ProfileSuggestion : IProfileSuggestion
{
    
    private readonly Dictionary<PublicProfileDto, HarvestUploadDto> _userSuggestionList;
    private readonly IMatchesDbs _matchesDbs;
    private readonly IProfileDbs _profileDbs;
    private readonly IHarvestDbs _harvestDbs;
    
    public ProfileSuggestion(IMatchesDbs matchesDbs, IProfileDbs profileDbs, IHarvestDbs harvestDbs, int profileId, List<int> tagIds, int preloadCount)
    {
        _userSuggestionList = new Dictionary<PublicProfileDto, HarvestUploadDto>();
        _matchesDbs = matchesDbs;
        _profileDbs = profileDbs;
        _harvestDbs = harvestDbs;
        CreateProfileSuggestions(profileId, CreateHarvestSuggestions(profileId, tagIds, preloadCount));
    }
    
    public List<HarvestUploadDto> CreateHarvestSuggestions(int profileId, List<int> tagIds, int preloadCount)
    {
        var harvestSuggestion = new HarvestSuggestion(_harvestDbs, profileId, tagIds, preloadCount);
        return harvestSuggestion.GetHarvestSuggestionList();
    }

    public void CreateProfileSuggestions(int profileId, List<HarvestUploadDto> harvestSuggestions)
    {
        //Gehe durch jedes einzelne HarvestUpload durch
        foreach (var harvestUpload in harvestSuggestions)
        {
            //Gebe dir für jedes HarvestUpload das passende ProfileDto zurück
            var targetProfile = _profileDbs.GetPublicProfile(harvestUpload.ProfileId);
            //Wenn das Profil noch nicht bewertet worden ist....
            if (!ProfileAlreadyRated(profileId, harvestUpload.ProfileId))
            {
                //Füge ProfileDto + HarvestUpload zur Dictionary 
                _userSuggestionList.Add(targetProfile, harvestUpload);
            }
        }
    }

    public bool ProfileAlreadyRated (int profileIdReceiver,  int profileIdCreator)
    {
        //Datenbank gibt MatchDto zurück
        var matchDto = _matchesDbs.GetMatchInfo(profileIdReceiver, profileIdCreator);
        //Überprüfe ob Receiver schon Creator bewertet hat
        return matchDto.ContentReceiver != null; 
    }
    
    public Dictionary<PublicProfileDto, HarvestUploadDto> GetProfileSuggestionList()
        => _userSuggestionList;
}