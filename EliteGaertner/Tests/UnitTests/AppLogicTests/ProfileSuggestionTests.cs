using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using Contracts.Enumeration;

namespace Tests.UnitTests.AppLogicTests;

//Komplett durch ChatGPT generiert!!!
[TestClass]
[TestCategory("Unit")]
public class ProfileSuggestionTests
{
    [TestMethod]
    public void CreateProfileSuggestions_FiltersAlreadyRatedProfiles()
    {
        // Arrange
        int receiverId = 1;

        var harvestUploads = new List<HarvestUploadDto>
        {
            new HarvestUploadDto { UploadId = 1, ProfileId = 2 },
            new HarvestUploadDto { UploadId = 2, ProfileId = 3 }
        };

        var harvestDbs = new HarvestDbsFake(harvestUploads);

        // Profil 2 gilt als bereits bewertet und muss deshalb herausgefiltert werden
        var matchesDbs = new MatchesDbsFake(
            ratedPairs: new[] { (receiverId, 2) }
        );

        var profileDbs = new ProfileDbsFake();

        var sut = new ProfileSuggestion(
            matchesDbs,
            profileDbs,
            harvestDbs,
            receiverId,
            tagIds: new List<int> { 1, 2 },
            preloadCount: 10
        );

        // Act
        var result = sut.GetProfileSuggestionList();

        // Assert
        Assert.AreEqual(1, result.Count);

        var entry = result.Single();
        Assert.AreEqual(3, entry.Key.ProfileId);
        Assert.AreEqual(2, entry.Value.UploadId);
    }

    // ============================
    // ======== FAKES =============
    // ============================

    private sealed class HarvestDbsFake : IHarvestDbs
    {
        private readonly List<HarvestUploadDto> _uploads;

        public HarvestDbsFake(IEnumerable<HarvestUploadDto> uploads)
            => _uploads = uploads.ToList();

        public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds,
            HashSet<int> alreadyRatedProfiles, int preloadCount)
            => _uploads;

        public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId)
            => Enumerable.Empty<HarvestUploadDto>();

        public void CreateUploadDbs(HarvestUploadDto uploadDto)
            => throw new NotImplementedException();

        public HarvestUploadDto GetUploadDb(int uploadId)
            => throw new NotImplementedException();

        public void DeleteHarvestUpload(int uploadId)
        {
            return;
        }

        public void SetReportHarvestUpload(int uploadId, ReportReasons reason) { }
        public int GetReportCount(int uploadId) => 0;
    }

    private sealed class MatchesDbsFake : IMatchesDbs
    {
        private readonly HashSet<(int receiver, int creator)> _ratedPairs;

        public MatchesDbsFake(IEnumerable<(int receiver, int creator)> ratedPairs)
            => _ratedPairs = ratedPairs.ToHashSet();

        public bool ProfileAlreadyRated(int profileIdReceiver, int profileIdCreator)
            => _ratedPairs.Contains((profileIdReceiver, profileIdCreator));

        public IEnumerable<PublicProfileDto> GetActiveMatches(int profileId)
            => Enumerable.Empty<PublicProfileDto>();

        public void SaveMatchInfo(RateDto matchDto) { }
    }

    private sealed class ProfileDbsFake : IProfileDbs
    {
        public bool CheckUsernameExists(string username)
            => throw new NotImplementedException();

        public void SetNewProfile(PrivateProfileDto privateProfile, CredentialProfileDto credentials)
            => throw new NotImplementedException();

        public PrivateProfileDto SetNewProfile(PrivateProfileDto privateProfile)
            => throw new NotImplementedException();

        public PrivateProfileDto EditProfile(PrivateProfileDto privateProfile)
            => throw new NotImplementedException();

        public void EditPassword(CredentialProfileDto credentials)
        {
            throw new NotImplementedException();
        }

        public DataManagement.Entities.Profile? GetProfile(int profileId)
            => null; // im ProfileSuggestion-Test nicht benötigt

        public PrivateProfileDto GetPrivateProfile(int profileId)
            => throw new NotImplementedException();

        public PublicProfileDto GetPublicProfile(int profileId)
            => new PublicProfileDto
            {
                ProfileId = profileId,
                UserName = $"User{profileId}",
                EMail = null,
                Phonenumber = null
            };

        public bool UpdateContactVisibility(ContactVisibilityDto dto)
            => throw new NotImplementedException();

        public IEnumerable<PreferenceDto> GetUserPreference(int profileId)
            => throw new NotImplementedException();

        public bool SetUserPreference(List<PreferenceDto> preferences)
            => throw new NotImplementedException();

        public int? CheckPassword(string eMail, string passwordHash)
            => throw new NotImplementedException();
    }
}