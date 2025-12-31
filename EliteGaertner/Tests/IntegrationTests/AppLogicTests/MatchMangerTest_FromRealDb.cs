using System;
using System.Collections.Generic;
using System.Linq;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement;
using DataManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.IntegrationTests.AppLogicTests;

//Durch ChatGPT generiert!!!
[TestClass]
[DoNotParallelize]
[TestCategory("Integration")]
public class MatchManagerTest_FromRealDb : IntegrationTestBase
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

        var matchesDbs = new MatchesDbs(db);
        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);

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
        
        var manager = new MatchManager(matchesDbs, profileDbs, harvestDbs, receiver);

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
        var activeMatchesFromDb = matchesDbs.GetActiveMatches(receiver.ProfileId).ToList();

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

    [TestMethod]
    public void ReportHarvestUpload_ShouldIncreaseReportCount_AndNotDeleteBeforeThreshold()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var db = new EliteGaertnerDbContext(options);
        var matchesDbs = new MatchesDbs(db);
        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);

        var receiver = new PrivateProfileDto
        {
            ProfileId = 1,
            PreferenceDtos = new List<PreferenceDto>
            {
                new PreferenceDto { TagId = 3,  Profileid = 1 },
            }
        };

        var manager = new MatchManager(matchesDbs, profileDbs, harvestDbs, receiver);

        var uploadId = CreateTestUpload(db, profileId: 1);

        // Act (4 reports < threshold)
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);

        // Assert
        Assert.IsTrue(db.Harvestuploads.AsNoTracking().Any(h => h.Uploadid == uploadId),
            "Der Upload darf vor Erreichen des Report-Thresholds nicht gelöscht werden.");

        var count = harvestDbs.GetReportCount(uploadId);
        Assert.AreEqual(4, count, "Es müssen genau 4 Reports gezählt werden.");
    }

    [TestMethod]
    public void ReportHarvestUpload_ShouldDeleteUpload_WhenThresholdReached()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var db = new EliteGaertnerDbContext(options);
        var matchesDbs = new MatchesDbs(db);
        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);

        var receiver = new PrivateProfileDto
        {
            ProfileId = 1,
            PreferenceDtos = new List<PreferenceDto>
            {
                new PreferenceDto { TagId = 3,  Profileid = 1 },
            }
        };

        var manager = new MatchManager(matchesDbs, profileDbs, harvestDbs, receiver);

        var uploadId = CreateTestUpload(db, profileId: 1);

        // Act (5 reports == threshold)
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);
        manager.ReportHarvestUpload(uploadId, ReportReasons.Spam);

        // Assert
        Assert.IsFalse(db.Harvestuploads.AsNoTracking().Any(h => h.Uploadid == uploadId),
            "Der Upload muss gelöscht werden, sobald der Report-Threshold erreicht ist.");
    }

    private static int CreateTestUpload(EliteGaertnerDbContext db, int profileId)
    {
        var upload = new Harvestupload
        {
            Imageurl = "/uploads/test_report.jpg",
            Description = "IntegrationTest Upload for ReportHarvestUpload",
            Weightgramm = 123,
            Widthcm = 4,
            Lengthcm = 5,
            Uploaddate = DateTime.UtcNow,
            Profileid = profileId
        };

        db.Harvestuploads.Add(upload);
        db.SaveChanges();
        return upload.Uploadid;
    }
}