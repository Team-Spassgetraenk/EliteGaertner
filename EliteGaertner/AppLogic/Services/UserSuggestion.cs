using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using DataManagement;

namespace AppLogic.Services;

public class UserSuggestion : IUserSuggestion
{
    
    private readonly Dictionary<ProfileDto, HarvestUploadDto> _userSuggestionList;
    
    
    public UserSuggestion(ProfileDto contentReceiver, List<string> preferences, int preloadCount)
    {
        _userSuggestionList = new Dictionary<ProfileDto, HarvestUploadDto>();
        CreateUserSuggestions(contentReceiver.UserId, CreateHarvestSuggestions(contentReceiver, preferences, preloadCount));
    }

    public Dictionary<ProfileDto, HarvestUploadDto> GetUserSuggestionList(int userId)
        => _userSuggestionList;

    public void CreateUserSuggestions(int userId, List<HarvestUploadDto> harvestSuggestions)
    {
        IProfileDbs profileDbs = new ManagementDbs();
        
        //ÜBERPRÜFUNG OB MATCH SCHON BESTEHT FEHLT!!!!
        foreach (HarvestUploadDto harvestUpload in harvestSuggestions)
        {
            var profile = profileDbs.GetProfile(harvestUpload.ProfileId);
            if(ProfileAlreadyRated(userId, profile.UserId ))
            _userSuggestionList.Add(profile, harvestUpload);
        }
    }

    public List<HarvestUploadDto> CreateHarvestSuggestions(ProfileDto contentReceiver, List<string> preferences, int preloadCount)
    {
        var harvestSuggestion = new HarvestSuggestion(userId, preferences, preloadCount);
        return harvestSuggestion.GetHarvestSuggestionList();
    }
    
    public bool ProfileAlreadyRated(ProfileDto content int targetUserId)
    {
        IMatchesDbs matchesDbs = new ManagementDbs();

        MatchDto match = matchesDbs.GetMatchInfo();
    }
}