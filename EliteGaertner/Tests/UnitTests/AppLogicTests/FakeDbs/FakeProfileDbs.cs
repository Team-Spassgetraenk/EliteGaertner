using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;

namespace Tests.UnitTests.AppLogicTests.FakeDbs;

public class FakeProfileDbs : IProfileDbs
{
    public PrivateProfileDto GetPublicProfile(int profileId)
    {
        throw new NotImplementedException();
    }
}