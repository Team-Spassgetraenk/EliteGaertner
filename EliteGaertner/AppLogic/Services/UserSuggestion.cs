using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace AppLogic.Services;

public class UserSuggestion : IUserSuggestion
{
    
    private readonly Dictionary<ProfileDto, HarvestUploadDto> _userSuggestionList;
    
    
    public UserSuggestion(int userId, List<string> preferences, int preloadCount)
    {
        _userSuggestionList = new Dictionary<ProfileDto, HarvestUploadDto>();
        CreateUserSuggestions(CreateHarvestSuggestions(userId, preferences, preloadCount));
    }

    public Dictionary<ProfileDto, HarvestUploadDto> GetUserSuggestionList(int userId)
        => _userSuggestionList;

    public void CreateUserSuggestions(List<HarvestUploadDto> harvestSuggestions)
    {
        IProfileDBS profileDbs = new ManagementDBS();
        foreach (HarvestUploadDto harvestUpload in harvestSuggestions)
        {
            ProfileDto profile = profileDbs.GetProfile(harvestUpload.ProfileId);
            _userSuggestionList.Add(profile, harvestUpload);
        }
    }

    public List<HarvestUploadDto> CreateHarvestSuggestions(int userId, List<string> preferences, int preloadCount)
    {
        var harvestSuggestion = new HarvestSuggestion(userId, preferences, preloadCount);
        return harvestSuggestion.GetHarvestSuggestionList();
    }
    
    public bool WasProfileShown(int userId, int targetUserId)
    {
        throw new NotImplementedException();
    }
}