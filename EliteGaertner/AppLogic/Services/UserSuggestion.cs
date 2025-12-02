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
        IPreferenceDBS userPreference = new PreferenceDBS();
        HarvestSuggestion harvestSuggestion = new HarvestSuggestion(userId, preloadCount, userPreference.GetUserPreference(userId));
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