# Fathom - Development Plan

## Project Overview

### Game Concept
A 2D underwater adventure game where you control a diver exploring ocean depths, catching fish for profit, and upgrading equipment to dive deeper and stay underwater longer.

### Core Game Loop
```
Surface (Shop/Save/Refill Oxygen)
    ↓
Dive into Ocean (Oxygen Depleting)
    ↓
Hunt Fish with Spear Gun
    ↓
Inventory Full OR Low Oxygen
    ↓
Return to Surface
    ↓
Sell Fish & Buy Upgrades
    ↓
Repeat with Better Equipment
```

### Target Platform
- **Primary:** PC/Mac (Steam/itch.io)
- **Future:** Web (WebGL), Mobile (iOS/Android)

## Development Philosophy

**Build Vertical Slice First** - Prove the core loop is fun before expanding
**Iterate Based on Playtesting** - Test early and often
**Minimum Viable Product** - Ship a small, polished game rather than a large, incomplete one

## Technical Stack

### Development Environment
- **Engine:** Unity 2022.3 LTS (2D URP Template)
- **IDE:** Visual Studio Code
- **Version Control:** Git + GitHub + Git LFS
- **Platform:** Mac (Apple Silicon/Intel compatible)

### Required Unity Packages
- 2D Sprite
- 2D Animation
- 2D Tilemap
- Cinemachine (camera management)
- TextMeshPro (UI text)

## Revised Development Roadmap

### Phase 1: Vertical Slice (Weeks 1-2) ⭐ PRIORITY

**Goal:** Prove the core game loop is fun

#### Week 1: Foundation
- [x] Project setup & repository
- [ ] Unity 2D project initialization
- [ ] Basic scene setup (water, surface platform)
- [ ] Player swimming movement (WASD/Arrow keys)
- [ ] Simple physics (buoyancy, drag)
- [ ] Camera follow system

#### Week 2: Core Loop
- [ ] Oxygen system with UI bar
- [ ] Surface detection & air refill
- [ ] Spear gun implementation
- [ ] 1 basic fish type (slow, passive)
- [ ] Fish catching mechanic
- [ ] Simple surface shop (sell fish, buy 1 upgrade)
- [ ] Currency system

**Deliverable:** Playable loop - dive, catch 1 fish, surface, sell, buy faster swim fins

---

### Phase 2: Playtesting & Polish (Week 3)

**Goal:** Make the core loop feel great

- [ ] External playtesting (3-5 people)
- [ ] Collect feedback on:
  - Is swimming fun?
  - Is oxygen management stressful or annoying?
  - Is catching fish satisfying?
  - Do upgrades feel impactful?
- [ ] Iterate based on feedback
- [ ] Add juice/polish:
  - Particle effects (bubbles, splash)
  - Screen shake on catch
  - Sound effects placeholder
  - Better movement feel (acceleration curves)

**Decision Point:** If core loop isn't fun, pivot or redesign before continuing

---

### Phase 3: Content Expansion (Weeks 4-6)

**Goal:** Add variety and depth progression

#### Week 4: Fish Variety
- [ ] 3 fish types:
  - **Passive** - Slow, easy to catch, low value
  - **Evasive** - Fast, flees from player, medium value
  - **Schooling** - Moves in groups, medium value
- [ ] Fish AI behaviors
- [ ] Visual variety (different sprites/colors)

#### Week 5: Equipment Upgrades
- [ ] **Oxygen Tank Upgrades** (3 tiers)
  - Basic: 30s → Tier 2: 45s → Tier 3: 60s
- [ ] **Swim Fins** (3 tiers)
  - Base speed → +25% → +50%
- [ ] **Spear Gun Upgrades** (3 tiers)
  - Range/reload speed improvements
- [ ] Equipment UI & persistence
- [ ] Pricing balance

#### Week 6: Zone System
- [ ] Depth-based zones (3 zones)
  - **Shallows** (0-20m): Starter fish
  - **Mid-depth** (20-50m): Better fish, need oxygen upgrade
  - **Deep** (50-80m): Best fish, need all upgrades
- [ ] Visual differentiation (lighting, color grading)
- [ ] Zone-specific fish spawning
- [ ] Depth meter UI

---

### Phase 4: Systems & Persistence (Weeks 7-8)

**Goal:** Save progress and polish systems

#### Week 7: Save System
- [ ] JSON-based save system
- [ ] Save data structure:
  - Currency
  - Owned equipment
  - Unlocked zones
  - Statistics (fish caught, deepest dive)
- [ ] Auto-save on surface
- [ ] Load game on startup

#### Week 8: UI & Menus
- [ ] Main menu (New Game, Continue, Settings, Quit)
- [ ] Pause menu
- [ ] Shop UI polish
- [ ] HUD refinement:
  - Oxygen bar
  - Depth meter
  - Currency display
  - Inventory count
- [ ] Settings menu (volume, resolution, controls)

---

### Phase 5: Polish & Content (Weeks 9-10)

**Goal:** Make the game feel complete

#### Week 9: Audio
- [ ] Underwater ambience loop
- [ ] Sound effects:
  - Swimming
  - Spear gun fire
  - Fish catch
  - Surfacing splash
  - Shop purchase
- [ ] Background music (calm, atmospheric)
- [ ] Audio mixing

#### Week 10: Visual Polish
- [ ] Particle effects (bubbles, water distortion)
- [ ] Animation polish
- [ ] Lighting effects (depth darkness)
- [ ] UI animations
- [ ] Screen effects (low oxygen warning)

---

### Phase 6: Level Design & Balance (Weeks 11-12)

**Goal:** Create full game experience

#### Week 11: World Building
- [ ] Full ocean layout
- [ ] Interesting terrain/obstacles
- [ ] Hidden areas (optional treasure spots)
- [ ] Tutorial flow (gradual introduction)
- [ ] Progression gating (depth limits)

#### Week 12: Balance & Testing
- [ ] Economy balance:
  - Fish values
  - Equipment costs
  - Time to unlock all upgrades (target: 2-3 hours)
- [ ] Difficulty curve testing
- [ ] Bug fixes
- [ ] Performance optimization
- [ ] Quality settings (low/medium/high)

---

### Phase 7: Release Preparation (Weeks 13-14)

**Goal:** Ship the game

#### Week 13: Build & Deploy
- [ ] Build for PC/Mac
- [ ] Test builds on multiple machines
- [ ] Create game page (itch.io)
- [ ] Screenshots & GIFs
- [ ] Trailer video (30-60s)

#### Week 14: Launch
- [ ] Final bug fixes
- [ ] Marketing materials
- [ ] Press kit
- [ ] Submit to itch.io
- [ ] Share on social media
- [ ] Post-launch monitoring

---

## Technical Architecture

### Core Systems

```
GameManager.cs          // Game state, scene management, singleton
EconomyManager.cs       // Currency, transactions
SaveSystem.cs           // JSON save/load
InputManager.cs         // Input handling

PlayerController.cs     // Movement, physics
OxygenSystem.cs         // Air management
WeaponController.cs     // Spear gun
PlayerInventory.cs      // Fish storage

FishAI.cs              // Base fish behavior
PassiveFish.cs         // Simple movement
EvasiveFish.cs         // Flee from player
SchoolingFish.cs       // Group behavior

ZoneManager.cs         // Depth zones
FishSpawner.cs         // Population management

HUDController.cs       // Oxygen, depth, money display
ShopUI.cs              // Purchase interface
MenuManager.cs         // Scene transitions
```

### Design Patterns

**Singleton Pattern** for managers
```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

**Object Pooling** for fish and projectiles
```csharp
public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(prefab);
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
```

**Event System** for decoupling
```csharp
public static class GameEvents
{
    public static event Action<int> OnFishCaught;
    public static event Action<float> OnOxygenChanged;
    public static event Action<int> OnMoneyChanged;
    public static event Action<int> OnDepthChanged;
}
```

---

## Key Implementation Details

### Player Movement System
```csharp
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseSwimSpeed = 5f;
    public float acceleration = 2f;
    public float dragCoefficient = 1.5f;

    [Header("Physics")]
    public float buoyancy = 0.5f;
    public float terminalVelocity = 10f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = -buoyancy; // Upward force
        rb.drag = dragCoefficient;
    }

    void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * baseSwimSpeed;
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // Clamp velocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, terminalVelocity);
    }
}
```

### Oxygen System
```csharp
public class OxygenSystem : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygen = 30f;
    public float depletionRate = 1f; // Units per second

    private float currentOxygen;
    private bool isUnderwater = true;

    void Start()
    {
        currentOxygen = maxOxygen;
    }

    void Update()
    {
        if (isUnderwater)
        {
            ConsumeOxygen(depletionRate * Time.deltaTime);
        }
        else
        {
            RefillOxygen();
        }
    }

    void ConsumeOxygen(float amount)
    {
        currentOxygen -= amount;
        currentOxygen = Mathf.Max(currentOxygen, 0);

        GameEvents.OnOxygenChanged?.Invoke(currentOxygen / maxOxygen);

        if (currentOxygen <= 0)
        {
            OnOxygenDepleted();
        }
    }

    void OnOxygenDepleted()
    {
        // Force return to surface, lose some fish
        Debug.Log("Out of oxygen! Returning to surface...");
    }

    public void RefillOxygen()
    {
        currentOxygen = maxOxygen;
        GameEvents.OnOxygenChanged?.Invoke(1f);
    }
}
```

### Fish AI Base Class
```csharp
public abstract class FishAI : MonoBehaviour
{
    [Header("Base Stats")]
    public int monetaryValue = 10;
    public float swimSpeed = 3f;

    [Header("Behavior")]
    public float detectionRange = 5f;
    public Vector2 swimBounds = new Vector2(20, 20);

    protected Transform player;
    protected Vector2 targetPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ChooseNewTarget();
    }

    void Update()
    {
        UpdateBehavior();
    }

    protected abstract void UpdateBehavior();

    protected bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) < detectionRange;
    }

    protected virtual void ChooseNewTarget()
    {
        targetPosition = new Vector2(
            Random.Range(-swimBounds.x, swimBounds.x),
            Random.Range(-swimBounds.y, swimBounds.y)
        );
    }

    public virtual void OnCaught()
    {
        GameEvents.OnFishCaught?.Invoke(monetaryValue);
        Destroy(gameObject);
    }
}
```

---

## Game Design Decisions

### What Happens When Oxygen Runs Out?
**Option A (Recommended):** Force teleport to surface, lose 50% of caught fish
- Creates tension without full punishment
- Encourages strategic surfacing

**Option B:** Game over, lose all fish
- Higher stakes, more hardcore
- May be frustrating for casual players

**Decision:** Use Option A for initial release

### Zone Access Gating
**Soft Gating:** Deeper zones are accessible but deadly without upgrades
- Player takes pressure damage below certain depth
- Encourages exploration but requires upgrades for safety

### Session Length Target
**One Complete Cycle:** 2-3 minutes (dive → catch → surface → sell)
**Unlock All Equipment:** 2-3 hours of gameplay
**Full Completion:** 4-5 hours (including secrets/achievements if added)

---

## Asset Resources

### Free Art Assets
- **Kenney.nl** - UI elements, sprites
- **OpenGameArt.org** - Underwater tilesets
- **itch.io** - Free fish sprite packs
- **Pixel Art Tools:** Aseprite, Piskel

### Free Audio Assets
- **Freesound.org** - Sound effects
- **OpenGameArt.org** - Music loops
- **Zapsplat** - Water/bubble sounds

### Tools
- **Level Design:** Unity Tilemap Editor
- **Animation:** Unity Animator
- **Audio Editing:** Audacity

---

## Performance Targets

### Target Frame Rate
- **PC/Mac:** 60 FPS minimum
- **Web:** 30 FPS minimum

### Optimization Strategies
- Object pooling for fish/projectiles
- Sprite atlasing
- Occlusion culling for deep zones
- LOD system (optional):
  - Close: Full animation
  - Far: Simplified animation or static sprite

---

## Testing & QA

### Playtest Checklist

**Core Mechanics**
- [ ] Swimming feels responsive and smooth
- [ ] Oxygen system creates appropriate tension
- [ ] Catching fish is satisfying
- [ ] Upgrades feel impactful

**Economy**
- [ ] Fish values feel fair
- [ ] Equipment costs are balanced
- [ ] Progression feels rewarding
- [ ] No exploits or cheese strategies

**Technical**
- [ ] Stable frame rate on target hardware
- [ ] No game-breaking bugs
- [ ] Save/load works reliably
- [ ] UI scales properly on different resolutions

**User Experience**
- [ ] Tutorial is clear and concise
- [ ] Menus are intuitive
- [ ] Controls feel natural
- [ ] Audio levels are balanced

---

## Risk Management

### Primary Risks

**Risk:** Core loop isn't fun
**Mitigation:** Vertical slice + early playtesting (Week 3 decision point)
**Backup Plan:** Pivot to arcade scoring system vs. upgrade progression

**Risk:** Scope creep
**Mitigation:** Strict adherence to phase plan, defer features to v2.0
**Feature Parking Lot:** Bosses, achievements, secrets, daily challenges

**Risk:** Art production bottleneck
**Mitigation:** Use free assets initially, focus on mechanics
**Alternative:** Embrace minimalist art style

---

## Post-Launch Content (v2.0)

### Potential Features
- Boss encounters (giant fish, octopus)
- Achievement system
- Hidden treasures & secrets
- Equipment customization (visual)
- New zones (coral reefs, shipwrecks, trenches)
- Story/lore elements
- Leaderboards (deepest dive, most money)
- Daily challenges

### Platform Expansion
- WebGL build for easy sharing
- Mobile port with touch controls
- Steam release with achievements

---

## Version Control Strategy

### Branch Strategy
- **main** - Production-ready code
- **develop** - Active development
- **feature/*** - Individual features
- **hotfix/*** - Bug fixes

### Commit Conventions
```
feat: Add fish schooling behavior
fix: Resolve oxygen UI not updating
polish: Improve swimming animation
refactor: Simplify fish AI inheritance
```

---

## Development Environment Setup

### Mac Setup Commands
```bash
# Install Unity Hub (manually from website)
# Install Unity 2022.3 LTS through Unity Hub

# VS Code Extensions
code --install-extension visualstudiotoolsforunity.vstuc
code --install-extension ms-dotnettools.csdevkit
code --install-extension jordan-matthes.unity-snippets

# Git LFS (if not already installed)
brew install git-lfs
git lfs install
```

### Unity Project Structure
```
Assets/
├── Scenes/
│   ├── MainMenu.unity
│   ├── Game.unity
│   └── Testing.unity
├── Scripts/
│   ├── Core/
│   ├── Player/
│   ├── Fish/
│   ├── UI/
│   └── Managers/
├── Prefabs/
│   ├── Player/
│   ├── Fish/
│   └── UI/
├── Sprites/
│   ├── Player/
│   ├── Fish/
│   ├── Environment/
│   └── UI/
├── Audio/
│   ├── Music/
│   └── SFX/
└── Resources/
    └── SaveData/
```

---

## Debug Tools

### In-Game Debug Panel
```csharp
#if UNITY_EDITOR
public class DebugPanel : MonoBehaviour
{
    private bool showDebug = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            showDebug = !showDebug;
        }
    }

    void OnGUI()
    {
        if (!showDebug) return;

        GUILayout.BeginArea(new Rect(10, 10, 200, 300));
        GUILayout.Label("=== DEBUG MENU ===");

        if (GUILayout.Button("Add $1000"))
        {
            EconomyManager.Instance.AddMoney(1000);
        }

        if (GUILayout.Button("Refill Oxygen"))
        {
            OxygenSystem.Instance.RefillOxygen();
        }

        if (GUILayout.Button("Unlock All Equipment"))
        {
            // Unlock logic
        }

        GUILayout.EndArea();
    }
}
#endif
```

---

## Success Metrics

### Release Goals
- **Completion Rate:** 50% of players finish the game
- **Session Length:** Average 30+ minutes per session
- **Player Feedback:** 4+ stars on itch.io
- **Downloads:** 1000+ in first month

### Learning Goals
- Complete a full game development cycle
- Ship a polished, playable product
- Build portfolio piece
- Learn Unity 2D fundamentals

---

## Next Steps

1. ✅ Create GitHub repository
2. ✅ Initialize project structure
3. ✅ Write development plan
4. [ ] Initialize Unity project
5. [ ] Set up VS Code integration
6. [ ] Create first scene (ocean + surface)
7. [ ] Implement player movement prototype

---

## Notes & Resources

### Useful Unity Tutorials
- Brackeys - 2D Movement
- Sebastian Lague - Camera Following
- Blackthornprod - 2D Water Effects

### Community
- Unity Forums
- Reddit: r/Unity2D, r/IndieDev
- Discord: Unity Developer Community

---

**Last Updated:** 2025-11-01
**Current Phase:** Phase 1 - Vertical Slice (Week 1)
