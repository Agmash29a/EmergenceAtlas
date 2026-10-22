namespace EmergenceAtlas.Core.Experiments;

public sealed class ExperimentRunner
{
    private readonly IReadOnlyList<IExperiment> experiments;

    public ExperimentRunner(IEnumerable<IExperiment>? experiments = null)
    {
        this.experiments = (experiments ?? new IExperiment[]
        {
            new CollectionExperiment(),
            new GraphExperiment(),
            new InformationExperiment(),
            new IsingExperiment()
        }).ToArray();
    }

    public IReadOnlyList<string> Available => experiments.Select(experiment => experiment.Key).ToArray();

    public IReadOnlyList<ExperimentResult> Run(string selection, ExperimentOptions options)
    {
        var chosen = selection.Equals("all", StringComparison.OrdinalIgnoreCase)
            ? experiments
            : experiments.Where(e => e.Key.Equals(selection, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (chosen.Count == 0)
            throw new ArgumentException($"Unknown experiment '{selection}'. Valid: all, {string.Join(", ", Available)}.");
        return chosen.Select(experiment => experiment.Run(options)).ToArray();
    }
}
