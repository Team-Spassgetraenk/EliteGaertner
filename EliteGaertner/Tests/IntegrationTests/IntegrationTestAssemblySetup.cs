using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.IntegrationTests;

//Durch ChatGPT generiert!!!
//Initialisiert unsere Datenbank
[TestClass]
public sealed class IntegrationTestAssemblySetup
{
    private static readonly string ComposeFile =
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../SetUp/docker-compose.yaml"));

    private const string ComposeProjectName = "elitegaertner_tests";

    [AssemblyInitialize]
    public static void AssemblyInit(TestContext _)
    {
        // Einmal sauber aufräumen (falls ein vorheriger Run abgebrochen wurde)
        Run("docker", "compose", "-p", ComposeProjectName, "-f", ComposeFile, "down", "-v", "--remove-orphans");

        // Einmal DB hochfahren für ALLE Integrationstests
        Run("docker", "compose", "-p", ComposeProjectName, "-f", ComposeFile, "up", "-d", "--remove-orphans");

        // DB hochfahren lassen
        Thread.Sleep(15000);
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        // Einmal runterfahren nachdem ALLE Integrationstests fertig sind
        Run("docker", "compose", "-p", ComposeProjectName, "-f", ComposeFile, "down", "-v", "--remove-orphans");
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