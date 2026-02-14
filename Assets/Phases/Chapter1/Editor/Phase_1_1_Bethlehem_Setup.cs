using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

namespace CristoAdventure.Editor
{
    /// <summary>
    /// Scene setup script for Phase 1.1 Bethlehem
    /// Run this script from the Unity Editor menu: GameObject > Phase Setup > Create Phase 1.1 Bethlehem
    /// </summary>
    public class Phase_1_1_Bethlehem_Setup : EditorWindow
    {
        [MenuItem("GameObject/Phase Setup/Create Phase 1.1 Bethlehem Scene")]
        public static void CreatePhase1_1BethlehemScene()
        {
            // Create new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            scene.name = "Phase_1_1_Bethlehem";

            // Setup Lighting
            SetupLighting();

            // Setup Ground
            SetupGround();

            // Setup PhaseManager
            SetupPhaseManager();

            // Setup Player Spawn Point
            SetupPlayerSpawn();

            // Setup POIs
            SetupPOIs();

            // Setup Phase Exit
            SetupPhaseExit();

            // Save scene
            string scenePath = "Assets/Phases/Chapter1/Phase_1_1_Bethlehem.unity";
            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log($"[PhaseSetup] Scene created at: {scenePath}");

            // Add to build settings
            AddSceneToBuildSettings(scenePath);
        }

        private static void SetupLighting()
        {
            // Warm directional light (simulating candlelight/church interior)
            GameObject mainLight = new GameObject("MainLight");
            Light lightComp = mainLight.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            lightComp.color = new Color(1f, 0.90588236f, 0.7490196f); // Warm yellow
            lightComp.intensity = 0.8f;
            lightComp.shadows = LightShadows.Soft;
            lightComp.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            // Ambient point light for soft fill
            GameObject ambientLight = new GameObject("AmbientLight");
            Light ambientComp = ambientLight.AddComponent<Light>();
            ambientComp.type = LightType.Point;
            ambientComp.color = new Color(1f, 0.8666667f, 0.7490196f); // Warm orange
            ambientComp.intensity = 0.5f;
            ambientComp.range = 20f;
            ambientComp.transform.position = new Vector3(0f, 5f, 0f);

            Debug.Log("[PhaseSetup] Lighting configured");
        }

        private static void SetupGround()
        {
            // Create ground plane
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(50f, 1f, 50f);
            ground.transform.position = Vector3.zero;

            // Remove collider if exists (MeshCollider was added by CreatePrimitive)
            Collider collider = ground.GetComponent<Collider>();
            if (collider != null)
            {
                Object.DestroyImmediate(collider);
            }

            // Add proper mesh collider
            MeshCollider meshCollider = ground.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = ground.GetComponent<MeshFilter>().sharedMesh;

            // Set layer to Ground
            ground.layer = LayerMask.NameToLayer("Default");

            Debug.Log("[PhaseSetup] Ground created");
        }

        private static void SetupPhaseManager()
        {
            GameObject phaseManagerObj = new GameObject("PhaseManager");
            PhaseManager phaseManager = phaseManagerObj.AddComponent<PhaseManager>();

            // Configure serialized fields via reflection (since we're in editor)
            SerializedObject so = new SerializedObject(phaseManager);

            so.FindProperty("_phaseId").stringValue = "1.1";
            so.FindProperty("_phaseNameKey").stringValue = "Phase.1.1.Name";
            so.FindProperty("_requiredStars").intValue = 2;
            so.FindProperty("_autoSaveOnComplete").boolValue = true;
            so.FindProperty("_nextPhaseId").stringValue = "1.2";
            so.FindProperty("_totalPOIs").intValue = 6;

            so.ApplyModifiedProperties();

            Debug.Log("[PhaseSetup] PhaseManager configured");
        }

        private static void SetupPlayerSpawn()
        {
            GameObject spawnPoint = new GameObject("PlayerSpawn");
            spawnPoint.transform.position = new Vector3(0f, 0f, 10f);
            spawnPoint.tag = "PlayerSpawn";

            Debug.Log("[PhaseSetup] Player spawn point created at (0, 0, 10)");
        }

        private static void SetupPOIs()
        {
            // POI-001: History Plaque (InfoPlaque)
            GameObject plaque1 = CreatePOI("POI-001_HistoryPlaque", PrimitiveType.Cube,
                new Vector3(0f, 1.5f, 5f), Quaternion.identity);

            // POI-002: Silver Star (ReliquaryPOI)
            GameObject star = CreatePOI("POI-002_SilverStar", PrimitiveType.Sphere,
                new Vector3(0f, 0.5f, 0f), Quaternion.identity);

            // POI-003: Manger Plaque (InfoPlaque)
            GameObject plaque2 = CreatePOI("POI-003_MangerPlaque", PrimitiveType.Cube,
                new Vector3(2f, 1.5f, 0f), Quaternion.identity);

            // POI-004: Father Elias (NPCGuidePOI)
            GameObject npc = CreatePOI("POI-004_FatherElias", PrimitiveType.Capsule,
                new Vector3(-5f, 1f, -5f), Quaternion.identity);
            npc.transform.localScale = new Vector3(1f, 2f, 1f);

            // POI-005: Photo Spot (PhotoSpotPOI)
            GameObject photoSpot = CreatePOI("POI-005_PhotoSpot", PrimitiveType.Cube,
                new Vector3(0f, 1f, -10f), Quaternion.identity);

            // POI-006: Luke 2:7 Verse (VerseMarkerPOI)
            GameObject verse = CreatePOI("POI-006_Luke2_7Verse", PrimitiveType.Cube,
                new Vector3(3f, 1f, -8f), Quaternion.identity);

            Debug.Log("[PhaseSetup] All 6 POIs created");
        }

        private static GameObject CreatePOI(string name, PrimitiveType type, Vector3 position, Quaternion rotation)
        {
            GameObject poi = GameObject.CreatePrimitive(type);
            poi.name = name;
            poi.transform.position = position;
            poi.transform.rotation = rotation;

            // Add appropriate POI component based on name
            System.Type poiComponentType = null;

            if (name.Contains("Plaque"))
            {
                poiComponentType = System.Type.GetType("CristoAdventure.Gameplay.InfoPlaque");
            }
            else if (name.Contains("Star"))
            {
                poiComponentType = System.Type.GetType("CristoAdventure.Gameplay.ReliquaryPOI");
            }
            else if (name.Contains("FatherElias"))
            {
                poiComponentType = System.Type.GetType("CristoAdventure.Gameplay.NPCGuidePOI");
            }
            else if (name.Contains("Photo"))
            {
                poiComponentType = System.Type.GetType("CristoAdventure.Gameplay.PhotoSpotPOI");
            }
            else if (name.Contains("Verse"))
            {
                poiComponentType = System.Type.GetType("CristoAdventure.Gameplay.VerseMarkerPOI");
            }

            if (poiComponentType != null)
            {
                poi.AddComponent(poiComponentType);
            }

            return poi;
        }

        private static void SetupPhaseExit()
        {
            GameObject exitObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            exitObj.name = "PhaseExit";
            exitObj.transform.position = new Vector3(0f, 1.5f, -15f);
            exitObj.transform.localScale = new Vector3(2f, 3f, 0.5f);

            // Add PhaseExit component
            System.Type exitType = System.Type.GetType("CristoAdventure.Gameplay.PhaseExit");
            if (exitType != null)
            {
                exitObj.AddComponent(exitType);
            }

            Debug.Log("[PhaseSetup] Phase exit created at (0, 1.5, -15)");
        }

        private static void AddSceneToBuildSettings(string scenePath)
        {
            var buildScenes = EditorBuildSettings.scenes;

            // Check if scene is already in build settings
            foreach (var scene in buildScenes)
            {
                if (scene.path == scenePath)
                {
                    Debug.Log("[PhaseSetup] Scene already in build settings");
                    return;
                }
            }

            // Add scene to build settings
            var newSceneEntry = new EditorBuildSettingsScene(scenePath, true);
            System.Array.Resize(ref buildScenes, buildScenes.Length + 1);
            buildScenes[buildScenes.Length - 1] = newSceneEntry;
            EditorBuildSettings.scenes = buildScenes;

            Debug.Log("[PhaseSetup] Scene added to build settings");
        }

        [MenuItem("GameObject/Phase Setup/Documentation")]
        public static void ShowDocumentation()
        {
            EditorWindow.GetWindow<Phase_1_1_Bethlehem_Setup>(true, "Phase 1.1 Setup Documentation");
        }

        private void OnGUI()
        {
            GUILayout.Label("Phase 1.1 Bethlehem Scene Setup", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            GUILayout.Label("Instructions:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "1. Open Unity Editor\n" +
                "2. Click the menu: GameObject > Phase Setup > Create Phase 1.1 Bethlehem Scene\n" +
                "3. The scene will be automatically created and configured\n" +
                "4. Scene will be saved to: Assets/Phases/Chapter1/Phase_1_1_Bethlehem.unity\n" +
                "5. Scene will be added to Build Settings",
                MessageType.Info
            );

            EditorGUILayout.Space();
            GUILayout.Label("Scene Contents:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "- Warm cinematic lighting setup\n" +
                "- Ground plane with collider\n" +
                "- PhaseManager component (Phase ID: 1.1)\n" +
                "- Player spawn point at (0, 0, 10)\n" +
                "- 6 POI GameObjects:\n" +
                "  * POI-001: History Plaque (InfoPlaque)\n" +
                "  * POI-002: Silver Star (ReliquaryPOI)\n" +
                "  * POI-003: Manger Plaque (InfoPlaque)\n" +
                "  * POI-004: Father Elias (NPCGuidePOI)\n" +
                "  * POI-005: Photo Spot (PhotoSpotPOI)\n" +
                "  * POI-006: Luke 2:7 Verse (VerseMarkerPOI)\n" +
                "- PhaseExit at end of path",
                MessageType.None
            );

            EditorGUILayout.Space();
            GUILayout.Label("Notes:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "- All GameObjects use placeholder primitives (cubes, spheres)\n" +
                "- Visual 3D models will be created by Agent-11 (3D Artist)\n" +
                "- POI components are attached but need configuration in Inspector\n" +
                "- Localization keys need to be set for each POI",
                MessageType.Warning
            );

            if (GUILayout.Button("Create Scene Now"))
            {
                CreatePhase1_1BethlehemScene();
                this.Close();
            }
        }
    }
}
