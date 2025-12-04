using Contracts.Data_Transfer_Objects;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class MatchesDBS : IMatchesDBS
{
    public MatchDto GetMatchDto(ProfileDto contentReceiver, ProfileDto targetProfile)
    {
        throw new NotImplementedException();
    }

    public void SaveMatchInfo(MatchDto matchDto)
    {
        throw new NotImplementedException();
    }
}