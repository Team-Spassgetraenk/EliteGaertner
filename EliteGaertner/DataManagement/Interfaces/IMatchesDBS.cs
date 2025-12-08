using Contracts.Data_Transfer_Objects;

namespace Infrastructure.Interfaces;

public interface IMatchesDBS
{

    public MatchDto GetMatchInfo(ProfileDto contentReceiver, ProfileDto targetProfile );

    public List<MatchDto> GetSuccessfulMatches(ProfileDto contentReceiver);

    public void SaveMatchInfo(MatchDto matchDto);

}