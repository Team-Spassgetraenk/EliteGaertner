using System;
using System.Collections.Generic;
using System.Linq;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.UnitTests.AppLogicTests;

// generierter und händisch verbesserter Test (aktualisiert nach Split der DB-Interfaces)
[TestClass]
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
        _profileMgm = new ProfileMgm(_mockProfileDbs.Object, _mockHarvestDbs.Object);
    }

    [TestMethod]
    public void CheckUsernameExists_UsernameExists_ReturnsTrue()
    {
        // Arrange
        const string username = "testuser";
        _mockProfileDbs.Setup(x => x.CheckUsernameExists(username)).Returns(true);

        // Act
        bool result = _profileMgm.CheckUsernameExists(username);

        // Assert
        Assert.IsTrue(result);
        _mockProfileDbs.Verify(x => x.CheckUsernameExists(username), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void CheckUsernameExists_UsernameNotExists_ReturnsFalse()
    {
        // Arrange
        const string username = "nonexistent";
        _mockProfileDbs.Setup(x => x.CheckUsernameExists(username)).Returns(false);

        // Act
        bool result = _profileMgm.CheckUsernameExists(username);

        // Assert
        Assert.IsFalse(result);
        _mockProfileDbs.Verify(x => x.CheckUsernameExists(username), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void VisitPublicProfile_ValidProfileId_ReturnsPublicProfileBuiltFromPrivateProfileAndHarvests()
    {
        // Arrange
        const int profileId = 1;

        var profileInfo = new PrivateProfileDto
        {
            ProfileId = profileId,
            ProfilepictureUrl = "profile.jpg",
            UserName = "testuser",
            Profiletext = "Test profile",
            UserCreated = new DateTime(2023, 1, 1),
            // Felder, die hier egal sind, dürfen gesetzt sein – werden aber in PublicProfileDto nicht übernommen
            EMail = "test@example.com",
            Phonenumber = "123456789",
            ShareMail = true,
            SharePhoneNumber = false,
            PreferenceDtos = new List<PreferenceDto>()
        };

        var harvests = new List<HarvestUploadDto>
        {
            new HarvestUploadDto { UploadId = 11, ProfileId = profileId },
            new HarvestUploadDto { UploadId = 12, ProfileId = profileId }
        };

        _mockHarvestDbs.Setup(x => x.GetProfileHarvestUploads(profileId)).Returns(harvests);
        _mockProfileDbs.Setup(x => x.GetPrivateProfile(profileId)).Returns(profileInfo);

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
        CollectionAssert.AreEqual(harvests.Select(h => h.UploadId).ToList(), result.HarvestUploads.Select(h => h.UploadId).ToList());

        _mockHarvestDbs.Verify(x => x.GetProfileHarvestUploads(profileId), Times.Once);
        _mockProfileDbs.Verify(x => x.GetPrivateProfile(profileId), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void VisitPrivateProfile_ValidProfileId_ReturnsPrivateProfileWithHarvests()
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
            PasswordHash = "hash123",
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

        Assert.IsNotNull(result.PreferenceDtos);
        Assert.AreEqual(1, result.PreferenceDtos.Count);
        Assert.AreEqual(1, result.PreferenceDtos[0].TagId);

        _mockHarvestDbs.Verify(x => x.GetProfileHarvestUploads(profileId), Times.Once);
        _mockProfileDbs.Verify(x => x.GetPrivateProfile(profileId), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateProfile_Success_ReturnsTrue()
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

        var updatedProfile = new PrivateProfileDto { ProfileId = 1 };
        _mockProfileDbs.Setup(x => x.EditProfile(dto)).Returns(updatedProfile);

        // Act
        bool result = _profileMgm.UpdateProfile(dto);

        // Assert
        Assert.IsTrue(result);
        _mockProfileDbs.Verify(x => x.EditProfile(dto), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateProfile_Failure_ReturnsFalse()
    {
        // Arrange
        var dto = new PrivateProfileDto { ProfileId = 0 };
        _mockProfileDbs.Setup(x => x.EditProfile(dto)).Returns(dto);

        // Act
        bool result = _profileMgm.UpdateProfile(dto);

        // Assert
        Assert.IsFalse(result);
        _mockProfileDbs.Verify(x => x.EditProfile(dto), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateContactVisibility_Success_ReturnsTrue()
    {
        // Arrange
        var dto = new ContactVisibilityDto
        {
            profileId = 1,
            EMail = "test@example.com",
            PhoneNumber = "123456789",
            ShareMail = true,
            SharePhoneNumber = false
        };

        _mockProfileDbs.Setup(x => x.UpdateContactVisibility(dto)).Returns(true);

        // Act
        bool result = _profileMgm.UpdateContactVisibility(dto);

        // Assert
        Assert.IsTrue(result);
        _mockProfileDbs.Verify(x => x.UpdateContactVisibility(dto), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateContactVisibility_Failure_ReturnsFalse()
    {
        // Arrange
        var dto = new ContactVisibilityDto
        {
            profileId = 1,
            EMail = "test@example.com",
            PhoneNumber = "123456789",
            ShareMail = true,
            SharePhoneNumber = false
        };

        _mockProfileDbs.Setup(x => x.UpdateContactVisibility(dto)).Returns(false);

        // Act
        bool result = _profileMgm.UpdateContactVisibility(dto);

        // Assert
        Assert.IsFalse(result);
        _mockProfileDbs.Verify(x => x.UpdateContactVisibility(dto), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void RegisterProfile_CallsSetNewProfile_ReturnsProfileFromRepo()
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
            PasswordHash = "newhash",
            HarvestUploads = null,
            PreferenceDtos = null
        };

        var registeredProfile = new PrivateProfileDto
        {
            ProfileId = 1,
            ProfilepictureUrl = "new.jpg",
            UserName = "newuser",
            FirstName = "New",
            LastName = "User",
            EMail = "new@example.com",
            HarvestUploads = null,
            PreferenceDtos = null
        };

        _mockProfileDbs.Setup(x => x.SetNewProfile(newProfile)).Returns(registeredProfile);

        // Act
        var result = _profileMgm.RegisterProfile(newProfile, TODO);

        // Assert
        Assert.AreEqual(1, result.ProfileId);
        Assert.AreEqual("new.jpg", result.ProfilepictureUrl);
        Assert.AreEqual("newuser", result.UserName);
        Assert.AreEqual("new@example.com", result.EMail);
        Assert.IsNull(result.HarvestUploads);
        Assert.IsNull(result.PreferenceDtos);

        _mockProfileDbs.Verify(x => x.SetNewProfile(newProfile), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void LoginProfile_ValidCredentials_ReturnsCompletePrivateProfile()
    {
        // Arrange
        var loginProfile = new PrivateProfileDto
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
        var result = _profileMgm.LoginProfile(TODO);

        // Assert
        Assert.AreEqual(profileId, result.ProfileId);
        Assert.AreEqual("testuser", result.UserName);
        Assert.AreEqual("test@example.com", result.EMail);
        Assert.IsNotNull(result.HarvestUploads);
        Assert.AreEqual(1, result.HarvestUploads.Count);
        Assert.AreEqual(31, result.HarvestUploads[0].UploadId);

        _mockProfileDbs.Verify(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash), Times.Once);
        _mockHarvestDbs.Verify(x => x.GetProfileHarvestUploads(profileId), Times.Once);
        _mockProfileDbs.Verify(x => x.GetPrivateProfile(profileId), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void LoginProfile_InvalidCredentials_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var loginProfile = new PrivateProfileDto
        {
            EMail = "invalid@example.com",
            PasswordHash = "wrong"
        };

        _mockProfileDbs.Setup(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash)).Returns((int?)null);

        // Act & Assert
        Assert.ThrowsException<UnauthorizedAccessException>(() => _profileMgm.LoginProfile(TODO));
        _mockProfileDbs.Verify(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetPreference_ValidProfileId_ReturnsPreferences()
    {
        // Arrange
        const int profileId = 1;

        var expectedPreferences = new[]
        {
            new PreferenceDto { TagId = 1, Profileid = profileId, DateUpdated = DateTime.UtcNow },
            new PreferenceDto { TagId = 2, Profileid = profileId, DateUpdated = DateTime.UtcNow }
        };

        _mockProfileDbs.Setup(x => x.GetUserPreference(profileId)).Returns(expectedPreferences);

        // Act
        var result = _profileMgm.GetPreference(profileId);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(1, result[0].TagId);
        Assert.AreEqual(profileId, result[0].Profileid);
        Assert.AreEqual(2, result[1].TagId);

        _mockProfileDbs.Verify(x => x.GetUserPreference(profileId), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void SetPreference_Success_ReturnsTrue()
    {
        // Arrange
        var preferences = new List<PreferenceDto>
        {
            new PreferenceDto { TagId = 1, Profileid = 1, DateUpdated = DateTime.UtcNow },
            new PreferenceDto { TagId = 2, Profileid = 1, DateUpdated = DateTime.UtcNow }
        };

        _mockProfileDbs.Setup(x => x.SetUserPreference(preferences)).Returns(true);

        // Act
        bool result = _profileMgm.SetPreference(preferences);

        // Assert
        Assert.IsTrue(result);
        _mockProfileDbs.Verify(x => x.SetUserPreference(preferences), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void SetPreference_Failure_ReturnsFalse()
    {
        // Arrange
        var preferences = new List<PreferenceDto>();
        _mockProfileDbs.Setup(x => x.SetUserPreference(preferences)).Returns(false);

        // Act
        bool result = _profileMgm.SetPreference(preferences);

        // Assert
        Assert.IsFalse(result);
        _mockProfileDbs.Verify(x => x.SetUserPreference(preferences), Times.Once);
        _mockProfileDbs.VerifyNoOtherCalls();
        _mockHarvestDbs.VerifyNoOtherCalls();
    }
}