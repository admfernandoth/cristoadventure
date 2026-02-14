# Complete Loading System Setup Guide

## Overview
This is a complete loading screen system with multilingual support, phase splashes, progress tracking, and smooth animations.

## Files Created

### 1. LoadingManager.cs
The core loading screen controller that handles:
- Progress bar updates
- Tip rotation (every 5 seconds)
- Phase splash displays
- Icon rotation
- Fading animations
- Error handling

### 2. LoadingScreenConfig.cs
ScriptableObject containing:
- Array of educational tips (English, Spanish, French)
- Phase splash configurations
- Animation timings
- Color scheme

### 3. SceneLoader.cs
Handles scene loading with:
- Async scene loading
- Error handling
- Minimum loading time
- Preloading utility

### 4. LocalizationManager.cs
Manages language switching:
- Support for English, Spanish, French
- Global language state
- Easy text localization

## Setup Steps

### 1. Create LoadingScreenConfig
```csharp
// In Unity Editor:
// Right-click in Project window → Create → UI → Loading Screen Config
// This will create the asset at Assets/Resources/LoadingScreenConfig.asset
```

### 2. Create Loading UI Prefab
Create a UI prefab with the following structure:

```
LoadingCanvas (Canvas)
├── Background (Image)
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

### 3. Assign References in Inspector
- Create an empty GameObject named "LoadingSystem"
- Add LoadingManager component
- Drag the Loading UI prefab to the `loadingUI` field
- Set other references (progress bar, tip text, etc.)

### 4. Usage in Other Scripts

#### Basic Scene Loading
```csharp
// Simple scene loading
SceneLoaderManager.LoadScene("Level_1", 1);

// With SceneLoader reference
SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
sceneLoader.LoadScene("Level_2", 2);
```

#### Loading with Error Handling
```csharp
try
{
    SceneLoaderManager.LoadScene("Level_1", 1);
}
{
    // Handle loading errors
    Debug.LogError("Failed to load scene");
}
```

#### Language Switching
```csharp
// Switch language
LocalizationManager.Instance.SetLanguage("es"); // Spanish
LocalizationManager.Instance.SetLanguage("fr"); // French
LocalizationManager.Instance.SetLanguage("en"); // English

// Get current language string
string currentLang = LocalizationManager.Instance.GetCurrentLanguage();
```

## Features Implemented

### 1. Loading Screen UI
- **Progress Bar**: Smoothly fills from 0-100%
- **Rotating Icon**: Continuously rotating loading icon
- **Phase Splash**: Shows phase name and chapter with fade transitions
- **Tips System**: Educational tips that rotate every 5 seconds

### 2. Multilingual Support
- **3 Languages**: English, Spanish, French
- **Dynamic Switching**: Change language during gameplay
- **Fallback System**: Defaults to English if translation missing
- **Educational Content**: Tips about gameplay, history, controls

### 3. Progress Tracking
- **Async Loading**: Tracks Unity's async loading progress
- **Smooth Updates**: Progress bar animates smoothly
- **Activity Display**: Shows current loading progress
- **Error Handling**: Displays error messages if loading fails

### 4. Animations
- **Fade Transitions**: Smooth fade in/out for loading screen
- **Icon Rotation**: Continuous rotation for visual feedback
- **Tip Fade**: Smooth transitions between tips

## Customization Options

### Editing Tips
```csharp// In LoadingScreenConfig:
public List<LoadingTip> tips = new List<LoadingTip>
{
    new LoadingTip
    {
        id = "tip1",
        english = "Your English text here",
        spanish = "Tu texto en español aquí",
        french = "Votre texte en français ici"
    }
};
```

### Customizing Colors
```csharp// In LoadingScreenConfig:
public Color primaryColor = new Color(0.2f, 0.6f, 1f);
public Color backgroundColor = new Color(0.1f, 0.1f, 0.2f);
public Color textColor = Color.white;
```

### Adjusting Timings
```csharp// In LoadingScreenConfig:
public float tipRotationInterval = 5f;
public float fadeDuration = 0.5f;
public float iconRotationSpeed = 180f;
```

## Example Integration

### 1. MainMenu.cs
```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoaderManager.LoadScene("GameScene", 1);
    }

    public void Options()
    {
        SceneLoaderManager.LoadScene("OptionsScene", 0);
    }
}
```

### 2. LevelComplete.cs
```csharp
public class LevelComplete : MonoBehaviour
{
    public void NextLevel()
    {
        SceneLoaderManager.LoadScene("Level_" + (currentLevel + 1), currentLevel + 1);
    }

    public void MainMenu()
    {
        SceneLoaderManager.LoadScene("MainMenu", 0);
    }
}
```

## Performance Considerations

- The loading system is designed to be lightweight
- Coroutine-based animations don't block the main thread
- UI is only active when actually loading
- Easy to disable when not needed

## Troubleshooting

### Common Issues

1. **LoadingScreenConfig not found**
   - Make sure it's saved in Assets/Resources/
   - Check the filename is exactly "LoadingScreenConfig"

2. **UI elements not visible**
   - Check Canvas Scaler settings
   - Verify anchors and pivots
   - Ensure sorting layer is correct

3. **Tips not rotating**
   - Check config has tips added
   - Verify coroutine isn't being stopped early
   - Check deltaTime is being used correctly

4. **Scene loading hangs**
   - Verify scene name is correct
   - Check build settings includes the scene
   - Look for errors in console

## Advanced Features

### Preloading Scenes
```csharp
// Preload scene while player is in current scene
SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
sceneLoader.PreloadScene("Level_2");
```

### Custom Loading Messages
```csharp
// Extend SceneLoader to add custom loading activities
public IEnumerator CustomLoadRoutine(string sceneName)
{
    yield return StartCoroutine(AsyncLoadScene(sceneName));

    // Add custom post-loading logic
    SaveSystem.SaveProgress();
}
```

The loading system is now complete and ready for use!