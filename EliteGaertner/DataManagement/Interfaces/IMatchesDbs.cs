using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;

public interface IMatchesDbs
{
    public bool ProfileAlreadyRated(int profileIdReceiver, int profileIdCreator);

    public IEnumerable<PublicProfileDto> GetActiveMatches(int profileIdReceiver);

    public void SaveMatchInfo(RateDto matchDto);
}