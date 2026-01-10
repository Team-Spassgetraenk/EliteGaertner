using System;
using System.Linq;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.IntegrationTests.AppLogicTests;


//Komplett durch ChatGPT generiert!!!
[TestClass]
[DoNotParallelize]
[TestCategory("Integration")]
public class UploadServiceImplTest_FromRealDb : IntegrationTestBase
{
    public TestContext TestContext { get; set; } = null!;

    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=elitegaertner_test;Username=postgres;Password=postgres";

    private static EliteGaertnerDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new EliteGaertnerDbContext(options);
    }

    [TestMethod]
    public void Create_Get_Delete_Upload_ShouldWorkAgainstRealDb()
    {
        using var db = CreateDb();
        using var tx = db.Database.BeginTransaction(); // rollback am Ende

        var harvestDbs = new HarvestDbs(db);
        var sut = new UploadServiceImpl(harvestDbs);

        // Für CreateUploadDbs muss mindestens ein gültiger Tag angegeben werden
        var existingTagId = db.Tags.AsNoTracking().Select(t => t.Tagid).First();

        // Arrange
        const int profileId = 1; // Seed: tomatentiger hat i.d.R. ProfileId=1
        var uniqueUrl = $"/uploads/it_{Guid.NewGuid():N}.jpg";
        var description = "IntegrationTest UploadServiceImpl";
        const float weight = 123f;
        const int width = 4;
        const int length = 5;

        // Act 1: Create
        var uploadDto = new HarvestUploadDto
        {
            ProfileId = profileId,
            ImageUrl = uniqueUrl,
            Description = description,
            WeightGram = weight,
            WidthCm = width,
            LengthCm = length,
            UploadDate = DateTime.UtcNow,
            TagIds = new System.Collections.Generic.List<int> { existingTagId }
        };

        sut.CreateHarvestUpload(uploadDto);

        // UploadId aus DB holen (über unique URL)
        var createdEntity = db.Harvestuploads
            .AsNoTracking()
            .SingleOrDefault(h => h.Imageurl == uniqueUrl);

        Assert.IsNotNull(createdEntity, "Upload muss in der DB vorhanden sein.");
        var uploadId = createdEntity!.Uploadid;

        TestContext.WriteLine($"Created UploadId={uploadId}, ProfileId={profileId}, Url={uniqueUrl}");

        // Assert 2: Created upload should be retrievable via DataManagement
        var dto = harvestDbs.GetHarvestUploadDto(uploadId);

        Assert.IsNotNull(dto, "Nach Create muss der Upload über HarvestDbs wieder geladen werden können.");
        Assert.AreEqual(uploadId, dto.UploadId);
        Assert.AreEqual(profileId, dto.ProfileId);
        Assert.AreEqual(uniqueUrl, dto.ImageUrl);
        Assert.AreEqual(description, dto.Description);
        Assert.AreEqual(weight, dto.WeightGram);
        Assert.AreEqual(width, dto.WidthCm);
        Assert.AreEqual(length, dto.LengthCm);
        Assert.IsNotNull(dto.TagIds);
        Assert.IsTrue(dto.TagIds.Contains(existingTagId), "Der Upload muss den gesetzten Tag enthalten.");

        // Act 3: Delete (liefert ImageUrl zurück)
        var returnedFileName = sut.DeleteHarvestUpload(uploadId);

        // Assert Delete
        Assert.AreEqual(uniqueUrl, returnedFileName, "DeleteHarvestUpload soll die ImageUrl zurückgeben.");

        var stillExists = db.Harvestuploads.AsNoTracking().Any(h => h.Uploadid == uploadId);
        Assert.IsFalse(stillExists, "Upload muss nach DeleteHarvestUpload aus der DB entfernt sein.");

        tx.Rollback(); // Datenbank bleibt wie Seed
    }

    [TestMethod]
    public void DeleteUpload_ShouldReturnNull_WhenUploadDoesNotExist()
    {
        using var db = CreateDb();
        var harvestDbs = new HarvestDbs(db);
        var sut = new UploadServiceImpl(harvestDbs);

        // Arrange
        const int nonExistingUploadId = 9_999_999; // positive ID, die in der Seed-DB nicht existiert

        // Act
        var result = sut.DeleteHarvestUpload(nonExistingUploadId);

        // Assert
        Assert.IsNull(result, "Wenn Upload nicht existiert, soll DeleteUpload null zurückgeben.");
    }
}