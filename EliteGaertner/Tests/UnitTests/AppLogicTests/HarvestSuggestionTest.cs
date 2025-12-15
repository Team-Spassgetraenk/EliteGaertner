using Contracts.Data_Transfer_Objects;
using AppLogic.Services;

namespace Tests.UnitTests.AppLogicTests;

[TestClass]
public class HarvestSuggestionTest
{
    [TestMethod]
    public void Constructor_LoadsHarvestSuggestions_FromRepo()
    {
        // Arrange
        var fakeRepo = new FakeHarvestDbs();

        var profile = new ProfileDto
        {
            ProfileId = 1,
            PreferenceDtos = new List<PreferenceDto>
            {
                new PreferenceDto { TagId = 3 }
            }
        };

        // Act
        var suggestion = new HarvestSuggestion(fakeRepo, profile, preloadCount: 10);
        var result = suggestion.GetHarvestSuggestionList();

        // Assert
        Assert.AreEqual(3, result.Count);
    }
}