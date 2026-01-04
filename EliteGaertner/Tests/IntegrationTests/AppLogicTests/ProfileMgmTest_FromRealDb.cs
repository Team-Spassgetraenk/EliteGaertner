using System;
using System.Collections.Generic;
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
public class ProfileMgmTest_FromRealDb : IntegrationTestBase
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

    private static (int profileId, string username, string email, string passwordHash) GetTomatenTiger(EliteGaertnerDbContext db)
    {
        // Seed: UserName = 'tomatentiger', EMail = 'tomatentiger@elitegaertner.test', PasswordHash = 'hash_tomate'
        var p = db.Profiles.AsNoTracking().Single(x => x.Username == "tomatentiger");
        return (p.Profileid, p.Username, p.Email, p.Passwordhash);
    }

    [TestMethod]
    public void CheckUsernameExists_FromRealDb_ShouldReturnTrue_ForSeedUser()
    {
        using var db = CreateDb();

        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);
        var sut = new ProfileMgm(profileDbs, harvestDbs);

        var result = sut.CheckUsernameExists("TomatenTiger"); // absichtlich anders geschrieben (Normalisierung)

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void VisitPublicProfile_FromRealDb_ShouldReturnProfile_WithHarvestUploads()
    {
        using var db = CreateDb();

        var (profileId, username, _, _) = GetTomatenTiger(db);

        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);
        var sut = new ProfileMgm(profileDbs, harvestDbs);

        var result = sut.VisitPublicProfile(profileId);

        Assert.IsNotNull(result);
        Assert.AreEqual(profileId, result.ProfileId);
        Assert.AreEqual(username, result.UserName);

        // Seed: TomatenTiger hat 3 Uploads (UploadId 1..3 in deinem Seed-SQL)
        Assert.IsNotNull(result.HarvestUploads);
        Assert.AreEqual(3, result.HarvestUploads.Count);

        // Konsistenz: alle Uploads müssen zum Profil gehören
        Assert.IsTrue(result.HarvestUploads.All(h => h.ProfileId == profileId));
    }

    [TestMethod]
    public void VisitPrivateProfile_FromRealDb_ShouldReturnProfile_WithPreferences_AndHarvestUploads()
    {
        using var db = CreateDb();

        var (profileId, username, email, _) = GetTomatenTiger(db);

        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);
        var sut = new ProfileMgm(profileDbs, harvestDbs);

        var result = sut.VisitPrivateProfile(profileId);

        Assert.IsNotNull(result);
        Assert.AreEqual(profileId, result.ProfileId);
        Assert.AreEqual(username, result.UserName);
        Assert.AreEqual(email, result.EMail);

        // Uploads (Seed: 3)
        Assert.IsNotNull(result.HarvestUploads);
        Assert.AreEqual(3, result.HarvestUploads.Count);

        // Preferences (Seed: TomatenTiger = Tomaten(3), Paprika(5), Zucchini(6))
        Assert.IsNotNull(result.PreferenceDtos);
        var tagIds = result.PreferenceDtos.Select(p => p.TagId).OrderBy(x => x).ToList();

        CollectionAssert.AreEqual(new List<int> { 3, 5, 6 }, tagIds);
    }

    [TestMethod]
    public void LoginProfile_FromRealDb_WithValidCredentials_ShouldReturnCompletePrivateProfile()
    {
        using var db = CreateDb();

        var (profileId, username, email, passwordHash) = GetTomatenTiger(db);

        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);
        var sut = new ProfileMgm(profileDbs, harvestDbs);

        var loginDto = new PrivateProfileDto
        {
            EMail = email,
            PasswordHash = passwordHash
        };

        var result = sut.LoginProfile(TODO);

        Assert.IsNotNull(result);
        Assert.AreEqual(profileId, result.ProfileId);
        Assert.AreEqual(username, result.UserName);
        Assert.AreEqual(email, result.EMail);

        Assert.IsNotNull(result.HarvestUploads);
        Assert.AreEqual(3, result.HarvestUploads.Count);

        Assert.IsNotNull(result.PreferenceDtos);
        Assert.AreEqual(3, result.PreferenceDtos.Count);
    }

    [TestMethod]
    public void UpdateContactVisibility_FromRealDb_ShouldPersistChanges()
    {
        using var db = CreateDb();

        var (profileId, _, _, _) = GetTomatenTiger(db);

        // Wir ändern DB-Werte -> Transaction, damit der Test sauber bleibt.
        using var tx = db.Database.BeginTransaction();

        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);
        var sut = new ProfileMgm(profileDbs, harvestDbs);

        // Vorher-Werte lesen
        var before = db.Profiles.AsNoTracking().Single(p => p.Profileid == profileId);

        var dto = new ContactVisibilityDto
        {
            profileId = profileId,
            // Email/Phone lassen wir null, damit wir nichts "Unique" riskieren
            ShareMail = !before.Sharemail,
            SharePhoneNumber = !before.Sharephonenumber
        };

        var ok = sut.UpdateContactVisibility(dto);
        Assert.IsTrue(ok);

        var after = db.Profiles.AsNoTracking().Single(p => p.Profileid == profileId);

        Assert.AreEqual(dto.ShareMail, after.Sharemail);
        Assert.AreEqual(dto.SharePhoneNumber, after.Sharephonenumber);

        // Rollback, damit Seed-Zustand erhalten bleibt
        tx.Rollback();
    }
}