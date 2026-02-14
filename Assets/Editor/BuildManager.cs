using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

namespace CristoAdventure.BuildSystem
{
    public class BuildManager : EditorWindow
    {
        private Vector2 scrollPosition;
        private bool showAdvancedSettings = false;
        private BuildTargetGroup currentTargetGroup = BuildTargetGroup.Android;
        private List<BuildProfile> buildProfiles = new List<BuildProfile>();

        [MenuItem("Cristo/Build Manager", priority = 200)]
        public static void ShowWindow()
        {
            GetWindow<BuildManager>("Build Manager");
        }

        private void OnEnable()
        {
            // Load build profiles
            LoadBuildProfiles();
        }

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.LabelField("Cristo Adventure - Build Manager", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // Quick Build Buttons
            GUILayout.Label("Quick Builds", EditorStyles.boldLabel);
            if (GUILayout.Button("Configure & Build Dev APK", GUILayout.Height(40)))
            {
                ConfigureAndBuild(true);
            }
            if (GUILayout.Button("Configure & Build Release APK", GUILayout.Height(40)))
            {
                ConfigureAndBuild(false);
            }

            GUILayout.Space(20);

            // Build Profiles
            GUILayout.Label("Build Profiles", EditorStyles.boldLabel);
            if (GUILayout.Button("Add New Profile"))
            {
                AddBuildProfile();
            }

            // List existing profiles
            for (int i = 0; i < buildProfiles.Count; i++)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField(buildProfiles[i].name, GUILayout.Width(150));
                    if (GUILayout.Button("Edit", GUILayout.Width(50)))
                    {
                        EditBuildProfile(i);
                    }
                    if (GUILayout.Button("Build", GUILayout.Width(50)))
                    {
                        BuildProfile(buildProfiles[i], i);
                    }
                    if (GUILayout.Button("Delete", GUILayout.Width(50), GUILayout.Height(20)))
                    {
                        buildProfiles.RemoveAt(i);
                        SaveBuildProfiles();
                    }
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(20);

            // Build Statistics
            EditorGUILayout.LabelField("Build Statistics", EditorStyles.boldLabel);
            DrawBuildStatistics();

            GUILayout.Space(20);

            // Advanced Settings
            if (GUILayout.Button("Advanced Settings"))
            {
                showAdvancedSettings = !showAdvancedSettings;
            }

            if (showAdvancedSettings)
            {
                DrawAdvancedSettings();
            }

            GUILayout.Space(20);

            // Build History
            GUILayout.Label("Recent Builds", EditorStyles.boldLabel);
            DrawBuildHistory();

            GUILayout.EndScrollView();
        }

        private void ConfigureAndBuild(bool isDevelopment)
        {
            // First configure the build
            AndroidBuildSetup.ConfigureAndroidBuild();

            // Then build the APK
            bool success = BuildAPK.BuildAPKWrapper(isDevelopment);

            if (success)
            {
                // Save this as a recent build
                AddToBuildHistory(isDevelopment);
            }
        }

        private void DrawBuildStatistics()
        {
            var stats = BuildAPK.GetBuildStatistics();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                foreach (var stat in stats)
                {
                    EditorGUILayout.LabelField($"{stat.Key}: {stat.Value}");
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawAdvancedSettings()
        {
            EditorGUILayout.LabelField("Advanced Build Settings", EditorStyles.boldLabel);

            // Bundle identifier
            string currentBundleId = PlayerSettings.GetApplicationIdentifier(BuildTarget.Android);
            string newBundleId = EditorGUILayout.TextField("Bundle Identifier", currentBundleId);
            if (newBundleId != currentBundleId)
            {
                PlayerSettings.SetApplicationIdentifier(BuildTarget.Android, newBundleId);
            }

            // Version
            string currentVersion = PlayerSettings.bundleVersion;
            string newVersion = EditorGUILayout.TextField("Version", currentVersion);
            if (newVersion != currentVersion)
            {
                PlayerSettings.bundleVersion = newVersion;
            }

            // Version code
            int versionCode = PlayerSettings.bundleVersionCode;
            int newVersionCode = EditorGUILayout.IntField("Version Code", versionCode);
            if (newVersionCode != versionCode)
            {
                PlayerSettings.bundleVersionCode = newVersionCode;
            }

            // Minimum API level
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Min API Level", GUILayout.Width(100));
                PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions)EditorGUILayout.EnumPopup(PlayerSettings.Android.minSdkVersion);
            }
            EditorGUILayout.EndHorizontal();

            // Scripting backend
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Scripting Backend", GUILayout.Width(120));
                PlayerSettings.SetScriptingBackend(BuildTarget.Android,
                    (ScriptingBackend)EditorGUILayout.EnumPopup(PlayerSettings.GetScriptingBackend(BuildTarget.Android)));
            }
            EditorGUILayout.EndHorizontal();

            // Architecture
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Architecture", GUILayout.Width(100));
                PlayerSettings.SetArchitectureForTarget(BuildTargetGroup.Android,
                    (BuildTargetArchitecture)EditorGUILayout.EnumPopup(PlayerSettings.GetArchitectureForTarget(BuildTargetGroup.Android)));
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawBuildHistory()
        {
            var buildHistory = GetBuildHistory();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                foreach (var build in buildHistory)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(build.Type, GUILayout.Width(100));
                        EditorGUILayout.LabelField(build.Timestamp, GUILayout.Width(200));
                        if (GUILayout.Button("Open", GUILayout.Width(50)))
                        {
                            System.Diagnostics.Process.Start("explorer.exe", "\"" + build.Path + "\" + "\"");
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private List<BuildRecord> GetBuildHistory()
        {
            var history = new List<BuildRecord>();
            if (!Directory.Exists(BUILD_PATH))
                return history;

            foreach (var file in Directory.GetFiles(BUILD_PATH, "*.apk"))
            {
                var record = new BuildRecord
                {
                    Path = file,
                    Type = file.Contains("dev") ? "Dev" : "Release",
                    Timestamp = File.GetCreationTime(file).ToString("yyyy-MM-dd HH:mm:ss")
                };
                history.Add(record);
            }

            history.Sort((a, b) => b.Timestamp.CompareTo(a.Timestamp));
            return history;
        }

        private void AddToBuildHistory(bool isDevelopment)
        {
            // This is handled by the GetBuildHistory method
        }

        private void LoadBuildProfiles()
        {
            string profilePath = Path.Combine(BUILD_PATH, "build_profiles.json");
            if (File.Exists(profilePath))
            {
                string json = File.ReadAllText(profilePath);
                buildProfiles = JsonUtility.FromJson<BuildProfilesWrapper>(json).profiles;
            }
        }

        private void SaveBuildProfiles()
        {
            string profilePath = Path.Combine(BUILD_PATH, "build_profiles.json");
            Directory.CreateDirectory(BUILD_PATH);
            var wrapper = new BuildProfilesWrapper { profiles = buildProfiles };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(profilePath, json);
        }

        private void AddBuildProfile()
        {
            var profile = new BuildProfile
            {
                name = $"Profile {buildProfiles.Count + 1}",
                isDevelopment = true,
                customBundleId = "",
                customVersion = "",
                scenes = new List<string>(),
                customDefines = new List<string>()
            };
            buildProfiles.Add(profile);
            SaveBuildProfiles();
        }

        private void EditBuildProfile(int index)
        {
            // Open a dialog to edit profile
            Debug.Log($"Edit profile: {buildProfiles[index].name}");
            // Implementation would open a custom editor window
        }

        private void BuildProfile(BuildProfile profile, int index)
        {
            Debug.Log($"Building profile: {profile.name}");
            // Implementation would build with profile settings
        }

        private const string BUILD_PATH = "C:/Projects/cristo/Builds/Android";

        // Data structures
        [Serializable]
        public class BuildProfile
        {
            public string name;
            public bool isDevelopment;
            public string customBundleId;
            public string customVersion;
            public List<string> scenes;
            public List<string> customDefines;
        }

        [Serializable]
        public class BuildRecord
        {
            public string Path;
            public string Type;
            public string Timestamp;
        }

        [Serializable]
        public class BuildProfilesWrapper
        {
            public List<BuildProfile> profiles;
        }
    }
}