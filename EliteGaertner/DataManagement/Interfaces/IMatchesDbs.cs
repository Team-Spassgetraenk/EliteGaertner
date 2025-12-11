using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;

public interface IMatchesDbs
{

    public MatchDto GetMatchInfo(ProfileDto contentReceiver, ProfileDto targetProfile);

    public List<ProfileDto> GetSuccessfulMatches(ProfileDto contentReceiver);

    public void SaveMatchInfo(MatchDto matchDto);
    

}