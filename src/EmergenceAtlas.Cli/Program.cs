using System.Globalization;
using EmergenceAtlas.Core.Experiments;
using EmergenceAtlas.Core.Reporting;

namespace EmergenceAtlas.Cli;

internal static class Program
{
    private static int Main(string[] args)
    {
        try
        {
            if (args.Contains("--help", StringComparer.OrdinalIgnoreCase) ||
                args.Contains("-h", StringComparer.OrdinalIgnoreCase))
            {
                ShowHelp();
                return 0;
            }
            string experiment = args.Length > 0 && !args[0].StartsWith("--", StringComparison.Ordinal)
                ? args[0] : "all";
            string output = GetOption(args, "--out") ?? "output";
            var options = new ExperimentOptions(
                Seed: GetInt(args, "--seed", 42),
                CountMaxParticles: GetInt(args, "--max-count", 20),
                GraphMaxParticles: GetInt(args, "--max-graph", 5),
                IsingSide: GetInt(args, "--side", 16),
                WarmupSweeps: GetInt(args, "--warmup", 350),
                SampleSweeps: GetInt(args, "--sweeps", 600));

            var runner = new ExperimentRunner();
            var results = runner.Run(experiment, options);
            Console.WriteLine("EMERGENCE ATLAS — computational demonstrations");
            Console.WriteLine(new string('=', 58));
            foreach (var result in results)
            {
                Console.WriteLine($"\n[{result.Name.ToUpperInvariant()}] {result.Description}");
                Console.WriteLine(result.Details);
            }
            new ReportExporter().Export(output, results);
            Console.WriteLine($"Report: {Path.GetFullPath(Path.Combine(output, "report.html"))}");
            Console.WriteLine($"Raw data: {Path.GetFullPath(Path.Combine(output, "measurements.csv"))}");
            Console.WriteLine($"Atlas definitions: {Path.GetFullPath(Path.Combine(output, "atlas.json"))}");
            Console.WriteLine($"Detector results: {Path.GetFullPath(Path.Combine(output, "findings.json"))}");
            Console.WriteLine("\nIMPORTANT: descriptive and computational metrics ≠ proof of ontological emergence.");
            return 0;
        }
        catch (Exception e) when (e is ArgumentException or FormatException or OverflowException or IOException)
        {
            Console.Error.WriteLine($"Error: {e.Message}");
            Console.Error.WriteLine("Use --help for command-line options.");
            return 1;
        }
    }

    private static string? GetOption(string[] args, string name)
    {
        int index = Array.FindIndex(args, a => a.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (index < 0) return null;
        if (index + 1 >= args.Length || args[index + 1].StartsWith("--", StringComparison.Ordinal))
            throw new ArgumentException($"Missing value for {name}.");
        return args[index + 1];
    }

    private static int GetInt(string[] args, string name, int defaultValue)
    {
        string? value = GetOption(args, name);
        return value is null ? defaultValue : int.Parse(value, CultureInfo.InvariantCulture);
    }

    private static void ShowHelp()
    {
        Console.WriteLine("""
            Emergence Atlas — .NET 8 console experiments

            Usage:
              dotnet run --project src/EmergenceAtlas.Cli -- [all|count|graphs|information|ising] [options]

            Options:
              --out FOLDER        Output folder containing report.html, measurements.csv, atlas.json
              --seed N            Reproducible pseudo-random seed (default 42)
              --max-count N       Largest particle count in counting experiment (1-30; default 20)
              --max-graph N       Largest graph size to enumerate (1-5; default 5)
              --side N            Ising lattice side length (4-128; default 16)
              --warmup N          Ising warm-up sweeps (default 350)
              --sweeps N          Ising measured sweeps (default 600; minimum 5)
              --help              Show this help

            Tests:
              dotnet run --project tests/EmergenceAtlas.Tests

            The program does not claim to prove emergence. See README.md.
            """);
    }
}
