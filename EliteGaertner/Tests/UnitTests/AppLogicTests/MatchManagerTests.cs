using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using Contracts.Enumeration;

namespace Tests.UnitTests.AppLogicTests;

// Komplett durch ChatGPT generiert!!!!!!
[TestClass]
[TestCategory("Unit")]
public class MatchManagerTests
{
    // --------------------------------------------------
    // TEST 1
    // --------------------------------------------------
    [TestMethod]
    public void RateUser_ShouldRemoveSuggestion_AndNotReAdd_WhenNoNewSuggestionsExist()
    {
        // Arrange
        var receiver = CreateReceiver(profileId: 1);

        var matchesDbs = new MatchesDbsFake(activeMatchesSequence: new[]
        {
            new List<PublicProfileDto>(), // ctor
            new List<PublicProfileDto>    // nach RateUser
            {
                new PublicProfileDto { ProfileId = 2, UserName = "User2" }
            }
        });

        var profileDbs = new ProfileDbsFake();

        // Harvest liefert beim ctor 1 Upload, danach NICHTS mehr
        var harvestDbs = new HarvestDbsFake(new[]
        {
            new List<HarvestUploadDto>
            {
                new HarvestUploadDto { UploadId = 100, ProfileId = 2 }
            },
            new List<HarvestUploadDto>() // AddSuggestions()
        });

        var sut = new MatchManager(matchesDbs, profileDbs, harvestDbs, receiver, preloadCount: 10);

        var creator = profileDbs.GetPublicProfile(2);

        Assert.AreEqual(1, sut.GetProfileSuggestionList().Count);

        // Act
        sut.RateUser(creator, true);

        Assert.AreEqual(1, matchesDbs.SaveCalls.Count);
        Assert.AreEqual(1, matchesDbs.SaveCalls[0].ContentReceiver);
        Assert.AreEqual(2, matchesDbs.SaveCalls[0].ContentCreator);
        Assert.AreEqual(true, matchesDbs.SaveCalls[0].ContentReceiverValue);
        Assert.IsNotNull(matchesDbs.SaveCalls[0].ContentReceiverRatingDate);

        // Assert: Creator bleibt entfernt
        Assert.IsFalse(
            sut.GetProfileSuggestionList().Any(x => x.Key.ProfileId == 2),
            "Creator darf nach Bewertung nicht erneut vorgeschlagen werden");

        // ActiveMatches aktualisiert
        Assert.AreEqual(1, sut.GetActiveMatches().Count);
        Assert.AreEqual(2, sut.GetActiveMatches()[0].ProfileId);
    }

    // --------------------------------------------------
    // TEST 2
    // --------------------------------------------------
    [TestMethod]
    public void RateUser_ShouldRemoveSuggestion_AndAddNewSuggestion_WhenAvailable()
    {
        // Arrange
        var receiver = CreateReceiver(profileId: 1);

        var matchesDbs = new MatchesDbsFake(activeMatchesSequence: new[]
        {
            new List<PublicProfileDto>(), // ctor
            new List<PublicProfileDto>()  // nach RateUser (irrelevant für diesen Test)
        });

        var profileDbs = new ProfileDbsFake();

        // 1. Call → Creator 2
        // 2. Call → neuer Creator 3
        var harvestDbs = new HarvestDbsFake(new[]
        {
            new List<HarvestUploadDto>
            {
                new HarvestUploadDto { UploadId = 100, ProfileId = 2 }
            },
            new List<HarvestUploadDto>
            {
                new HarvestUploadDto { UploadId = 200, ProfileId = 3 }
            }
        });

        var sut = new MatchManager(matchesDbs, profileDbs, harvestDbs, receiver, preloadCount: 10);

        var creator2 = profileDbs.GetPublicProfile(2);

        Assert.AreEqual(1, sut.GetProfileSuggestionList().Count);

        // Act
        sut.RateUser(creator2, true);

        Assert.AreEqual(1, matchesDbs.SaveCalls.Count);
        Assert.AreEqual(1, matchesDbs.SaveCalls[0].ContentReceiver);
        Assert.AreEqual(2, matchesDbs.SaveCalls[0].ContentCreator);
        Assert.AreEqual(true, matchesDbs.SaveCalls[0].ContentReceiverValue);
        Assert.IsNotNull(matchesDbs.SaveCalls[0].ContentReceiverRatingDate);

        // Assert: alter Creator weg
        Assert.IsFalse(
            sut.GetProfileSuggestionList().Any(x => x.Key.ProfileId == 2));

        // Assert: neuer Creator drin
        Assert.IsTrue(
            sut.GetProfileSuggestionList().Any(x => x.Key.ProfileId == 3),
            "Neuer Creator muss nach AddSuggestions auftauchen");
    }

    // --------------------------------------------------
    // TEST 3
    // --------------------------------------------------
    [TestMethod]
    public void ReportHarvestUpload_ShouldDeleteUpload_WhenReportThresholdReached()
    {
        // Arrange
        var receiver = CreateReceiver(profileId: 1);

        var matchesDbs = new MatchesDbsFake(activeMatchesSequence: new[]
        {
            new List<PublicProfileDto>() // ctor
        });

        var profileDbs = new ProfileDbsFake();

        // Für diesen Test egal, aber ctor braucht mindestens eine Sequenz
        var harvestDbs = new HarvestDbsFake(new[]
        {
            new List<HarvestUploadDto>()
        })
        {
            ReportCountToReturn = 5
        };

        var sut = new MatchManager(matchesDbs, profileDbs, harvestDbs, receiver, preloadCount: 10);

        // Act
        sut.ReportHarvestUpload(uploadId: 123, reason: ReportReasons.Spam);

        // Assert
        Assert.AreEqual(1, harvestDbs.SetReportCalls.Count, "SetReportHarvestUpload muss genau 1x aufgerufen werden.");
        Assert.AreEqual(123, harvestDbs.SetReportCalls[0].uploadId);
        Assert.AreEqual(ReportReasons.Spam, harvestDbs.SetReportCalls[0].reason);

        Assert.AreEqual(1, harvestDbs.DeleteCalls.Count, "Bei >= 5 Reports muss das Upload gelöscht werden.");
        Assert.AreEqual(123, harvestDbs.DeleteCalls[0]);
    }

    // --------------------------------------------------
    // TEST 4
    // --------------------------------------------------
    [TestMethod]
    public void ReportHarvestUpload_ShouldNotDeleteUpload_WhenBelowThreshold()
    {
        // Arrange
        var receiver = CreateReceiver(profileId: 1);

        var matchesDbs = new MatchesDbsFake(activeMatchesSequence: new[]
        {
            new List<PublicProfileDto>() // ctor
        });

        var profileDbs = new ProfileDbsFake();

        var harvestDbs = new HarvestDbsFake(new[]
        {
            new List<HarvestUploadDto>()
        })
        {
            ReportCountToReturn = 4
        };

        var sut = new MatchManager(matchesDbs, profileDbs, harvestDbs, receiver, preloadCount: 10);

        // Act
        sut.ReportHarvestUpload(uploadId: 456, reason: ReportReasons.CatFishing);

        // Assert
        Assert.AreEqual(1, harvestDbs.SetReportCalls.Count, "SetReportHarvestUpload muss genau 1x aufgerufen werden.");
        Assert.AreEqual(456, harvestDbs.SetReportCalls[0].uploadId);
        Assert.AreEqual(ReportReasons.CatFishing, harvestDbs.SetReportCalls[0].reason);

        Assert.AreEqual(0, harvestDbs.DeleteCalls.Count, "Bei < 5 Reports darf NICHT gelöscht werden.");
    }

    // --------------------------------------------------
    // HELPERS
    // --------------------------------------------------
    private static PrivateProfileDto CreateReceiver(int profileId)
        => new()
        {
            ProfileId = profileId,
            PreferenceDtos = new List<PreferenceDto>
            {
                new PreferenceDto { Profileid = profileId, TagId = 1 }
            }
        };

    // --------------------------------------------------
    // FAKES
    // --------------------------------------------------

    private sealed class MatchesDbsFake : IMatchesDbs
    {
        private readonly Queue<List<PublicProfileDto>> _queue;
        private readonly HashSet<(int receiverId, int creatorId)> _ratedPairs = new();

        public List<RateDto> SaveCalls { get; } = new();

        public MatchesDbsFake(IEnumerable<List<PublicProfileDto>> activeMatchesSequence)
            => _queue = new Queue<List<PublicProfileDto>>(activeMatchesSequence);

        public IEnumerable<PublicProfileDto> GetActiveMatches(int profileIdReceiver)
            => _queue.Count > 0 ? _queue.Dequeue() : Enumerable.Empty<PublicProfileDto>();

        public void SaveMatchInfo(RateDto matchDto)
        {
            SaveCalls.Add(matchDto);

            // Sobald gespeichert wurde, gilt das Paar als "bereits bewertet"
            _ratedPairs.Add((matchDto.ContentReceiver, matchDto.ContentCreator));
        }

        public bool ProfileAlreadyRated(int profileIdReceiver, int profileIdCreator)
            => _ratedPairs.Contains((profileIdReceiver, profileIdCreator));
    }

    private sealed class ProfileDbsFake : IProfileDbs
    {
        public bool CheckUsernameExists(string username)
            => throw new NotImplementedException();

        public PrivateProfileDto SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials)
            => throw new NotImplementedException();

        public PrivateProfileDto SetNewProfile(PrivateProfileDto privateProfile)
            => throw new NotImplementedException();

        public PrivateProfileDto EditProfile(PrivateProfileDto privateProfile)
            => throw new NotImplementedException();

        public DataManagement.Entities.Profile? GetProfile(int profileId)
            => null;

        public PrivateProfileDto GetPrivateProfile(int profileId)
            => throw new NotImplementedException();

        public PublicProfileDto GetPublicProfile(int profileId)
            => new()
            {
                ProfileId = profileId,
                UserName = $"User{profileId}"
            };

        public bool UpdateContactVisibility(ContactVisibilityDto dto)
            => throw new NotImplementedException();

        public int? CheckPassword(string eMail, string passwordHash)
            => throw new NotImplementedException();

        public IEnumerable<PreferenceDto> GetUserPreference(int profileId)
            => throw new NotImplementedException();

        public bool SetUserPreference(List<PreferenceDto> preferences)
            => throw new NotImplementedException();
    }

    private sealed class HarvestDbsFake : IHarvestDbs
    {
        private readonly Queue<List<HarvestUploadDto>> _queue;

        public int ReportCountToReturn { get; set; } = 0;

        public List<(int uploadId, ReportReasons reason)> SetReportCalls { get; } = new();
        public List<int> DeleteCalls { get; } = new();

        public HarvestDbsFake(IEnumerable<List<HarvestUploadDto>> sequence)
            => _queue = new Queue<List<HarvestUploadDto>>(sequence);

        public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount)
            => _queue.Count > 0 ? _queue.Dequeue() : Enumerable.Empty<HarvestUploadDto>();

        public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId)
            => Enumerable.Empty<HarvestUploadDto>();

        public bool DeleteHarvestUpload(int uploadId)
        {
            DeleteCalls.Add(uploadId);
            return true;
        }

        public void SetReportHarvestUpload(int uploadId, ReportReasons reason)
            => SetReportCalls.Add((uploadId, reason));

        public int GetReportCount(int uploadId) => ReportCountToReturn;

        public bool CreateUploadDbs(HarvestUploadDto uploadDto)
            => throw new NotImplementedException();

        public HarvestUploadDto GetUploadDb(int uploadId)
            => throw new NotImplementedException();
    }
}