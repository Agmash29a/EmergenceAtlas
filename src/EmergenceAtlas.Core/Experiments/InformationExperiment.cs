using EmergenceAtlas.Core.Analysis;

namespace EmergenceAtlas.Core.Experiments;

public sealed class InformationExperiment : IExperiment
{
    private readonly BinaryGateAnalyzer analyzer = new();
    public string Key => "information";

    public ExperimentResult Run(ExperimentOptions options)
    {
        var rows = new List<ExperimentRow>();
        var table = new System.Text.StringBuilder();
        var detector = new JointInformationGainDetector();
        var findings = new List<PropertyFinding>();
        table.AppendLine(" Gate        H(B)   I(A1;B)  I(A2;B)  I((A1,A2);B)  extra joint info");
        foreach (var gate in Enum.GetValues<BinaryGate>())
        {
            GateSummary s = analyzer.Analyse(gate);
            findings.Add(detector.Evaluate(s));
            Add(gate, "OutputEntropy", s.OutputEntropyBits);
            Add(gate, "LeftMutualInformation", s.LeftInformationBits);
            Add(gate, "RightMutualInformation", s.RightInformationBits);
            Add(gate, "JointMutualInformation", s.JointInformationBits);
            Add(gate, "ExtraJointPredictiveInformation", s.ExtraJointPredictiveInformationBits);
            table.AppendLine($" {gate,-10} {s.OutputEntropyBits,5:F3} {s.LeftInformationBits,9:F3} {s.RightInformationBits,9:F3} {s.JointInformationBits,14:F3} {s.ExtraJointPredictiveInformationBits,17:F3}");
        }
        table.AppendLine("Independent uniform A1/A2; B is a deterministic gate output.");
        table.AppendLine("Extra joint information = joint MI minus the larger single-input MI.");
        table.AppendLine("This is NOT a general PID synergy estimator and is NOT a proof of emergence.");
        return new ExperimentResult(Key,
            "XOR illustrates a collective variable that requires both inputs; copy and AND are useful comparators.",
            rows, table.ToString(), findings);

        void Add(BinaryGate gate, string metric, double value) =>
            rows.Add(new ExperimentRow(Key, gate.ToString(), 2, "independent fair binary inputs",
                metric, value, "bits", "Joint gain is a simple predictive comparison, not an ontological emergence test."));
    }
}
