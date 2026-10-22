namespace EmergenceAtlas.Core.Analysis;

public enum BinaryGate { CopyLeft, And, Xor }

public sealed record GateSummary(
    BinaryGate Gate,
    double OutputEntropyBits,
    double LeftInformationBits,
    double RightInformationBits,
    double JointInformationBits,
    double ExtraJointPredictiveInformationBits);

/// <summary>
/// Exhaustive uniform enumeration of two independent binary inputs.
/// Extra joint predictive information := I((A1,A2);B) - max(I(A1;B), I(A2;B)).
/// This is an illustrative comparison, NOT a general partial-information-decomposition synergy measure.
/// </summary>
public sealed class BinaryGateAnalyzer
{
    private readonly InformationTheory information = new();

    public GateSummary Analyse(BinaryGate gate)
    {
        int[] a1 = { 0, 0, 1, 1 };
        int[] a2 = { 0, 1, 0, 1 };
        int[] pair = { 0, 1, 2, 3 };
        int[] output = new int[4];
        for (int i = 0; i < 4; i++)
            output[i] = gate switch
            {
                BinaryGate.CopyLeft => a1[i],
                BinaryGate.And => a1[i] & a2[i],
                BinaryGate.Xor => a1[i] ^ a2[i],
                _ => throw new ArgumentOutOfRangeException(nameof(gate))
            };
        double left = information.MutualInformation(a1, output);
        double right = information.MutualInformation(a2, output);
        double joint = information.MutualInformation(pair, output);
        return new GateSummary(gate, information.Entropy(output), left, right, joint,
            joint - Math.Max(left, right));
    }
}
