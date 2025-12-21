using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;

public interface IMatchesDbs
{

    public MatchDto GetMatchInfo(int profileIdReceiver, int profileIdCreator);

    public List<PublicProfileDto> GetActiveMatches(int profileIdReceiver);

    public void SaveMatchInfo(MatchDto matchDto);
    

}