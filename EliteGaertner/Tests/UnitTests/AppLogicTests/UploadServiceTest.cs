using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AppLogic.Services;
using DataManagement.Interfaces;
using Contracts.Data_Transfer_Objects;
using System;

namespace  Tests.UnitTests.AppLogicTests
{
    [TestClass]
    public class UploadServiceImplTests
    {
        private Mock<IHarvestDbs> _mockHarvestDbs;
        private UploadServiceImpl _uploadService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockHarvestDbs = new Mock<IHarvestDbs>();
            _uploadService = new UploadServiceImpl(_mockHarvestDbs.Object);
        }

        [TestMethod]
        public void CreateHarvestUpload_aus_Dto_Wahr()
        {
            // Arrange Testdaten erstellen
            var uploadDto = new HarvestUploadDto
            {
                ProfileId = 1,
                ImageUrl = "test.jpg",
                Description = "Test upload",
                WeightGram = 500,
                WidthCm = 10,
                LengthCm = 20
            };
            _mockHarvestDbs.Setup(x => x.CreateUploadDbs(uploadDto)).Returns(true);

            // Act = Aufruf
            bool result = _uploadService.CreateHarvestUpload(uploadDto);

            // Assert
            Assert.IsTrue(result);
            _mockHarvestDbs.Verify(x => x.CreateUploadDbs(uploadDto), Times.Once);
        }

        [TestMethod]
        public void CreateHarvestUpload_aus_Dto_ReturnsFalse()
        {
            // Arrange
            var uploadDto = new HarvestUploadDto();
            _mockHarvestDbs.Setup(x => x.CreateUploadDbs(uploadDto)).Returns(false);

            // Act
            bool result = _uploadService.CreateHarvestUpload(uploadDto);

            // Assert
            Assert.IsFalse(result);
            _mockHarvestDbs.Verify(x => x.CreateUploadDbs(uploadDto), Times.Once);
        }

        [TestMethod]
        public void CreateHarvestUpload_aus_Paramatern_Wahr()
        {
            // Arrange
            int userId = 1;
            string imageUrl = "test.jpg";
            string description = "Test upload";
            float weight = 500.5f;
            int width = 10;
            int length = 20;

            var expectedDto = new HarvestUploadDto
            {
                ProfileId = userId,
                ImageUrl = imageUrl,
                Description = description,
                WeightGram = weight,
                WidthCm = width,
                LengthCm = length,
                UploadDate = DateTime.UtcNow
            };

            _mockHarvestDbs.Setup(x => x.CreateUploadDbs(It.Is<HarvestUploadDto>(dto => 
                dto.ProfileId == userId &&
                dto.ImageUrl == imageUrl &&
                dto.Description == description &&
                dto.WeightGram == weight &&
                dto.WidthCm == width &&
                dto.LengthCm == length))).Returns(true);

            // Act
            bool result = _uploadService.CreateHarvestUpload(userId, imageUrl, description, weight, width, length);

            // Assert
            Assert.IsTrue(result);
            _mockHarvestDbs.Verify(x => x.CreateUploadDbs(It.IsAny<HarvestUploadDto>()), Times.Once);
        }

        [TestMethod]
        public void CreateHarvestUpload_WithParameters_Failure_ReturnsFalse()
        {
            // Arrange
            _mockHarvestDbs.Setup(x => x.CreateUploadDbs(It.IsAny<HarvestUploadDto>())).Returns(false);

            // Act
            bool result = _uploadService.CreateHarvestUpload(1, "test.jpg", "test", 100f, 10, 20);

            // Assert
            Assert.IsFalse(result);
            _mockHarvestDbs.Verify(x => x.CreateUploadDbs(It.IsAny<HarvestUploadDto>()), Times.Once);
        }
    }
}
