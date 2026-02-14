using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace CristoAdventure.Editor
{
    /// <summary>
    /// Editor helper to set up Phase 1.1 scene from JSON configuration
    /// Run this from the Unity Editor menu: Cristo/Phase 1.1/Setup Scene
    /// </summary>
    public class Phase1_1_SceneSetupHelper : EditorWindow
    {
        private const string CONFIG_PATH = "Assets/Phases/Chapter1/Data/Phase1_1_POIConfig.json";
        private const string PREFABS_PATH = "Assets/Prefabs/Interactables/";

        [MenuItem("Cristo/Phase 1.1/Setup Scene from JSON")]
        public static void SetupSceneFromJSON()
        {
            // Check if config file exists
            if (!File.Exists(CONFIG_PATH))
            {
                EditorUtility.DisplayDialog("Error",
                    $"Configuration file not found:\n{CONFIG_PATH}\n\nPlease create the configuration file first.",
                    "OK");
                return;
            }

            // Load JSON config
            string json = File.ReadAllText(CONFIG_PATH);
            var config = JsonUtility.FromJson<PhaseConfig>(json);

            if (config == null || config.pois == null || config.pois.Length == 0)
            {
                EditorUtility.DisplayDialog("Error",
                    "Failed to load POI configuration or no POIs found.",
                    "OK");
                return;
            }

            // Get the current scene root
            GameObject sceneRoot = GameObject.Find("_SceneRoot");
            if (sceneRoot == null)
            {
                sceneRoot = new GameObject("_SceneRoot");
            }

            // Create POI container
            Transform poiContainer = sceneRoot.transform.Find("POIs");
            if (poiContainer == null)
            {
                GameObject poiContainerObj = new GameObject("POIs");
                poiContainer = poiContainerObj.transform;
                poiContainer.SetParent(sceneRoot.transform);
            }

            int createdCount = 0;
            int updatedCount = 0;
            int errorCount = 0;

            // Process each POI
            foreach (var poiData in config.pois)
            {
                try
                {
                    // Find existing POI or create new
                    GameObject poiObj = GameObject.Find(poiData.id);
                    bool createdNew = false;

                    if (poiObj == null)
                    {
                        // Load prefab
                        string prefabPath = PREFABS_PATH + poiData.type + ".prefab";
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                        if (prefab == null)
                        {
                            // Create fallback GameObject if prefab doesn't exist
                            poiObj = new GameObject(poiData.id);
                            createdNew = true;
                        }
                        else
                        {
                            poiObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                            poiObj.name = poiData.id;
                            createdNew = true;
                        }
                    }

                    // Set position, rotation, scale
                    poiObj.transform.position = new Vector3(poiData.position.x, poiData.position.y, poiData.position.z);
                    poiObj.transform.rotation = Quaternion.Euler(poiData.rotation.x, poiData.rotation.y, poiData.rotation.z);
                    poiObj.transform.localScale = new Vector3(poiData.scale.x, poiData.scale.y, poiData.scale.z);

                    // Set parent
                    poiObj.transform.SetParent(poiContainer);

                    // Add POI identifier component
                    var identifier = poiObj.GetComponent<POIIdentifier>();
                    if (identifier == null)
                    {
                        identifier = poiObj.AddComponent<POIIdentifier>();
                    }
                    identifier.poiId = poiData.id;
                    identifier.poiType = poiData.type;

                    if (createdNew)
                    {
                        createdCount++;
                    }
                    else
                    {
                        updatedCount++;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to create POI {poiData.id}: {e.Message}");
                    errorCount++;
                }
            }

            // Show results
            string message = $"Scene setup complete!\n\n" +
                $"Created: {createdCount} POIs\n" +
                $"Updated: {updatedCount} POIs\n" +
                $"Errors: {errorCount}";

            EditorUtility.DisplayDialog("Scene Setup Complete", message, "OK");

            Debug.Log($"[Phase1_1_SceneSetupHelper] {message}");
        }

        [MenuItem("Cristo/Phase 1.1/Create Phase Manager")]
        public static void CreatePhaseManager()
        {
            GameObject sceneRoot = GameObject.Find("_SceneRoot");
            if (sceneRoot == null)
            {
                sceneRoot = new GameObject("_SceneRoot");
            }

            // Check if PhaseManager already exists
            GameObject phaseManagerObj = GameObject.Find("PhaseManager_1_1");
            if (phaseManagerObj != null)
            {
                EditorUtility.DisplayDialog("Info",
                    "PhaseManager already exists in the scene.",
                    "OK");
                return;
            }

            // Create PhaseManager GameObject
            phaseManagerObj = new GameObject("PhaseManager_1_1");

            // Add PhaseManager component (assuming it exists)
            var phaseManager = phaseManagerObj.AddComponent<CristoAdventure.Gameplay.PhaseManager>();

            // Set phase ID
            // Note: This requires PhaseManager to have a phaseId field that can be set via reflection or public method
            // For now, we'll just create the GameObject and component

            phaseManagerObj.transform.SetParent(sceneRoot.transform);

            EditorUtility.DisplayDialog("Phase Manager Created",
                "PhaseManager_1_1 has been created.\n\n" +
                "Configure it in the Inspector.",
                "OK");

            Debug.Log("[Phase1_1_SceneSetupHelper] Created PhaseManager_1_1");
        }

        [MenuItem("Cristo/Phase 1.1/Open Phase 1.1 Scene")]
        public static void OpenPhase1_1Scene()
        {
            string scenePath = "Assets/Phases/Chapter1/Phase_1_1_Bethlehem.unity";

            if (!File.Exists(scenePath))
            {
                EditorUtility.DisplayDialog("Error",
                    $"Scene not found:\n{scenePath}",
                    "OK");
                return;
            }

            EditorSceneManager.OpenScene(scenePath);
        }

        // Data structures for JSON parsing
        [System.Serializable]
        public class PhaseConfig
        {
            public string phaseId;
            public string phaseName;
            public POIData[] pois;
            public PhaseCompletionData phaseCompletion;
        }

        [System.Serializable]
        public class POIData
        {
            public string id;
            public string name;
            public string type;
            public PositionData position;
            public RotationData rotation;
            public ScaleData scale;
            public POIContentData data;
        }

        [System.Serializable]
        public class PositionData
        {
            public float x;
            public float y;
            public float z;
        }

        [System.Serializable]
        public class RotationData
        {
            public float x;
            public float y;
            public float z;
        }

        [System.Serializable]
        public class ScaleData
        {
            public float x;
            public float y;
            public float z;
        }

        [System.Serializable]
        public class POIContentData
        {
            public string titleKey;
            public string contentKey;
            public string relicId;
            public string npcNameKey;
            public string dialogueId;
            public string promptKey;
            public string referenceKey;
            public string verseKey;
            public string puzzleId;
            public string nextPhaseId;
            public int requiredVisits;
            public string iconEmoji;
        }

        [System.Serializable]
        public class PhaseCompletionData
        {
            public int requiredVisits;
            public int requiredPuzzles;
            public string rewardRelicId;
            public int rewardXP;
        }

        // Helper component to identify POIs
        private class POIIdentifier : MonoBehaviour
        {
            public string poiId;
            public string poiType;
        }
    }
}
