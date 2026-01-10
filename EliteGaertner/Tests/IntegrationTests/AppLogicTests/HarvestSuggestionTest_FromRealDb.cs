using System;
using System.Collections.Generic;
using System.Linq;
using AppLogic.Services;
using Contracts.Data_Transfer_Objects;
using DataManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.IntegrationTests.AppLogicTests;

//Durch ChatGPT generiert!!!
[TestClass]
[DoNotParallelize]
[TestCategory("Integration")]
public class HarvestSuggestionTest_FromRealDb : IntegrationTestBase
{
    public TestContext TestContext { get; set; } = null!;
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=elitegaertner_test;Username=postgres;Password=postgres";

    [TestMethod]
    public void HarvestSuggestionTest()
    {
        // Arrange: echten DbContext bauen
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
       
        
        using var db = new EliteGaertnerDbContext(options);
        
        //Loggt mit wie viel Tupel er in den Tabellen finden kann.
        //Damit kann man prüfen, ob die DbContext funktioniert.
        TestContext.WriteLine("------------------------------------");
        TestContext.WriteLine("--ANZAHL DER TUPEL IN DEN TABELLEN--");
        TestContext.WriteLine($"Profiles: {db.Profiles.Count()}");
        TestContext.WriteLine($"Tags: {db.Tags.Count()}");
        TestContext.WriteLine($"Uploads: {db.Harvestuploads.Count()}");
        //Da es sich um eine reine Join Tabelle handelt, kann man nicht direkt auf die Tabelle zugreifen.
        var harvestTagsCount = db.Set<Dictionary<string, object>>("Harvesttag").Count();
        TestContext.WriteLine($"HarvestTags: {harvestTagsCount}");
        TestContext.WriteLine($"ProfilePreferences: {db.Profilepreferences.Count()}");
        TestContext.WriteLine($"Rating: {db.Ratings.Count()}");
        TestContext.WriteLine($"Reports: {db.Reports.Count()}");
        TestContext.WriteLine("------------------------------------");

        // Nach dem Split: HarvestDbs übernimmt die Harvest-spezifischen DB-Zugriffe
        var harvestDbs = new HarvestDbs(db);
        // MatchesDbs wird benötigt, um bereits bewertete ProfileIds auszuschließen
        var matchesDbs = new MatchesDbs(db);
        
        //TestUser DTO erstellen (TomatenTiger)
        var testDto = new PrivateProfileDto()
        {
            ProfileId = 1,
            PreferenceDtos = new List<PreferenceDto>
            {
                new PreferenceDto() { TagId = 3, Profileid = 1 }, //Tomaten
                new PreferenceDto() { TagId = 6, Profileid = 1 }, //Zuchini
                new PreferenceDto() { TagId = 9, Profileid = 1 }, //Salate
                new PreferenceDto() { TagId = 10, Profileid = 1 }, //Zwiebeln
                new PreferenceDto() { TagId = 17, Profileid = 1 }, //Trauben
                new PreferenceDto() { TagId = 22, Profileid = 1 }, //Mais
                new PreferenceDto() { TagId = 11, Profileid = 1 }, //Melonen
                new PreferenceDto() { TagId = 18, Profileid = 1 }, //Bohnen 
                new PreferenceDto() { TagId = 19, Profileid = 1 }, //Spinat
                new PreferenceDto() { TagId = 14, Profileid = 1 }, //Pfirsiche
                new PreferenceDto() { TagId = 8, Profileid = 1 }, //Karotten
            }
        };
        
        //Aufbereiten der ProfileId und TagId List
        var profileId = testDto.ProfileId;
        var tagIds = testDto.PreferenceDtos
            .Select(p => p.TagId)
            .Distinct()
            .ToList();
        
        
        // Bereits bewertete ProfileIds (Receiver = profileId) laden und an HarvestSuggestion weiterreichen
        var alreadyRatedProfiles = matchesDbs.GetAlreadyRatedProfileIds(profileId);
        //Testdaten werden jetzt an die Klasse übergeben
        var testSuggestions = new HarvestSuggestion(harvestDbs, profileId, tagIds, alreadyRatedProfiles, 10);
        var result = testSuggestions.GetHarvestSuggestionList();
        
        //Logging der Testresult
        TestContext.WriteLine("--TESTERGEBNIS--");
        TestContext.WriteLine($"AlreadyRatedProfiles (count={alreadyRatedProfiles.Count}): {{ {string.Join(", ", alreadyRatedProfiles.OrderBy(x => x))} }}");
        TestContext.WriteLine($"Result count: {result?.Count ?? 0}");
        TestContext.WriteLine("Erhaltene Uploads:");
        if (result != null)
            foreach (var r in result)
                TestContext.WriteLine(
                    $"UploadId={r.UploadId}, ProfileId={r.ProfileId}, UploadDate={r.UploadDate}");
        
        //Überprüfung
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());

        // Erwartung: keine Uploads vom Receiver selbst und keine Uploads von bereits bewerteten Profilen
        Assert.IsTrue(result.All(r => r.ProfileId != profileId), "Es dürfen keine eigenen Uploads vorgeschlagen werden.");
        Assert.IsTrue(result.All(r => !alreadyRatedProfiles.Contains(r.ProfileId)), "Es dürfen keine Uploads von bereits bewerteten Profilen vorgeschlagen werden.");

        // Erwartung: maximal preloadCount Ergebnisse
        Assert.IsTrue(result.Count <= 10, "Es dürfen nicht mehr als preloadCount Uploads zurückkommen.");
    }
}