# Loading UI Prefab Setup Guide

## Prefab Structure

The Loading UI prefab should include the following components:

### Main Canvas
- Canvas component with Sorting Layer set appropriately
- Canvas Scaler for responsive design
- Canvas Group for fade transitions

### UI Elements Hierarchy

```
LoadingCanvas (Canvas)
├── Background (Image)
│   └── backgroundImage (Sprite)
├── PhaseSplash (GameObject)
│   ├── PhaseSplashImage (Image)
│   ├── PhaseNameText (TextMeshPro - Text)
│   └── ChapterText (TextMeshPro - Text)
├── LoadingIcon (Image)
│   └── rotatingIcon (Image)
├── ProgressBar (GameObject)
│   ├── ProgressBarBackground (Image)
│   └── ProgressBarFill (Image)
└── TipsArea (GameObject)
    └── TipText (TextMeshPro - Text)
```

### Required Components for Each Element

1. **Background Image**
   - Image component
   - Color set to backgroundColor from config

2. **Phase Splash**
   - PhaseSplashImage: Image component for phase art
   - PhaseNameText: TextMeshPro - Text component
   - ChapterText: TextMeshPro - Text component

3. **Loading Icon**
   - Image component
   - Sprite for rotating icon
   - Should be positioned in center

4. **Progress Bar**
   - Background Image: Fills width, colored darker
   - Fill Image: Fills width, colored primaryColor

5. **Tip Text**
   - TextMeshPro - Text component
   - Positioned at bottom of screen

### Example Prefab Configuration

```csharp
// LoadingUI prefab should be referenced in LoadingManager inspector
public GameObject loadingUI;
```

### Recommended Dimensions and Positions

- Screen Center: (0, 0, 0) in canvas space
- Phase Splash: Y = 50, Font Size: 32
- Chapter Text: Y = -50, Font Size: 24
- Progress Bar: Size (400, 30), Y = -100
- Icon: Size (80, 80), Center position
- Tip Text: Y = -200, Font Size: 18

### Materials and Styles

- Use TextMeshPro materials for text elements
- Use simple unlit materials for images
- Ensure proper anchoring and pivots

### Animation Components

Add these components for animations:

1. **Canvas Group**: For fade transitions
2. **LoadingIcon**: Rotating script (handled by LoadingManager)
3. **Tip Text**: Fade in/out animations

## Usage

1. Create the UI hierarchy in Unity
2. Set up all references in the LoadingManager inspector
3. Create the Loading UI prefab
4. Assign it to the LoadingManager loadingUI field