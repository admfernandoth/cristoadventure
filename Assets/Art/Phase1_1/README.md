# Phase 1.1 - Bethlehem Basilica Environment

Created by: Agent-11 (3D Artist)

## Asset Overview

This folder contains basic 3D environment assets for Phase 1.1 - Bethlehem Basilica, created using Unity primitives with low poly count requirements (<5000 per area).

## Assets Created

### Materials (in `/Materials`)

1. **StoneMaterial** - Gray stone with subtle noise texture
   - Usage: Walls, floors, columns, ceiling arches
   - Properties: Low metallic (0.1), medium glossiness (0.3)

2. **WoodMaterial** - Brown wood texture
   - Usage: Manger, wooden objects
   - Properties: No metallic, medium glossiness (0.4)

3. **GoldMaterial** - Metallic gold material
   - Usage: Decorative elements, lighting fixtures
   - Properties: Full metallic (1.0), high glossiness (0.8)

4. **SilverMaterial** - Metallic silver material
   - Usage: Star on floor, metallic objects
   - Properties: Full metallic (1.0), high glossiness (0.9)

### Prefabs (in `/Prefabs`)

#### 1. BasilicaInterior.prefab
- **Scale**: 20x5x10 (main nave)
- **Components**:
  - Main nave (long corridor)
  - 8 columns on each side
  - Arched ceiling (using scaled cubes)
  - Stone floor with grid pattern
  - Warm lighting (2 gold point lights)
- **Poly Count**: ~2000
- **Mood**: Sacred, warm, atmospheric

#### 2. NativityCave.prefab
- **Scale**: 5x3x5 (small cave room)
- **Components**:
  - Rough stone walls with procedural noise
  - StarOnFloor (silver cylinder with 14-point star pattern)
  - Manger (wood-colored box)
  - Atmospheric spotlight with ambient fill light
- **Poly Count**: ~1500
- **Mood**: Intimate, warm, sacred

#### 3. Courtyard.prefab
- **Scale**: 15x1x15 (outdoor area)
- **Components**:
  - Stone floor with grid
  - Night sky with stars (skybox)
  - Blue directional moonlight
  - Multiple ambient fill lights
- **Poly Count**: ~1000
- **Mood**: Peaceful, night-time, outdoor

#### 4. LightingSetup.prefab
- **Components**:
  - Main directional light (warm, rotation 45, 30, 0)
  - Ambient occlusion settings
  - 5 reflection probes for better lighting
  - Fill lights for atmosphere
  - Rim light for dramatic effect
- **Purpose**: Reusable lighting configuration for consistent mood

## Usage Instructions

### To Create Assets in Unity:

1. **Create Materials First**:
   - Open Unity Editor
   - Create a new C# script named `CreateAllPhase1_1Assets.cs`
   - Copy the contents from the creation script
   - Place in your project's Editor folder
   - Go to menu: Cristo → Create Phase 1.1 Assets
   - This will automatically create all materials and prefabs

2. **Manual Creation** (if needed):
   - Each area has its own creation script in the Prefabs folder
   - Run individual scripts to create specific prefabs

3. **Add to Scene**:
   - Drag prefabs into your Unity scene
   - Adjust lighting as needed for your specific scene
   - Ensure proper collision detection on walkable surfaces

## Notes

- All assets use basic Unity primitives for optimal performance
- Materials use Standard shader for compatibility
- Lighting is designed for warm, sacred atmosphere
- Poly counts kept low for mobile platforms
- Textures are procedural placeholders - replace with detailed textures later

## File Structure

```
Phase1_1/
├── Materials/
│   ├── BasicMaterialsCreation.cs
│   └── (Created materials: StoneMaterial.mat, WoodMaterial.mat, etc.)
├── Prefabs/
│   ├── CreateBasilicaInterior.cs
│   ├── CreateNativityCave.cs
│   ├── CreateCourtyard.cs
│   ├── CreateLightingSetup.cs
│   ├── (Created prefabs: BasilicaInterior.prefab, NativityCave.prefab, etc.)
│   └── CreateAllPhase1_1Assets.cs
├── CreateAllPhase1_1Assets.cs
├── package.json
└── README.md
```

## Next Steps

1. Import creation scripts into Unity
2. Run asset creation menu item
3. Place prefabs in your scenes
4. Add detailed textures later
5. Implement lighting bake for final quality