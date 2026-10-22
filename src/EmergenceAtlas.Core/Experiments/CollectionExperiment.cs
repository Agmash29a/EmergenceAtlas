using EmergenceAtlas.Core.Analysis;

namespace EmergenceAtlas.Core.Experiments;

public sealed class CollectionExperiment : IExperiment
{
    private readonly CollectionCounter counter = new();
    public string Key => "count";

    public ExperimentResult Run(ExperimentOptions options)
    {
        if (options.CountMaxParticles < 1 || options.CountMaxParticles > 30)
            throw new ArgumentOutOfRangeException(nameof(options), "Counting demo supports n=1..30.");
        var rows = new List<ExperimentRow>();
        var table = new System.Text.StringBuilder();
        table.AppendLine(" n  floor sum   size types   labelled subsets");
        for (int n = 1; n <= options.CountMaxParticles; n++)
        {
            long divisor = counter.SummatoryDivisorCount(n);
            int types = counter.CountSizeCategories(n);
            double subsets = (double)counter.CountNonemptySubsets(n);
            rows.Add(Row(n, "Floor-partition", "MultiscaleCount", divisor,
                "Sum floor(n/k); one non-overlapping partition per scale."));
            rows.Add(Row(n, "Size-only", "SizeCategories", types,
                "One category per integer collection size."));
            rows.Add(Row(n, "Labelled subsets", "NonemptySubsets", subsets,
                "All nonempty subsets of n distinguishable constituents."));
            table.AppendLine($"{n,2} {divisor,10} {types,12} {subsets,18:0}");
        }
        return new ExperimentResult(Key,
            "Same A-particle universe, three valid counting conventions. None counts emergent physical properties.",
            rows, table.ToString());
    }

    private static ExperimentRow Row(int n, string variant, string metric, double value, string interpretation) =>
        new("count", variant, n, "none", metric, value, "count", interpretation);
}
