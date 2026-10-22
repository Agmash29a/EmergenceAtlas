using EmergenceAtlas.Core.Experiments;

namespace EmergenceAtlas.Core.Analysis;

/// <summary>Hypotheses/operational examples, not an objectively complete emergence taxonomy.</summary>
public sealed class AtlasCatalogue
{
    public IReadOnlyList<AtlasEntry> Definitions() => new[]
    {
        new AtlasEntry(
            "Connected components", "structural", "simple undirected graphs",
            "At least two vertices joined by an edge",
            "Number and size of connected components",
            "Connectedness calculated from the adjacency graph",
            "Edge-free graph with the same number of vertices",
            "Basic structural description; connectivity alone does not establish emergence."),
        new AtlasEntry(
            "Cycles and triangles", "structural", "simple undirected graphs",
            "At least three vertices in a closed chain",
            "Cycle or triangle present, disregarding labels",
            "Motif occurs in canonical graph-isomorphism class",
            "Acyclic graph of comparable size",
            "This counts structural possibilities, not novel physical laws."),
        new AtlasEntry(
            "Joint-information gain", "informational", "two-input binary gates",
            "Two independent uniformly sampled inputs",
            "Information about B from the joint input beyond the best single input",
            "I((A1,A2);B) - max(I(A1;B), I(A2;B)) > 0",
            "CopyLeft gate (zero gain)",
            "Not equivalent to general partial information decomposition synergy; XOR is explicitly engineered."),
        new AtlasEntry(
            "Collective spin order", "dynamical", "finite 2-D Ising lattice",
            "Nearest-neighbour interactions on a periodic square lattice",
            "Absolute magnetization and adjacent-spin agreement",
            "Compare coupled J=1 simulations against non-interacting J=0 at equal T",
            "Non-interacting spins with same N and Monte Carlo schedule",
            "Finite noisy simulation: not proof of a thermodynamic phase transition; requires uncertainty estimates for inference.")
    };
}
