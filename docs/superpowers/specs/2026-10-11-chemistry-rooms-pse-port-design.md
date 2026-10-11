# Chemistry Rooms (3D-PSE) — Full Port — Design

**Date:** 2026-10-11 · **Status:** approved (user: "fine") · **Scope:** make the chemistry
worlds (`PseGallery` + per-element `ElementRoom`) work end-to-end in editor Play Mode.

## Problem

The chemistry worlds are a broken stub of a working thing:

- `Assets/LearningWorlds/Data/elements.json` holds **2 fake elements** ("element1/2") and its keys
  are corrupted (`"name德"` / `"description德"` — a Chinese character instead of `De`).
- `ElementData.LoadAll()` calls `Resources.Load<TextAsset>("elements")`, but the file is under
  `Data/`, **not** in any `Resources/` folder → loads nothing at runtime.
- The C# model (`nameDe`/`descriptionDe` + `ElementProperties{viscosity,meltingPoint,custom}`)
  does not match the JSON.
- `ElementRoom.ArrangeStationRing` spawns **5 identical `QuizStation`s**; the designed 5-station
  content is absent. `LatticeBuilder.cs` exists but is never called.
- `LearningWorldsApp.Go()` **never invokes a builder** — it only logs. Two competing registries
  exist (`LearningWorldsApp.Builders` and `BuildingBuilders`); `BuildingBuilders.RegisterAllWorlds()`
  is never called.
- Editor menus point at `LearningWorlds-*.unity` scenes that **do not exist**.

The real content exists upstream at `C:\Users\Tobias\git\periodic-table`:

- `docs/assets/elements-data.js` — 118 elements, merged with per-element room content.
- `docs/assets/room.js` — defines the 5 stations and the runtime-generated quiz.
- `scripts/element_content.py` — source of `uses` / experiment / discovery text.

## Goal

Port the real dataset and content into the Unity project and wire the world-building so the gallery
populates and each element room shows its 5 populated learning stations, playable in the editor.

## Architecture

- **Procedural, code-only** (no hand-authored `.unity`/`.prefab` YAML), consistent with the existing
  design. Worlds are built at runtime from C#.
- **Single dataset**: upstream `elements-data.js` → committed `Assets/LearningWorlds/Resources/elements.json`
  with descriptive keys, loaded via `Resources.Load<TextAsset>("elements")`. Provenance documented by a
  one-shot converter `tools/port_elements_data.py`; the JSON is the runtime artifact.
- **Single builder registry**: `BuildingBuilders`. `LearningWorldsApp.Go()` builds into the active scene
  (no `LoadSceneAsync`).
- **Launch**: an editor menu generates an empty bootstrap scene and sets a flag; the
  `[RuntimeInitializeOnLoadMethod]` boot builds the Hub only when that flag is set.

## Data

### Schema (per element)

Ported from upstream short keys to descriptive keys:

| upstream | ported field | type | notes |
|---|---|---|---|
| `n` | `number` | int | atomic number 1..118 |
| `s` | `symbol` | string | e.g. `H` |
| `name` | `name` | string | e.g. `Hydrogen` |
| `x` | `group` | int | 1..18 |
| `row` | `period` | int | 1..9 (8/9 = lanthanides/actinides) |
| `cat` | `category` | string | 10 normalized categories |
| `c` | `cpk` | string | 6-digit hex colour, no `#` |
| `m` | `mass` | double | atomic mass |
| `sh` | `shells` | int[] | electron shell counts |
| `phase` | `phase` | string | e.g. `Gas` |
| `melt` | `melt` | double | |
| `boil` | `boil` | double | |
| `dens` | `density` | double | |
| `en` | `electronegativity` | double | |
| `esem` | `electronConfig` | string | e.g. `1s1` |
| `sum` | `summary` | string | overview text |
| `u` | `uses` | string[] | "WHERE YOU MEET IT" (station 2) |
| `xp` | `experiment` | string | "THE CLASSIC EXPERIMENT" (station 4) |
| `pn` | `discovery` | string | "WHO FOUND IT" (station 3) |
| `mo.t` | `modelType` | string | crystal/molecule type (station 1) |
| `mo.c` | `modelCaption` | string | caption for the model (station 1) |

Top-level JSON shape: `{ "elements": [ ... ] }`.

### Model types (station 1)

Observed upstream distribution: `atom`(45), `hcp`(29), `fcc`(16), `bcc`(15), `dumbbell`(7),
`diamond`(3), `ring8`(1), `sc`(1), `tetra`(1). The builder must handle all nine (existing
`LatticeBuilder` covers only `bcc/fcc/hcp/diamond`; add `atom, dumbbell, ring8, sc, tetra`).

## Components

### 1. Data port & model

- Add `tools/port_elements_data.py` — reads the upstream `docs/assets/elements-data.js`, extracts the
  `ELEMENTS` array, expands short keys to the schema above, writes `Resources/elements.json`.
  Run once; commit script + output.
- Write `Assets/LearningWorlds/Resources/elements.json` (canonical runtime data, 118 elements).
- Delete the corrupt `Assets/LearningWorlds/Data/elements.json` stub.
- Rewrite `Assets/LearningWorlds/Scripts/Pse/ElementData.cs` to match the schema (Newtonsoft),
  with `LoadAll()` reading `Resources.Load<TextAsset>("elements")` and returning `List<ElementData>`.
- Add a category→colour map (10 normalized categories) and a `modelType`→builder map.

### 2. PSE Gallery (`Assets/LearningWorlds/Scripts/Pse/ElementGallery.cs`)

- Lay out elements from real `group`/`period`: x = group (1..18), z = period (1..9), with rows 8/9
  (lanthanides/actinides) offset below the main table; raise atoms onto a floor.
- Category-coloured `BohrAtomBuilder` atoms; trigger colliders for selection.
- Selecting an atom opens `ElementInfoPanel` (name, symbol, number, category, mass, phase, summary)
  with an "Enter room" action that sets `ElementRoom.SelectedElement` and calls
  `LearningWorldsApp.Go(World.ElementRoom, number)`.

### 3. ElementRoom (`Assets/LearningWorlds/Scripts/Pse/ElementRoom.cs`) — 5 stations

Central `BohrAtomBuilder` monument + fact card (name/number/mass/category) + summary card, then a ring
of **5 distinct** stations matching upstream order/colours:

1. **CRYSTAL & MOLECULE** — pedestal + `LatticeBuilder.BuildStructure(modelType, category, ...)`
   + `modelCaption` card.
2. **WHERE YOU MEET IT** — card listing `uses[]`.
3. **WHO FOUND IT** — card with `discovery`.
4. **THE CLASSIC EXPERIMENT** — card with `experiment`.
5. **TEST YOURSELF** — `QuizStation` generating the upstream 4 questions (atomic number, family,
   shell count, phase) with a deterministic shuffle, score and feedback.

Reuse `TextUtil.Wrap`, `PedestalCreator`, `TextMesh` + `LegacyRuntime.ttf`.

### 4. Wiring, entry & menus

- Remove the parallel `LearningWorldsApp.Builders` (`Func<int,IBuilder>`) dictionary and the unused
  `IBuilder`/`BaseBuilder` builder classes; keep a `CurrentWorldRoot` reference.
- `BuildingBuilders` is the only registry. Call `RegisterAllWorlds()` once from
  `InitializeApp()`; make `Register` idempotent (assignment, no hard error) and remove the per-class
  `RegisterInBuilders`/`Builders[...]=` side effects.
- `LearningWorldsApp.Go(World world, int element = -1)`: destroy `CurrentWorldRoot`; if
  `world == ElementRoom`, resolve `ElementRoom.SelectedElement` from `element` (atomic number);
  call `BuildingBuilders.GetBuilder(world)()`; store the returned root; call `EnsureRig()`.
  **Synchronous** — drop `async`/`LoadSceneAsync`/scene-name logic.
- `Portal.TriggerComponent.OnTriggerEnter`: call `LearningWorldsApp.Go(target_world)` directly
  (remove `.GetAwaiter().GetResult()`).
- `InitializeApp()`: keep the manager; build the Hub + rig **only when the bootstrap flag is set**
  (`SessionState` in editor / a static flag at runtime), so normal scene Play is unaffected.
- `Assets/LearningWorlds/Editor/LearningWorldsMenus.cs`: replace the broken scene items with:
  - **`LearningWorlds/Play Learning Worlds`** — `EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, Single)`,
    `SessionState.SetBool("LW_Boot", true)`, `EditorApplication.isPlaying = true`.
  - **`LearningWorlds/Open Element Room…`** — enter Play with `LW_Boot` set and
    `Go(World.ElementRoom, N)` for a chosen atomic number.
  - Drop `Open Hub Scene` / `Open PSE Gallery` / `Register Build Scenes` / `Create Hub Prefab`
    (they reference nonexistent scenes/do nothing useful).

## Verification

- **Static (scriptable, no Unity):**
  - `Resources/elements.json` parses; exactly **118** elements; atomic numbers 1..118 unique.
  - every element has non-empty `uses`, `experiment`, `discovery`, `modelType`.
  - `modelType` ⊆ {`atom, hcp, fcc, bcc, dumbbell, diamond, ring8, sc, tetra`}.
- **Unity compile gate:** `ci/unity-compile.sh` (batchmode) must pass.
- **Manual (editor Play):** menu → Play Learning Worlds → Hub → PSE portal → gallery of 118 →
  select element → room shows central atom + 5 populated stations; quiz answerable with score.

## Non-goals

- Convai tutors / NPCs, multiplayer (Photon), device build (editor play + compile gate only).
- Localization (English learning content / German signage only).
- PE eye-tracking / questionnaire integration in element rooms.

## Assumptions

- Upstream `elements-data.js` at `C:\Users\Tobias\git\periodic-table\docs\assets\elements-data.js`
  is the authoritative content source (118 elements, verified this session).
- Newtonsoft JSON (`com.unity.nuget.newtonsoft-json` 3.2.2) is available (already in the project).
