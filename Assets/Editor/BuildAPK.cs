using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace CristoAdventure.BuildSystem
{
    public static class BuildAPK
    {
        private static readonly string BUILD_PATH = "C:/Projects/cristo/Builds/Android";
        private const bool DEVELOPMENT_BUILD = true;

        [MenuItem("Build/Android/Quick Build Dev APK", priority = 4)]
        public static void QuickBuildDevAPK()
        {
            BuildAPKWrapper(true);
        }

        [MenuItem("Build/Android/Quick Build Release APK", priority = 5)]
        public static void QuickBuildReleaseAPK()
        {
            BuildAPKWrapper(false);
        }

        public static bool BuildAPKWrapper(bool isDevelopment)
        {
            try
            {
                // Validate configuration first
                if (!AndroidBuildSetup.ValidateConfiguration())
                {
                    Debug.LogError("Build failed: Configuration validation failed");
                    return false;
                }

                // Show build dialog
                if (!ShowBuildDialog(isDevelopment))
                {
                    return false;
                }

                // Create build settings
                var buildOptions = new BuildPlayerOptions();
                ConfigureBuildOptions(buildOptions, isDevelopment);

                // Create build directory if it doesn't exist
                if (!Directory.Exists(BUILD_PATH))
                {
                    Directory.CreateDirectory(BUILD_PATH);
                }

                // Start build process
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Debug.Log($"Starting {isDevelopment ? "development" : "release"} APK build...");
                BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

                stopwatch.Stop();

                // Handle build result
                if (report.summary.result == BuildResult.Succeeded)
                {
                    ShowSuccessDialog(isDevelopment, report, stopwatch.Elapsed);
                    PostBuildProcess(report.summary.outputPath, isDevelopment);
                    return true;
                }
                else
                {
                    ShowBuildFailedDialog(report.summary);
                    return false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Build failed with exception: {e.Message}");
                EditorUtility.DisplayDialog("Build Error",
                    $"Build failed with exception: {e.Message}\n\nCheck the console for details.", "OK");
                return false;
            }
        }

        private static void ConfigureBuildOptions(BuildPlayerOptions options, bool isDevelopment)
        {
            // Get enabled scenes
            List<string> scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }

            options.scenes = scenes.ToArray();
            options.locationPathName = GetBuildPath(isDevelopment);
            options.target = BuildTarget.Android;
            options.options = isDevelopment ?
                BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode :
                BuildOptions.None;
        }

        private static string GetBuildPath(bool isDevelopment)
        {
            string suffix = isDevelopment ? "_dev" : "";
            string fileName = $"{BuildConfigurator.PRODUCT_NAME}_{BuildConfigurator.VERSION}{suffix}.apk";
            return Path.Combine(BUILD_PATH, fileName);
        }

        private static bool ShowBuildDialog(bool isDevelopment)
        {
            string message = isDevelopment ?
                "Building Development APK\n\n" +
                "- Development build enabled\n" +
                "- Debug symbols included\n" +
                "- Script debugging allowed\n\n" +
                "Continue with build?" :
                "Building Release APK\n\n" +
                "- Development features disabled\n" +
                "- Optimized for performance\n" +
                "- Minimal build size\n\n" +
                "Continue with build?";

            return EditorUtility.DisplayDialog("Build Confirmation", message, "Build", "Cancel");
        }

        private static void ShowSuccessDialog(bool isDevelopment, BuildReport report, System.TimeSpan buildTime)
        {
            string buildInfo = $"Build completed successfully!\n\n" +
                $"Target: {report.summary.outputPath}\n" +
                $"File Size: {FormatFileSize(new FileInfo(report.summary.outputPath).Length)}\n" +
                $"Build Time: {buildTime.TotalSeconds:F1} seconds\n" +
                $"Target Size: {report.summary.totalSize} bytes\n\n" +
                $"Scenes Included: {report.steps.Length}\n" +
                $"Platform: Android\n" +
                $"Build Type: {isDevelopment ? "Development" : "Release"}";

            // Open build directory
            bool openFolder = EditorUtility.DisplayDialog("Build Complete", buildInfo, "Open Folder", "OK");

            if (openFolder)
            {
                System.Diagnostics.Process.Start("explorer.exe", "\"" + BUILD_PATH + "\" + "\"");
            }
        }

        private static void ShowBuildFailedDialog(BuildSummary summary)
        {
            string errorInfo = "Build failed!\n\n" +
                $"Error: {summary.error}\n" +
                $"Log: {summary.totalErrors} errors, {summary.totalWarnings} warnings\n\n" +
                "Check the Console for details.";

            EditorUtility.DisplayDialog("Build Failed", errorInfo, "OK");
        }

        private static void PostBuildProcess(string apkPath, bool isDevelopment)
        {
            // Verify APK file exists
            if (!File.Exists(apkPath))
            {
                Debug.LogError("APK file not found after build: " + apkPath);
                return;
            }

            // Generate build report
            GenerateBuildReport(apkPath, isDevelopment);

            // Update version code if needed
            if (!isDevelopment)
            {
                UpdateVersionCode();
            }

            // Clear build cache (optional)
            // ClearBuildCache();

            Debug.Log($"Build artifacts saved to: {BUILD_PATH}");
        }

        private static void GenerateBuildReport(string apkPath, bool isDevelopment)
        {
            var report = new
            {
                BuildTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Version = BuildConfigurator.VERSION,
                VersionCode = PlayerSettings.bundleVersionCode,
                BundleId = BuildConfigurator.BUNDLE_ID,
                Target = "Android",
                Architecture = BuildConfigurator.TARGET_ARCHITECTURE,
                MinApiLevel = BuildConfigurator.MIN_API_LEVEL,
                ScriptingBackend = BuildConfigurator.SCRIPTING_BACKEND,
                DevelopmentBuild = isDevelopment,
                ApkPath = apkPath,
                ApkSize = new FileInfo(apkPath).Length,
                ScenesCount = EditorBuildSettings.scenes.Length
            };

            string reportPath = Path.Combine(BUILD_PATH, $"build_report_{System.DateTime.Now:yyyyMMdd_HHmmss}.json");
            string json = JsonUtility.ToJson(report, true);
            File.WriteAllText(reportPath, json);

            Debug.Log($"Build report generated: {reportPath}");
        }

        private static void UpdateVersionCode()
        {
            // Increment version code for release builds
            int newVersionCode = PlayerSettings.bundleVersionCode + 1;
            PlayerSettings.bundleVersionCode = newVersionCode;

            Debug.Log($"Version code updated to: {newVersionCode}");
        }

        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        private static void ClearBuildCache()
        {
            // This would clear Unity's build cache
            // In practice, you might want to keep some cache files
            string cachePath = Path.Combine(BUILD_PATH, "Cache");
            if (Directory.Exists(cachePath))
            {
                Directory.Delete(cachePath, true);
                Debug.Log("Build cache cleared");
            }
        }

        // Additional utility method for custom builds
        public static BuildReport BuildCustomAPK(BuildPlayerOptions options)
        {
            return BuildPipeline.BuildPlayer(options);
        }

        // Method to get build statistics
        public static Dictionary<string, object> GetBuildStatistics()
        {
            var stats = new Dictionary<string, object>
            {
                {"BundleId", PlayerSettings.GetApplicationIdentifier(BuildTarget.Android)},
                {"Version", PlayerSettings.bundleVersion},
                {"VersionCode", PlayerSettings.bundleVersionCode},
                {"MinApiLevel", PlayerSettings.Android.minSdkVersion},
                {"TargetArchitecture", PlayerSettings.GetArchitectureForTarget(BuildTargetGroup.Android)},
                {"ScriptingBackend", PlayerSettings.GetScriptingBackend(BuildTarget.Android)},
                {"ScenesCount", EditorBuildSettings.scenes.Length},
                {"TotalScenesSize", CalculateScenesTotalSize()}
            };

            return stats;
        }

        private static long CalculateScenesTotalSize()
        {
            long totalSize = 0;
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (File.Exists(scene.path))
                {
                    totalSize += new FileInfo(scene.path).Length;
                }
            }
            return totalSize;
        }
    }
}