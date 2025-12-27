using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.AppLogicTests;

//Durch ChatGPT generiert!!!
[TestClass]
[DoNotParallelize]
[TestCategory("Integration")]
public class ProfileSuggestionTest_FromRealDb
{
    public TestContext TestContext { get; set; } = null!;

    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=elitegaertner_test;Username=postgres;Password=postgres";

    [TestMethod]
    public void ProfileSuggestion_FromRealDb_ShouldReturnOnlyNotYetRatedProfiles_AndMatchHarvestUploads()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var db = new EliteGaertnerDbContext(options);

        TestContext.WriteLine("------------------------------------");
        TestContext.WriteLine("--ANZAHL DER TUPEL IN DEN TABELLEN--");
        TestContext.WriteLine($"Profiles: {db.Profiles.Count()}");
        TestContext.WriteLine($"Tags: {db.Tags.Count()}");
        TestContext.WriteLine($"Uploads: {db.Harvestuploads.Count()}");
        var harvestTagsCount = db.Set<Dictionary<string, object>>("Harvesttag").Count();
        TestContext.WriteLine($"HarvestTags: {harvestTagsCount}");
        TestContext.WriteLine($"ProfilePreferences: {db.Profilepreferences.Count()}");
        TestContext.WriteLine($"Rating: {db.Ratings.Count()}");
        TestContext.WriteLine($"Reports: {db.Reports.Count()}");
        TestContext.WriteLine("------------------------------------");

        // ManagementDbs muss IMatchesDbs + IProfileDbs + IHarvestDbs erfüllen
        var repo = new ManagementDbs(db);

        var testDto = new PrivateProfileDto
        {
            ProfileId = 1,
            PreferenceDtos = new List<PreferenceDto>
            {
                new PreferenceDto { TagId = 3,  Profileid = 1 }, // Tomaten
                new PreferenceDto { TagId = 6,  Profileid = 1 }, // Zucchini
                new PreferenceDto { TagId = 9,  Profileid = 1 }, // Salate
                new PreferenceDto { TagId = 10, Profileid = 1 }, // Zwiebeln
                new PreferenceDto { TagId = 17, Profileid = 1 }, // Trauben
                new PreferenceDto { TagId = 22, Profileid = 1 }, // Mais
                new PreferenceDto { TagId = 11, Profileid = 1 }, // Melonen
                new PreferenceDto { TagId = 18, Profileid = 1 }, // Bohnen
                new PreferenceDto { TagId = 19, Profileid = 1 }, // Spinat
                new PreferenceDto { TagId = 14, Profileid = 1 }, // Pfirsiche
                new PreferenceDto { TagId = 8,  Profileid = 1 }, // Karotten
            }
        };

        var profileId = testDto.ProfileId;
        var tagIds = testDto.PreferenceDtos.Select(p => p.TagId).Distinct().ToList();
        const int preloadCount = 10;

        // 1) HarvestSuggestions als Basis
        var harvestSuggestion = new HarvestSuggestion(repo, profileId, tagIds, preloadCount);
        var harvestResults = harvestSuggestion.GetHarvestSuggestionList();

        Assert.IsNotNull(harvestResults);
        Assert.IsTrue(harvestResults.Any(), "HarvestSuggestions darf nicht leer sein (Seed/DB-Setup prüfen).");

        // 2) Erwartete ProfileSuggestions: nur Creator/Uploads, die der Receiver noch NICHT bewertet hat
        var expectedPairs = new List<(int creatorProfileId, int uploadId)>();

        foreach (var hu in harvestResults)
        {
            var notYetRated = !repo.ProfileAlreadyRated(profileId, hu.ProfileId);
            if (notYetRated)
                expectedPairs.Add((hu.ProfileId, hu.UploadId));
        }

        // Act
        var profileSuggestion = new ProfileSuggestion(repo, repo, repo, profileId, tagIds, preloadCount);
        var resultDict = profileSuggestion.GetProfileSuggestionList();

        // Assert – Basis
        Assert.IsNotNull(resultDict);
        Assert.IsTrue(resultDict.Count > 0,
            "ProfileSuggestions darf nicht leer sein (oder alles wurde schon bewertet).");

        // Assert – jedes Dictionary-Paar muss konsistent sein:
        foreach (var kv in resultDict)
        {
            var publicProfile = kv.Key;
            var harvestUpload = kv.Value;

            Assert.IsNotNull(publicProfile);
            Assert.IsNotNull(harvestUpload);

            Assert.AreEqual(harvestUpload.ProfileId, publicProfile.ProfileId,
                "PublicProfileDto.ProfileId muss mit HarvestUploadDto.ProfileId übereinstimmen.");

            var alreadyRated = repo.ProfileAlreadyRated(profileId, harvestUpload.ProfileId);
            Assert.IsFalse(alreadyRated,
                $"Returned suggestion must be NOT yet rated, but ProfileAlreadyRated was true. Receiver={profileId}, Creator={harvestUpload.ProfileId}, UploadId={harvestUpload.UploadId}.");
        }

        // Assert – Menge vergleichen (als Set), unabhängig von Dictionary-Reihenfolge
        var actualPairs = resultDict
            .Select(kv => (creatorProfileId: kv.Value.ProfileId, uploadId: kv.Value.UploadId))
            .ToList();

        CollectionAssert.AreEquivalent(expectedPairs, actualPairs,
            "ProfileSuggestions muss genau den gefilterten HarvestSuggestions entsprechen (NOT ProfileAlreadyRated-Prädikat).");

        // Logging
        TestContext.WriteLine("--PROFILE SUGGESTION TESTERGEBNIS--");
        TestContext.WriteLine($"Harvest base count: {harvestResults.Count}");
        TestContext.WriteLine($"Expected (after NOT ProfileAlreadyRated-filter): {expectedPairs.Count}");
        TestContext.WriteLine($"Actual dict count: {resultDict.Count}");

        foreach (var kv in resultDict)
        {
            TestContext.WriteLine(
                $"CreatorProfileId={kv.Key.ProfileId} -> UploadId={kv.Value.UploadId}, UploadProfileId={kv.Value.ProfileId}");
        }
    }
}