using System;
using System.Collections.Generic;
using System.Linq;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.IntegrationTests.AppLogicTests;

//Durch ChatGPT generiert!!!
[TestClass]
[DoNotParallelize]
[TestCategory("Integration")]
public class ProfileSuggestionTest_FromRealDb : IntegrationTestBase
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

        // Nach dem Split: separate DB-Klassen pro Interface
        var matchesDbs = new MatchesDbs(db);
        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);

        const int receiverProfileId = 1;

        // Bereits bewertete ProfileIds für den Receiver laden (werden in HarvestSuggestion/DB-Abfrage ausgeschlossen)
        var alreadyRatedProfiles = matchesDbs.GetAlreadyRatedProfileIds(profileIdReceiver: receiverProfileId);

        var testDto = new PrivateProfileDto
        {
            ProfileId = receiverProfileId,
            PreferenceDtos = new List<PreferenceDto>
            {
                new PreferenceDto { TagId = 3,  Profileid = receiverProfileId }, // Tomaten
                new PreferenceDto { TagId = 6,  Profileid = receiverProfileId }, // Zucchini
                new PreferenceDto { TagId = 9,  Profileid = receiverProfileId }, // Salate
                new PreferenceDto { TagId = 10, Profileid = receiverProfileId }, // Zwiebeln
                new PreferenceDto { TagId = 17, Profileid = receiverProfileId }, // Trauben
                new PreferenceDto { TagId = 22, Profileid = receiverProfileId }, // Mais
                new PreferenceDto { TagId = 11, Profileid = receiverProfileId }, // Melonen
                new PreferenceDto { TagId = 18, Profileid = receiverProfileId }, // Bohnen
                new PreferenceDto { TagId = 19, Profileid = receiverProfileId }, // Spinat
                new PreferenceDto { TagId = 14, Profileid = receiverProfileId }, // Pfirsiche
                new PreferenceDto { TagId = 8,  Profileid = receiverProfileId }, // Karotten
            }
        };

        var profileId = testDto.ProfileId;
        var tagIds = testDto.PreferenceDtos.Select(p => p.TagId).Distinct().ToList();
        const int preloadCount = 10;

        // 1) HarvestSuggestions als Basis
        var harvestSuggestion = new HarvestSuggestion(harvestDbs, profileId, tagIds, alreadyRatedProfiles, preloadCount);
        var harvestResults = harvestSuggestion.GetHarvestSuggestionList();

        Assert.IsNotNull(harvestResults);
        Assert.IsTrue(harvestResults.Any(), "HarvestSuggestions darf nicht leer sein (Seed/DB-Setup prüfen).");

        // Erwartete Creator-ProfileIds (unabhängig davon, welcher konkrete Upload pro Creator gewählt wird)
        var expectedCreatorIds = harvestResults
            .Where(hu => hu.ProfileId != profileId) // keine eigenen Uploads
            .Where(hu => !alreadyRatedProfiles.Contains(hu.ProfileId)) // nicht bereits bewertete
            .Select(hu => hu.ProfileId)
            .Distinct()
            .ToList();

        // Alle UploadIds aus der Harvest-Basis, um später zu prüfen, ob ProfileSuggestion nur aus dieser Basis auswählt.
        var harvestUploadIds = harvestResults.Select(hu => hu.UploadId).ToHashSet();

        // Act
        var profileSuggestion = new ProfileSuggestion(matchesDbs, profileDbs, harvestDbs, profileId, tagIds, preloadCount);
        var resultDict = profileSuggestion.GetProfileSuggestionList();

        // Assert – Basis
        Assert.IsNotNull(resultDict);
        Assert.IsTrue(resultDict.Count > 0,
            "ProfileSuggestions darf nicht leer sein (oder alles wurde schon bewertet).");

        // Assert – pro Creator-Profil nur ein Vorschlag (keine Duplikate)
        var actualCreatorIds = resultDict.Keys.Select(k => k.ProfileId).ToList();
        Assert.AreEqual(actualCreatorIds.Count, actualCreatorIds.Distinct().Count(),
            "ProfileSuggestion darf pro Creator-Profil nur einen Vorschlag enthalten (keine doppelten ProfileIds in den Keys).");

        // Assert – jedes Dictionary-Paar muss konsistent sein:
        foreach (var kv in resultDict)
        {
            var publicProfile = kv.Key;
            var harvestUpload = kv.Value;

            Assert.IsNotNull(publicProfile);
            Assert.IsNotNull(harvestUpload);

            Assert.AreEqual(harvestUpload.ProfileId, publicProfile.ProfileId,
                $"PublicProfileDto.ProfileId muss mit HarvestUploadDto.ProfileId übereinstimmen. KeyProfileId={publicProfile.ProfileId}, ValueProfileId={harvestUpload.ProfileId}, UploadId={harvestUpload.UploadId}");

            Assert.AreNotEqual(profileId, harvestUpload.ProfileId, "Es dürfen keine eigenen Uploads vorgeschlagen werden.");

            // Der gewählte Upload muss aus der Harvest-Basis stammen (sonst hat ProfileSuggestion eine andere Quelle)
            Assert.IsTrue(harvestUploadIds.Contains(harvestUpload.UploadId),
                $"ProfileSuggestion hat einen Upload ausgewählt, der nicht in der Harvest-Basis enthalten ist. UploadId={harvestUpload.UploadId}");

            var alreadyRated = alreadyRatedProfiles.Contains(harvestUpload.ProfileId);
            Assert.IsFalse(alreadyRated,
                $"Returned suggestion must be NOT yet rated. Receiver={profileId}, Creator={harvestUpload.ProfileId}, UploadId={harvestUpload.UploadId}.");
        }

        // Assert – Creator-Menge vergleichen (unabhängig von Dictionary-Reihenfolge)
        var actualCreatorIdsSet = resultDict.Keys.Select(k => k.ProfileId).ToList();

        try
        {
            CollectionAssert.AreEquivalent(expectedCreatorIds, actualCreatorIdsSet,
                "ProfileSuggestions muss dieselben Creator-ProfileIds liefern wie die Harvest-Basis (nach Filter: not rated, not self)." );
        }
        catch
        {
            TestContext.WriteLine("--EXPECTED CREATOR IDS--");
            foreach (var id in expectedCreatorIds.OrderBy(x => x))
                TestContext.WriteLine($"Expected CreatorProfileId={id}");

            TestContext.WriteLine("--ACTUAL CREATOR IDS--");
            foreach (var id in actualCreatorIdsSet.OrderBy(x => x))
                TestContext.WriteLine($"Actual   CreatorProfileId={id}");

            throw;
        }

        // Logging
        TestContext.WriteLine("--PROFILE SUGGESTION TESTERGEBNIS--");
        TestContext.WriteLine($"AlreadyRatedProfiles (count={alreadyRatedProfiles.Count}): {{ {string.Join(", ", alreadyRatedProfiles.OrderBy(x => x))} }}");
        TestContext.WriteLine($"Harvest base count: {harvestResults.Count}");
        TestContext.WriteLine($"Expected (unique eligible creators): {expectedCreatorIds.Count}");
        TestContext.WriteLine($"Actual dict count: {resultDict.Count}");

        foreach (var kv in resultDict)
        {
            TestContext.WriteLine(
                $"CreatorProfileId={kv.Key.ProfileId} -> UploadId={kv.Value.UploadId}, UploadProfileId={kv.Value.ProfileId}");
        }
    }
}