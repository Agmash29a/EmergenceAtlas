namespace EmergenceAtlas.Core.Analysis;

public sealed class InformationTheory
{
    public double Entropy(IReadOnlyList<int> outcomes)
    {
        if (outcomes.Count == 0) throw new ArgumentException("Data cannot be empty.", nameof(outcomes));
        return outcomes.GroupBy(x => x).Sum(group => NegativePLogP((double)group.Count() / outcomes.Count));
    }

    public double MutualInformation(IReadOnlyList<int> x, IReadOnlyList<int> y)
    {
        if (x.Count == 0 || x.Count != y.Count)
            throw new ArgumentException("Series must be nonempty and have the same length.");
        int n = x.Count;
        var xCounts = new Dictionary<int, int>();
        var yCounts = new Dictionary<int, int>();
        var xyCounts = new Dictionary<(int X, int Y), int>();
        for (int i = 0; i < n; i++)
        {
            xCounts[x[i]] = xCounts.GetValueOrDefault(x[i]) + 1;
            yCounts[y[i]] = yCounts.GetValueOrDefault(y[i]) + 1;
            var key = (x[i], y[i]);
            xyCounts[key] = xyCounts.GetValueOrDefault(key) + 1;
        }
        double information = 0;
        foreach (var (pair, jointCount) in xyCounts)
        {
            double pxy = (double)jointCount / n;
            double px = (double)xCounts[pair.X] / n;
            double py = (double)yCounts[pair.Y] / n;
            information += pxy * Math.Log2(pxy / (px * py));
        }
        return Math.Abs(information) < 1e-12 ? 0 : information;
    }

    private static double NegativePLogP(double p) => p <= 0 ? 0 : -p * Math.Log2(p);
}
