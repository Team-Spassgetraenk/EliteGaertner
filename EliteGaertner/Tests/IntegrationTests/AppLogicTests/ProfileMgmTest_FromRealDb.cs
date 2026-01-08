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

    private static string ConnectionString
    {
        get
        {
            // Ermöglicht parallelen Betrieb (z.B. Tests auf 5433) ohne dass Tests hart an 5432 hängen
            var port = Environment.GetEnvironmentVariable("ELITEGAERTNER_TEST_DB_PORT") ?? "5432";
            return $"Host=localhost;Port={port};Database=elitegaertner_test;Username=postgres;Password=postgres";
        }
    }

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
        var p = db.Profiles.AsNoTracking().Single(x => x.Username.ToLower() == "tomatentiger");
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

        // Uploads: Seed kann erweitert werden (z.B. mehr als 3 Einträge).
        Assert.IsNotNull(result.HarvestUploads);
        Assert.IsTrue(result.HarvestUploads.Count > 0, "Es müssen HarvestUploads vorhanden sein.");
        Assert.IsTrue(result.HarvestUploads.Count >= 3, "Seed-Erwartung: TomatenTiger hat mindestens 3 Uploads.");

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

        var result = sut.GetPrivProfile(profileId);

        Assert.IsNotNull(result);
        Assert.AreEqual(profileId, result.ProfileId);
        Assert.AreEqual(username, result.UserName);
        Assert.AreEqual(email, result.EMail);

        // Uploads: Erwartung dynamisch aus DB ableiten (Seed kann sich ändern)
        Assert.IsNotNull(result.HarvestUploads);

        var expectedUploadCount = db.Harvestuploads.AsNoTracking().Count(h => h.Profileid == profileId);
        Assert.AreEqual(expectedUploadCount, result.HarvestUploads.Count, "HarvestUploads.Count muss der DB-Anzahl entsprechen.");

        // Preferences: Seed kann erweitert werden (z.B. mehr als 3 Einträge).
        Assert.IsNotNull(result.PreferenceDtos);
        Assert.IsTrue(result.PreferenceDtos.Count > 0, "Es müssen Preferences vorhanden sein.");

        // Basis-Erwartung: TomatenTiger enthält mindestens Tomaten(3), Paprika(5), Zucchini(6).
        var tagIds = result.PreferenceDtos.Select(p => p.TagId).Distinct().ToList();
        CollectionAssert.IsSubsetOf(new List<int> { 3, 5, 6 }, tagIds);
    }

    [TestMethod]
    public void LoginProfile_FromRealDb_WithValidCredentials_ShouldReturnCompletePrivateProfile()
    {
        using var db = CreateDb();

        var (profileId, username, email, passwordHash) = GetTomatenTiger(db);

        // Wir ändern DB-Werte -> Transaction, damit der Test sauber bleibt.
        using var tx = db.Database.BeginTransaction();

        const string plainPassword = "pw_test_123";
        var pEntity = db.Profiles.Single(p => p.Profileid == profileId);
        pEntity.Passwordhash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        db.SaveChanges();

        var profileDbs = new ProfileDbs(db);
        var harvestDbs = new HarvestDbs(db);
        var sut = new ProfileMgm(profileDbs, harvestDbs);

        var credentials = new CredentialProfileDto
        {
            EMail = email,
            // Achtung: In der aktuellen Implementierung ist das Feld "PasswordHash" semantisch eigentlich Klartext-Passwort.
            // ProfileDbs.CheckPassword() macht BCrypt.Verify(klartext, gespeicherterHash)
            PasswordHash = plainPassword
        };

        var result = sut.LoginProfile(credentials);

        Assert.IsNotNull(result);
        Assert.AreEqual(profileId, result.ProfileId);
        Assert.AreEqual(username, result.UserName);
        Assert.AreEqual(email, result.EMail);

        Assert.IsNotNull(result.HarvestUploads);
        Assert.AreEqual(5, result.HarvestUploads.Count);

        Assert.IsNotNull(result.PreferenceDtos);
        Assert.IsTrue(result.PreferenceDtos.Count > 0, "Es müssen Preferences vorhanden sein.");

        // Seed-Erwartung: TomatenTiger enthält mindestens Tomaten(3), Paprika(5), Zucchini(6).
        // Falls ihr im Seed weitere Preferences ergänzt habt (z.B. 5 statt 3), soll der Test nicht unnötig brechen.
        var prefTagIds = result.PreferenceDtos.Select(p => p.TagId).Distinct().ToList();
        CollectionAssert.IsSubsetOf(new List<int> { 3, 5, 6 }, prefTagIds);

        tx.Rollback();
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