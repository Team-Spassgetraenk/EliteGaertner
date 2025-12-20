using Contracts.Data_Transfer_Objects;
using DataManagement;
using DataManagement.Entities;
using Microsoft.EntityFrameworkCore;
using AppLogic.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Tests.UnitTests.AppLogicTests;

public class UploadServiceImplTests : IDisposable
{
    private readonly EliteGaertnerDbContext _context;
    private readonly UploadServiceImpl _service;

    public UploadServiceImplTests()
    {
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EliteGaertnerDbContext(options);
        _context.Database.EnsureCreated();
        _service = new UploadServiceImpl(_context);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    [Fact]
    public void CreateUpload_ValidDtoWithExistingProfile_ReturnsTrue()
    {
        var testProfile = new Profile
        { 
            //Arrange
            Profileid = 1,
            Username = "testuser",
            Firstname = "Test",
            Lastname = "User",
            Email = "test@test.com",
            Passwordhash = "hash",
            Phonenumber = "123",
            Profiletext = "test",
            Sharemail = true,
            Sharephonenumber = true,
            Usercreated = DateTime.UtcNow
        };
        _context.Profiles.Add(testProfile);
        _context.SaveChanges();

        var dto = new HarvestUploadDto
        {
            ProfileId = 1,
            ImageUrl = "https://test.com/tomate.jpg",
            Description = "Rote Tomaten",
            WeightGram = 200,
            WidthCm = 8,
            LengthCm = 8,
            UploadDate = DateTime.UtcNow
        };

        // Act
        var result = _service.CreateUpload(dto);

        // Assert
        Assert.True(result);
        var savedUpload = _context.Harvestuploads.FirstOrDefault(u => u.Imageurl == dto.ImageUrl);
        Assert.NotNull(savedUpload);
        Assert.Equal(dto.Description, savedUpload.Description);
        Assert.Equal(dto.WeightGram, savedUpload.Weightgramm);
        Assert.Equal(dto.ProfileId, savedUpload.Profileid);
    }

    [Fact]
    public void CreateUpload_NonExistingProfile_ReturnsFalse()
    {
        // Arrange
        var dto = new HarvestUploadDto
        {
            ProfileId = 999,
            ImageUrl = "https://test.com/fake.jpg",
            Description = "Fake",
            WeightGram = 100f,
            WidthCm = 5,
            LengthCm = 5,
            UploadDate = DateTime.UtcNow
        };

        // Act
        var result = _service.CreateUpload(dto);

        // Assert
        Assert.False(result);
        Assert.Empty(_context.Harvestuploads);
    }
}