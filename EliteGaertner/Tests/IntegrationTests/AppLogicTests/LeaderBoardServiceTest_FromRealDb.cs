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
public class LeaderboardServiceTest_FromRealDb
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

        var sut = new LeaderboardService(
            leaderboardTitle: "Most Likes",
            tagId: null,
            goal: LeaderboardSearchGoal.MostLikes,
            leaderBoardDbs: dbs);

        // Act
        var dto = sut.GetLeaderBoardDto();
        var entries = dto.Entries;

        // Assert (Seed-basiert)
        Assert.AreEqual(LeaderboardSearchGoal.MostLikes, dto.Goal);
        Assert.IsNull(dto.TagId);

        // beerenboss hat 2 Likes; mit 1 Like (alphabetisch): apfelalchemist, gurkenguru, melonenmaster, tomatentiger, zucchinizauberer
        Assert.AreEqual(6, entries.Count, "Im Seed gibt es genau 6 Creator mit positiven Likes.");

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

        Assert.AreEqual(6, entries[5].Rank);
        Assert.AreEqual("zucchinizauberer", entries[5].UserName);
        Assert.AreEqual(1f, entries[5].Value);
    }

    [TestMethod]
    public void GetLeaderBoardDto_Heaviest_Trauben_ShouldMatchSeedExactly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var db = new EliteGaertnerDbContext(options);

        // TagId nicht hart coden: kann je nach bestehender DB / Sequence variieren
        var tagIdTrauben = db.Tags
            .AsNoTracking()
            .Single(t => t.Label == "Trauben")
            .Tagid;

        var goal = LeaderboardSearchGoal.Heaviest;

        var dbs = new LeaderboardDbs(db);

        var sut = new LeaderboardService(
            leaderboardTitle: "Heaviest Trauben",
            tagId: tagIdTrauben,
            goal: goal,
            leaderBoardDbs: dbs);

        // Act
        var dto = sut.GetLeaderBoardDto();
        var entries = dto.Entries;

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
    }
}