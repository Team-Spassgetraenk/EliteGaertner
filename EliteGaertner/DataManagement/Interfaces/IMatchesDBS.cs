using Contracts.Data_Transfer_Objects;

namespace Infrastructure.Interfaces;

public interface IMatchesDBS
{

    public MatchDto GetMatchDto(ProfileDto contentReceiver, ProfileDto targetProfile );

    public void SaveMatchInfo(MatchDto matchDto);

}