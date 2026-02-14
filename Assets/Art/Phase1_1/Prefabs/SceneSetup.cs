using UnityEngine;
using UnityEditor;

public class SceneSetup : MonoBehaviour
{
    [MenuItem("Cristo/Setup Phase 1.1 Scene")]
    static void SetupScene()
    {
        Debug.Log("Setting up Phase 1.1 scene...");

        // Clear existing scene
        SceneView.lastActiveSceneView.Frame();

        // Create empty game objects for organization
        GameObject environments = new GameObject("Environments");
        GameObject lights = new GameObject("Lights");
        GameObject gameManager = new GameObject("GameManager");

        // Load prefabs
        GameObject basilica = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Art/Phase1_1/Prefabs/BasilicaInterior.prefab");
        GameObject nativityCave = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Art/Phase1_1/Prefabs/NativityCave.prefab");
        GameObject courtyard = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Art/Phase1_1/Prefabs/Courtyard.prefab");
        GameObject lightingSetup = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Art/Phase1_1/Prefabs/LightingSetup.prefab");

        // Instantiate prefabs in scene
        if (basilica != null)
        {
            GameObject basilicaInstance = PrefabUtility.InstantiatePrefab(basilica) as GameObject;
            basilicaInstance.transform.SetParent(environments.transform);
            basilicaInstance.transform.position = new Vector3(0f, 0f, 0f);
        }

        if (nativityCave != null)
        {
            GameObject nativityInstance = PrefabUtility.InstantiatePrefab(nativityCave) as GameObject;
            nativityInstance.transform.SetParent(environments.transform);
            nativityInstance.transform.position = new Vector3(30f, 0f, 0f);
        }

        if (courtyard != null)
        {
            GameObject courtyardInstance = PrefabUtility.InstantiatePrefab(courtyard) as GameObject;
            courtyardInstance.transform.SetParent(environments.transform);
            courtyardInstance.transform.position = new Vector3(-30f, 0f, 0f);
        }

        if (lightingSetup != null)
        {
            GameObject lightingInstance = PrefabUtility.InstantiatePrefab(lightingSetup) as GameObject;
            lightingInstance.transform.SetParent(lights.transform);
            lightingInstance.transform.position = new Vector3(0f, 10f, 0f);
        }

        // Set up camera
        SetupMainCamera();

        // Configure render settings
        ConfigureRenderSettings();

        Debug.Log("Scene setup complete!");
    }

    static void SetupMainCamera()
    {
        // Check if main camera exists
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraGO = new GameObject("Main Camera");
            mainCamera = cameraGO.AddComponent<Camera>();
            cameraGO.tag = "MainCamera";
        }

        // Configure camera
        mainCamera.transform.position = new Vector3(0f, 5f, 20f);
        mainCamera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        mainCamera.clearFlags = CameraClearFlags.Skybox;
        mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.15f);
        mainCamera.fieldOfView = 60f;
        mainCamera.nearClipPlane = 0.1f;
        mainCamera.farClipPlane = 100f;
    }

    static void ConfigureRenderSettings()
    {
        // Skybox
        RenderSettings.skybox = AssetDatabase.LoadAssetAtPath<Material>("Assets/Art/Phase1_1/Materials/NightSkySkybox.mat");

        // Fog
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.2f, 0.2f, 0.3f);
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = 10f;
        RenderSettings.fogEndDistance = 100f;

        // Ambient lighting
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.25f, 1f);
        RenderSettings.ambientIntensity = 0.5f;
    }
}