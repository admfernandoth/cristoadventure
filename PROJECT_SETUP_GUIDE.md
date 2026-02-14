# Cristo Adventure - Project Setup Guide

## Quick Start

This guide will help you set up the Cristo Adventure project in Unity Editor and create your first Android build.

## Prerequisites

- Unity 2023 LTS or later
- Android SDK and NDK (for Android builds)
- Git LFS installed
- A text editor (VS Code recommended)

## Project Structure

```
Cristo Adventure/
├── Assets/
│   ├── Art/              # Art assets and materials
│   ├── Editor/           # Editor scripts and build system
│   ├── Localization/     # JSON localization files
│   ├── Phases/           # Phase-specific content
│   │   └── Chapter1/     # Phase 1 content (Bethlehem)
│   ├── Prefabs/          # POI and UI prefabs
│   ├── Scenes/           # Unity scenes
│   ├── Scripts/          # All C# scripts
│   │   ├── Core/         # Core game systems
│   │   ├── Gameplay/     # Gameplay mechanics
│   │   ├── Systems/      # Manager systems
│   │   ├── UI/           # UI scripts
│   │   ├── Player/       # Player controller
│   │   └── Audio/        # Audio system
│   ├── Tests/            # Test scripts
│   └── UI/               # UI prefabs and assets
├── Packages/             # Unity packages
├── ProjectSettings/      # Unity project settings
└── .claude/              # Claude Code configuration
```

## Step 1: Clone and Open Project

1. **Clone the repository:**
   ```bash
   git clone https://github.com/admfernandoth/cristoadventure.git
   cd cristoadventure
   ```

2. **Open in Unity Hub:**
   - Click "Add"
   - Browse to the project folder
   - Select "C:\Projects\cristo" (or your project location)
   - Click "Open"

3. **Wait for Unity to import assets:**
   - First import may take several minutes
   - Unity will compile all C# scripts
   - Check Console for any compilation errors

## Step 2: Configure Project Settings

The project includes automatic configuration. Use the Unity Editor menu:

1. **Configure Android Build:**
   - Menu: `Build > Configure Android`
   - This sets all Player Settings automatically

2. **Verify Configuration:**
   - Menu: `Cristo > Build Validation`
   - Check Console for validation results

## Step 3: Set Up Phase 1.1 Scene

1. **Open Phase 1.1 Scene:**
   - Menu: `Cristo > Phase 1.1 > Open Phase 1.1 Scene`
   - Or open: `Assets/Phases/Chapter1/Phase_1_1_Bethlehem.unity`

2. **Run Scene Setup Helper:**
   - Menu: `Cristo > Phase 1.1 > Setup Scene from JSON`
   - This creates all POI GameObjects from JSON configuration
   - POIs will be positioned according to the config

3. **Create Phase Manager:**
   - Menu: `Cristo > Phase 1.1 > Create Phase Manager`
   - This adds the PhaseManager component to the scene

4. **Save the Scene:**
   - `Ctrl+S` or `File > Save`

## Step 4: Test in Editor

1. **Press Play button** in Unity Editor
2. **Use controls:**
   - WASD: Move
   - Mouse: Look around
   - E: Interact with POIs
   - B: Open backpack
   - Esc: Pause menu
   - F10: Debug overlay
   - F11: Debug console

3. **Test features:**
   - Walk to POIs and press E to interact
   - Talk to NPC Father Elias
   - Solve the Timeline Puzzle
   - Collect relics
   - Complete the phase

## Step 5: Create Android Build

### Quick Build

1. **Development APK (for testing):**
   - Menu: `Build > Android > Quick Build Dev APK`

2. **Release APK:**
   - Menu: `Build > Android > Quick Build Release APK`

### Using Build Manager

1. **Open Build Manager:**
   - Menu: `Cristo > Build Manager`

2. **Choose build type:**
   - Click "Configure & Build Dev APK" for development
   - Click "Configure & Build Release APK" for release

3. **Wait for build to complete:**
   - Build may take 5-15 minutes
   - Progress shown in Unity Editor
   - APK saved to: `C:\Projects\cristo\Builds\Android\`

## Step 6: Test on Android Device

1. **Enable Developer Mode** on your Android device
2. **Enable USB Debugging**
3. **Connect device** via USB
4. **Install APK:**
   - Copy APK from build folder
   - Use `adb install` command OR
   - Transfer to device and install

## JSON Data Files

The project uses JSON files for content that can be edited without Unity Editor:

### Localization Files
- `Assets/Localization/Phase1_1_Localization.json` (Portuguese)
- `Assets/Localization/Phase1_1_Localization_EN.json` (English)
- `Assets/Localization/Phase1_1_Localization_ES.json` (Spanish)

### Puzzle Data
- `Assets/Phases/Chapter1/Data/Phase1_1_NativityTimelinePuzzle.json`

### Dialogue Data
- `Assets/Phases/Chapter1/Dialogue/FatherElias_Dialogue.json`

### POI Configuration
- `Assets/Phases/Chapter1/Data/Phase1_1_POIConfig.json`

## Editing Content

### Adding New POIs

1. Edit `Phase1_1_POIConfig.json`
2. Add new POI entry with position and data
3. Run `Cristo > Phase 1.1 > Setup Scene from JSON`

### Adding New Dialogue

1. Create new JSON file in `Assets/Phases/Chapter1/Dialogue/`
2. Follow the structure of `FatherElias_Dialogue.json`
3. Update NPC POI to reference new dialogue ID

### Adding New Puzzles

1. Create new JSON file in `Assets/Phases/Chapter1/Data/`
2. Follow the structure of `Phase1_1_NativityTimelinePuzzle.json`
3. Update Puzzle Trigger POI to reference new puzzle ID

### Adding New Translations

1. Open the appropriate localization JSON file
2. Add key-value pairs for new strings
3. Restart Unity Editor to reload

## Troubleshooting

### Build Errors

**"Script compilation errors"**
- Check Console for specific error
- Make sure all scripts are imported
- Try `Assets > Reimport All`

**"Android SDK not found"**
- Open Edit > Preferences > External Tools
- Set Android SDK path
- Install Android Build Support via Unity Hub

**"IL2CPP build failed"**
- Increase IL2CPP build timeout in Project Settings
- Close other applications to free memory
- Try building Development APK first

### Scene Setup Issues

**POIs not appearing**
- Make sure JSON config file exists
- Check Console for errors
- Verify prefabs exist in `Assets/Prefabs/Interactables/`

**Phase Manager errors**
- Run `Cristo > Phase 1.1 > Create Phase Manager`
- Check that GameManager exists in scene

### Localization Issues

**Strings not appearing**
- Check JSON files are valid
- Verify `JSONLocalizationLoader.Initialize()` is called
- Check key names match between code and JSON

## Performance Optimization

For better performance on Android:

1. **Enable IL2CPP** (already configured)
2. **Use ARM64 architecture** (already configured)
3. **Enable Managed Stripping** (set to Low or Medium)
4. **Optimize textures**:
   - Use ASTC compression
   - Max size 1024 or 512
5. **Reduce draw calls**:
   - Use static batching
   - Combine meshes where possible

## Next Development Steps

1. **Create Phase 1.2** (Jerusalem - Temple Mount)
2. **Add more POIs** to existing phases
3. **Create new puzzles** and dialogue
4. **Add audio** (music, sound effects)
5. **Add more UI panels** (backpack, settings)
6. **Test on actual devices**

## Support

For issues or questions:

- Check Unity Console for error messages
- Review documentation in `Assets/Editor/AndroidBuildSystem.md`
- Run `Cristo > Build Validation` to check configuration

## Development Team

This project uses autonomous multi-agent development via Claude Code.

**Project:** Cristo Adventure
**Version:** 0.1.0
**Phase:** 1.1 (Bethlehem - Basilica of the Nativity)
**Build System:** Android APK with IL2CPP
**Languages:** Portuguese, English, Spanish

---

Last updated: 2025-02-14
