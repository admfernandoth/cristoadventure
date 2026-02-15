# Cristo Adventure - System Integration Documentation

## Overview

The Cristo Adventure system integration provides a robust, ordered initialization framework that ensures all game systems are properly initialized and connected before gameplay begins.

## Architecture

### Core Components

1. **MasterGameInitializer.cs** - Controls initialization order
2. **GameRuntime.cs** - Main runtime coordinator
3. **SystemDiagnostics.cs** - Logging, monitoring, and debugging

### System Initialization Order

The systems are initialized in strict dependency order:

1. **GameManager** (first) - Core game state and data management
2. **SaveManager** - Local and cloud save operations
3. **AudioManager** - Music, SFX, and voiceover playback
4. **UIManager** - All UI screens and elements
5. **LocalizationManager** - Multi-language support
6. **FirebaseManager** - Cloud services (optional)
7. **DialogueManager** - NPC dialogue system
8. **PuzzleManager** - Educational puzzles
9. **PhaseManager** (per scene) - Scene-specific logic

## Usage

### Automatic Initialization

The system automatically initializes when the game starts. Add the **MasterGameInitializer** component to your initial scene (e.g., MainMenu or Bootstrap scene).

```csharp
// The initializer will:
// 1. Create all manager singletons in order
// 2. Auto-wire connections between systems
// 3. Load player data
// 4. Set up event subscriptions
// 5. Initialize runtime systems
```

### Manual Initialization

If you need to manually trigger initialization:

```csharp
// Access the initializer
var initializer = MasterGameInitializer.Instance;

// Check if initialized
if (initializer.IsInitialized)
{
    // Systems are ready
}

// Subscribe to initialization complete
initializer.OnInitializationComplete += () =>
{
    Debug.Log("All systems ready!");
};
```

## System Connections

### Auto-Wired Connections

The following connections are automatically established:

1. **GameManager → All Managers**
   - Central coordination point
   - Accessible via singleton pattern

2. **UIManager → GameManager**
   - State change notifications
   - Game flow control

3. **POI → PhaseManager**
   - Visit tracking
   - Progress monitoring

4. **PuzzleManager → GameManager**
   - Reward distribution
   - Progress saving

5. **SaveManager → FirebaseManager**
   - Cloud sync
   - Remote save storage

### Manual Connections

To connect to system events:

```csharp
// Subscribe to game state changes
GameManager.Instance.OnGameStateChanged += (state) =>
{
    Debug.Log($"Game state: {state}");
};

// Subscribe to phase completion
var phaseManager = FindObjectOfType<PhaseManager>();
phaseManager.OnPhaseComplete += (stars, time) =>
{
    Debug.Log($"Phase complete: {stars} stars");
};
```

## Diagnostics and Debugging

### System Logging

The **SystemDiagnostics** class provides comprehensive logging:

```csharp
// Log system events
SystemDiagnostics.LogSystemEvent("MySystem", "Something happened");

// Log system connections
SystemDiagnostics.LogSystemConnection("SystemA", "SystemB", "Purpose");

// Log warnings
SystemDiagnostics.LogSystemWarning("MySystem", "Potential issue detected");

// Log errors
SystemDiagnostics.LogSystemError("MySystem", "Error occurred", stackTrace);

// Log performance
SystemDiagnostics.LogPerformance("MySystem", durationMs, "Operation name");
```

### Debug Console

Press **F11** in-game to open the debug console.

**Available Commands:**
- `help` - Show available commands
- `list` / `ls` - List all active systems
- `status <system>` - Show system status
- `logs [n]` - Show last n log entries
- `errors` - Show all errors
- `warnings` - Show all warnings
- `clear` - Clear console
- `save` - Save logs to file
- `fps` - Show current FPS
- `memory` - Show memory usage
- `phase` - Show current phase info
- `player` - Show player info

### Performance Monitoring

Press **F10** to toggle the debug overlay showing:
- Current FPS
- Memory usage
- Player level and coins
- Game state
- Error and warning counts

## Best Practices

### 1. System Access

Always use singleton instances to access systems:

```csharp
// Correct
var gameManager = GameManager.Instance;
var audioManager = AudioManager.Instance;

// Avoid creating new instances
```

### 2. Event Subscriptions

Subscribe to events during initialization and unsubscribe during cleanup:

```csharp
private void OnEnable()
{
    GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
}

private void OnDisable()
{
    GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
}
```

### 3. Scene Initialization

For per-scene managers like PhaseManager:

```csharp
void Start()
{
    // Notify master initializer that scene is ready
    MasterGameInitializer.Instance?.InitializePhaseManager();
}
```

### 4. Error Handling

Always check for null when accessing systems:

```csharp
var gameManager = GameManager.Instance;
if (gameManager != null)
{
    gameManager.SaveGame();
}
```

## System Health Monitoring

The diagnostics system tracks:
- Last activity time for each system
- Error and warning counts
- Performance metrics (FPS, memory)
- Log history (up to 1000 entries)

### Check System Health

```csharp
var diagnostics = SystemDiagnostics.Instance;

// Get last activity
DateTime lastActivity = diagnostics.GetLastSystemActivity("AudioManager");

// Get error counts
int errors = diagnostics.ErrorCount;
int warnings = diagnostics.WarningCount;

// Get performance
float fps = diagnostics.CurrentFPS;
```

## File Organization

```
Assets/Scripts/Core/
├── GameManager.cs              # Core game state management
├── SaveManager.cs              # Save/load operations
├── MasterGameInitializer.cs    # System initialization controller
├── GameRuntime.cs              # Runtime coordination
└── SystemDiagnostics.cs        # Logging and monitoring

Assets/Scripts/Systems/
├── AudioManager.cs             # Audio playback
├── UIManager.cs                # UI management
├── LocalizationManager.cs      # Multi-language support
└── InteractionSystem.cs        # POI interaction base

Assets/Scripts/Gameplay/
├── DialogueManager.cs          # NPC dialogue
├── PuzzleManager.cs            # Educational puzzles
├── PhaseManager.cs             # Scene phase logic
└── POI_Components.cs           # Point of Interest components

Assets/Scripts/Network/
└── FirebaseManager.cs          # Cloud services
```

## Troubleshooting

### Systems Not Initializing

1. Check the initialization order in MasterGameInitializer
2. Look for errors in the SystemDiagnostics log
3. Verify all required components are present
4. Use `status <system>` console command to check individual systems

### Performance Issues

1. Monitor FPS with the debug overlay (F10)
2. Check for excessive logging in SystemDiagnostics
3. Use the `performance` log category to track slow operations
4. Review memory usage in the debug console

### Connection Problems

1. Verify auto-wiring completed in GameRuntime
2. Check SystemDiagnostics for connection logs
3. Ensure all singletons are properly initialized
4. Use `list` command to verify all systems are active

## Extension Guide

### Adding New Systems

1. Create the system manager with singleton pattern
2. Add initialization step to MasterGameInitializer
3. Add auto-wiring connection in GameRuntime
4. Add diagnostics logging calls
5. Update this documentation

### Example: Adding Analytics Manager

```csharp
// 1. Create manager
public class AnalyticsManager : MonoBehaviour
{
    private static AnalyticsManager _instance;
    public static AnalyticsManager Instance => _instance;
    // ...
}

// 2. Add to MasterGameInitializer initialization order
private enum InitializationStep
{
    // ... existing steps
    AnalyticsManager,  // Add here
    Complete
}

// 3. Add initialization function
private bool InitializeAnalyticsManager()
{
    var manager = AnalyticsManager.Instance;
    SystemDiagnostics.LogInitialization("AnalyticsManager", "Initialized");
    return manager != null;
}

// 4. Add to auto-wiring in GameRuntime
private void WireAnalyticsManager()
{
    var manager = AnalyticsManager.Instance;
    // Wire connections...
}

// 5. Use in code
SystemDiagnostics.LogSystemEvent("Analytics", "Event tracked");
```

## Support

For issues or questions:
1. Check the debug console (F11) for errors
2. Review the system logs
3. Verify initialization order
4. Check system status with console commands

---

**Version:** 1.0.0
**Last Updated:** 2025-02-14
**Author:** Agent-02 (Technical Director)
