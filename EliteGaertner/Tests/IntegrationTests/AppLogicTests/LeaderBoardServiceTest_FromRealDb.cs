using System;
using System.Linq;
using AppLogic.Services;
using Contracts.Enumeration;
using DataManagement;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.IntegrationTests.AppLogicTests;

//Komplett durch ChatGPT generiert
[TestClass]
[DoNotParallelize]
[TestCategory("Integration")]
public class LeaderboardServiceTest_FromRealDb : IntegrationTestBase
{
    private static string ConnectionString =>
        // Falls du den Test-Postgres auf 5433 laufen lässt:
        // setze ENV TEST_DB_PORT=5433
        $"Host=localhost;Port={Environment.GetEnvironmentVariable("TEST_DB_PORT") ?? "5432"};" +
        "Database=elitegaertner_test;Username=postgres;Password=postgres";

    [TestMethod]
    public void GetLeaderBoardDto_MostLikes_ShouldMatchSeedExactly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var db = new EliteGaertnerDbContext(options);
        var loggerFactory = NullLoggerFactory.Instance;
        var dbs = new LeaderboardDbs(db, NullLogger<LeaderboardDbs>.Instance);

        // "Eingeloggter" User für PersonalEntry: beerenboss (hat im Seed 2 Likes)
        var myProfileId = db.Profiles
            .AsNoTracking()
            .Single(p => p.Username == "beerenboss")
            .Profileid;

        var sut = new LeaderboardService(
            leaderboardTitle: "Most Likes",
            leaderboardId: 1,
            profileId: myProfileId,
            tagId: null,
            goal: LeaderboardSearchGoal.MostLikes,
            leaderBoardDbs: dbs);

        // Act
        var dto = sut.GetLeaderBoardDto();
        var entries = dto.Entries;
        var personal = dto.PersonalEntry;

        // Assert (Seed-basiert)
        Assert.AreEqual(LeaderboardSearchGoal.MostLikes, dto.Goal);
        Assert.IsNull(dto.TagId);
        Assert.AreEqual(1, dto.LeaderboardId);

        // Erwartung direkt aus LeaderboardDbs ableiten (inkl. Tie-Break: Likes DESC, Username ASC)
        var expectedTop5 = dbs
            .GetLeaderBoardEntries(profileId: myProfileId, tagId: null, goal: LeaderboardSearchGoal.MostLikes)
            .ToList();

        Assert.AreEqual(expectedTop5.Count, entries.Count, "Top-List Count muss der LeaderboardDbs-Implementierung entsprechen.");

        for (var i = 0; i < entries.Count; i++)
        {
            Assert.AreEqual(expectedTop5[i].Rank, entries[i].Rank, $"Rank mismatch at index {i}.");
            Assert.AreEqual(expectedTop5[i].UserName, entries[i].UserName, $"UserName mismatch at index {i}.");
            Assert.AreEqual(expectedTop5[i].Value, entries[i].Value, $"Value mismatch at index {i}.");
        }

        // PersonalEntry Erwartung ebenfalls aus LeaderboardDbs ableiten
        var expectedPersonal = dbs.GetPersonalLeaderBoardEntry(myProfileId, tagId: null, goal: LeaderboardSearchGoal.MostLikes);

        Assert.IsNotNull(personal);
        Assert.AreEqual(expectedPersonal.Rank, personal!.Rank, "Personal Rank muss der LeaderboardDbs-Implementierung entsprechen.");
        Assert.AreEqual(expectedPersonal.UserName, personal.UserName, "Personal UserName muss der LeaderboardDbs-Implementierung entsprechen.");
        Assert.AreEqual(expectedPersonal.Value, personal.Value, "Personal Value muss der LeaderboardDbs-Implementierung entsprechen.");
    }

    [TestMethod]
    public void GetLeaderBoardDto_Heaviest_Trauben_ShouldMatchSeedExactly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var db = new EliteGaertnerDbContext(options);

        var myProfileId = db.Profiles
            .AsNoTracking()
            .Single(p => p.Username == "beerenboss")
            .Profileid;

        // TagId nicht hart coden: kann je nach bestehender DB / Sequence variieren
        var tagIdTrauben = db.Tags
            .AsNoTracking()
            .Single(t => t.Label == "Trauben")
            .Tagid;

        var goal = LeaderboardSearchGoal.Heaviest;

        var loggerFactory = NullLoggerFactory.Instance;
        var dbs = new LeaderboardDbs(db, NullLogger<LeaderboardDbs>.Instance);

        var sut = new LeaderboardService(
            leaderboardTitle: "Heaviest Trauben",
            leaderboardId: 2,
            profileId: myProfileId,
            tagId: tagIdTrauben,
            goal: goal,
            leaderBoardDbs: dbs);

        // Act
        var dto = sut.GetLeaderBoardDto();
        var entries = dto.Entries;
        var personal = dto.PersonalEntry;

        // Assert (Seed-basiert)
        Assert.AreEqual(goal, dto.Goal);
        Assert.AreEqual(tagIdTrauben, dto.TagId);
        Assert.AreEqual(2, dto.LeaderboardId);

        // Trauben-Uploads gibt es für: birnenbarde (820g max), traubentaktiker (700g max) und beerenboss (200g max)
        Assert.AreEqual(3, entries.Count, "Im Seed gibt es genau 3 Profile mit Trauben-Uploads.");

        Assert.AreEqual(1, entries[0].Rank);
        Assert.AreEqual("birnenbarde", entries[0].UserName);
        Assert.AreEqual(820f, entries[0].Value);

        Assert.AreEqual(2, entries[1].Rank);
        Assert.AreEqual("traubentaktiker", entries[1].UserName);
        Assert.AreEqual(700f, entries[1].Value);

        Assert.AreEqual(3, entries[2].Rank);
        Assert.AreEqual("beerenboss", entries[2].UserName);
        Assert.AreEqual(200f, entries[2].Value);

        // PersonalEntry: beerenboss hat bei Trauben max 200g und ist Rang 3
        Assert.IsNotNull(personal);
        Assert.AreEqual(3, personal!.Rank);
        Assert.AreEqual("beerenboss", personal.UserName);
        Assert.AreEqual(200f, personal.Value);
    }
}
