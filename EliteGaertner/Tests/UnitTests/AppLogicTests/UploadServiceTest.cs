using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AppLogic.Services;
using DataManagement.Interfaces;
using Contracts.Data_Transfer_Objects;
using System;

namespace Tests.UnitTests.AppLogicTests
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
            int profileId = 1;
            string imageUrl = "test.jpg";
            string description = "Test upload";
            float weight = 500.5f;
            int width = 10;
            int length = 20;

            _mockHarvestDbs.Setup(x => x.CreateUploadDbs(It.Is<HarvestUploadDto>(dto =>
                dto.ProfileId == profileId &&
                dto.ImageUrl == imageUrl &&
                dto.Description == description &&
                dto.WeightGram == weight &&
                dto.WidthCm == width &&
                dto.LengthCm == length))).Returns(true);

            // Act
            bool result = _uploadService.CreateHarvestUpload(profileId, imageUrl, description, weight, width, length);

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

        // NEU: GetUploadDto – gültige Id liefert DTO
        [TestMethod]
        public void GetUploadDto_GueltigeId_GibtDtoZurueck()
        {
            // Arrange
            int uploadId = 1;
            var expectedDto = new HarvestUploadDto { UploadId = uploadId, ImageUrl = "test.jpg" };
            _mockHarvestDbs.Setup(x => x.GetUploadDb(uploadId)).Returns(expectedDto);

            // Act
            var result = _uploadService.GetUploadDto(uploadId);

            // Assert
            Assert.AreEqual(expectedDto, result);
            _mockHarvestDbs.Verify(x => x.GetUploadDb(uploadId), Times.Once);
        }

        // NEU: GetUploadDto – ungueltige Id liefert null
        [TestMethod]
        public void GetUploadDto_UngueltigeId_GibtNullZurueck()
        {
            // Arrange
            int uploadId = 999;
            _mockHarvestDbs.Setup(x => x.GetUploadDb(uploadId)).Returns((HarvestUploadDto)null);

            // Act
            var result = _uploadService.GetUploadDto(uploadId);

            // Assert
            Assert.IsNull(result);
            _mockHarvestDbs.Verify(x => x.GetUploadDb(uploadId), Times.Once);
        }

        // NEU: DeleteUpload – gültiger Upload mit ImageUrl
        [TestMethod]
        public void DeleteUpload_GueltigerUploadMitImageUrl_GibtDateinameZurueck()
        {
            // Arrange
            int uploadId = 1;
            int profileId = 1;
            var uploadDto = new HarvestUploadDto { UploadId = uploadId, ImageUrl = "test.jpg" };
            _mockHarvestDbs.Setup(x => x.GetUploadDb(uploadId)).Returns(uploadDto);

            // Act
            var result = _uploadService.DeleteUpload(uploadId, profileId);

            // Assert
            Assert.AreEqual("test.jpg", result);
            _mockHarvestDbs.Verify(x => x.DeleteHarvestUpload(uploadId), Times.Once);
        }

        // NEU: DeleteUpload – DTO null
        [TestMethod]
        public void DeleteUpload_UploadExistiertNicht_GibtNullZurueck()
        {
            // Arrange
            int uploadId = 999;
            int profileId = 1;
            _mockHarvestDbs.Setup(x => x.GetUploadDb(uploadId)).Returns((HarvestUploadDto)null);

            // Act
            var result = _uploadService.DeleteUpload(uploadId, profileId);

            // Assert
            Assert.IsNull(result);
            _mockHarvestDbs.Verify(x => x.DeleteHarvestUpload(It.IsAny<int>()), Times.Never);
        }

        // NEU: DeleteUpload – leere ImageUrl
        [TestMethod]
        public void DeleteUpload_LeereImageUrl_GibtNullZurueck()
        {
            // Arrange
            int uploadId = 1;
            int profileId = 1;
            var uploadDto = new HarvestUploadDto { UploadId = uploadId, ImageUrl = "" };
            _mockHarvestDbs.Setup(x => x.GetUploadDb(uploadId)).Returns(uploadDto);

            // Act
            var result = _uploadService.DeleteUpload(uploadId, profileId);

            // Assert
            Assert.IsNull(result);
            _mockHarvestDbs.Verify(x => x.DeleteHarvestUpload(It.IsAny<int>()), Times.Never);
        }
    }
}
