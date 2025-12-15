namespace Tests.IntegrationTests.AppLogicTests;
using Microsoft.EntityFrameworkCore;


[TestClass]
public class HarvestSuggestionTest_FromRealDb
{
    private const string ComposeFile = "docker-compose.test.yml";
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=elitegaertner_test;Username=postgres;Password=postgres";

    [ClassInitialize]
    public static void ClassInit(TestContext _)
    {
        Run("docker", $"compose -f {ComposeFile} up -d");
        WaitForDbReady();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Run("docker", $"compose -f {ComposeFile} down -v");
    }

    [TestMethod]
    public void HarvestSuggestionTest_FromRealDb()
    {
        // Arrange: echten DbContext bauen
        var options = new DbContextOptionsBuilder<EliteGaertnerDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var db = new EliteGaertnerDbContext(options);

        // Repo muss DbContext annehmen (empfohlen)
        var repo = new ManagementDbs(db);

        var profileDto = new ProfileDto
        {
            UserId = 1,
            PreferenceDtos = new()
            {
                new PreferenceDto { TagId = 3 },
                new PreferenceDto { TagId = 7 }
            }
        };

        // Act
        var sut = new HarvestSuggestion(repo, profileDto, preloadCount: 10);
        var result = sut.GetHarvestSuggestionList();

        // Assert
        Assert.IsTrue(result.Count > 0);
        Assert.IsTrue(result.Select(x => x.ProfileId).Distinct().Count() == result.Count); // max 1 Upload pro User
    }

    private static void Run(string file, string args)
    {
        var p = Process.Start(new ProcessStartInfo
        {
            FileName = file,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        });
        p!.WaitForExit();
        if (p.ExitCode != 0)
            throw new AssertFailedException($"Command failed: {file} {args}\n{p.StandardError.ReadToEnd()}");
    }

    private static void WaitForDbReady()
    {
        // simpel: ein paar Sekunden warten oder besser: in Schleife Verbindung testen
        System.Threading.Thread.Sleep(3000);
    }
}