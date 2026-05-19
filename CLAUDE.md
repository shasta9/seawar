# Seawar - Project Context for Claude Code

## About the Developer
I have nearly 50 years of software development experience. My primary professional languages
were C and C#, with embedded C development using Keil uVision and STM32CubeIDE. I am
experienced and opinionated about good software design but am learning Go as part of this
project. Please don't oversimplify explanations, but do point out Go idioms and patterns
where they differ from C or C# conventions.

## Project Overview
Seawar is a networked two-player naval warfare game, recreating a game originally written
in HP Time-Shared BASIC, first developed around 1977 on an HP 2000B/C time-sharing system.
The original used a shared disc file with locking as its IPC mechanism between two user
sessions - a constraint we are now free of.

The game is an asymmetric two-player experience:
- **Player 1 - The Submarine commander:** Moves covertly around a map, attacking merchant
  ships, managing fuel and torpedoes, returning to port for repairs and re-arming.
- **Player 2 - The Destroyer commander:** Hunts the submarine using intelligence reports,
  manages fuel and depth charges, returns to port for repairs and re-arming.

### Core Game Mechanics
- A map of a sea area with ports, shipping lanes, and merchant ship positions
- The submarine attacks merchant ships; each attack triggers a distress signal to the
  destroyer with the attacked ship's position
- "Spy" reports notify each player when the other enters port for repairs, refuelling,
  or re-arming - providing intelligence without direct visibility
- Both players manage consumable resources (fuel, weapons) and must make port periodically
- Fog of war is central to the experience - neither player has full visibility of the other

### Design Philosophy
- The original was played on 300 baud teletypes - information was naturally rationed and
  there was real tension in deciding to print a map. Preserve this spirit: information
  should be on demand, not constantly pushed at the player.
- The asymmetric information model (each player knows different things) is core to the
  game's appeal and should be carefully maintained in the architecture.
- Keep the feel of the original where it enhances gameplay, while taking advantage of
  modern capabilities where they improve the experience.

## Technical Architecture

### Language and Platform
- **Language:** Go
- **Target platforms:** macOS (primary development), Windows, Linux
- Cross-platform compatibility is a firm requirement - use only standard library or
  well-supported cross-platform packages where possible.

### Architecture Pattern
- **Client-server:** A single server process manages all game state. Two client processes
  connect over a network - one per player.
- **Clean separation between model and presentation layer is mandatory.** The game logic
  and state must be completely independent of how it is rendered. This is to facilitate
  a future migration from terminal to GUI without rewriting the game logic.
- Think in terms of MVC or a similar pattern: the server owns the model, clients own
  the view, and the protocol between them defines the controller interactions.

### Presentation
- **Phase 1:** Terminal/text interface. Simple, faithful to the original spirit.
- **Phase 2 (future):** GUI - to be decided, but the clean model/presentation separation
  should make this straightforward when the time comes.

### Networking
- Two remote players connecting over the internet (not LAN-only)
- The server should be hostable by either player
- Consider how to handle connection drops and reconnection gracefully - the game can take
  a while to play and network interruptions are a reality

### Game State
- All authoritative game state lives on the server
- Clients receive only the information their player is entitled to see - the server is
  responsible for filtering state before sending to each client
- This is architecturally important: a client should never receive information that breaks
  the fog of war, even if it doesn't display it

## Go-Specific Notes
- This is my first Go project. Please explain Go idioms as they arise, particularly where
  they differ from C or C# patterns I may default to.
- Favour idiomatic Go over transliterations of C or C# patterns
- Goroutines and channels are likely the right approach for managing concurrent client
  connections and game state updates - please guide me toward good patterns here
- The project should follow standard Go project layout conventions

## Game Flow and Timing Model
The game is **real-time with action-locking**, not turn-based:
- Both players act concurrently and independently
- When a player issues a command, they are locked out of further input until that action
  completes (e.g. a move resolves, a weapon fires)
- The other player can act freely during this time
- This maps naturally to Go goroutines - one per player connection, with the server
  managing state updates safely via channels or mutexes

### Session Startup
- First player to connect chooses their role (submarine or destroyer)
- Second player to connect gets the remaining role
- Each player is asked if they are ready
- The game starts when both players confirm ready
- Server is authoritative on session state throughout

### Commands
- Original used single-character commands for speed on 300 baud teletypes
- Commands covered: movement, weapons (torpedoes, depth charges, guns), depth changes,
  port entry, and status queries
- Consider preserving single-character commands in Phase 1 terminal version for
  authenticity, with the option to support longer commands in Phase 2 GUI

## Map Design
- **Size:** 64x64 grid
- **Cell types:** Each cell is either land or sea
- **Entities:** A sea cell can contain a merchant ship, the destroyer, or the surfaced
  submarine. A submerged submarine can occupy the same cell as another entity - depth
  provides a third dimension to the 2D grid.
- **Three-character cell rendering:** Each cell is represented by 3 ASCII characters in
  the terminal view, e.g.:
  - Land: `***`
  - Open sea: `...` (or similar)
  - Surfaced sub: `(+)` (tentative - to be confirmed)
  - Other entities TBD
- **Local view:** 9x9 grid centred on the player's position (4 cells either side).
  At 3 chars per cell this is 27 characters wide - comfortable on any terminal.
- **Full map print:** A startup command prints the entire 64x64 map (land/sea only,
  no entities) in sections designed to be joined together for reference. Preserve
  this in the modern version for authenticity.
- **Depth:** The submarine has a depth value. When submerged sufficiently, it can occupy
  the same grid cell as surface entities without being directly visible. Detection
  likely depends on depth differential and destroyer equipment.

### Map file contents (geography only)
- Land/sea cell layout
- Port locations (4 total: 2 per side)

### Server initialises at game start (not in map file)
- Submarine starting position (random sea cell)
- Destroyer starting position (random sea cell)
- Merchant ship positions (random sea cells)
- Constraints for random placement TBD (e.g. minimum separation
  between starting positions, not adjacent to enemy ports)

## Movement Model
- **Command syntax:** `M` prompts for `Bearing, Distance, Speed?` e.g. `135,4,18`
  Shorthand: `M,135,4,18` enters everything on one line.
- **Bearing:** Compass bearing in degrees. Only multiples of 45° are valid:
  0, 45, 90, 135, 180, 225, 270, 315. This constraint adds tactical depth,
  particularly for torpedo firing solutions where the sub must anticipate target position.
- **Distance:** Number of grid squares to move along the chosen bearing.
- **Speed:** In knots. Scaled to real elapsed time - a fast long move takes proportionally
  longer and locks the player out for that duration. Committing to a move is a real
  tactical decision.
- **Action duration:** Moves have real time duration proportional to distance/speed.
  Both players can be mid-move simultaneously. The server must handle concurrent
  in-progress actions correctly - this is a key concurrency requirement.
- **Action locking:** A player cannot issue further commands until their current action
  completes. The other player is unaffected and can act freely during this time.

## What We Don't Know Yet
The following game design details need to be worked out and this file updated as decisions
are made:
- Port locations on the map (approximate positions)
- Combat mechanics in detail (torpedo range/arcs, depth charge blast radius, ASDIC range)
- Battery capacity and drain rates (submerged speed vs range tradeoffs)
- Hull damage sink timer (how long before a leaking sub sinks)
- Exact command set for each player role
- Full cell rendering symbol set
- Damage thresholds (how much total damage before sinking)

## Map Design Tool
- The map is a separate concern from the game and should be designed/generated by a
  separate utility, not hardcoded into the game server.
- **Map file format:** JSON, pretty-printed. Each row of cells is encoded as a
  64-character string of `'0'` (sea) and `'1'` (land) characters — compact and
  human-readable. The server loads this file at startup. See `pkg/mapdata/` for the
  authoritative type definitions and JSON encoding.
- **Coordinate convention:** `(0,0)` is top-left. X increases east, Y increases south.
  Grid access is `Cells[Y][X]`.
- **North/south split:** The map is divided at row 32 (height/2).
  - Sub ports and sub starting position: southern half (rows 32–63)
  - Destroyer ports and destroyer starting position: northern half (rows 0–31)
- **Procedural generation:** `cmd/mapgen` is the generator tool. It supports:
  - `--seed` — RNG seed (0 = random; printed to stderr for reproducibility)
  - `--island-density` — probability of land in the initial noise pass before CA
    smoothing (default 0.35). Actual land coverage after smoothing will be
    significantly lower (~10–15% at the default). Controls island density, not
    final land ratio.
  - `--merchants` — number of merchant ships (default 10)
  - `--name`, `--desc` — map metadata
  - `--out` — output file path (default: stdout)
  - Retries up to 10 times with derived seeds if placement or validation fails.
- **Generation pipeline:** random noise seed → 5× cellular automata smoothing →
  remove tiny islands → ensure single connected sea region (carve corridors if
  needed) → place ports → validate.
- **Validation:** `pkg/mapdata/validate.go` runs named checks (ports on sea, coastal,
  reachable from each other, minimum separation). Used by both the generator and the
  server at map load time.
- **Balance implications:** Different maps fundamentally change game balance. Choke
  points favour the destroyer (forcing the sub into predictable paths). Open maps
  favour the submarine (more hiding room). This is a feature, not a bug.
- **Original:** Only one map existed. Multiple maps are an enhancement.
- **Status:** Map generator complete (`cmd/mapgen`). Shared types in `pkg/mapdata/`.

## Development Approach
- Start with the game state model before touching networking or presentation
- Get the core data structures and game logic solid first
- Test the model thoroughly before adding the network layer
- Add the terminal presentation last in Phase 1

## Project Status
- Environment setup: complete
- Map generator (`cmd/mapgen`): complete
- Game server: not started
- Client: not started
- Development machine: MacBook Air M4 15-inch, 24GB RAM, 512GB SSD
- Claude Code subscription: Pro plan

## Depth Mechanics (Submarine)
- **Range:** 0 (surfaced) to 300 feet, in increments of 10 feet
- **Periscope depth:** 30 feet is special - the submarine can raise/lower its periscope
  - Scope up: player can view the local 9x9 area, but the centre cell of the sub's
    display shows '+', making it visible on the destroyer's map
  - Scope down: submarine is not visible on the destroyer's map
- **Diving and surfacing** take real elapsed time (like movement - action-locking applies)
- **Collision penalties:**
  - Surfacing into an occupied cell damages the submarine
  - Raising the periscope in an occupied cell damages the periscope
- **Firing constraints:**
  - Deck gun: surfaced only (depth 0)
  - Torpedoes: surfaced or submerged down to periscope depth (0-30 feet)
  - At depth > 30 feet: no weapons can be fired

## Detection and Sensors

### Submarine Sensors
- **Hydrophones:** Reports bearing and contact type of propeller noise within detection
  range. Types: high-speed (destroyer) or low-speed (merchant ship). Cannot detect
  stationary targets. Primary method for locating merchant ship targets.

### Destroyer Sensors
- **Hydrophones:** Same as submarine - bearing and type, no distance, cannot detect
  stationary targets.
- **ASDIC (sonar):** Reports bearing AND distance to the submarine, even when stationary.
  This is the destroyer's primary sub-hunting tool and creates the core asymmetry:
  the sub must keep moving (burning fuel, making noise) while the destroyer can
  systematically search.

## Weapons

### Submarine
- **Deck gun:** Surface only. Direct fire against surface targets.
- **Torpedoes:** Fireable surfaced or at periscope depth (0-30 feet).

### Destroyer
- **Deck gun:** Direct fire against surface targets (including surfaced submarine).
- **Depth charges:**
  - Detonation depth set by a separate command; applies until changed
  - Can be dropped while stationary (one at a time)
  - Can be dropped sequentially during a move by pressing a key - creates a pattern
    along the movement track, requiring the destroyer to predict the sub's position.
    This is a genuine skill mechanic.

## Damage Model
- Each entity (submarine, destroyer, merchant ships?) has multiple subsystems
- Each subsystem can sustain damage at 3 levels of increasing severity
- When total damage across all subsystems exceeds a threshold, the entity sinks
- Sinking ends the game
- Damaged but functional subsystems create interesting degraded-capability gameplay
  (e.g. damaged periscope, damaged engines affecting speed, damaged ASDIC)
- Specific subsystems and damage thresholds TBD

## Win Condition
- Last entity afloat wins. The game ends immediately when either the submarine or
  destroyer sinks.
- The destroyer is generally faster and more powerful, but a well-played submarine
  can and often does win with torpedo hits and gunfire.
- Future enhancement: points system based on game actions and goals (merchants sunk,
  damage dealt, etc.) to add strategic depth beyond simple survival.

## Merchant Ships
- Stationary - do not move during the game
- Show up on hydrophones despite being stationary (background noise source)
- Primary targets for the submarine; sinking them is the sub's strategic goal
- Stationary targets chosen deliberately: the 45-degree movement constraint and
  torpedo firing mechanics make moving targets too hard to hit

## Ports
- 4 ports total: 2 per player (sub has 2, destroyer has 2)
- Special map locations - just cells designated as ports
- Entering port allows: refuelling, damage repair, and restocking
  (shells, torpedoes, depth charges as appropriate)
- Spy mechanic: entering port triggers an intelligence report to the other player

## Subsystem Damage Model

### Submarine Subsystems
- **Engines:** Damage reduces maximum speed (surface and submerged)
- **Batteries:** Powers the sub when submerged. Damage reduces submerged range and
  max speed. High submerged speed drains batteries faster even when undamaged.
  The sub runs on diesel when surfaced, electric when submerged.
  Battery failure behaviour by depth:
  - Below periscope depth (>30ft): cannot move at all - completely stranded
  - At periscope depth (30ft): can move using snorkel, running on diesel, but the
    snorkel mast is visible on the destroyer map (different symbol to periscope).
    This is often tactically worse than surfacing, since surfacing opens up the
    option of attacking the destroyer with the deck gun.
  - Surfaced: unaffected, diesel as normal.
  - Note: snorkel may also be available as a normal option with working batteries -
    to be confirmed.
- **Periscope:** Damage may prevent use at periscope depth
- **Ballast tanks:** Damage restricts maximum diving depth - forces sub to stay
  shallower and therefore more detectable
- **Hull:** Damage = leaking = will sink within a time limit if not repaired.
  Most severe subsystem damage. Creates a racing clock alongside fuel depletion.
- **Hydrophones:** Damage = no passive detection capability
- **Guns (deck gun):** Damage reduces or prevents surface gunnery
- **Torpedo tubes:** Damage reduces number of functional tubes / ability to fire
- **Fuel tanks:** Damage causes slow fuel depletion even when stationary.
  Combined with hull damage creates two simultaneous racing clocks.

### Destroyer Subsystems
- **Engines:** Damage reduces maximum speed
- **Hull:** Damage = leaking = will sink within a time limit if not repaired
- **Hydrophones:** Damage = no passive detection
- **ASDIC:** Damage = loss of active sonar - major tactical disadvantage
- **Guns:** Damage reduces or prevents gunnery
- **Depth charge systems:** Damage reduces or prevents depth charge deployment
- **Fuel tanks:** Damage causes slow fuel depletion
