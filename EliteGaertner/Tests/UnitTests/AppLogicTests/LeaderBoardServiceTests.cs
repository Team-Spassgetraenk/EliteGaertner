using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.UnitTests.AppLogicTests;

//Komplett von ChatGPT erstellt!!
[TestClass]
public class LeaderboardServiceTests
{
    private sealed class FakeLeaderBoardDbs : ILeaderBoardDbs
    {
        public int? LastTagId { get; private set; }
        public LeaderboardSearchGoal LastGoal { get; private set; }

        private readonly IEnumerable<LeaderboardEntryDto> _returnEntries;

        public FakeLeaderBoardDbs(IEnumerable<LeaderboardEntryDto> returnEntries)
        {
            _returnEntries = returnEntries;
        }

        public IEnumerable<LeaderboardEntryDto> GetLeaderBoardEntries(int? tagId, LeaderboardSearchGoal goal)
        {
            LastTagId = tagId;
            LastGoal = goal;
            return _returnEntries;
        }

        public IEnumerable<LeaderboardEntryDto> GetMostLikes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LeaderboardEntryDto> GetHarvestEntries(int tagId, LeaderboardSearchGoal goal)
        {
            throw new NotImplementedException();
        }
    }

    [TestMethod]
    public void GetLeaderBoardDto_ShouldMapAllFields_AndMaterializeEntries()
    {
        // Arrange
        var title = "Top 10 Likes";
        int? tagId = null;
        var goal = LeaderboardSearchGoal.MostLikes;

        var providedEnumerable = BuildYieldingEntries(); // liefert IEnumerable (kein List)
        var fakeDbs = new FakeLeaderBoardDbs(providedEnumerable);

        var sut = new LeaderboardService(title, tagId, goal, fakeDbs);

        // Act
        var dto = sut.GetLeaderBoardDto();

        // Assert: DB wurde mit korrekten Parametern aufgerufen
        Assert.AreEqual(tagId, fakeDbs.LastTagId);
        Assert.AreEqual(goal, fakeDbs.LastGoal);

        // Assert: Felder korrekt gemappt
        Assert.AreEqual(title, dto.LeaderboardTitle);
        Assert.AreEqual(tagId, dto.TagId);
        Assert.AreEqual(goal, dto.Goal);

        // Assert: Entries sind materialisiert als IReadOnlyList (ToList im Service)
        Assert.IsNotNull(dto.Entries);
        Assert.AreEqual(2, dto.Entries.Count);

        Assert.AreEqual(1, dto.Entries[0].Rank);
        Assert.AreEqual(10, dto.Entries[0].ProfileId);
        Assert.AreEqual("Alice", dto.Entries[0].UserName);
        Assert.AreEqual(5f, dto.Entries[0].Value);

        Assert.AreEqual(2, dto.Entries[1].Rank);
        Assert.AreEqual(11, dto.Entries[1].ProfileId);
        Assert.AreEqual("Bob", dto.Entries[1].UserName);
        Assert.AreEqual(3f, dto.Entries[1].Value);
    }

    private static IEnumerable<LeaderboardEntryDto> BuildYieldingEntries()
    {
        // bewusst yield, damit klar ist: Service materialisiert via ToList()
        yield return new LeaderboardEntryDto
        {
            Rank = 1,
            ProfileId = 10,
            UserName = "Alice",
            Value = 5f
        };
        yield return new LeaderboardEntryDto
        {
            Rank = 2,
            ProfileId = 11,
            UserName = "Bob",
            Value = 3f
        };
    }
}