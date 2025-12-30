using System;
using System.Linq;
using AppLogic.Services;
using Contracts.Enumeration;
using DataManagement;
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
        var dbs = new LeaderboardDbs(db);

        // "Eingeloggter" User für PersonalEntry: beerenboss (hat im Seed 2 Likes)
        var myProfileId = db.Profiles
            .AsNoTracking()
            .Single(p => p.Username == "beerenboss")
            .Profileid;

        var sut = new LeaderboardService(
            leaderboardTitle: "Most Likes",
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

        // beerenboss hat 2 Likes; mit 1 Like (alphabetisch): apfelalchemist, gurkenguru, melonenmaster, tomatentiger, zucchinizauberer
        Assert.AreEqual(5, entries.Count, "Im Seed gibt es 6 Creator mit positiven Likes, aber das Leaderboard liefert Top 5.");

        Assert.AreEqual(1, entries[0].Rank);
        Assert.AreEqual("beerenboss", entries[0].UserName);
        Assert.AreEqual(2f, entries[0].Value);

        Assert.AreEqual(2, entries[1].Rank);
        Assert.AreEqual("apfelalchemist", entries[1].UserName);
        Assert.AreEqual(1f, entries[1].Value);

        Assert.AreEqual(3, entries[2].Rank);
        Assert.AreEqual("gurkenguru", entries[2].UserName);
        Assert.AreEqual(1f, entries[2].Value);

        Assert.AreEqual(4, entries[3].Rank);
        Assert.AreEqual("melonenmaster", entries[3].UserName);
        Assert.AreEqual(1f, entries[3].Value);

        Assert.AreEqual(5, entries[4].Rank);
        Assert.AreEqual("tomatentiger", entries[4].UserName);
        Assert.AreEqual(1f, entries[4].Value);

        // PersonalEntry (eingeloggter User)
        Assert.IsNotNull(personal);
        Assert.AreEqual(1, personal!.Rank);
        Assert.AreEqual("beerenboss", personal.UserName);
        Assert.AreEqual(2f, personal.Value);
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

        var dbs = new LeaderboardDbs(db);

        var sut = new LeaderboardService(
            leaderboardTitle: "Heaviest Trauben",
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

        // Trauben-Uploads gibt es für: beerenboss (200g max) und traubentaktiker (650g max)
        Assert.AreEqual(2, entries.Count, "Im Seed gibt es genau 2 Profile mit Trauben-Uploads.");

        Assert.AreEqual(1, entries[0].Rank);
        Assert.AreEqual("traubentaktiker", entries[0].UserName);
        Assert.AreEqual(650f, entries[0].Value);

        Assert.AreEqual(2, entries[1].Rank);
        Assert.AreEqual("beerenboss", entries[1].UserName);
        Assert.AreEqual(200f, entries[1].Value);

        // PersonalEntry: beerenboss hat bei Trauben max 200g und ist Rang 2
        Assert.IsNotNull(personal);
        Assert.AreEqual(2, personal!.Rank);
        Assert.AreEqual("beerenboss", personal.UserName);
        Assert.AreEqual(200f, personal.Value);
    }
}
