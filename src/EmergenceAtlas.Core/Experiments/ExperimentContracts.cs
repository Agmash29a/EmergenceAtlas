namespace EmergenceAtlas.Core.Experiments;

public sealed record ExperimentOptions(
    int Seed = 42,
    int CountMaxParticles = 20,
    int GraphMaxParticles = 5,
    int IsingSide = 16,
    int WarmupSweeps = 350,
    int SampleSweeps = 600);

/// <summary>Long-form numeric observation. "Property" is an operational metric, not an ontological claim.</summary>
public sealed record ExperimentRow(
    string Experiment, string Variant, int Particles, string Parameter,
    string Metric, double Value, string Unit, string Interpretation);

public sealed record ExperimentResult(string Name, string Description,
    IReadOnlyList<ExperimentRow> Rows, string Details,
    IReadOnlyList<EmergenceAtlas.Core.Analysis.PropertyFinding>? Findings = null);

public interface IExperiment
{
    string Key { get; }
    ExperimentResult Run(ExperimentOptions options);
}

public sealed record AtlasEntry(
    string Name, string Category, string System, string RequiredOrganisation,
    string Observable, string OperationalCriterion, string Control,
    string Limitations);
