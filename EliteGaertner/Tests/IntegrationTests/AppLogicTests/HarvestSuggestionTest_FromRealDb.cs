using System.Diagnostics;
using AppLogic.Services;
using Microsoft.EntityFrameworkCore;
using DataManagement;
using Contracts.Data_Transfer_Objects;

namespace Tests.IntegrationTests.AppLogicTests;

//WAS PRÜFT DIE TEST KLASSE?
//TestAnforderungen erklären und was am Ende rauskommen soll
    
    
[TestClass]
public class HarvestSuggestionTest_FromRealDb
{
    public TestContext TestContext { get; set; } = null!;
    private static readonly string ComposeFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../SetUp/docker-compose.yaml"));
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=elitegaertner_test;Username=postgres;Password=postgres";

    [ClassInitialize]
    public static void ClassInit(TestContext _)
    {
        Run("docker", "compose", "-f", ComposeFile, "down", "-v");
        Run("docker", "compose", "-f", ComposeFile, "up", "-d");
        //Kurze Pause, damit die Datenbank Zeit hat hochzufahren.
        Thread.Sleep(15000);
    }

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

        // Repo muss DbContext annehmen 
        var repo = new ManagementDbs(db);
        
        //TestUser DTO erstellen (TomatenTiger)
        var testDto = new ProfileDto()
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
        
        //Testdaten werden jetzt an die Klasse übergeben
        var testSuggestions = new HarvestSuggestion(repo, testDto, 10);
        var result = testSuggestions.GetHarvestSuggestionList();
        
        //Logging der Testresult
        TestContext.WriteLine("--TESTERGEBNIS--");
        TestContext.WriteLine("Erwartete UploadIds: {43, 17, 34, 26, 36, 31, 24, 10, 21, 22}");
        TestContext.WriteLine($"Result count: {result?.Count ?? 0}");
        TestContext.WriteLine("Erhaltene Uploads:");
        if (result != null)
            foreach (var r in result)
                TestContext.WriteLine(
                    $"UploadId={r.UploadId}, ProfileId={r.ProfileId}, UploadDate={r.UploadDate}");
        
        //Überprüfung
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());

        var resultIds = result.Select(r => r.UploadId).ToList();
        
        CollectionAssert.AreEquivalent( new[] {43, 17, 34, 26, 36, 31, 24, 10, 21, 22}, resultIds );
    }

    private static void Run(string file, params string[] args)
    {
        var psi = new ProcessStartInfo
        {
            FileName = file,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        foreach (var a in args)
            psi.ArgumentList.Add(a);

        using var p = Process.Start(psi);
        if (p == null)
            throw new AssertFailedException($"Failed to start process: {file}");

        p.WaitForExit();

        var stdout = p.StandardOutput.ReadToEnd();
        var stderr = p.StandardError.ReadToEnd();

        if (p.ExitCode != 0)
            throw new AssertFailedException(
                $"Command failed: {file} {string.Join(" ", args)}\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");
    }
}