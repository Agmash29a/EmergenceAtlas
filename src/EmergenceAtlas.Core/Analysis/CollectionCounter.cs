using System.Numerics;

namespace EmergenceAtlas.Core.Analysis;

/// <summary>
/// Three DISTINCT conventions: floor-partition counts across scales,
/// size categories, and every nonempty subset of labelled particles.
/// None is a count of physical emergent properties.
/// </summary>
public sealed class CollectionCounter
{
    public long CountCompleteGroups(int particleCount, int groupSize)
    {
        CheckCount(particleCount);
        if (groupSize < 1) throw new ArgumentOutOfRangeException(nameof(groupSize));
        return particleCount / groupSize;
    }

    public long SummatoryDivisorCount(int particleCount)
    {
        CheckCount(particleCount);
        long result = 0;
        for (int size = 1; size <= particleCount; size++)
            checked { result += CountCompleteGroups(particleCount, size); }
        return result;
    }

    public int CountSizeCategories(int particleCount)
    {
        CheckCount(particleCount);
        return particleCount;
    }

    public BigInteger CountNonemptySubsets(int particleCount)
    {
        CheckCount(particleCount);
        return BigInteger.Pow(2, particleCount) - BigInteger.One;
    }

    private static void CheckCount(int particleCount)
    {
        if (particleCount < 1) throw new ArgumentOutOfRangeException(nameof(particleCount));
    }
}
