# Emergence Atlas — C# object-oriented experiments

An experimental **.NET 8** console application for exploring an operational classification of emergent phenomena. No third-party packages are required. Output includes a **standalone illustrated HTML report**, CSV measurements, and JSON definitions of candidate properties.

The software intentionally distinguishes:

1. **Collections** — a counting rule, not proof of a new property.
2. **Structures** — graphs distinguish organisations with the same constituent count.
3. **Information** — XOR, AND and COPY demonstrate how a joint variable can contain information unavailable to individual inputs.
4. **Collective dynamics** — a small 2-D Ising spin lattice exhibits greater spatial ordering with nearest-neighbour interactions than a non-interacting control.

None of these experiments establishes metaphysically strong emergence. They supply repeatable toy systems and metrics for investigating *operational* emergence.

## Research documents

These documents preserve the **first two research responses** that motivated this C# implementation, reformatted for standalone reading:

1. [Is the emergence classification idea useful?](docs/01-Is-the-Idea-Useful.md) — evaluation of the original A-particle counting idea and its limitations.
2. [An Atlas of Emergence](docs/02-An-Atlas-of-Emergence.md) — expanded hypothesis, formal model, classification criteria, experiments and research directions.

## Requirements

- Windows, Linux or macOS with [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later compatible SDK.
- No external NuGet packages, services, network access or Python required.

## Run

From the folder containing this README:

```powershell
dotnet run --project src/EmergenceAtlas.Cli -- all --out output
```

Then open `output/report.html` in any browser. Also inspect `output/measurements.csv` (long-format numerical results) and `output/atlas.json` (experimental criteria and caveats).

Run one experiment:

```powershell
dotnet run --project src/EmergenceAtlas.Cli -- count
dotnet run --project src/EmergenceAtlas.Cli -- graphs
dotnet run --project src/EmergenceAtlas.Cli -- information
dotnet run --project src/EmergenceAtlas.Cli -- ising --side 24 --seed 42 --warmup 600 --sweeps 1200
```

To run the built-in deterministic tests:

```powershell
dotnet run --project tests/EmergenceAtlas.Tests
```

Or open `EmergenceAtlas.sln` in Visual Studio 2022, select `EmergenceAtlas.Cli` as the startup project, and run it. The tests project is a zero-dependency command-line test runner rather than xUnit, which would require additional NuGet packages.

## Design and responsibilities

| Type | Responsibility |
|---|---|
| `Particle`, `ParticleCollection` | Define fundamental constituents and arbitrary collections, keeping identity distinct from emergence. |
| `CollectionCounter` | Compute floor-division sum, number of sizes, and number of nonempty subsets. |
| `UndirectedGraph`, `GraphCatalogue` | Encode connections; enumerate graphs, remove vertex-label duplicates using canonical permutations, and detect connectivity/cycles/triangles. |
| `InformationTheory`, `BinaryGateAnalyzer` | Estimate exact discrete entropy/mutual information for small truth tables. |
| `IsingLattice`, `IsingAnalyzer` | Encapsulate spin states, nearest-neighbour Metropolis updates, measurements and sampling. |
| `IExperiment` and concrete experiments | Pluggable OO experiments producing the same structured metrics interface. |
| `ExperimentRunner` | Select and orchestrate experiments. |
| `AtlasCatalogue`, `AtlasEntry` | Machine-readable operational criteria, controls and limitations. |
| `ReportExporter` | Export raw CSV, atlas JSON and a dependency-free illustrated HTML report. |

## What is being counted?

For `n=5` particles, there are multiple *equally valid but incompatible* bookkeeping conventions:

- Floor-partition count: `5 + 2 + 1 + 1 + 1 = 10`
- Collection-size categories: `5`
- Nonempty subsets of **distinguishable** constituents: `2^5 - 1 = 31`

None is a count of discovered emergent physical properties. In the graph experiment, each possible simple, undirected labelled graph is enumerated. Isomorphic graphs (the same graph with different vertex labels) are collapsed. Known graph counts provide regression tests: unlabeled simple graphs with 1–5 vertices are **1, 2, 4, 11, 34**; connected counts are **1, 1, 2, 6, 21**.

## Information experiment

`A1` and `A2` are independent fair bits, and `B` is one of three deterministic outputs:

- `COPY`: `B = A1`. Individual information from A1 is 1 bit; from A2, 0 bits; joint information, 1 bit.
- `AND`: `B = A1 AND A2`. Partial predictive benefit from considering both inputs.
- `XOR`: `B = A1 XOR A2`. Each input alone carries **0 bits** of mutual information with B; both inputs together carry **1 bit**.

The reported extra joint predictive information is:

`I((A1,A2);B) - max(I(A1;B), I(A2;B))`.

This is **not** a general estimator of synergistic information in the formal partial-information-decomposition sense. The logic gate B is specified by the modeller, so the demonstration is not itself proof of emergent physical behaviour.

## Ising experiment

A square lattice holds spins `+1` or `-1` with periodic boundaries. Each proposed flip changes energy by:

`ΔE = 2 J sᵢ Σ₍ⱼ neighbors of i₎ sⱼ`.

Metropolis updates accept an energy-decreasing flip, or accept an energy-increasing one with probability `exp(-ΔE/T)`, taking Boltzmann's constant as 1. Five temperatures are used: `1.5`, `2.0`, `2.269`, `2.6`, `3.5`. They are compared for `J=0` and `J=1`. The numerical neighbourhood agreement and average absolute magnetization are recorded.

- **Control:** `J=0`, independent spins, with approximately 50% nearest-neighbour agreement.
- **Interacting system:** `J=1`, which can exhibit collective ordering at lower temperatures.
- **Caution:** a small finite lattice does **not** have a mathematically sharp thermodynamic phase transition. The familiar infinite-square-lattice Ising critical temperature is about `2.269` for `J/kB=1`, but no critical point is inferred from this short run. Results depend on seed, lattice size, thermalisation, autocorrelation and sample length; there are no uncertainty intervals.

## Working toward a research programme

The JSON atlas starts with four operational test cases (connected components, cycles, joint-information gain, collective spin order). A more serious emergence theory would require identifying observables and equivalence classes across multiple rule systems, calibration against null models, statistical uncertainty, and out-of-sample predictions.

Possible future extensions: graph evolution rules, multiple particle types, cellular automata, information decomposition, repeated-run confidence intervals, finite-size scaling, and a model-agnostic `IPropertyDetector` interface that can test candidate classes across different dynamics.

## Publish to GitHub

If this solution has not yet been uploaded, install [GitHub CLI](https://cli.github.com/), sign in using `gh auth login`, and run one of:

```powershell
.\publish-to-github.ps1 -Visibility Public
.\publish-to-github.ps1 -Visibility Private
```

The script initialises Git if necessary, commits the files, creates `Agmash29a/EmergenceAtlas` when it does not exist, and pushes `main`. If the repository already exists, it attempts to push the local branch without overwriting unrelated history. Git must have an author name and email configured. You can also [create an empty repository on GitHub](https://github.com/new) and publish using Git manually.

## Continuous integration

The GitHub Actions workflow at `.github/workflows/build.yml` builds the .NET 8 solution and runs the built-in deterministic test runner on pushes and pull requests. These checks will only run after publishing the repository.

## Build-validation caveat

The files were authored and inspected in an environment without the .NET SDK. **Compilation and runtime tests were not executed here.** Run the commands above on a Windows machine with .NET installed; report any build diagnostics for follow-up corrections.

## License

MIT — see `LICENSE`.

## Automated candidate-property detectors

The solution also defines `IPropertyDetector<TObservation>` implementations with explicit operational thresholds:

- `TriangleFeasibilityDetector`: detects that a triangle is *structurally possible* within an enumerated graph class (not that a novel physical property has appeared).
- `JointInformationGainDetector`: by default requires a joint predictive advantage greater than 0.5 bit, so XOR passes but a copy gate does not.
- `SpinOrderContrastDetector`: requires coupled spins to exceed a non-interacting control on both absolute magnetization and neighbour agreement by preset margins. This is a descriptive heuristic, not a statistical test.

Each evaluation is exported with its criterion, score, comparison baseline and limitation in `output/findings.json`. The browser report shows a pass/not-met table. These rules can be replaced without changing the experiment runner, which is the central object-oriented extensibility point.

### One-command Windows run

In PowerShell, from the extracted folder:

```powershell
.\run.ps1
```

This runs the tests first, runs all experiments if they pass, and opens the report. On systems that restrict scripts, use the `dotnet run` commands instead of changing PowerShell security settings.
