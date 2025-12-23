using System.Diagnostics;
using System.Linq;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests.AppLogicTests;

//Durch ChatGPT generiert!!!
[TestClass]
[DoNotParallelize]
[TestCategory("Integration")]
public class MatchManagerTest_FromRealDb
{
    public TestContext TestContext { get; set; } = null!;
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=elitegaertner_test;Username=postgres;Password=postgres";

    [TestMethod]
    public void RateUser_ShouldSaveRating_RemoveSuggestion_AndRefreshActiveMatches()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var db = new EliteGaertnerDbContext(options);

        var repo = new ManagementDbs(db);

        var receiver = new PrivateProfileDto
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

        const int preloadCount = 10;
        var manager = new MatchManager(repo, repo, repo, receiver, preloadCount);

        var suggestionsBefore = manager.GetProfileSuggestionList();
        Assert.IsNotNull(suggestionsBefore);
        Assert.IsTrue(suggestionsBefore.Any(), "Es müssen Suggestions vorhanden sein (Seed/DB Setup prüfen).");

        // Nimm einen beliebigen Creator aus den Suggestions
        var creatorProfile = suggestionsBefore.Keys.First();
        var creatorId = creatorProfile.ProfileId;

        // Für Logging
        TestContext.WriteLine($"Receiver={receiver.ProfileId}, Creator={creatorId}");
        TestContext.WriteLine($"Suggestions before: {suggestionsBefore.Count}");

        var value = true; // oder false – je nachdem was du testen willst

        // Act
        manager.RateUser(creatorProfile, value);

        // Assert 1: Rating wurde in DB gespeichert (Receiver -> Creator)
        var ratingAfter = db.Ratings
            .SingleOrDefault(r => r.Contentreceiverid == receiver.ProfileId && r.Contentcreatorid == creatorId);

        Assert.IsNotNull(ratingAfter, "Es muss ein Rating-Datensatz (Receiver->Creator) existieren.");
        Assert.AreEqual(value, ratingAfter.Profilerating, "Profilerating muss gespeichert/aktualisiert sein.");
        Assert.IsNotNull(ratingAfter.Ratingdate, "Ratingdate muss gesetzt sein.");

        // Assert 2: Creator darf nicht mehr in Suggestions vorkommen
        var suggestionsAfter = manager.GetProfileSuggestionList();
        Assert.IsFalse(suggestionsAfter.Keys.Any(p => p.ProfileId == creatorId),
            "Der bewertete Creator darf nicht mehr vorgeschlagen werden.");

        // Assert 3: ActiveMatches wurden refresht und entsprechen DB
        var activeMatchesFromManager = manager.GetActiveMatches();
        var activeMatchesFromDb = repo.GetActiveMatches(receiver.ProfileId).ToList();

        // Vergleich über ProfileIds (Reihenfolge egal)
        var managerIds = activeMatchesFromManager.Select(p => p.ProfileId).ToList();
        var dbIds = activeMatchesFromDb.Select(p => p.ProfileId).ToList();

        CollectionAssert.AreEquivalent(dbIds, managerIds,
            "ActiveMatches aus MatchManager müssen der DB entsprechen.");

        // Logging
        TestContext.WriteLine($"Suggestions after: {suggestionsAfter.Count}");
        TestContext.WriteLine($"ActiveMatches manager: {string.Join(", ", managerIds)}");
        TestContext.WriteLine($"ActiveMatches db:      {string.Join(", ", dbIds)}");
    }

}