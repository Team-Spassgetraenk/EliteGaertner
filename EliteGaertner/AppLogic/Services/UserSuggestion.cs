using AppLogic.Logic.Data_Transfer_Objects;
using AppLogic.Logic.Interfaces;

namespace AppLogic.Logic.Services;

public class UserSuggestion : IUserSuggestion
{
    public bool RateUser(int userId, int targetUserId, int value)
    {
        throw new NotImplementedException();
    }

    public IDictionary<ProfilMgmDto, HarvestUploadDto> ReturnRecommendedUserList(int userId, IList<HarvestUploadDto> harvestUploadDtos)
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