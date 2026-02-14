using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace CristoAdventure.BuildSystem
{
    public static class BuildConfigurator
    {
        public const string COMPANY_NAME = "Cristo Adventure Studios";
        public const string PRODUCT_NAME = "Cristo Adventure";
        public const string BUNDLE_ID = "com.cristoadventure.game";
        public const string VERSION = "0.1.0";
        public const string COMPANY_URL = "https://cristoadventure.com";

        // Build Settings
        public const int MIN_API_LEVEL = 26; // Android 8.0
        public const string TARGET_ARCHITECTURE = "ARM64";
        public const ScriptingBackend SCRIPTING_BACKEND = ScriptingBackend.IL2CPP;
        public const ApiCompatibilityLevel API_COMPATIBILITY_LEVEL = ApiCompatibilityLevel.NET_4_6;

        // Define symbols for Android build
        public static readonly string[] ANDROID_DEFINES =
        {
            "UNITY_ANDROID",
            "CRISTO_ADVENTURE_ANDROID",
            "Firebase_Messaging",
            "Firebase_Crashlytics"
        };

        public static void ConfigureAndroidBuild()
        {
            // Configure Player Settings
            ConfigurePlayerSettings();

            // Configure Build Settings
            ConfigureBuildSettings();

            // Define symbols for Firebase integration
            DefineSymbolsForAndroid();
        }

        private static void ConfigurePlayerSettings()
        {
            // Set company name
            PlayerSettings.companyName = COMPANY_NAME;

            // Set product name
            PlayerSettings.productName = PRODUCT_NAME;

            // Set bundle identifier
            PlayerSettings.SetApplicationIdentifier(BuildTarget.Android, BUNDLE_ID);

            // Set version
            PlayerSettings.bundleVersion = VERSION;
            PlayerSettings.bundleVersionCode = 1;

            // Set default orientation
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

            // Set default screen orientation
            PlayerSettings.defaultIsNativeOrientation = false;

            // Set immersive mode for Android
            PlayerSettings.runInBackground = true;

            // Set graphics API for Android
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[]
            {
                GraphicsDeviceType.Vulkan,
                GraphicsDeviceType.OpenGLES3
            });

            // Set rendering path
            PlayerSettings.defaultRenderingPath = RenderingPath.UsePlayerSettings;

            // Set color space
            PlayerSettings.colorSpace = ColorSpace.Linear;

            // Set splash screen
            PlayerSettings.SetSplashScreenOrientation(BuildTarget.Android, UIOrientation.LandscapeLeft);

            // Set minimal API level
            PlayerSettings.Android.targetSdkVersion = AndroidTargetSdkVersion.AndroidApiLevelAuto;

            // Set targeted API level to auto (latest)
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;

            // Set IL2CPP scripting backend
            PlayerSettings.SetScriptingBackend(BuildTarget.Android, SCRIPTING_BACKEND);

            // Set API compatibility level
            PlayerSettings.SetApiCompatibilityLevel(BuildTarget.Android, API_COMPATIBILITY_LEVEL);

            // Enable custom main manifest
            PlayerSettings.Android.useCustomMainManifest = true;

            // Enable custom Gradle build
            PlayerSettings.Android.useCustomGradleProperties = true;

            // Set Keystore and alias (if needed)
            // PlayerSettings.Android.keystoreName = "path/to/keystore";
            // PlayerSettings.Android.keystorePass = "your_keystore_password";
            // PlayerSettings.Android.keyaliasName = "your_alias";
            // PlayerSettings.Android.keyaliasPass = "your_alias_password";
        }

        private static void ConfigureBuildSettings()
        {
            // Get current build settings
            BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;

            // Set architecture
            PlayerSettings.SetArchitectureForTarget(buildTargetGroup, BuildTargetArchitecture.ARM64);

            // Enable APK splitting by architecture
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;

            // Enable managed stripping levels
            PlayerSettings.managedStrippingLevel = ManagedStrippingLevel.Low;

            // Set scripting define symbols
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            string androidDefines = "";

            foreach (var define in ANDROID_DEFINES)
            {
                if (!currentDefines.Contains(define))
                {
                    androidDefines += define + ";";
                }
            }

            if (!string.IsNullOrEmpty(androidDefines))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup,
                    currentDefines + ";" + androidDefines);
            }
        }

        private static void DefineSymbolsForAndroid()
        {
            BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            // Ensure Firebase-related defines are set
            string firebaseDefines = "";
            if (!currentDefines.Contains("Firebase_Messaging"))
            {
                firebaseDefines += "Firebase_Messaging;";
            }
            if (!currentDefines.Contains("Firebase_Crashlytics"))
            {
                firebaseDefines += "Firebase_Crashlytics;";
            }

            if (!string.IsNullOrEmpty(firebaseDefines))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup,
                    currentDefines + ";" + firebaseDefines);
            }
        }

        public static void AutoAddCurrentSceneToBuild()
        {
            string[] scenes = { "Assets/Scenes/" + EditorSceneManager.GetActiveScene().name + ".unity" };

            // Find existing build settings
            string[] existingScenes = new string[EditorBuildSettings.scenes.Length];
            EditorBuildSettings.scenes.CopyTo(existingScenes, 0);

            // Add current scene if not already in build
            bool sceneExists = false;
            foreach (string existingScene in existingScenes)
            {
                if (existingScene == scenes[0])
                {
                    sceneExists = true;
                    break;
                }
            }

            if (!sceneExists)
            {
                var newSceneList = new EditorBuildSceneSettings[existingScenes.Length + 1];
                for (int i = 0; i < existingScenes.Length; i++)
                {
                    newSceneList[i] = EditorBuildSettings.scenes[i];
                }
                newSceneList[existingScenes.Length] = new EditorBuildSceneSettings
                {
                    path = scenes[0],
                    enabled = true
                };

                EditorBuildSettings.scenes = newSceneList;
            }
        }
    }
}