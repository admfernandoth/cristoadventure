using UnityEngine;

public class CreateLightingSetup : MonoBehaviour
{
    [Header("Lighting Setup Creator")]
    public GameObject lightingParent;

    void Start()
    {
        CreateLightingSetupPrefab();
    }

    void CreateLightingSetupPrefab()
    {
        // Create parent object for lighting setup
        GameObject lightingSetup = new GameObject("LightingSetup");

        // Create main directional light with warm settings
        GameObject mainLight = new GameObject("MainDirectionalLight");
        mainLight.transform.SetParent(lightingSetup.transform);
        mainLight.transform.rotation = Quaternion.Euler(45f, 30f, 0f);

        Light directionalLight = mainLight.AddComponent<Light>();
        directionalLight.type = LightType.Directional;
        directionalLight.color = new Color(1f, 0.9f, 0.7f, 1f); // Warm light
        directionalLight.intensity = 1f;
        directionalLight.shadows = LightShadows.Soft;

        // Configure ambient occlusion settings
        ConfigureAmbientOcclusion();

        // Add reflection probe for better lighting
        CreateReflectionProbes(lightingSetup);

        // Create additional atmosphere lights
        CreateAtmosphereLights(lightingSetup);

        // Debug.Log("LightingSetup prefab created with " + lightingSetup.transform.childCount + " child objects");
    }

    void ConfigureAmbientOcclusion()
    {
        // Configure ambient settings
        RenderSettings.ambientMode = AmbientMode.Skybox;
        RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.25f, 1f);
        RenderSettings.ambientIntensity = 0.5f;

        // Create ambient occlusion object
        GameObject aoSettings = new GameObject("AmbientOcclusionSettings");
        aoSettings.transform.SetParent(transform);

        // In a real implementation, you would add:
        // - Post-processing volume
        // - SSAO settings
        // - Skybox settings
    }

    void CreateReflectionProbes(GameObject lightingSetup)
    {
        // Create reflection probe for each major area
        Vector3[] probePositions = {
            new Vector3(0f, 2f, 0f),    // Center probe
            new Vector3(-10f, 2f, 0f),   // Left probe
            new Vector3(10f, 2f, 0f),    // Right probe
            new Vector3(0f, 2f, -10f),   // Back probe
            new Vector3(0f, 2f, 10f)    // Front probe
        };

        for (int i = 0; i < probePositions.Length; i++)
        {
            GameObject probeObj = new GameObject($"ReflectionProbe_{i + 1}");
            probeObj.transform.SetParent(lightingSetup.transform);
            probeObj.transform.position = probePositions[i];

            ReflectionProbe probe = probeObj.AddComponent<ReflectionProbe>();
            probe.size = new Vector3(20f, 20f, 20f);
            probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;
            probe.boxProjection = true;
            probe.importance = ReflectionProbeImportance.High;
        }
    }

    void CreateAtmosphereLights(GameObject lightingSetup)
    {
        // Create fill lights for better atmosphere
        Vector3[] fillLightPositions = {
            new Vector3(-5f, 8f, -5f),
            new Vector3(5f, 8f, 5f),
            new Vector3(0f, 10f, 0f)
        };

        Color[] fillLightColors = {
            new Color(1f, 0.8f, 0.6f, 0.3f),  // Warm fill
            new Color(0.8f, 0.9f, 1f, 0.2f),  // Cool fill
            new Color(0.9f, 0.85f, 0.7f, 0.25f) // Neutral warm
        };

        for (int i = 0; i < fillLightPositions.Length; i++)
        {
            GameObject fillLightObj = new GameObject($"FillLight_{i + 1}");
            fillLightObj.transform.SetParent(lightingSetup.transform);
            fillLightObj.transform.position = fillLightPositions[i];

            Light fillLight = fillLightObj.AddComponent<Light>();
            fillLight.type = LightType.Point;
            fillLight.color = fillLightColors[i];
            fillLight.intensity = 0.5f;
            fillLight.range = 15f;
            fillLight.shadowRadius = 0.1f;
        }

        // Create rim light for dramatic effect
        GameObject rimLightObj = new GameObject("RimLight");
        rimLightObj.transform.SetParent(lightingSetup.transform);
        rimLightObj.transform.position = new Vector3(0f, 15f, 0f);
        rimLightObj.transform.rotation = Quaternion.Euler(60f, 0f, 0f);

        Light rimLight = rimLightObj.AddComponent<Light>();
        rimLight.type = LightType.Spot;
        rimLight.color = new Color(1f, 0.9f, 0.6f, 0.4f);
        rimLight.intensity = 0.8f;
        rimLight.range = 20f;
        rimLight.spotAngle = 60f;
        rimLight.shadowRadius = 0.05f;
    }
}