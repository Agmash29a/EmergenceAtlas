using EmergenceAtlas.Core.Analysis;

namespace EmergenceAtlas.Core.Experiments;

public sealed class GraphExperiment : IExperiment
{
    private readonly GraphCatalogue graphs = new();
    public string Key => "graphs";

    public ExperimentResult Run(ExperimentOptions options)
    {
        if (options.GraphMaxParticles < 1 || options.GraphMaxParticles > 5)
            throw new ArgumentOutOfRangeException(nameof(options), "Graphs demo supports n=1..5.");
        var rows = new List<ExperimentRow>();
        var table = new System.Text.StringBuilder();
        var findings = new List<PropertyFinding>();
        var detector = new TriangleFeasibilityDetector();
        table.AppendLine(" n  labelled  unique graphs  connected   with cycles  with triangles");
        for (int n = 1; n <= options.GraphMaxParticles; n++)
        {
            GraphSummary s = graphs.Analyse(n);
            findings.Add(detector.Evaluate(s));
            Add(n, "LabelledGraphs", s.LabelledGraphs, "Distinguishable vertex labels.");
            Add(n, "DistinctTopologies", s.StructuralClasses, "Isomorphism classes; labels discarded.");
            Add(n, "ConnectedTopologies", s.ConnectedClasses, "Connected isomorphism classes.");
            Add(n, "CycleTopologies", s.ClassesContainingCycle, "Topology classes containing a cycle.");
            Add(n, "TriangleTopologies", s.ClassesContainingTriangle, "Topology classes containing a triangle.");
            table.AppendLine($"{n,2} {s.LabelledGraphs,9} {s.StructuralClasses,14} {s.ConnectedClasses,10} {s.ClassesContainingCycle,13} {s.ClassesContainingTriangle,15}");
        }
        return new ExperimentResult(Key,
            "Identical particle counts can admit different connectivity structures. Structural motifs are candidates, not automatically emergent properties.",
            rows, table.ToString(), findings);

        void Add(int n, string metric, int value, string note) =>
            rows.Add(new ExperimentRow(Key, "Undirected simple graph", n, "all edge assignments", metric, value, "count", note));
    }
}
