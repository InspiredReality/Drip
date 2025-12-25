# Drip - Unity Game Scripts

## Project Overview
Drip is a 3D turn-based multiplayer game built in Unity 6.3 for web (mobile-first) and mobile platforms.

**Core Gameplay:**
- 2-player turn-based game with async multiplayer support
- 3-day move timer per turn
- 3 phases per turn: Drop, Divide, Douse

## Folder Structure

```
Drip_Scripts/
├── Core/                    # Core game systems
│   ├── CameraController.cs  # Manages camera transitions between phases
│   └── InputManager.cs      # Cross-platform input handling (mouse/touch/keyboard)
│
├── Managers/                # Central management systems
│   ├── GameManager.cs       # Main singleton, game state, turn management
│   ├── PhaseManager.cs      # Handles phase transitions
│   ├── TurnManager.cs       # Turn timer and turn-based logic
│   └── SaveManager.cs       # Game persistence (save/load)
│
├── Player/                  # Player-related scripts (future expansion)
│
├── Phases/                  # Phase-specific controllers
│   ├── IPhaseController.cs  # Interface for all phase controllers
│   │
│   ├── Drop/               # Phase 1: Drop water droplets
│   │   ├── DropPhaseController.cs
│   │   ├── WaterDroplet.cs
│   │   └── Bucket.cs
│   │
│   ├── Divide/             # Phase 2: Spend water on upgrades
│   │   └── DividePhaseController.cs
│   │
│   └── Douse/              # Phase 3: Attack opponents
│       ├── DousePhaseController.cs
│       ├── PlayerBalloon.cs
│       └── WaterProjectile.cs
│
├── Multiplayer/            # Networking and multiplayer
│   └── NetworkManager.cs   # Placeholder for networking implementation
│
├── UI/                     # User interface
│   └── UIManager.cs        # Main UI controller
│
├── Data/                   # Data structures
│   ├── PlayerData.cs       # Player information and stats
│   ├── GameData.cs         # Serializable game state
│   └── GameConstants.cs    # Centralized configuration values
│
├── Utilities/              # Helper systems
│   └── ObjectPooler.cs     # Object pooling for performance
│
├── Audio/                  # Audio management
│   └── AudioManager.cs     # Sound effects and music
│
└── VFX/                    # Visual effects
    └── ParticleManager.cs  # Particle system management
```

## Setup Instructions

### 1. Unity Setup
1. Import all scripts into your Unity project under `Assets/Scripts/`
2. Create empty GameObjects in your scene hierarchy:
   - `GameManager` (attach GameManager, PhaseManager, TurnManager, SaveManager)
   - `NetworkManager` (attach NetworkManager)
   - `AudioManager` (attach AudioManager)
   - `UIManager` (attach UIManager)
   - `InputManager` (attach InputManager)
   - `ObjectPooler` (attach ObjectPooler)
   - `ParticleManager` (attach ParticleManager)

### 2. Scene Setup
Recommended scenes:
- **MainMenu** - Title, login, game mode selection
- **Lobby** - Matchmaking, player setup
- **GameScene** - Main gameplay (all 3 phases occur here)
- **ResultsScreen** - End game results
- **Settings** - Audio/graphics settings

### 3. GameScene Hierarchy Example
```
GameScene
├── Managers
│   ├── GameManager (with PhaseManager, TurnManager, SaveManager)
│   ├── NetworkManager
│   ├── AudioManager
│   ├── UIManager
│   ├── InputManager
│   └── ObjectPooler
│
├── Camera
│   ├── Main Camera (attach CameraController)
│   ├── DropPhasePosition (empty transform)
│   ├── DividePhasePosition (empty transform)
│   └── DousePhasePosition (empty transform)
│
├── Environment
│   ├── DropPhase Area
│   │   ├── DropSpawnPoint (water source)
│   │   ├── Bucket (attach Bucket.cs)
│   │   └── Ground (trigger for missed droplets)
│   │
│   └── DousePhase Area
│       ├── BalloonPosition1
│       └── BalloonPosition2
│
├── UI Canvas
│   ├── MainMenuPanel
│   ├── GameplayPanel
│   ├── PausePanel
│   └── GameOverPanel
│
└── Prefabs
    ├── WaterDroplet
    ├── WaterProjectile
    └── PlayerBalloon
```

### 4. Prefab Setup

**WaterDroplet Prefab:**
- Add `WaterDroplet.cs` script
- Add `Rigidbody` (gravity enabled)
- Add `SphereCollider` (isTrigger = true)
- Tag: none needed

**Bucket Prefab:**
- Add `Bucket.cs` script
- Add `BoxCollider` (isTrigger = true)
- Tag: "Bucket"

**PlayerBalloon Prefab:**
- Add `PlayerBalloon.cs` script
- Add UI Canvas (World Space)
- Add health bar slider
- Add player name text
- Add `SphereCollider` (isTrigger = true)
- Tag: "Balloon"

**WaterProjectile Prefab:**
- Add `WaterProjectile.cs` script
- Add `Rigidbody` (gravity disabled)
- Add `SphereCollider` (isTrigger = true)

### 5. Tags to Create
- `Bucket`
- `Ground`
- `Balloon`
- `Player`

### 6. Manager Configuration

**GameManager:**
- Assign PhaseManager reference
- Assign TurnManager reference
- Set max players (2)
- Set turn time limit (259200 seconds = 3 days)
- Set starting balloon health (100)

**PhaseManager:**
- Assign DropPhaseController
- Assign DividePhaseController
- Assign DousePhaseController

**CameraController:**
- Assign drop/divide/douse phase position transforms
- Set transition speed

**UIManager:**
- Assign all UI panel references
- Assign text field references
- Assign button references

## Key Scripts Explanation

### GameManager.cs
Main singleton that controls:
- Overall game state (MainMenu, Playing, Paused, GameOver)
- Current game phase (Drop, Divide, Douse)
- Player data management
- Phase and turn advancement
- Win/loss conditions

**Key Methods:**
- `StartNewGame(playerNames)` - Initialize new game
- `AdvancePhase()` - Move to next phase
- `EndTurn()` - Complete turn and switch players
- `ChangeGameState(newState)` - Change overall game state

### Phase Controllers
Each phase implements `IPhaseController`:
- `EnterPhase()` - Initialize phase
- `ExitPhase()` - Clean up phase
- `IsPhaseComplete()` - Check if ready to advance

**DropPhaseController:**
- Spawns water droplets
- Manages collection
- Tracks water earned

**DividePhaseController:**
- Handles upgrades (drainage systems)
- Water gun purchase
- Ammo conversion

**DousePhaseController:**
- Spawns player balloons
- Manages shooting mechanics
- Tracks damage dealt

### Data Flow
1. User starts game → `GameManager.StartNewGame()`
2. Game creates player data → `PlayerData` objects
3. Turn begins → `TurnManager.StartNewTurn()`
4. Phase starts → `PhaseManager.SetPhase(Drop)`
5. Player completes phase → `GameManager.AdvancePhase()`
6. After Douse phase → `GameManager.EndTurn()`
7. Game state saved → `SaveManager.SaveGameState()`
8. Check win condition → If balloon health ≤ 0, game over

## Multiplayer Implementation

The `NetworkManager.cs` is a placeholder. To implement multiplayer:

### Option 1: REST API + Database (Recommended for turn-based)
- Use UnityWebRequest for HTTP calls
- Store game state in backend (Firebase, PlayFab, custom server)
- Players fetch game state when it's their turn
- No real-time connection needed

### Option 2: Photon Unity Networking (PUN)
- Import Photon Unity Networking asset
- Replace NetworkManager with Photon callbacks
- Use Photon RPC for turn-based actions

### Option 3: Unity Netcode for GameObjects
- Use Unity's official networking solution
- Good for real-time, less ideal for async turn-based

## Mobile Optimization Tips

1. **Object Pooling:** Use `ObjectPooler` for water droplets
2. **Batch Draw Calls:** Combine materials where possible
3. **Simplified Physics:** Use triggers instead of collisions
4. **LOD (Level of Detail):** Reduce polygon count for distant objects
5. **Texture Compression:** Use appropriate compression for mobile
6. **Audio Compression:** Use compressed audio formats

## WebGL Build Settings

1. **Publishing Settings:**
   - Compression Format: Gzip
   - Enable WebGL 2.0
   - Disable Exception Support (for smaller build)

2. **Player Settings:**
   - Rendering: Auto Graphics API
   - Color Space: Linear (for better visuals)

3. **Memory:**
   - Increase memory size if needed in index.html

## Testing Checklist

- [ ] Drop phase spawns droplets correctly
- [ ] Bucket collects droplets properly
- [ ] Water amount updates in UI
- [ ] Divide phase UI shows correct costs
- [ ] Upgrades deduct water correctly
- [ ] Douse phase spawns all balloons
- [ ] Shooting consumes ammo
- [ ] Damage applies to correct balloon
- [ ] Turn timer counts down
- [ ] Turn switches to next player
- [ ] Game ends when balloon health reaches 0
- [ ] Save/load works correctly
- [ ] Camera transitions between phases
- [ ] Input works on mouse and touch

## Next Steps

1. **Art Assets:**
   - Create water droplet model/sprite
   - Design balloon models
   - Create UI graphics
   - Design background environments

2. **Audio:**
   - Water splash sound
   - Collection sound
   - Water gun shooting sound
   - Balloon hit/pop sounds
   - Background music

3. **Multiplayer Backend:**
   - Set up server or use cloud service
   - Implement authentication
   - Create matchmaking system
   - Add push notifications for turn alerts

4. **Polish:**
   - Particle effects for water
   - Smooth animations
   - Tutorial system
   - Achievement system
   - Leaderboards

## Common Issues & Solutions

**Issue:** Droplets fall through bucket
- **Solution:** Ensure bucket has trigger collider, check droplet speed

**Issue:** Turn timer doesn't update
- **Solution:** Verify TurnManager is updating in Update() loop

**Issue:** Phase doesn't advance
- **Solution:** Check PhaseManager references in inspector

**Issue:** Players can't join multiplayer
- **Solution:** Implement actual networking in NetworkManager

## Contact & Support

For questions or issues with these scripts, refer to Unity documentation or the project repository.

---

**Version:** 1.0  
**Unity Version:** 6.3  
**Target Platforms:** WebGL (mobile-first), iOS, Android
