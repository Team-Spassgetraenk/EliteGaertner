using System;
using System.Collections.Generic;
using System.Linq;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using Contracts.Enumeration;
using DataManagement.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.UnitTests.AppLogicTests;

// Komplett von ChatGPT erstellt!
[TestClass]
[TestCategory("Unit")]
public class HarvestSuggestionTests
{
    private sealed class HarvestDbsFake : IHarvestDbs
    {
        private readonly IEnumerable<HarvestUploadDto> _result;

        public int? LastProfileId { get; private set; }
        public List<int>? LastTagIds { get; private set; }
        public HashSet<int>? LastAlreadyRatedProfiles { get; private set; }
        public int? LastPreloadCount { get; private set; }

        public HarvestDbsFake(IEnumerable<HarvestUploadDto> result)
            => _result = result;

        public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(int profileId, List<int> tagIds,
            HashSet<int> alreadyRatedProfiles, int preloadCount)
        {
            LastProfileId = profileId;
            LastTagIds = tagIds;
            LastAlreadyRatedProfiles = alreadyRatedProfiles;
            LastPreloadCount = preloadCount;
            return _result;
        }

        // --- Rest nur "Dummy", weil der Unit-Test sie nicht braucht ---
        public IEnumerable<HarvestUploadDto> GetProfileHarvestUploads(int profileId)
            => throw new NotImplementedException();

        public void CreateUploadDbs(HarvestUploadDto uploadDto)
            => throw new NotImplementedException();

        public HarvestUploadDto GetHarvestUploadDto(int uploadId)
            => throw new NotImplementedException();

        public void DeleteHarvestUpload(int uploadId)
            => throw new NotImplementedException();

        public void SetReportHarvestUpload(int uploadId, ReportReasons reason)
            => throw new NotImplementedException();

        public int GetReportCount(int uploadId)
            => 0;
    }

    [TestMethod]
    public void Ctor_ShouldLoadSuggestions_FromRepo()
    {
        // Arrange
        var expected = new[]
        {
            new HarvestUploadDto { UploadId = 1, ProfileId = 99 },
            new HarvestUploadDto { UploadId = 2, ProfileId = 88 },
        };

        var fakeRepo = new HarvestDbsFake(expected);
        var profileId = 10;
        var tagIds = new List<int> { 3, 6, 9 };
        var alreadyRatedProfiles = new HashSet<int> { 111, 222 };
        var preloadCount = 10;
        
        // Act
        var sut = new HarvestSuggestion(
            NullLogger<HarvestSuggestion>.Instance,
            fakeRepo,
            profileId,
            tagIds,
            alreadyRatedProfiles,
            preloadCount
        );
        var result = sut.GetHarvestSuggestionList();

        // Assert
        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(expected.Select(x => x.UploadId).ToList(),
                                  result.Select(x => x.UploadId).ToList());

        Assert.AreEqual(profileId, fakeRepo.LastProfileId);
        CollectionAssert.AreEqual(tagIds, fakeRepo.LastTagIds);
        CollectionAssert.AreEquivalent(alreadyRatedProfiles.ToList(), fakeRepo.LastAlreadyRatedProfiles!.ToList());
        Assert.AreEqual(preloadCount, fakeRepo.LastPreloadCount);
    }

    [TestMethod]
    public void Ctor_WhenRepoReturnsEmpty_ShouldReturnEmptyList()
    {
        // Arrange
        var fakeRepo = new HarvestDbsFake(Array.Empty<HarvestUploadDto>());

        // Act
        var sut = new HarvestSuggestion(
            NullLogger<HarvestSuggestion>.Instance,
            fakeRepo,
            1,
            new List<int> { 3 },
            new HashSet<int>(),
            10
        );
        var result = sut.GetHarvestSuggestionList();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
}