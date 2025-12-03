using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace AppLogic.Services;

public class UserSuggestion : IUserSuggestion
{
    
    private readonly IDictionary<ProfileDto, HarvestUploadDto> _userSuggestionList;
    

    
    public UserSuggestion(int userId, int preloadCount)
    {
        _userSuggestionList = new Dictionary<ProfileDto, HarvestUploadDto>();
        CreateUserSuggestions(CreateHarvestSuggestions(userId, preloadCount));
    }

    public void CreateUserSuggestions(List<HarvestUploadDto> harvestSuggestions)
    {
        IProfileDBS profileDbs = new ProfileDBS();
        foreach (HarvestUploadDto harvestUpload in harvestSuggestions)
        {
            ProfileDto profile = profileDbs.GetProfile(harvestUpload.UserId);
            _userSuggestionList.Add(profile, harvestUpload);
        }
    }

    public List<HarvestUploadDto> CreateHarvestSuggestions(int userId, int preloadCount)
    {
        IPreferenceDBS userPreference = new PreferenceDBS();
        var harvestSuggestion = new HarvestSuggestion(userId, preloadCount, userPreference.GetUserPreference(userId));
        return harvestSuggestion.GetHarvestSuggestionList();
    }

    

    public bool WasProfileShown(int userId, int targetUserId)
    {
        throw new NotImplementedException();
    }
    
}