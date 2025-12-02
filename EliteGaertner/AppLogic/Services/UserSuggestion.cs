using AppLogic.Interfaces;
using Contracts.Data_Transfer_Objects;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace AppLogic.Services;

public class UserSuggestion : IUserSuggestion
{

    private readonly IDictionary<ProfileDto, HarvestUploadDto> _userSuggestionList;
    private readonly int _preloadCount;



    public UserSuggestion(int userId, int preloadCount)
    {
        _userSuggestionList = new Dictionary<ProfileDto, HarvestUploadDto>();
        _preloadCount = preloadCount;
        CreateUserSuggestions(CreateHarvestSuggestions(userId));
    }

    public Dictionary<ProfileDto, HarvestUploadDto> CreateUserSuggestions(List<HarvestUploadDto> harvestSuggestions)
    {
        throw new NotImplementedException();
    }

    private List<HarvestUploadDto> CreateHarvestSuggestions(int userId)
    {
        IPreferenceDBS userPreference = new PreferenceDBS();
        var harvestSuggestion = new HarvestSuggestion(userId, _preloadCount, userPreference.GetUserPreference(userId));
        return harvestSuggestion.GetHarvestSuggestionList();
    }


    public bool RateUser(int userId, int targetUserId, int value)
    {
        throw new NotImplementedException();
    }

    public IDictionary<ProfileDto, HarvestUploadDto> ReturnRecommendedUserList(int userId, IList<HarvestUploadDto> harvestUploadDtos)
    {
        throw new NotImplementedException();
    }

    public bool WasProfileShown(int userId, int targetUserId)
    {
        throw new NotImplementedException();
    }

    public bool RecommendedProfileCount(IDictionary<ProfilMgmDto, HarvestUploadDto> userList, int threshholdCount)
    {
        throw new NotImplementedException();
    }
}