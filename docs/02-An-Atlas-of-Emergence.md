# An Atlas of Emergence

## Towards a general classification of emergent properties

> Research discussion 2 — expanded research proposal, adapted to Markdown from the second response in the conversation that led to Emergence Atlas.

## Abstract

The physical universe appears to be built from a relatively small number of fundamental constituents and interactions, yet it supports an extraordinary variety of structures, behaviours and properties.

Atoms form molecules. Molecules form materials and biological systems. Interacting particles produce collective states such as magnetism and superconductivity. Neurons form networks capable of processing information. Simple computational rules can produce complex and persistent structures. These phenomena are commonly described as *emergent*.

Although emergence has been studied extensively, there remains an opportunity to compare different forms of emergence within a common, systematic framework.

This proposal investigates whether emergent properties can be classified by the minimum number of constituents required, their organisation, their interaction rules, their collective behaviour, and the degree to which they can be predicted or explained at lower levels.

It begins with an intentionally simple universe containing only one type of fundamental particle. An existing mathematical counting function provides an initial account of possible groupings across scales. The framework then distinguishes arbitrary collections from genuinely interesting collective properties.

The long-term objective is to create an **Atlas of Emergence**: a catalogue of recurring organisational principles, mathematical relationships and emergence mechanisms that appear across otherwise unrelated physical, biological and computational systems.

The central hypothesis is that different emergent phenomena may share identifiable structural and dynamical signatures, potentially allowing new system-level behaviours to be anticipated instead of merely recognised after discovery.

---

## 1. The problem of emergence

Comparatively simple ingredients can produce extraordinarily complicated results.

A collection of atoms has properties determined in part by its constituents. Arrange the atoms differently, and new structures and behaviours become possible. Molecular geometry and bonding are essential to chemical behaviour; listing the atoms alone does not suffice. Temperature in a gas is a statistical collective description rather than the kinetic energy of a particular isolated particle.

Four themes recur:

1. New structures become possible when constituents interact.
2. New behaviours become possible when those structures form.
3. New descriptions become useful at different scales.
4. Some collective properties persist even when individual constituents are replaced or rearranged.

Being *explained by lower-level constituents* is different from being *usefully described at the constituent level*. We need not assume that emergence violates physical reductionism. A property may be completely determined by microscopic laws while still requiring a distinct higher-level description for efficient prediction or explanation.

**Research question:** Can higher-level phenomena be compared and classified within a common mathematical framework?

## 2. A universe containing only A

Imagine a universe with `n` fundamental, indivisible particles, all called `A`. They are initially identical, apart from their location or role in an arrangement.

Consider collections of sizes 1 through `n`:

| Label | Composition | Size |
|---|---|---:|
| A | A | 1 |
| B | AA | 2 |
| C | AAA | 3 |
| D | AAAA | 4 |
| E | AAAAA | 5 |

At this stage, B through E are **labels for collections, not demonstrated emergent particles**.

For five particles, a floor-division grouping scheme produces:

| Scale | Representation | Complete groups |
|---|---|---:|
| 1 | `A A A A A` | 5 |
| 2 | `(AA) (AA) A` | 2 |
| 3 | `(AAA) A A` | 1 |
| 4 | `(AAAA) A` | 1 |
| 5 | `(AAAAA)` | 1 |

The total is:

$$
T(n)=\sum_{k=1}^{n}\left\lfloor\frac{n}{k}\right\rfloor,
\qquad T(5)=10.
$$

This is the established *summatory divisor function*. It counts groups according to a chosen scale-dependent partition rule, not new physical properties.

Other conventions lead to different answers. With five distinguishable particles, every nonempty subset produces `2^5 - 1 = 31` groups. Counting only group-size categories gives five.

**First principle:** distinguish the number of possible collections, distinct structures and genuinely new properties.

## 3. From collections to structures

Four A particles may be isolated, connected in a chain, connected as a four-cycle, or organised as a star. These systems share constituent count but differ in connectivity and may behave differently.

Introduce **organisation** via a graph of particles and their relationships.

Define a system as:

$$
S=(n,G,R,X)
$$

where:

- `n` is constituent count;
- `G` is the connection graph or topology;
- `R` is the collection of interaction and transition rules;
- `X` is the current microscopic state.

The model now distinguishes size, arrangement, behaviour and state. It can also reveal whether systems with different numbers of constituents exhibit similar collective phenomena.

An emergent property may belong to an **organisational class**, not a particular number of particles.

## 4. When does a collection become an emergent entity?

Two noninteracting A particles can always be called B, but this may add no explanatory value. Two A particles that form a stable interacting structure may support identifiable collective behaviour.

Possible operational dimensions are:

- **Structural distinction:** an identifiable network, lattice, bound cluster or geometry.
- **Dynamical distinction:** new collective oscillation, propagation, feedback or coordinated behaviour.
- **Persistence:** the structure or behaviour persists on a specified timescale.
- **Predictive usefulness:** higher-level variables permit an accurate and economical account.
- **Robustness:** collective behaviour survives a suitable range of perturbations.

No single criterion need be universally necessary or sufficient. The aim is to compare candidate properties across explicit, reproducible dimensions.

## 5. A hierarchy of entities

Suppose two A particles can form a B, three can form a C, and two B entities can form D:

```text
A + A       -> B
A + A + A   -> C
B + B       -> D
```

These are hypothetical formation rules, not claims about physical reactions.

D contains four fundamental A constituents but can also be described as two organised B units. These descriptions do not necessarily explain the same behaviour. A four-A structure formed by another arrangement may be quite different.

A useful catalogue entry therefore includes:

- Unique entity or property identifier.
- Fundamental constituent types and counts.
- Internal topology and organisation.
- Formation and environmental conditions.
- Observable characteristic properties.
- Dynamics and persistence.
- Interactions with other entities and surroundings.
- Relationships to other catalogue entries.

This resembles a periodic table in ambition, but emergent categories may overlap and depend on observation timescale; they are not expected to behave like mutually exclusive chemical elements.

## 6. A proposed multidimensional classification

The proposed categories are *operational lenses*, not mutually exclusive classes and not a replacement for the existing emergence literature.

### Type 0 — Aggregation

A property is obtained straightforwardly from the constituents, such as combining the masses of two objects. This is the baseline against which more complex claims should be tested.

### Type 1 — Structural emergence

Organised connections create distinct structures: motifs, connected clusters, networks and lattices. The defining feature is **organisation**.

### Type 2 — Dynamical emergence

Local interactions produce collective time-dependent behaviour: synchronisation, travelling waves or persistent oscillations.

### Type 3 — Informational emergence

A joint system exhibits statistical or informational relationships that cannot be captured by separately examining isolated components. The precise information measure must be specified.

### Type 4 — Functional emergence

Organised components collectively perform functions such as homeostasis, error correction or distributed control. Functions and environmental criteria must be operationally defined.

### Type 5 — Critical or phase-transition emergence

Collective behaviour changes qualitatively with parameters such as temperature, connection density or interaction strength. Some cases have characteristic scaling laws in large-system limits.

A single example can occupy multiple types: e.g., a synchronising network is structural and dynamical, and might be informational as well.

## 7. Measuring emergence

Classification becomes scientifically useful when evidence and metrics can be compared.

Potential measurements include:

1. **Predictive advantage:** compare properly controlled microscopic and macroscopic predictors on held-out data.
2. **Description length:** assess whether a macro description offers substantial compression at an acceptable prediction error.
3. **Joint information:** assess statistical synergy or the benefit of collective predictors.
4. **Causal analysis:** test whether macro descriptions offer more useful accounts of interventions under explicitly defined assumptions.
5. **Robustness and persistence:** quantify response to perturbations and duration over time.

No single number should be presented as a universal measure. A complete microscopic model with unlimited information and computation cannot simply be assumed to underperform a derived macro model. Comparisons must control observation quality, model complexity and available computational resources.

An **emergence profile** might record structural novelty, persistence, information synergy, predictive advantage, causal effectiveness, robustness and minimum organisational scale.

## 8. Returning to the counting problem

The original count depended only on `n`. Define instead:

$$
E(n,R,G,C)
$$

where `E` is the number of distinct qualifying property classes, given constituent number `n`, interaction rules `R`, organisation `G` and explicit classification criteria `C`.

Crucially, `C` includes observables, timescales, tolerances and equivalence rules. Without them, the notion of *number of properties* is ambiguous or potentially unbounded.

`E` might rise with size, remain constant over wide intervals, or fall as conditions change. Density, topology, boundary conditions, temperature, energy or interaction strength may matter more than `n` alone.

**Research question:** Are there general relationships between the emergence, persistence and diversity of property classes and the underlying rules, structure and scale?

## 9. Emergence thresholds

Some properties have minimum structural requirements: an edge needs two endpoints, a triangle needs three connected vertices, and long-range connectivity requires a suitable extended structure.

Define a property's *minimum emergence scale* as the smallest number of constituents in a system able to exhibit it under specified rules.

But distinguish carefully:

- **Possibility:** the rules permit the property.
- **Formation probability:** it occurs under particular initial and boundary conditions.
- **Persistence:** it continues over time.
- **Stability:** it survives perturbations.
- **Criticality:** behaviour changes sharply near a control-parameter threshold.

The Atlas could map the regions of parameter space where property classes occur, rather than merely assigning a number of particles to each property.

## 10. Universal emergence classes

Different microscopic systems may share mathematical macroscopic behaviour. A biological network and a distributed engineered system might both synchronise; a material and a random network might share threshold behaviour.

Potential classes include:

- Synchronising systems.
- Self-organising spatial patterns.
- Persistent collective memory.
- Connectivity transitions.
- Coherent propagating structures.
- Collective error correction.
- Stable higher-level dynamical descriptions.

Statistical physics already establishes universality for certain critical phenomena. The open challenge is whether useful comparable principles extend to a broader, precisely delimited set of systems.

## 11. Experimental programme

### Experiment 1 — Non-interacting controls

Generate increasing numbers of A particles with no interaction. Recover the combinatorial counting baselines; avoid declaring arbitrary groupings to be collective phenomena.

### Experiment 2 — Graph organisation

Allow A particles to form edges. Enumerate small connection topologies, identify graph isomorphism classes, and detect connected clusters, cycles and motifs.

### Experiment 3 — Interacting binary particles

Give each A a binary state and define local updates. Search for persistent patterns, coordinated states, and joint informational features. Compare against randomised and noninteracting baselines.

### Experiment 4 — Multiple microscopic rules

Repeat the analysis for alternative rules and compare the resulting higher-level profiles. Look for systems that display similar macro behaviour despite microscopic differences.

### Experiment 5 — Recognised complex-system benchmarks

Compare proposed classification criteria against established results from cellular automata, Ising models, percolation and coupled oscillators.

A useful classification must distinguish genuine collective behaviour from artifacts of arbitrary grouping, choice of representation or detection threshold.

## 12. What would make this scientifically useful?

The project must accomplish more than rename familiar phenomena. Its possible contributions are:

- **Classification:** reproducible grouping of otherwise different examples.
- **Explanation:** identification of the interactions and arrangements responsible for collective behaviour.
- **Prediction:** forecast the conditions producing or destroying a property in held-out systems.
- **Discovery:** identify overlooked collective structure or dynamics.
- **Compression:** provide more tractable descriptions with useful predictive power.

The most compelling demonstration would be predicting a new system's emergent behaviour based on a classification calibrated on other systems.

## 13. Limitations

Important open problems include:

1. Properties may be observer-, timescale- and variable-dependent.
2. Different collective descriptions overlap.
3. Counting properties requires a principled equivalence relation.
4. Unrestricted property descriptions may be unbounded.
5. Microscopic predictability may be computationally demanding.
6. Operational emergence does not by itself settle debates about strong emergence, consciousness or reductionism.

These are reasons to make assumptions explicit, not to abandon the research direction.

## 14. Conclusion

The original two-particle observation illustrates **multiscale bookkeeping**: the number of meaningful descriptive entities need not equal the number of fundamental constituents. But grouping alone does not establish novelty.

The deeper research programme asks which constituent arrangements and interactions generate collective structure, dynamics, stability and useful higher-level explanations.

> There may be general classes of organisational and dynamical conditions under which genuinely new, higher-level descriptions become possible and useful.

If those conditions can be operationally identified and compared, emergence becomes a phenomenon that can be investigated, classified and sometimes predicted.

---

## Worked example: XOR and collective information

For two independent fair binary variables `A1`, `A2`, define a collective output:

$$
B=A_1\oplus A_2.
$$

| A1 | A2 | B (XOR) |
|---:|---:|---:|
| 0 | 0 | 0 |
| 0 | 1 | 1 |
| 1 | 0 | 1 |
| 1 | 1 | 0 |

For the uniform independent inputs:

$$
I(A_1;B)=I(A_2;B)=0,\qquad I((A_1,A_2);B)=1\text{ bit}.
$$

Neither individual input predicts the output; jointly they determine it. This illustrates *informational synergy*. It is **not** proof of irreducible physics or, by itself, a demonstration that every XOR gate is an emergent physical system. The logic gate remains fully determined by its rule.

The original `1 + 1 = 3` observation can be made more rigorous by saying: two constituent variables can jointly define an additional variable with information properties absent from either constituent viewed alone.

## Relation to prior research

Substantial quantitative and classificatory work already exists. The contribution of the Atlas must therefore be **systematic, cross-domain classification and testable prediction**, not claiming that emergence had no previous mathematical treatment.

- [Fromm (2005), *Types and Forms of Emergence*](https://arxiv.org/abs/nlin/0506028)
- [Hoel, Albantakis and Tononi (2013), *Quantifying causal emergence*](https://pmc.ncbi.nlm.nih.gov/articles/PMC3856819/)
- [Rosas et al. (2020), *Reconciling emergences*](https://pmc.ncbi.nlm.nih.gov/articles/PMC7833221/)
- [Crutchfield and Shalizi (1999), *Thermodynamic Depth of Causal States*](https://journals.aps.org/pre/abstract/10.1103/PhysRevE.59.275)
- [*Finding emergence in data by maximizing effective information*](https://academic.oup.com/nsr/article/12/1/nwae279/7732052)

## Practical first implementation

Build an **Emergence Explorer**, beginning with systems of 2–8 binary-state constituents and a small selection of explicitly defined interaction rules. Some state spaces are then manageable, but exhaustive enumeration of all possible graphs and rules still scales rapidly.

1. Generate a universe with `n` particles, a topology and rule set.
2. Simulate its states, transitions and perturbations.
3. Detect connected motifs, stable states and recurrent dynamics.
4. Evaluate candidates against explicit thresholds, null models and robustness tests.
5. Record their evidence and limitations in the Atlas.

The project's formal target is `E(n,R,G,C)` with operational criteria `C`. Three testable hypotheses motivate the implementation:

1. Some property classes have minimum structural requirements.
2. Different microscopic systems produce higher-level phenomena belonging to common equivalence classes.
3. Property-class diversity may obey reproducible scaling relationships **within specific families of rules**, rather than one universal law.

**Suggested decisive result:** identify two microscopic systems with differing rules that fall into the same higher-level class and predict that similarity before completing the full simulation.

See [README](../README.md) for the initial C# experiments.
