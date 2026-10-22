using System.Globalization;
using EmergenceAtlas.Core.Analysis;

namespace EmergenceAtlas.Core.Experiments;

public sealed class IsingExperiment : IExperiment
{
    private readonly IsingAnalyzer analyzer = new();
    private static readonly double[] Temperatures = { 1.5, 2.0, 2.269, 2.6, 3.5 };
    public string Key => "ising";

    public ExperimentResult Run(ExperimentOptions options)
    {
        var rows = new List<ExperimentRow>();
        var table = new System.Text.StringBuilder();
        var findings = new List<PropertyFinding>();
        var detector = new SpinOrderContrastDetector();
        table.AppendLine($"Ising {options.IsingSide}x{options.IsingSide}, warmup={options.WarmupSweeps}, samples every 5 sweeps over {options.SampleSweeps} sweeps; seed={options.Seed}");
        table.AppendLine(" T      J     abs magnetization   neighbour agreement");
        foreach (double temperature in Temperatures)
        {
            IsingSummary? independent = null;
            IsingSummary? coupled = null;
            foreach (double coupling in new[] { 0.0, 1.0 })
            {
                // Separate reproducible RNG streams for each parameter setting.
                int seed = options.Seed + (int)Math.Round(temperature * 1000) + (coupling > 0 ? 100000 : 0);
                IsingSummary s = analyzer.Analyse(options.IsingSide, temperature, coupling,
                    seed, options.WarmupSweeps, options.SampleSweeps);
                if (coupling == 0) independent = s; else coupled = s;
                string variant = coupling == 0 ? "Independent J=0" : "Coupled J=1";
                string parameter = $"T={temperature.ToString("0.000", CultureInfo.InvariantCulture)};J={coupling.ToString("0", CultureInfo.InvariantCulture)};samples={s.Samples}";
                Add("MeanAbsMagnetization", s.MeanAbsoluteMagnetization,
                    "Finite-size order indicator: average |sum of spins / N|.");
                Add("MeanSignedMagnetization", s.MeanSignedMagnetization,
                    "Signed magnetization can cancel across equally ordered ± phases.");
                Add("MeanNeighbourAgreement", s.MeanNeighbourAgreement,
                    "Fraction of equal nearest-neighbour bonds, each counted once.");
                table.AppendLine($" {temperature,4:F3}  {coupling,3:F1} {s.MeanAbsoluteMagnetization,18:F4} {s.MeanNeighbourAgreement,21:F4}");

                void Add(string metric, double value, string note) =>
                    rows.Add(new ExperimentRow(Key, variant, s.Side * s.Side, parameter,
                        metric, value, "fraction", note));
            }
            findings.Add(detector.Evaluate(new SpinOrderContrast(coupled!, independent!)));
        }
        table.AppendLine("J=0 is the no-interaction negative control. J=1 encourages spatial order at lower T.");
        table.AppendLine("Finite size/simulation length and random sampling prevent a proof of a sharp phase transition.");
        return new ExperimentResult(Key,
            "Collective order as temperature changes, compared against a non-interacting control.",
            rows, table.ToString(), findings);
    }
}
