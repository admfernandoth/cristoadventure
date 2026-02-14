using UnityEngine;

public class CreateCourtyard : MonoBehaviour
{
    [Header("Courtyard Creator")]
    public GameObject courtyardParent;

    void Start()
    {
        CreateCourtyardPrefab();
    }

    void CreateCourtyardPrefab()
    {
        // Create parent object for the courtyard
        GameObject courtyard = new GameObject("Courtyard");
        courtyard.tag = "Environment";

        // Create outdoor area (scale 15x1x15)
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.SetParent(courtyard.transform);
        ground.transform.localScale = new Vector3(15f, 0.1f, 15f); // Scale 15x1x15
        ground.transform.position = new Vector3(0f, -0.05f, 0f);
        ground.GetComponent<MeshRenderer>().material = BasicMaterialsCreation.CreateStoneMaterial();
        ground.tag = "Ground";

        // Add collider to ground
        BoxCollider groundCollider = ground.AddComponent<BoxCollider>();

        // Create stone floor with grid pattern
        CreateStoneFloorGrid(courtyard);

        // Set up skybox for night sky with stars
        RenderSettings.skybox = CreateNightSkySkybox();

        // Add ambient moonlight
        GameObject moonLight = new GameObject("MoonLight");
        moonLight.transform.SetParent(courtyard.transform);
        moonLight.transform.position = new Vector3(10f, 10f, 10f);
        Light directionalLight = moonLight.AddComponent<Light>();
        directionalLight.type = LightType.Directional;
        directionalLight.color = new Color(0.4f, 0.5f, 0.8f, 1f); // Blue moonlight
        directionalLight.intensity = 0.5f;
        directionalLight.transform.rotation = Quaternion.Euler(45f, 30f, 0f);

        // Add subtle point lights for additional atmosphere
        GameObject[] lightPositions = {
            new Vector3(-5f, 2f, -5f),
            new Vector3(5f, 2f, 5f),
            new Vector3(0f, 2f, -10f)
        };

        for (int i = 0; i < lightPositions.Length; i++)
        {
            GameObject pointLightObj = new GameObject($"AmbientLight_{i + 1}");
            pointLightObj.transform.SetParent(courtyard.transform);
            pointLightObj.transform.position = lightPositions[i];
            Light pointLight = pointLightObj.AddComponent<Light>();
            pointLight.type = LightType.Point;
            pointLight.color = new Color(0.6f, 0.7f, 0.9f, 0.3f); // Cool blue-white light
            pointLight.intensity = 0.8f;
            pointLight.range = 8f;
        }

        // Debug.Log("Courtyard prefab created with " + courtyard.transform.childCount + " child objects");
    }

    void CreateStoneFloorGrid(GameObject courtyard)
    {
        // Create grid pattern on the floor
        Material stoneGrid = new Material(Shader.Find("Standard"));
        stoneGrid.name = "StoneGridMaterial";
        stoneGrid.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        stoneGrid.SetFloat("_Metallic", 0.1f);
        stoneGrid.SetFloat("_Glossiness", 0.2f);

        // Create stone grid texture
        Texture2D gridTexture = new Texture2D(512, 512);
        gridTexture.filterMode = FilterMode.Bilinear;

        // Fill with stone color
        for (int y = 0; y < gridTexture.height; y++)
        {
            for (int x = 0; x < gridTexture.width; x++)
            {
                gridTexture.SetPixel(x, y, new Color(0.7f, 0.7f, 0.7f));
            }
        }

        // Add grid lines
        Color gridColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        int gridSize = 32; // Grid cell size

        // Vertical lines
        for (int x = 0; x < gridTexture.width; x += gridSize)
        {
            for (int y = 0; y < gridTexture.height; y++)
            {
                gridTexture.SetPixel(x, y, gridColor);
            }
        }

        // Horizontal lines
        for (int y = 0; y < gridTexture.height; y += gridSize)
        {
            for (int x = 0; x < gridTexture.width; x++)
            {
                gridTexture.SetPixel(x, y, gridColor);
            }
        }

        gridTexture.Apply();
        stoneGrid.mainTexture = gridTexture;

        // Apply to ground
        GameObject ground = GameObject.Find("Courtyard/Ground");
        if (ground != null)
        {
            ground.GetComponent<MeshRenderer>().material = stoneGrid;
        }
    }

    Material CreateNightSkySkybox()
    {
        // Create a simple night sky material
        Material skyboxMaterial = new Material(Shader.Find("Skybox/Cubemap"));
        skyboxMaterial.name = "NightSkySkybox";

        // In a real implementation, you would load a proper night sky cubemap
        // For now, we'll create a simple gradient
        Texture2D skyGradient = new Texture2D(256, 256);
        for (int y = 0; y < skyGradient.height; y++)
        {
            for (int x = 0; x < skyGradient.width; x++)
            {
                float gradient = (float)y / skyGradient.height;
                Color topColor = new Color(0.1f, 0.2f, 0.4f); // Deep blue
                Color bottomColor = new Color(0.8f, 0.4f, 0.2f); // Orange horizon
                Color lerpedColor = Color.Lerp(topColor, bottomColor, gradient);
                skyGradient.SetPixel(x, y, lerpedColor);
            }
        }
        skyGradient.Apply();

        // Note: This would need to be converted to a cubemap for proper skybox usage
        // skyboxMaterial.SetTexture("_Tex", skyGradient);

        return skyboxMaterial;
    }
}