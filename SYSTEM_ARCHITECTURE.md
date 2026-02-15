# Cristo Adventure - System Architecture Diagram

## Initialization Flow

```
┌─────────────────────────────────────────────────────────────┐
│                    Bootstrap Scene                           │
│                  (Contains MasterGameInitializer)            │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│              MasterGameInitializer.Start()                   │
│              (Awake - Creates Singletons)                    │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│           InitializationSequence() Coroutine                 │
└────────────────────────┬────────────────────────────────────┘
                         │
         ┌───────────────┼───────────────┐
         │               │               │
         ▼               ▼               ▼
    ┌─────────┐    ┌─────────┐    ┌─────────┐
    │  Step 1 │    │  Step 2 │    │  Step 3 │
    │GameManager│  │SaveManager│  │AudioManager│
    └────┬────┘    └────┬────┘    └────┬────┘
         │               │               │
         └───────────────┼───────────────┘
                         │
         ┌───────────────┼───────────────┐
         │               │               │
         ▼               ▼               ▼
    ┌─────────┐    ┌─────────┐    ┌─────────┐
    │  Step 4 │    │  Step 5 │    │  Step 6 │
    │ UIManager│   │Localization│  │Firebase │
    │          │   │  Manager   │  │Manager  │
    └────┬────┘    └────┬────┘    └────┬────┘
         │               │               │
         └───────────────┼───────────────┘
                         │
         ┌───────────────┼───────────────┐
         │               │               │
         ▼               ▼               ▼
    ┌─────────┐    ┌─────────┐    ┌─────────┐
    │  Step 7 │    │  Step 8 │    │  Step 9 │
    │Dialogue │    │ Puzzle  │    │ Phase   │
    │Manager  │    │ Manager │    │Manager  │
    └────┬────┘    └────┬────┘    └────┬────┘
         │               │               │
         └───────────────┼───────────────┘
                         │
                         ▼
              ┌─────────────────┐
              │ AutoWireConnections() │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │ GameRuntime.Initialize() │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │ Load Player Data │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │ Setup Events    │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │ Initialization Complete │
              └─────────────────┘
```

## System Connections

```
┌──────────────────────────────────────────────────────────────┐
│                         GameManager                          │
│                    (Central Hub)                             │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Properties:                                            │ │
│  │  - SaveManager                                          │ │
│  │  - AudioManager                                         │ │
│  │  - UIManager                                            │ │
│  │  - LocalizationManager                                  │ │
│  │  - FirebaseManager                                      │ │
│  │  - PlayerData                                           │ │
│  │                                                         │ │
│  │  Events:                                                │ │
│  │  - OnGameStateChanged                                   │ │
│  └────────────────────────────────────────────────────────┘ │
└──┬─────────┬─────────┬─────────┬─────────┬──────────────────┘
   │         │         │         │         │
   │         │         │         │         │
   ▼         ▼         ▼         ▼         ▼
┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐
│Save │  │Audio│  │ UI  │  │ Loc │  │Fire │
│Mgr  │  │Mgr  │  │Mgr  │  │Mgr  │  │base │
└──┬──┘  └──┬──┘  └──┬──┘  └──┬──┘  └──┬──┘
   │        │        │        │        │
   │        │        │        │        │
   │        │        │        │        │
   ▼        ▼        ▼        ▼        ▼
┌─────────────────────────────────────────────────┐
│              Player Data (Shared)                │
│  - Level, Experience, Coins                     │
│  - CompletedPhases                              │
│  - InventoryItems                               │
│  - GameSettings                                 │
│  - Statistics                                   │
└─────────────────────────────────────────────────┘
```

## Event Flow

```
┌──────────────┐
│ Player Action│
└──────┬───────┘
       │
       ▼
┌──────────────┐
│  POI/System  │
└──────┬───────┘
       │
       ├──► DialogueManager.StartDialogue()
       │        │
       │        ▼
       │   ┌─────────────┐
       │   │GameManager. │
       │   │SetState(    │
       │   │InDialogue)  │
       │   └──────┬──────┘
       │          │
       │          ▼
       │   ┌─────────────┐
       │   │ UIManager.  │
       │   │ShowDialogue │
       │   └─────────────┘
       │
       ├──► PuzzleManager.StartPuzzle()
       │        │
       │        ▼
       │   ┌─────────────┐
       │   │GameManager. │
       │   │SetState(    │
       │   │InPuzzle)    │
       │   └──────┬──────┘
       │          │
       │          ▼
       │   ┌─────────────┐
       │   │ UIManager.  │
       │   │ShowPuzzle   │
       │   └──────┬──────┘
       │          │
       │          ▼
       │   ┌─────────────┐
       │   │OnComplete   │
       │   └──────┬──────┘
       │          │
       │          ▼
       │   ┌─────────────┐
       │   │GameManager. │
       │   │AddCoins()   │
       │   │SaveGame()   │
       │   └─────────────┘
       │
       └──► PhaseManager.RegisterPOIVisit()
                │
                ▼
           ┌─────────────┐
           │GameManager. │
           │GetPlayerData│
           └──────┬──────┘
                  │
                  ▼
           ┌─────────────┐
           │Firebase.    │
           │LogEvent()   │
           └─────────────┘
```

## Data Flow

```
┌─────────────────────────────────────────────────────────────┐
│                     Data Layer                              │
└─────────────────────────────────────────────────────────────┘
                         │
         ┌───────────────┼───────────────┐
         │               │               │
         ▼               ▼               ▼
    ┌─────────┐    ┌─────────┐    ┌─────────┐
    │  Local  │    │  Cloud  │    │  Memory │
    │  JSON   │    │ Firebase│  │ Runtime │
    └────┬────┘    └────┬────┘    └────┬────┘
         │               │               │
         └───────────────┼───────────────┘
                         │
                         ▼
              ┌─────────────────┐
              │  SaveManager    │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │  GameManager    │
              │  (PlayerData)   │
              └────────┬────────┘
                       │
         ┌─────────────┼─────────────┐
         │             │             │
         ▼             ▼             ▼
    ┌─────────┐  ┌─────────┐  ┌─────────┐
    │Systems  │  │UI       │  │Gameplay │
    │Read/Write│ │Display  │  │Logic    │
    └─────────┘  └─────────┘  └─────────┘
```

## Scene Flow

```
┌──────────────┐
│  MainMenu   │
│  Scene       │
└──────┬───────┘
       │
       │ User selects:
       │ - New Game
       │ - Continue
       │ - Settings
       │
       ▼
┌──────────────┐
│  Bootstrap   │
│  (Optional)  │
│  - Init all  │
│    systems   │
└──────┬───────┘
       │
       ▼
┌──────────────┐
│  Phase Scene │
│  (e.g., 1.1) │
│              │
│  Components: │
│  - PhaseManager│
│  - POI Objects│
│  - NPCs      │
│  - Puzzles   │
└──────┬───────┘
       │
       │ Phase Complete
       │
       ▼
┌──────────────┐
│  Phase       │
│  Summary     │
│  - Stars     │
│  - Rewards   │
└──────┬───────┘
       │
       │ Auto-save
       │
       ▼
┌──────────────┐
│  Next Phase  │
│  OR MainMenu │
└──────────────┘
```

## Diagnostic Flow

```
┌─────────────────────────────────────────────────────────────┐
│                    SystemDiagnostics                        │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐  │
│  │  LogEntry History (max 1000 entries)                │  │
│  │  - Timestamp                                         │  │
│  │  - Category (System, Event, Error, etc.)           │  │
│  │  - System Name                                       │  │
│  │  - Message                                           │  │
│  │  - Stack Trace (errors)                             │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐  │
│  │  Performance Monitoring                             │  │
│  │  - FPS tracking                                      │  │
│  │  - Memory usage                                      │  │
│  │  - Frame time                                        │  │
│  └─────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐  │
│  │  System Health                                      │  │
│  │  - Last activity time per system                    │  │
│  │  - Error/warning counts                             │  │
│  │  - Connection status                                │  │
│  └─────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                         │
                         │ Logs To:
                         ▼
              ┌─────────────────┐
              │  Console        │
              │  (F11)          │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │  File           │
              │  (persistentDataPath/Logs/)│
              └─────────────────┘
```

## Command Pattern (Debug Console)

```
User Input (F11)
       │
       ▼
┌──────────────┐
│ Parse Command│
└──────┬───────┘
       │
       ▼
┌──────────────┐
│ Route to     │
│ Handler      │
└──────┬───────┘
       │
       ├──► help       → ShowHelp()
       ├──► list       → ListSystems()
       ├──► status     → ShowSystemStatus()
       ├──► logs       → ShowLogs()
       ├──► errors     → ShowErrors()
       ├──► warnings   → ShowWarnings()
       ├──► fps        → ShowFPS()
       ├──► memory     → ShowMemory()
       ├──► phase      → ShowPhaseInfo()
       └──► player     → ShowPlayerInfo()
              │
              ▼
       ┌──────────┐
       │ Output   │
       │ to Console│
       └──────────┘
```

## Key Design Patterns

1. **Singleton Pattern** - All managers use singletons for global access
2. **Observer Pattern** - Events for system communication
3. **Facade Pattern** - GameManager provides simplified interface
4. **Strategy Pattern** - Different puzzle types, POI types
5. **Command Pattern** - Debug console commands
6. **Dependency Injection** - Manual wiring in GameRuntime

## Thread Safety

- All Unity callbacks run on main thread
- Firebase operations use ContinueWithOnMainThread
- No multi-threading issues in current implementation

## Performance Considerations

- Object pooling for SFX (AudioManager)
- Lazy loading for localization strings
- Log history limit (1000 entries)
- FPS/memory update intervals to avoid overhead
- Coroutine-based async operations

---

**Generated by:** Agent-02 (Technical Director)
**Date:** 2025-02-14
**Version:** 1.0.0
