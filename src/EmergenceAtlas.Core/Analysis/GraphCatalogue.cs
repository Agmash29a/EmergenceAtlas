using EmergenceAtlas.Core.Models;

namespace EmergenceAtlas.Core.Analysis;

public sealed record GraphSummary(
    int Particles, int LabelledGraphs, int StructuralClasses,
    int ConnectedClasses, int ClassesContainingCycle, int ClassesContainingTriangle);

public sealed class GraphCatalogue
{
    public GraphSummary Analyse(int particleCount)
    {
        if (particleCount < 1 || particleCount > 5)
            throw new ArgumentOutOfRangeException(nameof(particleCount), "Graph enumeration is intentionally limited to n=1..5.");

        int possibleEdges = particleCount * (particleCount - 1) / 2;
        int labelledCount = 1 << possibleEdges;
        var representatives = new Dictionary<string, UndirectedGraph>(StringComparer.Ordinal);
        for (int mask = 0; mask < labelledCount; mask++)
        {
            var graph = UndirectedGraph.FromEdgeMask(particleCount, mask);
            var signature = graph.CanonicalSignature();
            representatives.TryAdd(signature, graph);
        }
        var classes = representatives.Values.ToArray();
        return new GraphSummary(
            particleCount,
            labelledCount,
            classes.Length,
            classes.Count(g => g.IsConnected()),
            classes.Count(g => g.HasCycle()),
            classes.Count(g => g.HasTriangle()));
    }
}
