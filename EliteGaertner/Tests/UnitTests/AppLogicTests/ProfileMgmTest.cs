using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AppLogic.Services;
using DataManagement.Interfaces;
using Contracts.Data_Transfer_Objects;
using System;
using System.Collections.Generic;
using System.Linq;

//generierter und händisch verbesserter Test
namespace Tests.UnitTests.AppLogicTests
{
    [TestClass]
    public class ProfileMgmUnitTests
    {
        private Mock<IProfileDbs> _mockProfileDbs;
        private ProfileMgm _profileMgm;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockProfileDbs = new Mock<IProfileDbs>();
            _profileMgm = new ProfileMgm(_mockProfileDbs.Object);
        }

        [TestMethod]
        public void CheckUsernameExists_UsernameExists_ReturnsTrue()
        {
            // Arrange
            string username = "testuser";
            _mockProfileDbs.Setup(x => x.CheckUsernameExists(username)).Returns(true);

            // Act
            bool result = _profileMgm.CheckUsernameExists(username);

            // Assert
            Assert.IsTrue(result);
            _mockProfileDbs.Verify(x => x.CheckUsernameExists(username), Times.Once);
        }

        [TestMethod]
        public void CheckUsernameExists_UsernameNotExists_ReturnsFalse()
        {
            // Arrange
            string username = "nonexistent";
            _mockProfileDbs.Setup(x => x.CheckUsernameExists(username)).Returns(false);

            // Act
            bool result = _profileMgm.CheckUsernameExists(username);

            // Assert
            Assert.IsFalse(result);
            _mockProfileDbs.Verify(x => x.CheckUsernameExists(username), Times.Once);
        }

        [TestMethod]
        public void VisitReceiverProfile_ValidUserId_ReturnsCompletePublicProfile()
        {
            // Arrange - Komplettes PublicProfileDto
            int userId = 1;
            var expectedProfile = new PublicProfileDto
            {
                ProfileId = userId,
                ProfilepictureUrl = "profile.jpg",
                UserName = "testuser",
                EMail = "test@example.com",
                Phonenumber = "123456789",
                Profiletext = "Test profile",
                ShareMail = true,
                SharePhoneNumber = false,
                UserCreated = new DateTime(2023, 1, 1),
                HarvestUploads = new List<HarvestUploadDto>()
            };
            _mockProfileDbs.Setup(x => x.GetPublicProfile(userId)).Returns(expectedProfile);

            // Act
            var result = _profileMgm.VisitReceiverProfile(userId);

            // Assert - Vollständige DTO-Prüfung
            Assert.AreEqual(expectedProfile.ProfileId, result.ProfileId);
            Assert.AreEqual(expectedProfile.ProfilepictureUrl, result.ProfilepictureUrl);
            Assert.AreEqual(expectedProfile.UserName, result.UserName);
            Assert.AreEqual(expectedProfile.EMail, result.EMail);
            Assert.AreEqual(expectedProfile.Phonenumber, result.Phonenumber);
            Assert.AreEqual(expectedProfile.Profiletext, result.Profiletext);
            Assert.AreEqual(expectedProfile.ShareMail, result.ShareMail);
            Assert.AreEqual(expectedProfile.SharePhoneNumber, result.SharePhoneNumber);
            Assert.AreEqual(expectedProfile.UserCreated, result.UserCreated);
            Assert.AreEqual(0, result.HarvestUploads.Count);
            _mockProfileDbs.Verify(x => x.GetPublicProfile(userId), Times.Once);
        }

        [TestMethod]
        public void VisitCreatorProfile_ValidUserId_ReturnsCompletePrivateProfile()
        {
            // Arrange - Komplettes PrivateProfileDto
            int userId = 1;
            var expectedProfile = new PrivateProfileDto
            {
                ProfileId = userId,
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
                HarvestUploads = new List<HarvestUploadDto>(),
                PreferenceDtos = new List<PreferenceDto>()
            };
            _mockProfileDbs.Setup(x => x.GetPrivateProfile(userId)).Returns(expectedProfile);

            // Act
            var result = _profileMgm.VisitCreatorProfile(userId);

            // Assert - Vollständige DTO-Prüfung
            Assert.AreEqual(expectedProfile.ProfileId, result.ProfileId);
            Assert.AreEqual(expectedProfile.ProfilepictureUrl, result.ProfilepictureUrl);
            Assert.AreEqual(expectedProfile.UserName, result.UserName);
            Assert.AreEqual(expectedProfile.FirstName, result.FirstName);
            Assert.AreEqual(expectedProfile.LastName, result.LastName);
            Assert.AreEqual(expectedProfile.EMail, result.EMail);
            Assert.AreEqual(expectedProfile.PasswordHash, result.PasswordHash);
            Assert.AreEqual(expectedProfile.Phonenumber, result.Phonenumber);
            Assert.AreEqual(expectedProfile.Profiletext, result.Profiletext);
            Assert.AreEqual(expectedProfile.ShareMail, result.ShareMail);
            Assert.AreEqual(expectedProfile.SharePhoneNumber, result.SharePhoneNumber);
            Assert.AreEqual(expectedProfile.UserCreated, result.UserCreated);
            Assert.AreEqual(0, result.HarvestUploads.Count);
            Assert.AreEqual(0, result.PreferenceDtos.Count);
            _mockProfileDbs.Verify(x => x.GetPrivateProfile(userId), Times.Once);
        }

        [TestMethod]
        public void UpdateProfile_Success_ReturnsTrue()
        {
            // Arrange - Komplettes PrivateProfileDto
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
        }

        [TestMethod]
        public void UpdateContactVisibility_Success_ReturnsTrue()
        {
            // Arrange - Komplettes ContactVisibilityDto
            var dto = new ContactVisibilityDto
            {
                UserId = 1,
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
        }

        [TestMethod]
        public void UpdateContactVisibility_Failure_ReturnsFalse()
        {
            // Arrange
            var dto = new ContactVisibilityDto
            {
                UserId = 1,
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
        }

        [TestMethod]
        public void RegisterProfile_CallsSetNewProfile_ReturnsCompleteProfile()
        {
            // Arrange - Komplettes PrivateProfileDto mit null Listen (wie in Doku beschrieben)
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
            var result = _profileMgm.RegisterProfile(newProfile);

            // Assert - Vollständige DTO-Prüfung
            Assert.AreEqual(1, result.ProfileId);
            Assert.AreEqual("new.jpg", result.ProfilepictureUrl);
            Assert.AreEqual("newuser", result.UserName);
            Assert.AreEqual("new@example.com", result.EMail);
            Assert.IsNull(result.HarvestUploads);
            Assert.IsNull(result.PreferenceDtos);
            _mockProfileDbs.Verify(x => x.SetNewProfile(newProfile), Times.Once);
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
            int profileId = 1;
            var expectedProfile = new PrivateProfileDto 
            { 
                ProfileId = profileId,
                UserName = "testuser",
                EMail = "test@example.com",
                HarvestUploads = new List<HarvestUploadDto>()
            };
            _mockProfileDbs.Setup(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash)).Returns(profileId);
            _mockProfileDbs.Setup(x => x.GetPrivateProfile(profileId)).Returns(expectedProfile);

            // Act
            var result = _profileMgm.LoginProfile(loginProfile);

            // Assert
            Assert.AreEqual(profileId, result.ProfileId);
            Assert.AreEqual("testuser", result.UserName);
            Assert.AreEqual("test@example.com", result.EMail);
            Assert.AreEqual(0, result.HarvestUploads.Count);
            _mockProfileDbs.Verify(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash), Times.Once);
            _mockProfileDbs.Verify(x => x.GetPrivateProfile(profileId), Times.Once);
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
            Assert.ThrowsException<UnauthorizedAccessException>(() => _profileMgm.LoginProfile(loginProfile));
            _mockProfileDbs.Verify(x => x.CheckPassword(loginProfile.EMail, loginProfile.PasswordHash), Times.Once);
        }

        [TestMethod]
        public void GetPreference_ValidProfileId_ReturnsCompletePreferences()
        {
            // Arrange - Komplette PreferenceDtos
            int profileId = 1;
            var expectedPreferences = new[]
            {
                new PreferenceDto { TagId = 1, Profileid = profileId, DateUpdated = DateTime.UtcNow },
                new PreferenceDto { TagId = 2, Profileid = profileId, DateUpdated = DateTime.UtcNow }
            }.AsQueryable();
            _mockProfileDbs.Setup(x => x.GetUserPreference(profileId)).Returns(expectedPreferences);

            // Act
            var result = _profileMgm.GetPreference(profileId);

            // Assert - Vollständige Prüfung
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].TagId);
            Assert.AreEqual(profileId, result[0].Profileid);
            Assert.AreEqual(2, result[1].TagId);
            _mockProfileDbs.Verify(x => x.GetUserPreference(profileId), Times.Once);
        }

        [TestMethod]
        public void SetPreference_Success_ReturnsTrue()
        {
            // Arrange - Komplette PreferenceDtos
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
        }
    }
}