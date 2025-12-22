using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.UnitTests.AppLogicTests;

//Komplett durch ChatGPT generiert!!!!!!
[TestClass]
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

        // Assert: alter Creator weg
        Assert.IsFalse(
            sut.GetProfileSuggestionList().Any(x => x.Key.ProfileId == 2));

        // Assert: neuer Creator drin
        Assert.IsTrue(
            sut.GetProfileSuggestionList().Any(x => x.Key.ProfileId == 3),
            "Neuer Creator muss nach AddSuggestions auftauchen");
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

        public List<MatchDto> SaveCalls { get; } = new();

        public MatchesDbsFake(IEnumerable<List<PublicProfileDto>> activeMatchesSequence)
            => _queue = new Queue<List<PublicProfileDto>>(activeMatchesSequence);

        public MatchDto GetMatchInfo(int profileIdReceiver, int profileIdCreator)
            => new MatchDto();

        public IEnumerable<PublicProfileDto> GetActiveMatches(int profileIdReceiver)
            => _queue.Count > 0 ? _queue.Dequeue() : Enumerable.Empty<PublicProfileDto>();

        public void SaveMatchInfo(MatchDto matchDto)
            => SaveCalls.Add(matchDto);
    }

    private sealed class ProfileDbsFake : IProfileDbs
    {
        public PrivateProfileDto SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials)
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

        public int? CheckPassword(string eMail, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class HarvestDbsFake : IHarvestDbs
    {
        private readonly Queue<List<HarvestUploadDto>> _queue;

        public HarvestDbsFake(IEnumerable<List<HarvestUploadDto>> sequence)
            => _queue = new Queue<List<HarvestUploadDto>>(sequence);

        public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds, int preloadCount)
            => _queue.Count > 0 ? _queue.Dequeue() : Enumerable.Empty<HarvestUploadDto>();

        public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId)
            => Enumerable.Empty<HarvestUploadDto>();

        public void SetHarvestUpload(HarvestUploadDto harvestUpload) { }
        public void DeleteHarvestUpload(int uploadId) { }
        public void SetReportHarvestUpload(int uploadId, Enum reason) { }
        public IEnumerable<ReportDto> GetReportHarvestUpload(int uploadId)
            => Enumerable.Empty<ReportDto>();
    }
}