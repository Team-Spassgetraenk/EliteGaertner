using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;

namespace Tests.UnitTests.AppLogicTests.FakeDbs;

public class FakeMatchDbs : IMatchesDbs
{
    public MatchDto GetMatchInfo(int profileIdReceiver, int profileIdCreator)
    {
        throw new NotImplementedException();
    }

    public List<PrivateProfileDto> GetSuccessfulMatches(PrivateProfileDto contentReceiver)
    {
        throw new NotImplementedException();
    }

    public void SaveMatchInfo(MatchDto matchDto)
    {
        throw new NotImplementedException();
    }
}