using System;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AppLogic.Services;

namespace Tests.UnitTests.AppLogicTests;

[TestClass]
public class UploadServiceImplTests
{
    private Mock<IHarvestDbs> _mockHarvestDbs = null!;
    private UploadServiceImpl _uploadService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockHarvestDbs = new Mock<IHarvestDbs>(MockBehavior.Strict);
        _uploadService = new UploadServiceImpl(_mockHarvestDbs.Object);
    }

    [TestMethod]
    public void CreateHarvestUpload_ValidDto_CallsDbsOnce()
    {
        // Arrange
        var uploadDto = new HarvestUploadDto
        {
            ProfileId = 1,
            ImageUrl = "test.jpg",
            Description = "Test upload",
            WeightGram = 500,
            WidthCm = 10,
            LengthCm = 20
        };

        _mockHarvestDbs
            .Setup(x => x.CreateUploadDbs(uploadDto));

        // Act
        _uploadService.CreateHarvestUpload(uploadDto);

        // Assert
        _mockHarvestDbs.Verify(x => x.CreateUploadDbs(uploadDto), Times.Once);
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void CreateHarvestUpload_WhenDbsThrows_WrapsInInvalidOperationException()
    {
        // Arrange
        var uploadDto = new HarvestUploadDto { ProfileId = 1, ImageUrl = "x.jpg" };
        var inner = new Exception("db failed");

        _mockHarvestDbs
            .Setup(x => x.CreateUploadDbs(uploadDto))
            .Throws(inner);

        // Act
        var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            _uploadService.CreateHarvestUpload(uploadDto));

        // Assert
        Assert.AreEqual("Upload fehlgeschlagen", ex.Message);
        Assert.AreSame(inner, ex.InnerException);

        _mockHarvestDbs.Verify(x => x.CreateUploadDbs(uploadDto), Times.Once);
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void DeleteHarvestUpload_ValidUploadWithImageUrl_ReturnsImageUrl_AndDeletesInDb()
    {
        // Arrange
        int uploadId = 1;
        var uploadDto = new HarvestUploadDto { UploadId = uploadId, ImageUrl = "test.jpg" };

        _mockHarvestDbs
            .Setup(x => x.GetHarvestUploadDto(uploadId))
            .Returns(uploadDto);

        _mockHarvestDbs
            .Setup(x => x.DeleteHarvestUpload(uploadId));

        // Act
        var result = _uploadService.DeleteHarvestUpload(uploadId);

        // Assert
        Assert.AreEqual("test.jpg", result);
        _mockHarvestDbs.Verify(x => x.GetHarvestUploadDto(uploadId), Times.Once);
        _mockHarvestDbs.Verify(x => x.DeleteHarvestUpload(uploadId), Times.Once);
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void DeleteHarvestUpload_UploadDoesNotExist_ReturnsNull_AndDoesNotDelete()
    {
        // Arrange
        int uploadId = 999;

        _mockHarvestDbs
            .Setup(x => x.GetHarvestUploadDto(uploadId))
            .Returns((HarvestUploadDto?)null);

        // Act
        var result = _uploadService.DeleteHarvestUpload(uploadId);

        // Assert
        Assert.IsNull(result);
        _mockHarvestDbs.Verify(x => x.GetHarvestUploadDto(uploadId), Times.Once);
        _mockHarvestDbs.Verify(x => x.DeleteHarvestUpload(It.IsAny<int>()), Times.Never);
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void DeleteHarvestUpload_EmptyImageUrl_ReturnsNull_AndDoesNotDelete()
    {
        // Arrange
        int uploadId = 1;
        var uploadDto = new HarvestUploadDto { UploadId = uploadId, ImageUrl = "  " };

        _mockHarvestDbs
            .Setup(x => x.GetHarvestUploadDto(uploadId))
            .Returns(uploadDto);

        // Act
        var result = _uploadService.DeleteHarvestUpload(uploadId);

        // Assert
        Assert.IsNull(result);
        _mockHarvestDbs.Verify(x => x.GetHarvestUploadDto(uploadId), Times.Once);
        _mockHarvestDbs.Verify(x => x.DeleteHarvestUpload(It.IsAny<int>()), Times.Never);
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void DeleteHarvestUpload_WhenGetUploadThrows_WrapsInInvalidOperationException()
    {
        // Arrange
        int uploadId = 1;
        var inner = new Exception("read failed");

        _mockHarvestDbs
            .Setup(x => x.GetHarvestUploadDto(uploadId))
            .Throws(inner);

        // Act
        var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            _uploadService.DeleteHarvestUpload(uploadId));

        // Assert
        Assert.AreEqual("Upload konnte nicht geladen werden.", ex.Message);
        Assert.AreSame(inner, ex.InnerException);

        _mockHarvestDbs.Verify(x => x.GetHarvestUploadDto(uploadId), Times.Once);
        _mockHarvestDbs.Verify(x => x.DeleteHarvestUpload(It.IsAny<int>()), Times.Never);
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void DeleteHarvestUpload_WhenDeleteThrows_WrapsInInvalidOperationException()
    {
        // Arrange
        int uploadId = 1;
        var uploadDto = new HarvestUploadDto { UploadId = uploadId, ImageUrl = "test.jpg" };
        var inner = new Exception("delete failed");

        _mockHarvestDbs
            .Setup(x => x.GetHarvestUploadDto(uploadId))
            .Returns(uploadDto);

        _mockHarvestDbs
            .Setup(x => x.DeleteHarvestUpload(uploadId))
            .Throws(inner);

        // Act
        var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            _uploadService.DeleteHarvestUpload(uploadId));

        // Assert
        Assert.AreEqual("Upload konnte nicht gelöscht werden.", ex.Message);
        Assert.AreSame(inner, ex.InnerException);

        _mockHarvestDbs.Verify(x => x.GetHarvestUploadDto(uploadId), Times.Once);
        _mockHarvestDbs.Verify(x => x.DeleteHarvestUpload(uploadId), Times.Once);
        _mockHarvestDbs.VerifyNoOtherCalls();
    }
}
