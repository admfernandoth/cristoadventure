# Phase 1.1 Bethlehem Scene Setup

## Overview
This directory contains the automated scene setup script for Phase 1.1 - Bethlehem.

## Files Created

### 1. Phase_1_1_Bethlehem.unity
The main Unity scene file with:
- ✅ Basic warm, cinematic lighting
- ✅ Ground plane with collider
- ✅ PhaseManager component (phaseId: "1.1")
- ✅ Player spawn point at entrance
- ✅ 6 POI GameObjects (placeholder primitives)
- ✅ PhaseExit at end of path

### 2. Phase_1_1_Bethlehem_Setup.cs
Editor script that generates the scene automatically.

## How to Use

### Method 1: Automatic Setup (Recommended)
1. Open Unity Editor
2. Navigate to menu: `GameObject > Phase Setup > Create Phase 1.1 Bethlehem Scene`
3. Scene will be created automatically with all required components
4. Scene is automatically saved and added to Build Settings

### Method 2: Manual Setup
If you prefer to build the scene manually:
1. Create a new scene in Unity
2. Use the GameObject menu items as reference
3. Follow the POI layout from Phase1_1_Bethlehem_Spec.md

## Scene Structure

### Lighting
- **Main Light**: Directional light with warm color (RGB: 1.0, 0.9, 0.75)
- **Ambient Light**: Point light for soft fill (RGB: 1.0, 0.87, 0.75)
- **Intensity**: Cinematic warm lighting setup

### POI Layout (Coordinates from Spec)
```
ENTRANCE (Player Spawn: 0, 0, 10)
    ↓
POI-001: History Plaque (0, 1.5, 5) - InfoPlaque
    ↓
POI-002: Silver Star (0, 0.5, 0) - ReliquaryPOI
    ↓
POI-003: Manger Plaque (2, 1.5, 0) - InfoPlaque
    ↓
POI-004: Father Elias (-5, 1, -5) - NPCGuidePOI
    ↓
POI-005: Photo Spot (0, 1, -10) - PhotoSpotPOI
    ↓
POI-006: Luke 2:7 Verse (3, 1, -8) - VerseMarkerPOI
    ↓
PHASE EXIT (0, 1.5, -15)
```

### POI Components Required
Each POI GameObject needs the following component attached:
- **POI-001, POI-003**: `InfoPlaque` component
- **POI-002**: `ReliquaryPOI` component
- **POI-004**: `NPCGuidePOI` component
- **POI-005**: `PhotoSpotPOI` component
- **POI-006**: `VerseMarkerPOI` component

## Configuration Needed

After scene generation, you'll need to configure:

### PhaseManager (GameObject: "PhaseManager")
- ✅ Phase ID: "1.1" (auto-configured)
- ✅ Required Stars: 2 (auto-configured)
- ⚠️ Next Phase ID: "1.2" (auto-configured, verify)
- ⚠️ Total POIs: 6 (auto-configured)

### POI Localization Keys
Set these in each POI's Inspector:
- **POI-001** (History Plaque):
  - Title Key: `Phase.1.1.POI.001.Title`
  - Content Key: `Phase.1.1.POI.001.Content`

- **POI-002** (Silver Star):
  - Relic Name Key: `Phase.1.1.POI.002.Name`
  - Description Key: `Phase.1.1.POI.002.Description`
  - Stamp ID: `bethlehem_silver_star`

- **POI-003** (Manger Plaque):
  - Title Key: `Phase.1.1.POI.003.Title`
  - Content Key: `Phase.1.1.POI.003.Content`

- **POI-004** (Father Elias):
  - NPC Name Key: `Phase.1.1.NPC.001.Name`
  - Assign DialogueNode asset

- **POI-005** (Photo Spot):
  - Spot Name Key: `Phase.1.1.Photo.001.Name`
  - Configure Photo Position/Rotation/FOV

- **POI-006** (Luke 2:7 Verse):
  - Reference: "Luke 2:7"
  - Verse Text Key: `Phase.1.1.Verse.001.Text`

## Next Steps

1. ✅ Scene structure created
2. ⏳ Agent-11 (3D Artist): Create visual models
   - Basilica exterior/interior
   - Nave with columns
   - Grotto of Nativity
   - Silver Star (14-pointed)
   - Manger
   - Father Elias NPC model
3. ⏳ Configure all POI components in Inspector
4. ⏳ Add localization strings to LanguageManager
5. ⏳ Create DialogueNode assets for Father Elias
6. ⏳ Test scene in Play mode

## Notes

- All GameObjects use placeholder primitives (cubes, spheres, capsules)
- The scene is functional for gameplay testing
- Visual assets will be replaced by Agent-11
- Scene uses Unity's default lighting and materials
- Ground is a 50x50 plane with MeshCollider

## Technical Details

- **Scene File**: Phase_1_1_Bethlehem.unity
- **Location**: Assets/Phases/Chapter1/
- **Build Settings**: Automatically added
- **Physics**: Ground has MeshCollider for player walking
- **Layering**: Default layer assignments
- **Tags**: PlayerSpawn tag on spawn point

## Troubleshooting

**Scene doesn't appear in Build Settings:**
- Manually add via File > Build Settings > Add Open Scenes

**POI components show errors:**
- Ensure POI_Components.cs script exists in Assets/Scripts/Gameplay/
- Check namespace: CristoAdventure.Gameplay

**Lighting looks wrong:**
- Open Window > Rendering > Lighting
- Click "Generate Lighting" to bake lightmaps

**Player can't walk:**
- Verify Ground has MeshCollider component
- Check PlayerController has CharacterController or Rigidbody

---

**Created by:** Agent-20 (Unity Programmer)
**Date:** 2026-02-14
**Status:** Scene structure complete, awaiting visual assets
