using System;
using System.Collections.Generic;
using System.Linq;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests.UnitTests.AppLogicTests;

// generierter und händisch verbesserter Test (aktualisiert nach Split der DB-Interfaces + Interface-Änderungen)
[TestClass]
[TestCategory("Unit")]
public class ProfileMgmUnitTests
{
    private Mock<IProfileDbs> _mockProfileDbs = null!;
    private Mock<IHarvestDbs> _mockHarvestDbs = null!;
    private ProfileMgm _profileMgm = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockProfileDbs = new Mock<IProfileDbs>(MockBehavior.Strict);
        _mockHarvestDbs = new Mock<IHarvestDbs>(MockBehavior.Strict);
        _profileMgm = new ProfileMgm(_mockProfileDbs.Object, _mockHarvestDbs.Object, NullLogger<ProfileMgm>.Instance);
    }

    [TestMethod]
    public void CheckProfileNameExists_NameExists_ReturnsTrue()
    {
        // Arrange
        const string profileName = "testuser";
        _mockProfileDbs.Setup(x => x.CheckProfileNameExists(profileName)).Returns(true);

        // Act
        bool result = _profileMgm.CheckProfileNameExists(profileName);

        // Assert
        Assert.IsTrue(result);
        _mockProfileDbs.Verify(x => x.CheckProfileNameExists(profileName), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void CheckProfileNameExists_NameNotExists_ReturnsFalse()
    {
        // Arrange
        const string profileName = "nonexistent";
        _mockProfileDbs.Setup(x => x.CheckProfileNameExists(profileName)).Returns(false);

        // Act
        bool result = _profileMgm.CheckProfileNameExists(profileName);

        // Assert
        Assert.IsFalse(result);
        _mockProfileDbs.Verify(x => x.CheckProfileNameExists(profileName), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void VisitPublicProfile_ValidProfileId_ReturnsPublicProfileWithHarvests()
    {
        // Arrange
        const int profileId = 1;

        var profileInfo = new PublicProfileDto
        {
            ProfileId = profileId,
            ProfilepictureUrl = "profile.jpg",
            UserName = "testuser",
            Profiletext = "Test profile",
            UserCreated = new DateTime(2023, 1, 1),
            EMail = "test@example.com",
            Phonenumber = "123456789",
        };

        var harvests = new List<HarvestUploadDto>
        {
            new HarvestUploadDto { UploadId = 11, ProfileId = profileId },
            new HarvestUploadDto { UploadId = 12, ProfileId = profileId }
        };

        _mockHarvestDbs.Setup(x => x.GetProfileHarvestUploads(profileId)).Returns(harvests);
        _mockProfileDbs.Setup(x => x.GetPublicProfile(profileId)).Returns(profileInfo);

        // Act
        var result = _profileMgm.VisitPublicProfile(profileId);

        // Assert
        Assert.AreEqual(profileId, result.ProfileId);
        Assert.AreEqual("profile.jpg", result.ProfilepictureUrl);
        Assert.AreEqual("testuser", result.UserName);
        Assert.AreEqual("Test profile", result.Profiletext);
        Assert.AreEqual(new DateTime(2023, 1, 1), result.UserCreated);

        Assert.IsNotNull(result.HarvestUploads);
        Assert.AreEqual(2, result.HarvestUploads.Count);
        CollectionAssert.AreEqual(
            harvests.Select(h => h.UploadId).ToList(),
            result.HarvestUploads.Select(h => h.UploadId).ToList());

        _mockHarvestDbs.Verify(x => x.GetProfileHarvestUploads(profileId), Times.Once);
        _mockProfileDbs.Verify(x => x.GetPublicProfile(profileId), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetPrivProfile_ValidProfileId_ReturnsPrivateProfileWithHarvests()
    {
        // Arrange
        const int profileId = 1;

        var expectedProfileInfo = new PrivateProfileDto
        {
            ProfileId = profileId,
            ProfilepictureUrl = "private.jpg",
            UserName = "testuser",
            FirstName = "Max",
            LastName = "Mustermann",
            EMail = "private@example.com",
            Phonenumber = "987654321",
            Profiletext = "Private profile text",
            ShareMail = true,
            SharePhoneNumber = false,
            UserCreated = new DateTime(2023, 1, 1),
            PreferenceDtos = new List<PreferenceDto>
            {
                new PreferenceDto { Profileid = profileId, TagId = 1 }
            }
        };

        var harvests = new List<HarvestUploadDto>
        {
            new HarvestUploadDto { UploadId = 21, ProfileId = profileId }
        };

        _mockHarvestDbs.Setup(x => x.GetProfileHarvestUploads(profileId)).Returns(harvests);
        _mockProfileDbs.Setup(x => x.GetPrivateProfile(profileId)).Returns(expectedProfileInfo);

        // Act
        var result = _profileMgm.GetPrivProfile(profileId);

        // Assert
        Assert.AreEqual(expectedProfileInfo.ProfileId, result.ProfileId);
        Assert.AreEqual(expectedProfileInfo.ProfilepictureUrl, result.ProfilepictureUrl);
        Assert.AreEqual(expectedProfileInfo.UserName, result.UserName);
        Assert.AreEqual(expectedProfileInfo.FirstName, result.FirstName);
        Assert.AreEqual(expectedProfileInfo.LastName, result.LastName);
        Assert.AreEqual(expectedProfileInfo.EMail, result.EMail);
        Assert.AreEqual(expectedProfileInfo.Phonenumber, result.Phonenumber);
        Assert.AreEqual(expectedProfileInfo.Profiletext, result.Profiletext);
        Assert.AreEqual(expectedProfileInfo.ShareMail, result.ShareMail);
        Assert.AreEqual(expectedProfileInfo.SharePhoneNumber, result.SharePhoneNumber);
        Assert.AreEqual(expectedProfileInfo.UserCreated, result.UserCreated);

        Assert.IsNotNull(result.HarvestUploads);
        Assert.AreEqual(1, result.HarvestUploads.Count);
        Assert.AreEqual(21, result.HarvestUploads[0].UploadId);

        _mockHarvestDbs.Verify(x => x.GetProfileHarvestUploads(profileId), Times.Once);
        _mockProfileDbs.Verify(x => x.GetPrivateProfile(profileId), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateProfile_Success_CallsRepo()
    {
        // Arrange
        var dto = new PrivateProfileDto
        {
            ProfileId = 1,
            ProfilepictureUrl = "updated.jpg",
            UserName = "updateduser",
            FirstName = "Updated",
            LastName = "User",
            EMail = "updated@example.com"
        };

        _mockProfileDbs.Setup(x => x.EditProfile(dto));

        // Act
        _profileMgm.UpdateProfile(dto);

        // Assert
        _mockProfileDbs.Verify(x => x.EditProfile(dto), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateProfile_RepoThrows_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new PrivateProfileDto { ProfileId = 1 };
        _mockProfileDbs.Setup(x => x.EditProfile(dto)).Throws(new Exception("db fail"));

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _profileMgm.UpdateProfile(dto));

        _mockProfileDbs.Verify(x => x.EditProfile(dto), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void RegisterProfile_CallsSetNewProfile_ReturnsProfileId()
    {
        // Arrange
        var newProfile = new PrivateProfileDto
        {
            ProfileId = 0,
            ProfilepictureUrl = "new.jpg",
            UserName = "newuser",
            FirstName = "New",
            LastName = "User",
            EMail = "new@example.com",
            HarvestUploads = null,
            PreferenceDtos = null
        };

        var newCredential = new CredentialProfileDto
        {
            EMail = "new@example.com",
            PasswordHash = "newhash"
        };

        _mockProfileDbs.Setup(x => x.SetNewProfile(newProfile, newCredential)).Returns(1);

        // Act
        var result = _profileMgm.RegisterProfile(newProfile, newCredential);

        // Assert
        Assert.AreEqual(1, result);
        _mockProfileDbs.Verify(x => x.SetNewProfile(newProfile, newCredential), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void RegisterProfile_RepoThrows_ThrowsInvalidOperationException()
    {
        // Arrange
        var newProfile = new PrivateProfileDto { ProfileId = 0 };
        var newCredential = new CredentialProfileDto { EMail = "x@y.z", PasswordHash = "pw" };

        _mockProfileDbs.Setup(x => x.SetNewProfile(newProfile, newCredential)).Throws(new Exception("db fail"));

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _profileMgm.RegisterProfile(newProfile, newCredential));

        _mockProfileDbs.Verify(x => x.SetNewProfile(newProfile, newCredential), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void LoginProfile_ValidCredentials_ReturnsCompletePrivateProfile()
    {
        // Arrange
        var loginProfile = new CredentialProfileDto
        {
            EMail = "test@example.com",
            PasswordHash = "hash123"
        };

        const int profileId = 1;

        var profileInfo = new PrivateProfileDto
        {
            ProfileId = profileId,
            UserName = "testuser",
            EMail = "test@example.com",
            PreferenceDtos = new List<PreferenceDto>()
        };

        var harvests = new List<HarvestUploadDto>
        {
            new HarvestUploadDto { UploadId = 31, ProfileId = profileId }
        };

        _mockProfileDbs.Setup(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash)).Returns(profileId);
        _mockHarvestDbs.Setup(x => x.GetProfileHarvestUploads(profileId)).Returns(harvests);
        _mockProfileDbs.Setup(x => x.GetPrivateProfile(profileId)).Returns(profileInfo);

        // Act
        var result = _profileMgm.LoginProfile(loginProfile);

        // Assert
        Assert.AreEqual(profileId, result.ProfileId);
        Assert.AreEqual("testuser", result.UserName);
        Assert.AreEqual("test@example.com", result.EMail);
        Assert.IsNotNull(result.HarvestUploads);
        Assert.AreEqual(1, result.HarvestUploads.Count);
        Assert.AreEqual(31, result.HarvestUploads[0].UploadId);

        _mockProfileDbs.Verify(x => x.CheckPassword("test@example.com", "hash123"), Times.Once);
        _mockHarvestDbs.Verify(x => x.GetProfileHarvestUploads(profileId), Times.Once);
        _mockProfileDbs.Verify(x => x.GetPrivateProfile(profileId), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void LoginProfile_InvalidCredentials_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var loginProfile = new CredentialProfileDto
        {
            EMail = "invalid@example.com",
            PasswordHash = "wrong"
        };

        _mockProfileDbs.Setup(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash)).Returns((int?)null);

        // Act & Assert
        Assert.ThrowsException<UnauthorizedAccessException>(() => _profileMgm.LoginProfile(loginProfile));

        _mockProfileDbs.Verify(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash), Times.Once);
        _mockProfileDbs.Verify(x => x.GetPrivateProfile(It.IsAny<int>()), Times.Never);
        _mockHarvestDbs.Verify(x => x.GetProfileHarvestUploads(It.IsAny<int>()), Times.Never);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void LoginProfile_CheckPasswordThrows_ThrowsInvalidOperationException()
    {
        // Arrange
        var loginProfile = new CredentialProfileDto
        {
            EMail = "test@example.com",
            PasswordHash = "hash123"
        };

        _mockProfileDbs.Setup(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash))
            .Throws(new Exception("db fail"));

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _profileMgm.LoginProfile(loginProfile));

        _mockProfileDbs.Verify(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateCredentials_Success_CallsRepo()
    {
        // Arrange
        var cred = new CredentialProfileDto { EMail = "x@y.z", PasswordHash = "pw" };
        _mockProfileDbs.Setup(x => x.EditPassword(cred));

        // Act
        _profileMgm.UpdateCredentials(cred);

        // Assert
        _mockProfileDbs.Verify(x => x.EditPassword(cred), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateCredentials_RepoThrows_ThrowsInvalidOperationException()
    {
        // Arrange
        var cred = new CredentialProfileDto { EMail = "x@y.z", PasswordHash = "pw" };
        _mockProfileDbs.Setup(x => x.EditPassword(cred)).Throws(new Exception("db fail"));

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _profileMgm.UpdateCredentials(cred));

        _mockProfileDbs.Verify(x => x.EditPassword(cred), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void SetPreference_Success_CallsRepo()
    {
        // Arrange
        var preferences = new List<PreferenceDto>
        {
            new PreferenceDto { TagId = 1, Profileid = 1, DateUpdated = DateTime.UtcNow },
            new PreferenceDto { TagId = 2, Profileid = 1, DateUpdated = DateTime.UtcNow }
        };

        _mockProfileDbs.Setup(x => x.SetProfilePreference(preferences));

        // Act
        _profileMgm.SetPreference(preferences);

        // Assert
        _mockProfileDbs.Verify(x => x.SetProfilePreference(preferences), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void SetPreference_RepoThrows_ThrowsInvalidOperationException()
    {
        // Arrange
        var preferences = new List<PreferenceDto>();
        _mockProfileDbs.Setup(x => x.SetProfilePreference(preferences)).Throws(new Exception("db fail"));

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => _profileMgm.SetPreference(preferences));

        _mockProfileDbs.Verify(x => x.SetProfilePreference(preferences), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }
}