using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

namespace CristoAdventure.BuildSystem
{
    public static class AndroidBuildSetup
    {
        private static readonly string BUILD_PATH = "C:/Projects/cristo/Builds/Android";
        private static readonly string CONFIG_FILE = BUILD_PATH + "/build_config.json";

        [MenuItem("Build/Configure Android", priority = 1)]
        public static void ConfigureAndroidBuild()
        {
            bool success = true;

            try
            {
                // Create build directory if it doesn't exist
                if (!Directory.Exists(BUILD_PATH))
                {
                    Directory.CreateDirectory(BUILD_PATH);
                }

                // Auto-add current scene to build
                BuildConfigurator.AutoAddCurrentSceneToBuild();

                // Configure Android build settings
                BuildConfigurator.ConfigureAndroidBuild();

                // Validate configuration
                if (!ValidateConfiguration())
                {
                    success = false;
                    return;
                }

                // Save configuration
                SaveBuildConfiguration();

                // Show success dialog
                ShowSuccessDialog();

                // Check if Firebase is configured
                CheckFirebaseSetup();
            }
            catch (System.Exception e)
            {
                success = false;
                ShowErrorDialog($"Failed to configure Android build: {e.Message}");
                Debug.LogError($"Android build configuration failed: {e.Message}");
            }

            if (success)
            {
                Debug.Log("Android build configuration completed successfully!");
            }
        }

        [MenuItem("Build/Android/Build Development APK", priority = 2)]
        public static void BuildDevelopmentAPK()
        {
            bool success = BuildAPK(true);
            if (success)
            {
                Debug.Log("Development APK built successfully!");
            }
        }

        [MenuItem("Build/Android/Build Release APK", priority = 3)]
        public static void BuildReleaseAPK()
        {
            bool success = BuildAPK(false);
            if (success)
            {
                Debug.Log("Release APK built successfully!");
            }
        }

        private static bool ValidateConfiguration()
        {
            bool isValid = true;
            string issues = "Configuration Issues:\n\n";

            // Check bundle identifier
            if (PlayerSettings.GetApplicationIdentifier(BuildTarget.Android) != BuildConfigurator.BUNDLE_ID)
            {
                issues += "- Bundle identifier is not configured correctly\n";
                isValid = false;
            }

            // Check minimum API level
            if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel26)
            {
                issues += "- Minimum API level should be at least 26 (Android 8.0)\n";
                isValid = false;
            }

            // Check scripting backend
            if (PlayerSettings.GetScriptingBackend(BuildTarget.Android) != ScriptingBackend.IL2CPP)
            {
                issues += "- Scripting backend should be IL2CPP\n";
                isValid = false;
            }

            // Check API compatibility level
            if (PlayerSettings.GetApiCompatibilityLevel(BuildTarget.Android) != BuildConfigurator.API_COMPATIBILITY_LEVEL)
            {
                issues += $"- API compatibility level should be {BuildConfigurator.API_COMPATIBILITY_LEVEL}\n";
                isValid = false;
            }

            // Check if scenes are added to build
            if (EditorBuildSettings.scenes.Length == 0)
            {
                issues += "- No scenes are added to the build settings\n";
                isValid = false;
            }

            if (!isValid)
            {
                issues += "\nPlease fix these issues before proceeding with the build.";
                ShowErrorDialog(issues);
            }

            return isValid;
        }

        private static void SaveBuildConfiguration()
        {
            var config = new
            {
                companyName = PlayerSettings.companyName,
                productName = PlayerSettings.productName,
                bundleId = PlayerSettings.GetApplicationIdentifier(BuildTarget.Android),
                version = PlayerSettings.bundleVersion,
                versionCode = PlayerSettings.bundleVersionCode,
                minApiLevel = PlayerSettings.Android.minSdkVersion,
                targetApiLevel = PlayerSettings.Android.targetSdkVersion,
                scriptingBackend = PlayerSettings.GetScriptingBackend(BuildTarget.Android),
                architecture = PlayerSettings.GetArchitectureForTarget(BuildTargetGroup.Android),
                apiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(BuildTarget.Android),
                timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string json = JsonUtility.ToJson(config, true);
            File.WriteAllText(CONFIG_FILE, json);
        }

        private static void ShowSuccessDialog()
        {
            string message = $"Android build configured successfully!\n\n" +
                            $"Company: {BuildConfigurator.COMPANY_NAME}\n" +
                            $"Product: {BuildConfigurator.PRODUCT_NAME}\n" +
                            $"Bundle ID: {BuildConfigurator.BUNDLE_ID}\n" +
                            $"Version: {BuildConfigurator.VERSION}\n" +
                            $"Min API Level: {BuildConfigurator.MIN_API_LEVEL}\n" +
                            $"Target Architecture: {BuildConfigurator.TARGET_ARCHITECTURE}";

            EditorUtility.DisplayDialog("Configuration Complete", message, "OK");
        }

        private static void ShowErrorDialog(string message)
        {
            EditorUtility.DisplayDialog("Configuration Error", message, "OK");
        }

        private static void CheckFirebaseSetup()
        {
            // Check if FirebaseManager exists
            string firebaseManagerPath = "Assets/Scripts/Network/FirebaseManager.cs";
            if (!File.Exists(firebaseManagerPath))
            {
                EditorUtility.DisplayDialog("Firebase Setup Note",
                    "FirebaseManager.cs not found. Firebase integration may not work properly.\n\n" +
                    "Make sure to configure Firebase in the Unity Editor by following these steps:\n" +
                    "1. Open Firebase Console\n"2. Add new project\n" +
                    "3. Download google-services.json\n" +
                    "4. Place it in Assets/Android\n" +
                    "5. Run 'Firebase: Setup Firebase Project' from the Window menu", "OK");
            }
        }

        private static string GetBuildPath(bool isDevelopment)
        {
            string suffix = isDevelopment ? "_dev" : "";
            string fileName = $"{BuildConfigurator.PRODUCT_NAME}_{BuildConfigurator.VERSION}{suffix}.apk";
            return Path.Combine(BUILD_PATH, fileName);
        }
    }
}