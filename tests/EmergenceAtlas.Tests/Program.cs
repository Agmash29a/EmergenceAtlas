using EmergenceAtlas.Core.Analysis;
using EmergenceAtlas.Core.Experiments;
using EmergenceAtlas.Core.Models;
using EmergenceAtlas.Core.Reporting;

namespace EmergenceAtlas.Tests;

internal static class Program
{
    private static int passed;
    private static int failed;

    private static int Main()
    {
        Test("Five-particle floor sum equals ten", () => Equal(10L, new CollectionCounter().SummatoryDivisorCount(5)));
        Test("Two-particle floor sum equals three", () => Equal(3L, new CollectionCounter().SummatoryDivisorCount(2)));
        Test("Five distinguishable particles have 31 nonempty subsets", () =>
            Equal(System.Numerics.BigInteger.Parse("31"), new CollectionCounter().CountNonemptySubsets(5)));
        Test("Five particles have five collection-size categories", () => Equal(5, new CollectionCounter().CountSizeCategories(5)));
        Test("Collection disallows duplicate constituents", () => Throws<ArgumentException>(() =>
            new ParticleCollection(new[] { new Particle(1), new Particle(1) })));
        Test("Group counter rejects size zero", () => Throws<ArgumentOutOfRangeException>(() =>
            new CollectionCounter().CountCompleteGroups(5, 0)));
        Test("Empty graph is disconnected for n=3", () => True(!new UndirectedGraph(3).IsConnected()));
        Test("Triangle graph has a cycle and a triangle", () =>
        {
            var triangle = new UndirectedGraph(3);
            triangle.AddEdge(0, 1); triangle.AddEdge(1, 2); triangle.AddEdge(0, 2);
            True(triangle.IsConnected() && triangle.HasCycle() && triangle.HasTriangle());
        });
        Test("Three-vertex path is connected but acyclic", () =>
        {
            var path = new UndirectedGraph(3);
            path.AddEdge(0, 1); path.AddEdge(1, 2);
            True(path.IsConnected() && !path.HasCycle() && !path.HasTriangle());
        });
        Test("Graph canonical label is invariant under relabelling", () =>
        {
            var a = new UndirectedGraph(4);
            a.AddEdge(0, 1); a.AddEdge(1, 2); a.AddEdge(2, 3);
            var b = new UndirectedGraph(4);
            b.AddEdge(3, 1); b.AddEdge(1, 0); b.AddEdge(0, 2);
            Equal(a.CanonicalSignature(), b.CanonicalSignature());
        });
        Test("Unlabelled graph class counts n=1..5: 1,2,4,11,34", () =>
        {
            var cat = new GraphCatalogue();
            int[] expected = { 1, 2, 4, 11, 34 };
            for (int n = 1; n <= 5; n++) Equal(expected[n - 1], cat.Analyse(n).StructuralClasses);
        });
        Test("Connected unlabelled graph counts n=1..5: 1,1,2,6,21", () =>
        {
            var cat = new GraphCatalogue();
            int[] expected = { 1, 1, 2, 6, 21 };
            for (int n = 1; n <= 5; n++) Equal(expected[n - 1], cat.Analyse(n).ConnectedClasses);
        });
        Test("No triangle can exist with fewer than three vertices", () =>
        {
            var cat = new GraphCatalogue();
            Equal(0, cat.Analyse(2).ClassesContainingTriangle);
            Equal(1, cat.Analyse(3).ClassesContainingTriangle);
        });
        Test("Mutual information equals one for a perfect copy", () =>
            Near(1, new InformationTheory().MutualInformation(new[] { 0, 1 }, new[] { 0, 1 })));
        Test("Mutual information equals zero for independent fair pairs", () =>
            Near(0, new InformationTheory().MutualInformation(new[] { 0, 0, 1, 1 }, new[] { 0, 1, 0, 1 })));
        Test("XOR gives zero individual and one joint bit", () =>
        {
            var s = new BinaryGateAnalyzer().Analyse(BinaryGate.Xor);
            Near(0, s.LeftInformationBits);
            Near(0, s.RightInformationBits);
            Near(1, s.JointInformationBits);
            Near(1, s.ExtraJointPredictiveInformationBits);
        });
        Test("Copy gate has no extra joint predictive information", () =>
        {
            var s = new BinaryGateAnalyzer().Analyse(BinaryGate.CopyLeft);
            Near(0, s.ExtraJointPredictiveInformationBits);
            Near(1, s.LeftInformationBits);
            Near(0, s.RightInformationBits);
        });
        Test("AND gives output entropy approximately 0.811278 bits", () =>
            Near(0.8112781244591328, new BinaryGateAnalyzer().Analyse(BinaryGate.And).OutputEntropyBits));
        Test("Ising observables are bounded and reproducible", () =>
        {
            var analyzer = new IsingAnalyzer();
            var a = analyzer.Analyse(8, 1.5, 1, 173, 50, 100);
            var b = analyzer.Analyse(8, 1.5, 1, 173, 50, 100);
            Near(a.MeanAbsoluteMagnetization, b.MeanAbsoluteMagnetization);
            True(a.MeanAbsoluteMagnetization is >= 0 and <= 1);
            True(a.MeanNeighbourAgreement is >= 0 and <= 1);
        });
        Test("At low temperature interacting spins order more than noninteracting spins", () =>
        {
            var analyzer = new IsingAnalyzer();
            var independent = analyzer.Analyse(12, 1.5, 0, 44, 150, 300);
            var interacting = analyzer.Analyse(12, 1.5, 1, 44, 150, 300);
            True(interacting.MeanNeighbourAgreement > independent.MeanNeighbourAgreement + 0.2);
            True(interacting.MeanAbsoluteMagnetization > independent.MeanAbsoluteMagnetization + 0.2);
        });
        Test("A structural detector rejects n=2 and accepts n=3 triangles", () =>
        {
            var detector = new TriangleFeasibilityDetector();
            var cat = new GraphCatalogue();
            True(!detector.Evaluate(cat.Analyse(2)).CriterionMet);
            True(detector.Evaluate(cat.Analyse(3)).CriterionMet);
        });
        Test("A joint gain detector flags XOR but not CopyLeft", () =>
        {
            var detector = new JointInformationGainDetector();
            var analyzer = new BinaryGateAnalyzer();
            True(detector.Evaluate(analyzer.Analyse(BinaryGate.Xor)).CriterionMet);
            True(!detector.Evaluate(analyzer.Analyse(BinaryGate.CopyLeft)).CriterionMet);
        });
        Test("Spin-order contrast detector compares like-for-like conditions", () =>
        {
            var analyzer = new IsingAnalyzer();
            var independent = analyzer.Analyse(12, 1.5, 0, 44, 150, 300);
            var interacting = analyzer.Analyse(12, 1.5, 1, 44, 150, 300);
            True(new SpinOrderContrastDetector().Evaluate(new SpinOrderContrast(interacting, independent)).CriterionMet);
            var otherTemp = analyzer.Analyse(12, 3.5, 0, 44, 150, 300);
            Throws<ArgumentException>(() => new SpinOrderContrastDetector().Evaluate(new SpinOrderContrast(interacting, otherTemp)));
        });
        Test("Experiment selection excludes other experiments", () =>
        {
            var result = new ExperimentRunner().Run("information", new ExperimentOptions());
            Equal(1, result.Count);
            Equal("information", result[0].Name);
        });
        Test("CSV, JSON and HTML report files are generated", () =>
        {
            var destination = Path.Combine(Path.GetTempPath(), "atlas_tests_" + Guid.NewGuid().ToString("N"));
            try
            {
                new ReportExporter().Export(destination,
                    new ExperimentRunner().Run("information", new ExperimentOptions()));
                True(File.Exists(Path.Combine(destination, "measurements.csv")));
                True(File.Exists(Path.Combine(destination, "atlas.json")));
                True(File.Exists(Path.Combine(destination, "findings.json")));
                True(File.ReadAllText(Path.Combine(destination, "report.html")).Contains("An Atlas of Emergence"));
            }
            finally { if (Directory.Exists(destination)) Directory.Delete(destination, true); }
        });

        Console.WriteLine($"\nTest results: {passed} passed; {failed} failed.");
        return failed == 0 ? 0 : 1;
    }

    private static void Test(string name, Action action)
    {
        try { action(); Console.WriteLine($"PASS  {name}"); passed++; }
        catch (Exception e) { Console.WriteLine($"FAIL  {name}: {e.Message}"); failed++; }
    }

    private static void Equal<T>(T expected, T actual) where T : notnull
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new Exception($"Expected {expected}, got {actual}.");
    }

    private static void Near(double expected, double actual, double tolerance = 1e-9)
    {
        if (Math.Abs(expected - actual) > tolerance)
            throw new Exception($"Expected {expected} ±{tolerance}, got {actual}.");
    }

    private static void True(bool value)
    {
        if (!value) throw new Exception("Expected true.");
    }

    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new Exception($"Expected exception {typeof(T).Name}.");
    }
}
