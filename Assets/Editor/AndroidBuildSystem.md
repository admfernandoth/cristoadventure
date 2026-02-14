# Android Build System for Cristo Adventure

This build system provides a comprehensive solution for building Android APKs for Cristo Adventure with the following features:

## Build Scripts Overview

### 1. BuildConfigurator.cs
- Configures Unity Player Settings for Android
- Sets minimum API level (26), target architecture (ARM64)
- Configures IL2CPP scripting backend
- Sets .NET Standard 2.1 API compatibility level
- Defines preprocessor symbols for Firebase integration
- Sets company name, product name, and bundle identifier

### 2. AndroidBuildSetup.cs
- Provides menu items in Unity Editor
- Validates build configuration
- Shows confirmation dialogs
- Checks Firebase setup
- Supports both development and release builds

### 3. BuildAPK.cs
- Handles the actual APK building process
- Provides build progress feedback
- Saves APK to `Builds/Android` folder
- Generates build reports
- Handles build errors and cleanup

### 4. BuildManager.cs (Editor Window)
- Comprehensive build management interface
- Quick build buttons
- Build profiles management
- Build statistics display
- Build history tracking
- Advanced settings editor

## Usage

### Quick Build Options

From Unity Editor:
- `Build/Configure Android` - Configure settings and validate
- `Build/Android/Build Development APK` - Build development APK
- `Build/Android/Build Release APK` - Build release APK

Or use the Build Manager window:
- `Cristo/Build Manager` - Full-featured build interface

### Build Directory Structure

```
C:\Projects\risto\Builds\
└── Android/
    ├── Cristo_Adventure_0.1.0_dev.apk     (Development APK)
    ├── Cristo_Adventure_0.1.0.apk        (Release APK)
    ├── build_config.json                  (Build configuration)
    ├── build_report_*.json               (Build reports)
    ├── build_profiles.json               (Custom build profiles)
    └── Cache/                            (Build cache - auto-deleted)
```

## Configuration Details

### Player Settings
- **Company Name**: Cristo Adventure Studios
- **Product Name**: Cristo Adventure
- **Bundle Identifier**: com.cristoadventure.game
- **Version**: 0.1.0
- **Default Orientation**: Landscape Left
- **Scripting Backend**: IL2CPP
- **API Compatibility Level**: .NET Standard 2.1
- **Minimum API Level**: 26 (Android 8.0)
- **Target Architecture**: ARM64

### Build Settings
- **APK Splitting**: Enabled (by architecture)
- **Development Build**: Enabled for dev builds
- **Script Debugging**: Enabled for dev builds
- **Managed Stripping**: Low level

### Preprocessor Defines
The following symbols are defined for Android builds:
- `UNITY_ANDROID`
- `CRISTO_ADVENTURE_ANDROID`
- `Firebase_Messaging`
- `Firebase_Crashlytics`

## Firebase Integration

The build system includes Firebase support. To configure Firebase:

1. Create a project in Firebase Console
2. Download `google-services.json`
3. Place it in `Assets/Android` folder
4. Run `Window > Firebase > Setup Firebase Project`

## Build Profiles

You can create custom build profiles for different configurations:
1. Open Build Manager window
2. Click "Add New Profile"
3. Configure settings:
   - Bundle identifier override
   - Version override
   - Scene selection
   - Custom defines
   - Development/Release settings

## Build Reports

Each build generates a JSON report with:
- Build timestamp
- Version information
- APK size and path
- Build settings
- Scene count
- Target architecture
- API levels

## Troubleshooting

### Common Issues

1. **Bundle Identifier Error**
   - Ensure bundle identifier follows Android package naming convention
   - Must be unique in Google Play Store

2. **Minimum API Level Error**
   - Check minimum SDK setting in Player Settings
   - Must be at least 26 (Android 8.0)

3. **Scripting Backend Error**
   - Verify IL2CPP is selected
   - May need to increase IL2CPP compilation time

4. **No Scenes in Build**
   - Use `Build/Configure Android` to auto-add current scene
   - Or manually add scenes in File > Build Settings

### Debug Information

Check Unity Console for:
- Build progress messages
- Error details
- Build statistics
- Warnings

### File Locations

- Scripts: `Assets/Editor/`
- Build Output: `C:\Projects\cristo\Builds\Android\`
- Reports: `C:\Projects\cristo\Builds\Android\build_report_*.json`

## Customization

To customize the build system:

1. Modify constants in `BuildConfigurator.cs`
2. Add custom preprocessor symbols
3. Update Firebase integration
4. Modify build paths and names
5. Add custom validation checks

## Integration Notes

- Build system validates configuration before building
- Automatic scene inclusion for current active scene
- Version code auto-increment for release builds
- Build history tracking with timestamps
- Performance metrics (build time, file size)