using EmergenceAtlas.Core.Models;

namespace EmergenceAtlas.Core.Analysis;

public sealed record IsingSummary(
    int Side, double Temperature, double Coupling, int Samples,
    double MeanAbsoluteMagnetization, double MeanSignedMagnetization,
    double MeanNeighbourAgreement, string FinalSnapshot);

public sealed class IsingAnalyzer
{
    public IsingSummary Analyse(int side, double temperature, double coupling, int seed,
        int warmupSweeps = 350, int sampleSweeps = 600, int sampleEvery = 5)
    {
        if (warmupSweeps < 0) throw new ArgumentOutOfRangeException(nameof(warmupSweeps));
        if (sampleSweeps < 1) throw new ArgumentOutOfRangeException(nameof(sampleSweeps));
        if (sampleEvery < 1) throw new ArgumentOutOfRangeException(nameof(sampleEvery));
        var lattice = new IsingLattice(side, seed);
        for (int sweep = 0; sweep < warmupSweeps; sweep++) lattice.Sweep(temperature, coupling);
        double absMag = 0, signedMag = 0, agreement = 0;
        int samples = 0;
        for (int sweep = 0; sweep < sampleSweeps; sweep++)
        {
            lattice.Sweep(temperature, coupling);
            if ((sweep + 1) % sampleEvery != 0) continue;
            double mag = lattice.SignedMagnetization();
            absMag += Math.Abs(mag);
            signedMag += mag;
            agreement += lattice.NeighbourAgreement();
            samples++;
        }
        if (samples == 0) throw new ArgumentException("At least one measurement is required.");
        return new IsingSummary(side, temperature, coupling, samples,
            absMag / samples, signedMag / samples, agreement / samples, lattice.AsciiSnapshot());
    }
}
