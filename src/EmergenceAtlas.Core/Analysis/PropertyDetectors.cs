using EmergenceAtlas.Core.Models;

namespace EmergenceAtlas.Core.Analysis;

/// <summary>A testable statement about a defined observable, never an automatic proof of ontological emergence.</summary>
public sealed record PropertyFinding(
    string PropertyName, string Category, string Scenario,
    bool CriterionMet, double Score, double Threshold,
    string Unit, string Control, string Explanation, string Limitation);

/// <summary>Strategy interface: use separate detector algorithms for separate observation types.</summary>
public interface IPropertyDetector<in TObservation>
{
    PropertyFinding Evaluate(TObservation observation);
}

public sealed class TriangleFeasibilityDetector : IPropertyDetector<GraphSummary>
{
    public PropertyFinding Evaluate(GraphSummary observation) => new(
        "Triangle topology exists", "structural", $"n={observation.Particles}",
        observation.ClassesContainingTriangle > 0,
        observation.ClassesContainingTriangle, 1, "topology classes",
        "n < 3 or triangle-free topologies",
        "At least one possible isomorphism class contains a triangle.",
        "Only structural feasibility is tested, not real-world formation or physical emergence.");
}

public sealed class JointInformationGainDetector : IPropertyDetector<GateSummary>
{
    private readonly double threshold;

    public JointInformationGainDetector(double thresholdBits = 0.5)
    {
        if (thresholdBits < 0 || double.IsNaN(thresholdBits))
            throw new ArgumentOutOfRangeException(nameof(thresholdBits));
        threshold = thresholdBits;
    }

    public PropertyFinding Evaluate(GateSummary observation) => new(
        "Joint predictive gain", "informational", observation.Gate.ToString(),
        observation.ExtraJointPredictiveInformationBits > threshold,
        observation.ExtraJointPredictiveInformationBits, threshold, "bits",
        "CopyLeft gate",
        "Information from the input pair exceeds the larger individual-input mutual information.",
        "Threshold is illustrative. This statistic is not a general formal synergy decomposition.");
}

public sealed record SpinOrderContrast(IsingSummary Interacting, IsingSummary Independent);

public sealed class SpinOrderContrastDetector : IPropertyDetector<SpinOrderContrast>
{
    public double MinimumExcessMagnetization { get; }
    public double MinimumExcessAgreement { get; }

    public SpinOrderContrastDetector(double minimumExcessMagnetization = 0.2,
        double minimumExcessAgreement = 0.15)
    {
        if (minimumExcessMagnetization < 0 || minimumExcessAgreement < 0 ||
            double.IsNaN(minimumExcessMagnetization) || double.IsNaN(minimumExcessAgreement))
            throw new ArgumentOutOfRangeException(nameof(minimumExcessMagnetization));
        MinimumExcessMagnetization = minimumExcessMagnetization;
        MinimumExcessAgreement = minimumExcessAgreement;
    }

    public PropertyFinding Evaluate(SpinOrderContrast observation)
    {
        IsingSummary coupled = observation.Interacting;
        IsingSummary control = observation.Independent;
        if (coupled.Side != control.Side || Math.Abs(coupled.Temperature - control.Temperature) > 1e-9)
            throw new ArgumentException("Comparison requires the same lattice size and temperature.");
        double extraMagnetization = coupled.MeanAbsoluteMagnetization - control.MeanAbsoluteMagnetization;
        double extraAgreement = coupled.MeanNeighbourAgreement - control.MeanNeighbourAgreement;
        return new PropertyFinding(
            "Collective spin-order contrast", "dynamical", $"T={coupled.Temperature:0.###}, side={coupled.Side}",
            extraMagnetization > MinimumExcessMagnetization && extraAgreement > MinimumExcessAgreement,
            extraMagnetization, MinimumExcessMagnetization, "excess absolute magnetization",
            "J=0 independent spins, same T and N",
            $"Requires magnetization contrast > {MinimumExcessMagnetization:0.###} AND agreement contrast > {MinimumExcessAgreement:0.###}. Observed agreement contrast {extraAgreement:0.###}.",
            "Heuristic thresholds, finite sampling, and no confidence intervals; not proof of a thermodynamic phase transition.");
    }
}
