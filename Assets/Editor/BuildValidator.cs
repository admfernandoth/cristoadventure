using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace CristoAdventure.BuildSystem
{
    public static class BuildValidator
    {
        public static ValidationResult ValidateBuildConfiguration()
        {
            var validationResult = new ValidationResult();

            // Validate player settings
            ValidatePlayerSettings(validationResult);

            // Validate build settings
            ValidateBuildSettings(validationResult);

            // Validate scenes
            ValidateScenes(validationResult);

            // Validate Firebase setup
            ValidateFirebaseSetup(validationResult);

            // Validate APK settings
            ValidateAPKSettings(validationResult);

            // Validate permissions
            ValidatePermissions(validationResult);

            return validationResult;
        }

        private static void ValidatePlayerSettings(ValidationResult result)
        {
            // Company name
            if (string.IsNullOrEmpty(PlayerSettings.companyName))
            {
                result.AddError("Company Name", "Company name is not set in Player Settings");
            }
            else if (PlayerSettings.companyName != BuildConfigurator.COMPANY_NAME)
            {
                result.AddWarning("Company Name",
                    $"Expected: {BuildConfigurator.COMPANY_NAME}, Current: {PlayerSettings.companyName}");
            }

            // Product name
            if (string.IsNullOrEmpty(PlayerSettings.productName))
            {
                result.AddError("Product Name", "Product name is not set in Player Settings");
            }
            else if (PlayerSettings.productName != BuildConfigurator.PRODUCT_NAME)
            {
                result.AddWarning("Product Name",
                    $"Expected: {BuildConfigurator.PRODUCT_NAME}, Current: {PlayerSettings.productName}");
            }

            // Bundle identifier
            string bundleId = PlayerSettings.GetApplicationIdentifier(BuildTarget.Android);
            if (string.IsNullOrEmpty(bundleId))
            {
                result.AddError("Bundle Identifier", "Bundle identifier is not set");
            }
            else if (bundleId != BuildConfigurator.BUNDLE_ID)
            {
                result.AddWarning("Bundle Identifier",
                    $"Expected: {BuildConfigurator.BUNDLE_ID}, Current: {bundleId}");
            }

            // Version
            if (string.IsNullOrEmpty(PlayerSettings.bundleVersion))
            {
                result.AddError("Version", "Version is not set in Player Settings");
            }

            // Version code
            if (PlayerSettings.bundleVersionCode == 0)
            {
                result.AddError("Version Code", "Version code is not set");
            }

            // Default orientation
            if (PlayerSettings.defaultInterfaceOrientation != UIOrientation.LandscapeLeft)
            {
                result.AddWarning("Default Orientation",
                    $"Expected: Landscape Left, Current: {PlayerSettings.defaultInterfaceOrientation}");
            }

            // Graphics APIs
            var graphicsAPIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
            bool hasVulkan = false;
            bool hasOpenGLES3 = false;

            foreach (var api in graphicsAPIs)
            {
                if (api == GraphicsDeviceType.Vulkan) hasVulkan = true;
                if (api == GraphicsDeviceType.OpenGLES3) hasOpenGLES3 = true;
            }

            if (!hasVulkan && !hasOpenGLES3)
            {
                result.AddError("Graphics APIs", "No valid graphics APIs configured for Android");
            }
            else if (!hasVulkan)
            {
                result.AddWarning("Graphics APIs", "Vulkan recommended for better performance");
            }

            // Color space
            if (PlayerSettings.colorSpace != ColorSpace.Linear)
            {
                result.AddWarning("Color Space",
                    $"Expected: Linear, Current: {PlayerSettings.colorSpace}");
            }

            // Rendering path
            if (PlayerSettings.defaultRenderingPath != RenderingPath.UsePlayerSettings)
            {
                result.AddWarning("Rendering Path",
                    $"Expected: UsePlayerSettings, Current: {PlayerSettings.defaultRenderingPath}");
            }
        }

        private static void ValidateBuildSettings(ValidationResult result)
        {
            // Scripting backend
            ScriptingBackend scriptingBackend = PlayerSettings.GetScriptingBackend(BuildTarget.Android);
            if (scriptingBackend != ScriptingBackend.IL2CPP)
            {
                result.AddError("Scripting Backend",
                    $"Expected: IL2CPP, Current: {scriptingBackend}");
            }

            // API compatibility level
            ApiCompatibilityLevel apiLevel = PlayerSettings.GetApiCompatibilityLevel(BuildTarget.Android);
            if (apiLevel != BuildConfigurator.API_COMPATIBILITY_LEVEL)
            {
                result.AddError("API Compatibility Level",
                    $"Expected: {BuildConfigurator.API_COMPATIBILITY_LEVEL}, Current: {apiLevel}");
            }

            // Minimum API level
            if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel26)
            {
                result.AddError("Minimum API Level",
                    $"Expected: 26 (Android 8.0), Current: {PlayerSettings.Android.minSdkVersion}");
            }

            // Architecture
            BuildTargetArchitecture architecture = PlayerSettings.GetArchitectureForTarget(BuildTargetGroup.Android);
            if (architecture != BuildTargetArchitecture.ARM64)
            {
                result.AddWarning("Target Architecture",
                    $"Expected: ARM64, Current: {architecture}");
            }

            // Target architectures
            AndroidArchitecture targetArch = PlayerSettings.Android.targetArchitectures;
            if ((targetArch & AndroidArchitecture.ARM64) == 0)
            {
                result.AddWarning("Target Architectures",
                    "ARM64 architecture should be included in target architectures");
            }

            // Managed stripping
            if (PlayerSettings.managedStrippingLevel == ManagedStrippingLevel.None)
            {
                result.AddWarning("Managed Stripping",
                    "Consider enabling managed stripping to reduce APK size");
            }

            // Define symbols
            BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            foreach (var expectedDefine in BuildConfigurator.ANDROID_DEFINES)
            {
                if (!defines.Contains(expectedDefine))
                {
                    result.AddWarning("Scripting Define Symbols",
                        $"Missing define: {expectedDefine}");
                }
            }

            // Custom manifest
            if (!PlayerSettings.Android.useCustomMainManifest)
            {
                result.AddWarning("Custom Manifest",
                    "Consider using custom Android manifest for better control");
            }

            // Custom Gradle
            if (!PlayerSettings.Android.useCustomGradleProperties)
            {
                result.AddWarning("Custom Gradle",
                    "Consider using custom Gradle properties for build optimization");
            }
        }

        private static void ValidateScenes(ValidationResult result)
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                result.AddError("Scenes", "No scenes are enabled in Build Settings");
                return;
            }

            int enabledScenes = 0;
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    enabledScenes++;
                }
            }

            if (enabledScenes == 0)
            {
                result.AddError("Scenes", "No scenes are enabled in Build Settings");
            }
            else
            {
                result.AddInfo("Scenes", $"{enabledScenes} scenes enabled in build");
            }
        }

        private static void ValidateFirebaseSetup(ValidationResult result)
        {
            // Check FirebaseManager
            string firebaseManagerPath = "Assets/Scripts/Network/FirebaseManager.cs";
            if (!File.Exists(firebaseManagerPath))
            {
                result.AddWarning("Firebase Setup",
                    "FirebaseManager.cs not found. Firebase integration may not work.");
            }

            // Check google-services.json
            string servicesJsonPath = "Assets/Android/google-services.json";
            if (!File.Exists(servicesJsonPath))
            {
                result.AddWarning("Firebase Setup",
                    "google-services.json not found. Firebase features may not work.");
            }

            // Check Firebase packages
            string packagesPath = "Packages/manifest.json";
            if (File.Exists(packagesPath))
            {
                string packages = File.ReadAllText(packagesPath);
                if (!packages.Contains("com.unity.services.firebase"))
                {
                    result.AddWarning("Firebase Setup",
                        "Firebase packages not installed. Install from Unity Package Manager.");
                }
            }
        }

        private static void ValidateAPKSettings(ValidationResult result)
        {
            // Check if build directory exists
            string buildPath = "C:/Projects/cristo/Builds/Android";
            if (!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
            }

            // Check write permissions
            string testFile = Path.Combine(buildPath, "test_write.tmp");
            try
            {
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
            }
            catch (UnauthorizedAccessException)
            {
                result.AddError("Build Directory",
                    "No write permissions to build directory: " + buildPath);
            }
        }

        private static void ValidatePermissions(ValidationResult result)
        {
            // Android manifest
            string manifestPath = "Assets/Android/AndroidManifest.xml";
            if (File.Exists(manifestPath))
            {
                string manifestContent = File.ReadAllText(manifestPath);

                // Check essential permissions
                string[] essentialPermissions = {
                    "android.permission.INTERNET",
                    "android.permission.ACCESS_NETWORK_STATE",
                    "android.permission.WAKE_LOCK"
                };

                foreach (var permission in essentialPermissions)
                {
                    if (!manifestContent.Contains(permission))
                    {
                        result.AddWarning("Android Permissions",
                            $"Missing permission: {permission}");
                    }
                }
            }
            else
            {
                result.AddWarning("Android Manifest",
                    "AndroidManifest.xml not found. Create in Assets/Android/");
            }
        }

        public class ValidationResult
        {
            public List<ValidationItem> Errors = new List<ValidationItem>();
            public List<ValidationItem> Warnings = new List<ValidationItem>();
            public List<ValidationItem> Infos = new List<ValidationItem>();

            public void AddError(string category, string message)
            {
                Errors.Add(new ValidationItem(category, message));
            }

            public void AddWarning(string category, string message)
            {
                Warnings.Add(new ValidationItem(category, message));
            }

            public void AddInfo(string category, string message)
            {
                Infos.Add(new ValidationItem(category, message));
            }

            public bool IsValid => Errors.Count == 0;
            public bool HasWarnings => Warnings.Count > 0;

            public string GetValidationReport()
            {
                var report = new System.Text.StringBuilder();

                report.AppendLine("=== Build Validation Report ===");
                report.AppendLine($"Valid: {IsValid}");
                report.AppendLine($"Errors: {Errors.Count}");
                report.AppendLine($"Warnings: {Warnings.Count}");
                report.AppendLine($"Infos: {Infos.Count}");

                if (Errors.Count > 0)
                {
                    report.AppendLine("\n--- Errors ---");
                    foreach (var error in Errors)
                    {
                        report.AppendLine($"[{error.Category}] {error.Message}");
                    }
                }

                if (Warnings.Count > 0)
                {
                    report.AppendLine("\n--- Warnings ---");
                    foreach (var warning in Warnings)
                    {
                        report.AppendLine($"[{warning.Category}] {warning.Message}");
                    }
                }

                if (Infos.Count > 0)
                {
                    report.AppendLine("\n--- Information ---");
                    foreach (var info in Infos)
                    {
                        report.AppendLine($"[{info.Category}] {info.Message}");
                    }
                }

                return report.ToString();
            }
        }

        public class ValidationItem
        {
            public string Category;
            public string Message;

            public ValidationItem(string category, string message)
            {
                Category = category;
                Message = message;
            }
        }

        [MenuItem("Cristo/Build Validation", priority = 150)]
        public static void RunValidation()
        {
            var validationResult = ValidateBuildConfiguration();
            string report = validationResult.GetValidationReport();

            Debug.Log(report);

            if (!validationResult.IsValid)
            {
                EditorUtility.DisplayDialog("Validation Failed",
                    "There are errors that must be fixed before building.\n\nCheck the console for details.", "OK");
            }
            else if (validationResult.HasWarnings)
            {
                EditorUtility.DisplayDialog("Validation Complete with Warnings",
                    "The build is valid but has some warnings.\n\nCheck the console for details.", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Validation Complete",
                    "The build configuration is valid!", "OK");
            }
        }
    }
}