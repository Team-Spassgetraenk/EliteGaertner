using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using DataManagement;

namespace AppLogic.Services;

public class UserSuggestion : IUserSuggestion
{
    
    private readonly Dictionary<ProfileDto, HarvestUploadDto> _userSuggestionList;
    private readonly IProfileDbs _profileDbs;
    private readonly IHarvestDbs _harvestDbs;
    
    public UserSuggestion(IProfileDbs profileDbs, IHarvestDbs harvestDbs, int profileId, List<int> tagIds, int preloadCount)
    {
        _userSuggestionList = new Dictionary<ProfileDto, HarvestUploadDto>();
        _profileDbs = profileDbs;
        _harvestDbs = harvestDbs;
        CreateUserSuggestions(profileId, CreateHarvestSuggestions(profileId, tagIds, preloadCount));
    }
    
    public List<HarvestUploadDto> CreateHarvestSuggestions(int profileId, List<int> tagIds, int preloadCount)
    {
        var harvestSuggestion = new HarvestSuggestion(_harvestDbs, profileId, tagIds, preloadCount);
        return harvestSuggestion.GetHarvestSuggestionList();
    }

    public void CreateUserSuggestions(int profileId, List<HarvestUploadDto> harvestSuggestions)
    {
        //Gehe durch jedes einzelne HarvestUpload durch
        foreach (var harvestUpload in harvestSuggestions)
        {
            //Gebe dir für jedes HarvestUpload das passende ProfileDto zurück
            var targetProfile = _profileDbs.GetProfile(harvestUpload.ProfileId);
            //Wenn das Profil noch nicht bewertet worden ist....
            if (!ProfileAlreadyRated(profileId, harvestUpload.ProfileId))
            {
                //Füge ProfileDto + HarvestUpload zur Dictionary 
                _userSuggestionList.Add(targetProfile, harvestUpload);
            }
        }
    }

    bool ProfileAlreadyRated (int profileIdReceiver,  int profileIdCreator)
    {
        IMatchesDbs matchesDbs = new ManagementDbs();
        var match = matchesDbs.GetMatchInfo(profileIdReceiver, profileIdCreator);
        
    }
    
    public Dictionary<ProfileDto, HarvestUploadDto> GetUserSuggestionList(int userId)
        => _userSuggestionList;
}