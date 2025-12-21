using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Tests.UnitTests.AppLogicTests
{
    [TestClass]
    public class HarvestSuggestionTests
    {
        // Fake Repo für Variante B
        private sealed class FakeHarvestDbs : IHarvestDbs
        {
            public int LastProfileId { get; private set; }
            public List<int> LastTagIds { get; private set; } = new();
            public int LastPreloadCount { get; private set; }

            private readonly List<HarvestUploadDto> _data;

            public FakeHarvestDbs(IEnumerable<HarvestUploadDto>? data = null)
            {
                _data = data?.ToList() ?? new List<HarvestUploadDto>();
            }

            public IEnumerable<HarvestUploadDto> GetHarvestUploadRepo(
                int profileId,
                List<int> tagIds,
                int preloadCount)
            {
                LastProfileId = profileId;
                LastTagIds = tagIds;
                LastPreloadCount = preloadCount;

                return _data.Take(preloadCount);
            }
        }

        [TestMethod]
        public void Ctor_WhenRepoReturnsEmpty_ResultIsEmpty()
        {
            // Arrange
            var repo = new FakeHarvestDbs();

            var profileId = 1;
            var tagIds = new List<int> { 3, 6, 9 };
            var preloadCount = 10;

            // Act
            var sut = new AppLogic.Services.HarvestSuggestion(
                repo,
                profileId,
                tagIds,
                preloadCount);

            var result = sut.GetHarvestSuggestionList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Ctor_WhenRepoReturnsMoreThanPreloadCount_ResultIsLimited()
        {
            // Arrange
            var data = Enumerable.Range(1, 50)
                .Select(i => new HarvestUploadDto
                {
                    UploadId = i,
                    ProfileId = 100 + i
                })
                .ToList();

            var repo = new FakeHarvestDbs(data);

            var profileId = 1;
            var tagIds = new List<int> { 3, 6, 9 };
            var preloadCount = 10;

            // Act
            var sut = new AppLogic.Services.HarvestSuggestion(
                repo,
                profileId,
                tagIds,
                preloadCount);

            var result = sut.GetHarvestSuggestionList();

            // Assert
            Assert.AreEqual(preloadCount, result.Count);
        }

        [TestMethod]
        public void Ctor_PassesCorrectArgumentsToRepo()
        {
            // Arrange
            var repo = new FakeHarvestDbs(new[]
            {
                new HarvestUploadDto { UploadId = 1, ProfileId = 2 }
            });

            var profileId = 42;
            var tagIds = new List<int> { 3, 6, 6, 9 };
            var preloadCount = 5;

            // Act
            _ = new AppLogic.Services.HarvestSuggestion(
                repo,
                profileId,
                tagIds,
                preloadCount);

            // Assert
            Assert.AreEqual(profileId, repo.LastProfileId);
            CollectionAssert.AreEquivalent(
                new List<int> { 3, 6, 9 },
                repo.LastTagIds,
                "TagIds müssen distinct übergeben werden");
            Assert.AreEqual(preloadCount, repo.LastPreloadCount);
        }
    }
}