using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;

namespace Tests.IntegrationTests;

public abstract class IntegrationTestBase
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=elitegaertner_test;Username=postgres;Password=postgres";

    private static readonly string SeedFile =
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../SetUp/db-init/seed_elitegaertner_test.sql"));

    [TestInitialize]
    public void ResetDatabase()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        conn.Open();

        // ALLES löschen
        using (var cmd = new NpgsqlCommand("TRUNCATE TABLE rating, report, harvesttags, harvestuploads, profilepreferences, profile, tags RESTART IDENTITY CASCADE;", conn))
        {
            cmd.ExecuteNonQuery();
        }

        // Seed neu laden
        if (!File.Exists(SeedFile))
            throw new AssertFailedException(
                $"Seed file not found at '{SeedFile}'. BaseDirectory='{AppContext.BaseDirectory}', CurrentDirectory='{Directory.GetCurrentDirectory()}'.");

        var rawSeedSql = File.ReadAllText(SeedFile);

        // This seed file was authored for the `psql` CLI. It uses `\gset` to store a value into a psql variable
        // and later references it via `:'seed_now'`. Npgsql cannot execute psql meta-commands or variables.
        // We translate that pattern to plain SQL so the script can be executed programmatically.
        rawSeedSql = rawSeedSql
            .Replace("\\gset", string.Empty)
            .Replace(":'seed_now'", "now()")
            .Replace(":\"seed_now\"", "now()");

        // In psql, `\gset` also acts as a statement terminator. After removing it, the SELECT may miss a semicolon.
        // Ensure the seed_now SELECT is properly terminated so the following INSERT starts a new statement.
        rawSeedSql = Regex.Replace(
            rawSeedSql,
            @"(SELECT\s+now\(\)\s+AS\s+seed_now)\s*(\r?\n)",
            "$1;\n",
            RegexOptions.IgnoreCase);

        // If this is a pg_dump/psql-style seed, it may contain psql meta-commands (\set, \i, \c, \.)
        // or COPY ... FROM stdin blocks. Those cannot be executed via ExecuteNonQuery.
        // We fail fast with a clear message if we detect COPY FROM stdin.
        if (Regex.IsMatch(rawSeedSql, @"\bCOPY\b[\s\S]*?\bFROM\s+stdin\b", RegexOptions.IgnoreCase))
        {
            throw new AssertFailedException(
                $"Seed file '{SeedFile}' looks like a psql/pg_dump dump and contains 'COPY ... FROM stdin'. " +
                "That format cannot be executed via NpgsqlCommand.ExecuteNonQuery(). " +
                "Please generate a seed script with plain INSERT statements (or we implement COPY import via Npgsql)." );
        }

        // Filter out psql meta-commands (lines starting with a backslash) like: \set, \i, \c, \.
        var sb = new StringBuilder();
        using (var reader = new StringReader(rawSeedSql))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var trimmed = line.TrimStart();
                if (trimmed.StartsWith("\\"))
                    continue;

                // Also skip psql meta-commands that sometimes appear alone (defensive)
                if (trimmed.Equals("\\gset", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.Equals("\\g", StringComparison.OrdinalIgnoreCase) ||
                    trimmed.Equals("\\gexec", StringComparison.OrdinalIgnoreCase))
                    continue;

                sb.AppendLine(line);
            }
        }

        var seedSql = sb.ToString();

        try
        {
            using var cmd = new NpgsqlCommand(seedSql, conn);
            cmd.ExecuteNonQuery();
        }
        catch (PostgresException ex)
        {
            var posText = Convert.ToString(ex.Position, CultureInfo.InvariantCulture);
            if (!string.IsNullOrWhiteSpace(posText) && int.TryParse(posText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var pos))
            {
                // `pos` is 1-based character position into the SQL string.
                var p = Math.Max(0, pos - 1);
                var start = Math.Max(0, p - 160);
                var len = Math.Min(seedSql.Length - start, 320);
                var snippet = seedSql.Substring(start, len);

                throw new AssertFailedException(
                    $"Seed SQL failed (SqlState={ex.SqlState}). Message: {ex.MessageText}. " +
                    $"Position={posText}. SeedFile='{SeedFile}'.\n" +
                    "--- SQL snippet around error ---\n" + snippet + "\n--- end snippet ---\n" +
                    $"BaseDirectory='{AppContext.BaseDirectory}', CurrentDirectory='{Directory.GetCurrentDirectory()}'.",
                    ex);
            }

            throw new AssertFailedException(
                $"Seed SQL failed (SqlState={ex.SqlState}). Message: {ex.MessageText}. SeedFile='{SeedFile}'.\n" +
                $"BaseDirectory='{AppContext.BaseDirectory}', CurrentDirectory='{Directory.GetCurrentDirectory()}'.",
                ex);
        }
    }
}