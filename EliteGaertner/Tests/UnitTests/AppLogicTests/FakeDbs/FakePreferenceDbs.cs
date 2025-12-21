using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;

namespace Tests.UnitTests.AppLogicTests.FakeDbs;

public class FakePreferenceDbs : IPreferenceDbs
{
    public IEnumerable<PreferenceDto> GetUserPreference(int profileId)
    {
        throw new NotImplementedException();
    }

    public void SetUserPreference(int profileId, PreferenceDto newUserPreference)
    {
        throw new NotImplementedException();
    }
}