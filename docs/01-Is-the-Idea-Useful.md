# Is the emergence classification idea useful?

> Research discussion 1 — the initial assessment of the *A particle* thought experiment. Adapted to Markdown from the first response in the conversation that led to Emergence Atlas.

## Overview

**Yes — there is a useful idea here, but the most promising part is slightly different from what the original argument claims.** The ambition is to turn emergence from a collection of case-specific explanations into something that can be classified mathematically. That is a legitimate scientific ambition, connected to mathematics, physics and complexity science.

The central question is:

> Can we develop a systematic classification of the new structures and properties that become possible when simpler components are combined?

However, the proposed counting rule does not yet capture emergence in the scientific sense. It counts groups of particular sizes, not necessarily new properties. That distinction determines whether this is a new classification framework or simply a mathematical observation.

## 1. What the model actually discovers

The universe contains `n` fundamental particles, each called `A`. Collections of sizes 1 through `n` receive different names:

- `A`: one fundamental particle.
- `B = AA`: a collection containing two.
- `C = AAA`: a collection containing three.
- `D = AAAA`: a collection containing four.
- And so on.

The number of complete groups of size `i` that fit into `n` particles is `floor(n/i)`.

For `n = 5`:

| Collection size | Number of complete groups | Label |
|---:|---:|---|
| 1 | 5 | A |
| 2 | 2 | B |
| 3 | 1 | C |
| 4 | 1 | D |
| 5 | 1 | E |
| **Total** | **10** | |

The expression is

$$
T(n) = \sum_{i=1}^{n} \left\lfloor \frac{n}{i} \right\rfloor.
$$

For `n = 5`, this is `5 + 2 + 1 + 1 + 1 = 10`; for `n = 2`, it is `2 + 1 = 3`.

Thus, under this counting convention, two `A` particles give *three counted entities*: two individual particles and their pair. The phrase **1 + 1 = 3** means that we are simultaneously counting at multiple descriptive scales. It does not contradict arithmetic.

This expression is already known as the **Dirichlet divisor summatory function** (or summatory divisor function). It counts the positive integer pairs `(i, j)` with `ij <= n`. For each `i` there are `floor(n/i)` allowed values of `j`.

The formula has a precise mathematical foundation, but that mathematical fact alone is not a theory of emergence.

## 2. The main problem: emergence has not been defined

The model assumes that each collection of several `A` particles can be given a new identity. Yet **a new name does not necessarily mean a new property**.

Consider two systems:

1. Two independent particles `A` sitting next to each other.
2. Two particles `A` interacting and forming a stable bound structure.

Both contain two particles. The second may have properties that the first does not. Their relationship and behaviour, not merely their number, make the difference.

Three additional ingredients are required:

### Relationships

Are the particles connected, separated, ordered, or interacting? Five unconnected particles and five constituents forming a stable arrangement may behave differently.

### Behavioural rules

Do the particles attract, repel, exchange information, bond, or change state? Without dynamics, the universe cannot generate collective behaviour.

### A criterion for novelty

What makes a higher-level property genuinely new? A new name is insufficient. Candidate criteria include new stable states, collective dynamics, measurable functions or properties unavailable to isolated parts.

There is a further subtlety: the groups of sizes two, three and four used in the example are **alternative overlapping descriptions across scales**. The total of ten does not imply that ten distinct physical objects coexist, or that five novel properties have appeared. The formula is best described as a *multiscale counting scheme*.

## 3. Is the classification problem real?

**Yes**, although its scope needs refinement. Physics, chemistry, biology and computer science use different concepts at different descriptive levels:

- **Molecular properties:** chemical bonding, structure and reactivity are not merely inventories of atoms.
- **Collective physical states:** magnetisation and phase transitions arise from collective organisation and interactions.
- **Organisational properties:** an ant colony exhibits coordination that is not a property of an isolated ant.
- **Computational properties:** simple local rules can generate persistent patterns and collective dynamics.

There are already scientific and philosophical frameworks for emergence, including theories of phase transitions, universality, information and causation. It would therefore be incorrect to claim that no classifications exist.

The opportunity is to build a **common operational framework** for comparing different forms of emergence, with formal descriptors applicable across domains.

## 4. How to develop the idea

Retain the single-particle universe, but replace the assumption that each collection size generates a property with the question:

> What kinds of new properties can arise at each level of organisation, under what conditions, and can these properties be predicted from a formal system description?

A potential classification has five dimensions:

| Dimension | Question |
|---|---|
| Scale | How many constituents are involved? |
| Organisation | How are the constituents arranged or connected? |
| Interaction | What rules govern their behaviour? |
| Novelty | What property appears at the collective level? |
| Predictability | Can that property be derived from lower-level rules, and at what computational or explanatory cost? |

A progression in the conceptual universe would be:

1. **Fundamental constituents:** isolated particles and intrinsic properties.
2. **Structured combinations:** binding, connectivity and arrangement.
3. **Collective systems:** feedback, system-level dynamics and useful macroscopic descriptions.

The transitions between stages depend on organisation and interaction rules, not simply particle counts.

## 5. Could the mathematics lead to anything new?

The original summatory divisor function grows approximately like `n log n`, with lower-order terms. This is useful combinatorics but not, by itself, an empirical law for emergent properties.

A more valuable investigation would examine:

- **Emergence thresholds:** conditions at which new collective behaviour first becomes possible or observable.
- **Universality:** different microscopic systems displaying the same macroscopic behaviour.
- **Complexity:** the diversity of distinct collective phenomena as size and topology change.
- **Predictability:** identifying collective behaviour from underlying rules, potentially without exhaustive simulation.

These are serious existing research questions; a novel contribution needs a precise unresolved problem and a demonstrable advance on existing methods.

## 6. Assessment

**Core intuition: useful.** It motivates a general account of higher-level entities and properties derived from simpler constituents.

**Original counting rule: insufficient.** Counting group sizes does not distinguish mere descriptions from genuinely collective phenomena.

**Best direction: taxonomy of emergence.** Build a formal classification based on scale, organisation, interaction, novelty and predictability, then test it on systems with known behaviours.

The proposal is worth developing as a research idea, but it is **not yet a new theory of emergence**. The goal is a classification that can explain or predict phenomena, not simply count groupings.

The next question to answer is:

> Can we define a mathematical signature for an emergent property that distinguishes it from an ordinary property of a collection?

## Starting references

- [Stanford Encyclopedia of Philosophy — Emergent Properties](https://plato.stanford.edu/entries/properties-emergent/)
- [Dirichlet divisor problem (background)](https://en.wikipedia.org/wiki/Divisor_summatory_function)
- [Further reading and the expanded proposal](02-An-Atlas-of-Emergence.md)
