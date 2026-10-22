namespace EmergenceAtlas.Core.Models;

/// <summary>A fundamental constituent. IDs distinguish instances, not particle types.</summary>
public sealed record Particle(int Id);

/// <summary>
/// A subset of constituent IDs. A collection is NOT automatically an emergent entity.
/// </summary>
public sealed class ParticleCollection
{
    public IReadOnlyList<Particle> Members { get; }
    public int Size => Members.Count;

    public ParticleCollection(IEnumerable<Particle> members)
    {
        var items = members.ToArray();
        if (items.Length == 0) throw new ArgumentException("A collection cannot be empty.", nameof(members));
        if (items.Select(p => p.Id).Distinct().Count() != items.Length)
            throw new ArgumentException("A particle cannot appear twice in the same collection.", nameof(members));
        Members = Array.AsReadOnly(items);
    }
}
