namespace EmergenceAtlas.Core.Models;

/// <summary>
/// Toy 2-D ferromagnetic Ising model with periodic boundaries and Metropolis updates.
/// Finite lattices exhibit a crossover, not a mathematically sharp thermodynamic transition.
/// </summary>
public sealed class IsingLattice
{
    private readonly int[,] spins;
    private readonly Random rng;
    public int Side { get; }
    public int ParticleCount => Side * Side;

    public IsingLattice(int side, int seed)
    {
        if (side < 4 || side > 128) throw new ArgumentOutOfRangeException(nameof(side), "Use a side length between 4 and 128.");
        Side = side;
        rng = new Random(seed);
        spins = new int[side, side];
        for (int y = 0; y < side; y++)
        for (int x = 0; x < side; x++) spins[y, x] = rng.Next(2) == 0 ? -1 : 1;
    }

    public void Sweep(double temperature, double coupling)
    {
        if (temperature <= 0 || double.IsNaN(temperature))
            throw new ArgumentOutOfRangeException(nameof(temperature));
        if (coupling < 0 || double.IsNaN(coupling))
            throw new ArgumentOutOfRangeException(nameof(coupling));
        for (int attempt = 0; attempt < ParticleCount; attempt++)
        {
            int x = rng.Next(Side);
            int y = rng.Next(Side);
            int spin = spins[y, x];
            int neighbours = spins[y, (x + Side - 1) % Side] + spins[y, (x + 1) % Side]
                + spins[(y + Side - 1) % Side, x] + spins[(y + 1) % Side, x];
            double deltaEnergy = 2 * coupling * spin * neighbours;
            if (deltaEnergy <= 0 || rng.NextDouble() < Math.Exp(-deltaEnergy / temperature))
                spins[y, x] = -spin;
        }
    }

    public double SignedMagnetization()
    {
        int total = 0;
        for (int y = 0; y < Side; y++)
        for (int x = 0; x < Side; x++) total += spins[y, x];
        return (double)total / ParticleCount;
    }

    public double NeighbourAgreement()
    {
        int agreements = 0;
        for (int y = 0; y < Side; y++)
        for (int x = 0; x < Side; x++)
        {
            if (spins[y, x] == spins[y, (x + 1) % Side]) agreements++;
            if (spins[y, x] == spins[(y + 1) % Side, x]) agreements++;
        }
        return (double)agreements / (2 * ParticleCount);
    }

    public string AsciiSnapshot(int maxSide = 32)
    {
        int view = Math.Min(Side, maxSide);
        return string.Join(Environment.NewLine, Enumerable.Range(0, view).Select(y =>
            new string(Enumerable.Range(0, view).Select(x => spins[y, x] > 0 ? '█' : '·').ToArray())));
    }
}
