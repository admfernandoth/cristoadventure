using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace CristoAdventure.BuildSystem
{
    public static class PostBuildProcessor
    {
        public static void PostProcessAndroidBuild(BuildReport report)
        {
            string apkPath = report.summary.outputPath;
            string buildPath = Path.GetDirectoryName(apkPath);

            try
            {
                // Generate APK information
                GenerateAPKInfo(apkPath);

                // Generate build report
                GenerateBuildReport(report);

                // Optimize APK (if tools are available)
                OptimizeAPK(apkPath);

                // Generate installation instructions
                GenerateInstallInstructions(buildPath);

                // Copy to release folder if needed
                CopyToReleaseFolder(apkPath);

                // Update build manifest
                UpdateBuildManifest(report);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Post-build processing failed: {e.Message}");
            }
        }

        private static void GenerateAPKInfo(string apkPath)
        {
            string infoPath = Path.ChangeExtension(apkPath, ".info");
            var info = new APKInfo(apkPath);

            string content = $"APK Information:\n" +
                $"Name: {info.FileName}\n" +
                $"Size: {info.FileSizeFormatted}\n" +
                $"Created: {info.CreationDate}\n" +
                $"Path: {info.FullPath}\n" +
                $"IsDevelopment: {info.IsDevelopment}\n" +
                $"Version: {info.Version}\n" +
                $"BuildTime: {BuildConfigurator.VERSION}\n" +
                $"Target: Android";

            File.WriteAllText(infoPath, content);
            Debug.Log($"APK info generated: {infoPath}");
        }

        private static void GenerateBuildReport(BuildReport report)
        {
            string reportPath = Path.Combine(
                Path.GetDirectoryName(report.summary.outputPath),
                $"build_report_{System.DateTime.Now:yyyyMMdd_HHmmss}.json"
            );

            var buildReport = new BuildReportData
            {
                BuildTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Version = PlayerSettings.bundleVersion,
                VersionCode = PlayerSettings.bundleVersionCode,
                BundleId = PlayerSettings.GetApplicationIdentifier(BuildTarget.Android),
                Target = "Android",
                MinApiLevel = PlayerSettings.Android.minSdkVersion,
                ScriptingBackend = PlayerSettings.GetScriptingBackend(BuildTarget.Android),
                Architecture = PlayerSettings.GetArchitectureForTarget(BuildTargetGroup.Android),
                DevelopmentBuild = report.summary.options.HasFlag(BuildOptions.Development),
                Scenes = GetSceneInfo(report),
                Steps = GetStepInfo(report),
                Summary = report.summary
            };

            string json = JsonUtility.ToJson(buildReport, true);
            File.WriteAllText(reportPath, json);
            Debug.Log($"Build report generated: {reportPath}");
        }

        private static List<SceneInfo> GetSceneInfo(BuildReport report)
        {
            var scenes = new List<SceneInfo>();
            foreach (var scene in report.steps)
            {
                scenes.Add(new SceneInfo
                {
                    Path = scene.name,
                    Type = scene.type.ToString(),
                    Duration = scene.duration,
                    Messages = scene.messages
                });
            }
            return scenes;
        }

        private static List<StepInfo> GetStepInfo(BuildReport report)
        {
            var steps = new List<StepInfo>();
            foreach (var step in report.steps)
            {
                steps.Add(new StepInfo
                {
                    Name = step.name,
                    Duration = step.duration,
                    Type = step.type.ToString(),
                    Messages = step.messages
                });
            }
            return steps;
        }

        private static void OptimizeAPK(string apkPath)
        {
            // This would run APK optimization tools if available
            // Examples:
            // - APK Analyzer (built-in Android Studio tool)
            // - zipalign tool from Android SDK
            // - apksigner for signing

            Debug.Log("APK optimization placeholder");
            // Implementation would depend on available tools
        }

        private static void GenerateInstallInstructions(string buildPath)
        {
            string instructions = CristoAdventure Install Instructions\n\n" +
                "1. Transfer APK to Android device:\n" +
                "   - Via USB\n" +
                "   - Email\n" +
                "   - Cloud storage\n\n" +
                "2. Enable Unknown Sources:\n" +
                "   Settings > Security > Unknown Sources\n" +
                "   Enable for this installation\n\n" +
                "3. Install APK:\n" +
                "   - Open APK file from file manager\n" +
                "   - Tap 'Install'\n" +
                "   - Wait for installation to complete\n\n" +
                "4. Launch app:\n" +
                "   - Find 'Cristo Adventure' in app drawer\n" +
                "   - Tap to launch\n\n" +
                "5. Verify installation:\n" +
                "   - Check version: " + PlayerSettings.bundleVersion + "\n" +
                "   - Test basic functionality\n\n" +
                "Troubleshooting:\n" +
                "- If installation fails, check device storage\n" +
                "- Ensure APK is not corrupted\n" +
                "- Check device meets minimum requirements\n" +
                "- Clear app cache if needed";

            string instructionPath = Path.Combine(buildPath, "install_instructions.txt");
            File.WriteAllText(instructionPath, instructions);
            Debug.Log($"Install instructions generated: {instructionPath}");
        }

        private static void CopyToReleaseFolder(string apkPath)
        {
            string releasePath = Path.Combine(
                Path.GetDirectoryName(Path.GetDirectoryName(apkPath)),
                "Releases"
            );

            if (!Directory.Exists(releasePath))
            {
                Directory.CreateDirectory(releasePath);
            }

            string fileName = Path.GetFileName(apkPath);
            string releaseApkPath = Path.Combine(releasePath, fileName);

            // Copy with timestamp
            string timestampedFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{System.DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(fileName)}";
            string finalPath = Path.Combine(releasePath, timestampedFileName);

            File.Copy(apkPath, finalPath, true);
            Debug.Log($"APK copied to release folder: {finalPath}");
        }

        private static void UpdateBuildManifest(BuildReport report)
        {
            string manifestPath = Path.Combine(
                Path.GetDirectoryName(report.summary.outputPath),
                "build_manifest.json"
            );

            var manifest = new BuildManifest
            {
                Version = PlayerSettings.bundleVersion,
                BuildNumber = PlayerSettings.bundleVersionCode,
                Platform = "Android",
                BuildType = report.summary.options.HasFlag(BuildOptions.Development) ? "Development" : "Release",
                BuildTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                CommitHash = GetGitCommitHash(),
                Branch = GetGitBranch(),
                BuildSize = report.summary.totalSize,
                APKSize = new FileInfo(report.summary.outputPath).Length,
                ScenesCount = EditorBuildSettings.scenes.Length,
                MinApiLevel = PlayerSettings.Android.minSdkVersion,
                TargetArchitecture = PlayerSettings.GetArchitectureForTarget(BuildTargetGroup.Android).ToString(),
                ScriptingBackend = PlayerSettings.GetScriptingBackend(BuildTarget.Android).ToString(),
                ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(BuildTarget.Android).ToString()
            };

            string json = JsonUtility.ToJson(manifest, true);
            File.WriteAllText(manifestPath, json);
            Debug.Log($"Build manifest updated: {manifestPath}");
        }

        private static string GetGitCommitHash()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "rev-parse HEAD",
                    WorkingDirectory = Directory.GetParent(Application.dataPath).FullName,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                using (StreamReader reader = process.StandardOutput)
                {
                return reader.ReadLine().Trim();
                }
                }
            }
            catch
            {
                return "unknown";
            }
        }

        private static string GetGitBranch()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "rev-parse --abbrev-ref HEAD",
                    WorkingDirectory = Directory.GetParent(Application.dataPath).FullName,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                using (StreamReader reader = process.StandardOutput)
                {
                return reader.ReadLine().Trim();
                }
                }
            }
            catch
            {
                return "unknown";
            }
        }

        // Data structures
        [Serializable]
        public class APKInfo
        {
            public string FileName;
            public long FileSize;
            public string FileSizeFormatted;
            public string CreationDate;
            public string FullPath;
            public bool IsDevelopment;
            public string Version;

            public APKInfo(string path)
            {
                var fileInfo = new FileInfo(path);
                FileName = fileInfo.Name;
                FileSize = fileInfo.Length;
                FileSizeFormatted = FormatFileSize(FileSize);
                CreationDate = fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
                FullPath = fileInfo.FullName;
                Version = PlayerSettings.bundleVersion;
                IsDevelopment = path.Contains("_dev");
            }
        }

        [Serializable]
        public class BuildReportData
        {
            public string BuildTime;
            public string Version;
            public int VersionCode;
            public string BundleId;
            public string Target;
            public string MinApiLevel;
            public string ScriptingBackend;
            public string Architecture;
            public bool DevelopmentBuild;
            public List<SceneInfo> Scenes;
            public List<StepInfo> Steps;
            public BuildSummary Summary;
        }

        [Serializable]
        public class SceneInfo
        {
            public string Path;
            public string Type;
            public float Duration;
            public string Messages;
        }

        [Serializable]
        public class StepInfo
        {
            public string Name;
            public float Duration;
            public string Type;
            public string Messages;
        }

        [Serializable]
        public class BuildManifest
        {
            public string Version;
            public int BuildNumber;
            public string Platform;
            public string BuildType;
            public string BuildTime;
            public string CommitHash;
            public string Branch;
            public long BuildSize;
            public long APKSize;
            public int ScenesCount;
            public string MinApiLevel;
            public string TargetArchitecture;
            public string ScriptingBackend;
            public string ApiCompatibilityLevel;
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
    }
}