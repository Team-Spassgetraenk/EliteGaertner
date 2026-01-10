using System;
using System.Collections.Generic;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.UnitTests.AppLogicTests;

//Komplett von ChatGPT erstellt!!
[TestClass]
[TestCategory("Unit")]
public class LeaderboardServiceTests
{
    private sealed class FakeLeaderBoardDbs : ILeaderBoardDbs
    {
        public int? LastProfileId { get; private set; }
        public int? LastTagId { get; private set; }
        public LeaderboardSearchGoal LastGoal { get; private set; }

        public int? LastPersonalProfileId { get; private set; }
        public int? LastPersonalTagId { get; private set; }
        public LeaderboardSearchGoal LastPersonalGoal { get; private set; }

        private readonly IEnumerable<LeaderboardEntryDto> _returnEntries;
        private readonly LeaderboardEntryDto _personalEntry;

        public FakeLeaderBoardDbs(IEnumerable<LeaderboardEntryDto> returnEntries, LeaderboardEntryDto? personalEntry = null)
        {
            _returnEntries = returnEntries;
            _personalEntry = personalEntry ?? new LeaderboardEntryDto();
        }

        public IEnumerable<LeaderboardEntryDto> GetLeaderBoardEntries(int profileId, int? tagId, LeaderboardSearchGoal goal)
        {
            LastProfileId = profileId;
            LastTagId = tagId;
            LastGoal = goal;
            return _returnEntries;
        }

        public LeaderboardEntryDto GetPersonalLeaderBoardEntry(int profileId, int? tagId, LeaderboardSearchGoal goal)
        {
            LastPersonalProfileId = profileId;
            LastPersonalTagId = tagId;
            LastPersonalGoal = goal;
            return _personalEntry;
        }

        public bool CheckParametersForPersonal(int profileId, int? tagId, LeaderboardSearchGoal goal)
            => true;

        public bool CheckParametersForTopList(int? tagId, LeaderboardSearchGoal goal)
            => true;

        public IEnumerable<LeaderboardEntryDto> GetMostLikes()
            => throw new NotImplementedException();

        public IEnumerable<LeaderboardEntryDto> GetHarvestEntries(int tagId, LeaderboardSearchGoal goal)
            => throw new NotImplementedException();

        public LeaderboardEntryDto GetPersonalLikes(int profileId)
            => throw new NotImplementedException();

        public LeaderboardEntryDto GetPersonalHarvestEntry(int profileId, int tagId, LeaderboardSearchGoal goal)
            => throw new NotImplementedException();
    }

    [TestMethod]
    public void GetLeaderBoardDto_ShouldMapAllFields_AndMaterializeEntries()
    {
        // Arrange
        var title = "Top 10 Likes";
        var leaderboardId = 1;
        var profileId = 42;
        int? tagId = null;
        var goal = LeaderboardSearchGoal.MostLikes;

        var providedEnumerable = BuildYieldingEntries(); // liefert IEnumerable (kein List)
        var personal = new LeaderboardEntryDto
        {
            Rank = 7,
            ProfileId = profileId,
            UserName = "Me",
            Value = 2f
        };

        var fakeDbs = new FakeLeaderBoardDbs(providedEnumerable, personal);

        var sut = new LeaderboardService(title, leaderboardId, profileId, tagId, goal, fakeDbs);

        // Act
        var dto = sut.GetLeaderBoardDto();

        // Assert: DB wurde mit korrekten Parametern aufgerufen
        Assert.AreEqual(profileId, fakeDbs.LastProfileId);
        Assert.AreEqual(tagId, fakeDbs.LastTagId);
        Assert.AreEqual(goal, fakeDbs.LastGoal);

        Assert.AreEqual(profileId, fakeDbs.LastPersonalProfileId);
        Assert.AreEqual(tagId, fakeDbs.LastPersonalTagId);
        Assert.AreEqual(goal, fakeDbs.LastPersonalGoal);

        // Assert: Felder korrekt gemappt
        Assert.AreEqual(title, dto.LeaderboardTitle);
        Assert.AreEqual(leaderboardId, dto.LeaderboardId);
        Assert.AreEqual(tagId, dto.TagId);
        Assert.AreEqual(goal, dto.Goal);

        Assert.IsNotNull(dto.PersonalEntry);
        Assert.AreEqual(7, dto.PersonalEntry.Rank);
        Assert.AreEqual(profileId, dto.PersonalEntry.ProfileId);
        Assert.AreEqual("Me", dto.PersonalEntry.UserName);
        Assert.AreEqual(2f, dto.PersonalEntry.Value);

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