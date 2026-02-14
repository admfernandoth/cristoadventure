using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CreateAllPhase1_1Assets : MonoBehaviour
{
    [MenuItem("Cristo/Create Phase 1.1 Assets")]
    static void CreateAllAssets()
    {
        Debug.Log("Starting creation of Phase 1.1 assets...");

        // Create all materials first
        CreateBasicMaterials();

        // Create all prefabs
        CreateBasilicaInteriorPrefab();
        CreateNativityCavePrefab();
        CreateCourtyardPrefab();
        CreateLightingSetupPrefab();

        Debug.Log("All Phase 1.1 assets created successfully!");
    }

    static void CreateBasicMaterials()
    {
        // Create materials folder if it doesn't exist
        string materialsPath = "Assets/Art/Phase1_1/Materials";
        if (!AssetDatabase.IsValidFolder(materialsPath))
        {
            string parentPath = "Assets/Art/Phase1_1";
            string materialsFolder = AssetDatabase.CreateFolder(parentPath, "Materials");
            Debug.Log("Created Materials folder: " + materialsFolder);
        }

        // Create Stone Material
        Material stoneMat = new Material(Shader.Find("Standard"));
        stoneMat.name = "StoneMaterial";
        stoneMat.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        stoneMat.SetFloat("_Metallic", 0.1f);
        stoneMat.SetFloat("_Glossiness", 0.3f);
        AssetDatabase.CreateAsset(stoneMat, materialsPath + "/StoneMaterial.mat");

        // Create Wood Material
        Material woodMat = new Material(Shader.Find("Standard"));
        woodMat.name = "WoodMaterial";
        woodMat.color = new Color(0.45f, 0.25f, 0.1f, 1f);
        woodMat.SetFloat("_Metallic", 0f);
        woodMat.SetFloat("_Glossiness", 0.4f);
        AssetDatabase.CreateAsset(woodMat, materialsPath + "/WoodMaterial.mat");

        // Create Gold Material
        Material goldMat = new Material(Shader.Find("Standard"));
        goldMat.name = "GoldMaterial";
        goldMat.color = new Color(1f, 0.84f, 0f, 1f);
        goldMat.SetFloat("_Metallic", 1f);
        goldMat.SetFloat("_Glossiness", 0.8f);
        AssetDatabase.CreateAsset(goldMat, materialsPath + "/GoldMaterial.mat");

        // Create Silver Material
        Material silverMat = new Material(Shader.Find("Standard"));
        silverMat.name = "SilverMaterial";
        silverMat.color = new Color(0.9f, 0.9f, 0.95f, 1f);
        silverMat.SetFloat("_Metallic", 1f);
        silverMat.SetFloat("_Glossiness", 0.9f);
        AssetDatabase.CreateAsset(silverMat, materialsPath + "/SilverMaterial.mat");

        Debug.Log("Basic materials created.");
    }

    static void CreateBasilicaInteriorPrefab()
    {
        string prefabsPath = "Assets/Art/Phase1_1/Prefabs";
        GameObject basilica = CreateBasilicaInteriorObject();
        string prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabsPath + "/BasilicaInterior.prefab");
        PrefabUtility.SaveAsPrefabAsset(basilica, prefabPath);
        GameObject.DestroyImmediate(basilica);
        Debug.Log("BasilicaInterior prefab created.");
    }

    static GameObject CreateBasilicaInteriorObject()
    {
        GameObject basilica = new GameObject("BasilicaInterior");
        basilica.tag = "Environment";

        // Main nave
        GameObject nave = GameObject.CreatePrimitive(PrimitiveType.Cube);
        nave.name = "MainNave";
        nave.transform.SetParent(basilica.transform);
        nave.transform.localScale = new Vector3(20f, 5f, 10f);
        nave.transform.position = Vector3.zero;
        Destroy(nave.GetComponent<BoxCollider>());

        // Stone floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "StoneFloor";
        floor.transform.SetParent(basilica.transform);
        floor.transform.localScale = new Vector3(20f, 0.5f, 10f);
        floor.transform.position = new Vector3(0f, -2.25f, 0f);
        floor.tag = "Ground";

        // Columns
        for (int i = 0; i < 8; i++)
        {
            // Left columns
            GameObject leftCol = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leftCol.name = $"Column_Left_{i + 1}";
            leftCol.transform.SetParent(basilica.transform);
            leftCol.transform.localScale = new Vector3(0.5f, 4f, 0.5f);
            leftCol.transform.position = new Vector3(-10f + (i * 2.5f), 2f, -5f);
            Destroy(leftCol.GetComponent<CapsuleCollider>());

            // Right columns
            GameObject rightCol = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rightCol.name = $"Column_Right_{i + 1}";
            rightCol.transform.SetParent(basilica.transform);
            rightCol.transform.localScale = new Vector3(0.5f, 4f, 0.5f);
            rightCol.transform.position = new Vector3(-10f + (i * 2.5f), 2f, 5f);
            Destroy(rightCol.GetComponent<CapsuleCollider>());
        }

        // Ceiling arches
        for (int i = 0; i < 10; i++)
        {
            GameObject arch = GameObject.CreatePrimitive(PrimitiveType.Cube);
            arch.name = $"CeilingArch_{i + 1}";
            arch.transform.SetParent(basilica.transform);
            arch.transform.localScale = new Vector3(2f, 1f, 10f);
            arch.transform.position = new Vector3(-10f + (i * 2f), 5.5f, 0f);
            Destroy(arch.GetComponent<BoxCollider>());
        }

        // Lighting
        CreateWarmLights(basilica, new Vector3[] {
            new Vector3(-5f, 3f, -3f),
            new Vector3(5f, 3f, 3f)
        });

        return basilica;
    }

    static void CreateNativityCavePrefab()
    {
        string prefabsPath = "Assets/Art/Phase1_1/Prefabs";
        GameObject cave = CreateNativityCaveObject();
        string prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabsPath + "/NativityCave.prefab");
        PrefabUtility.SaveAsPrefabAsset(cave, prefabPath);
        GameObject.DestroyImmediate(cave);
        Debug.Log("NativityCave prefab created.");
    }

    static GameObject CreateNativityCaveObject()
    {
        GameObject cave = new GameObject("NativityCave");
        cave.tag = "Environment";

        // Cave room
        GameObject caveRoom = GameObject.CreatePrimitive(PrimitiveType.Cube);
        caveRoom.name = "CaveRoom";
        caveRoom.transform.SetParent(cave.transform);
        caveRoom.transform.localScale = new Vector3(5f, 3f, 5f);
        caveRoom.transform.position = Vector3.zero;
        Destroy(caveRoom.GetComponent<BoxCollider>());

        // Walls
        GameObject[] walls = new GameObject[4];
        Vector3[] wallPositions = {
            new Vector3(0f, 0f, -2.5f),   // Back
            new Vector3(-2.5f, 0f, 0f),   // Left
            new Vector3(2.5f, 0f, 0f),    // Right
            new Vector3(0f, 1.75f, 0f)    // Top
        };
        Vector3[] wallScales = {
            new Vector3(5f, 3f, 0.5f),
            new Vector3(0.5f, 3f, 5f),
            new Vector3(0.5f, 3f, 5f),
            new Vector3(5f, 0.5f, 5f)
        };

        for (int i = 0; i < 4; i++)
        {
            walls[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            walls[i].name = i == 3 ? "Ceiling" : (i == 0 ? "BackWall" : (i == 1 ? "LeftWall" : "RightWall"));
            walls[i].transform.SetParent(cave.transform);
            walls[i].transform.localScale = wallScales[i];
            walls[i].transform.position = wallPositions[i];
            Destroy(walls[i].GetComponent<BoxCollider>());
        }

        // Star on floor
        GameObject star = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        star.name = "StarOnFloor";
        star.transform.SetParent(cave.transform);
        star.transform.localScale = new Vector3(2f, 0.1f, 2f);
        star.transform.position = new Vector3(0f, -1.5f, 0f);
        Destroy(star.GetComponent<CapsuleCollider>());

        // Manger
        GameObject manger = GameObject.CreatePrimitive(PrimitiveType.Cube);
        manger.name = "Manger";
        manger.transform.SetParent(cave.transform);
        manger.transform.localScale = new Vector3(2f, 0.5f, 1f);
        manger.transform.position = new Vector3(0f, -1.4f, -1.5f);

        // Atmospheric lighting
        CreateAtmosphericLights(cave);

        return cave;
    }

    static void CreateCourtyardPrefab()
    {
        string prefabsPath = "Assets/Art/Phase1_1/Prefabs";
        GameObject courtyard = CreateCourtyardObject();
        string prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabsPath + "/Courtyard.prefab");
        PrefabUtility.SaveAsPrefabAsset(courtyard, prefabPath);
        GameObject.DestroyImmediate(courtyard);
        Debug.Log("Courtyard prefab created.");
    }

    static GameObject CreateCourtyardObject()
    {
        GameObject courtyard = new GameObject("Courtyard");
        courtyard.tag = "Environment";

        // Ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.SetParent(courtyard.transform);
        ground.transform.localScale = new Vector3(15f, 0.1f, 15f);
        ground.transform.position = new Vector3(0f, -0.05f, 0f);
        ground.tag = "Ground";

        // Moonlight
        GameObject moonLight = new GameObject("MoonLight");
        moonLight.transform.SetParent(courtyard.transform);
        moonLight.transform.position = new Vector3(10f, 10f, 10f);
        Light directionalLight = moonLight.AddComponent<Light>();
        directionalLight.type = LightType.Directional;
        directionalLight.color = new Color(0.4f, 0.5f, 0.8f, 1f);
        directionalLight.intensity = 0.5f;
        directionalLight.transform.rotation = Quaternion.Euler(45f, 30f, 0f);

        return courtyard;
    }

    static void CreateLightingSetupPrefab()
    {
        string prefabsPath = "Assets/Art/Phase1_1/Prefabs";
        GameObject lighting = CreateLightingSetupObject();
        string prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabsPath + "/LightingSetup.prefab");
        PrefabUtility.SaveAsPrefabAsset(lighting, prefabPath);
        GameObject.DestroyImmediate(lighting);
        Debug.Log("LightingSetup prefab created.");
    }

    static GameObject CreateLightingSetupObject()
    {
        GameObject lighting = new GameObject("LightingSetup");

        // Main directional light
        GameObject mainLight = new GameObject("MainDirectionalLight");
        mainLight.transform.SetParent(lighting.transform);
        mainLight.transform.rotation = Quaternion.Euler(45f, 30f, 0f);
        Light dirLight = mainLight.AddComponent<Light>();
        dirLight.type = LightType.Directional;
        dirLight.color = new Color(1f, 0.9f, 0.7f, 1f);
        dirLight.intensity = 1f;

        return lighting;
    }

    static void CreateWarmLights(GameObject parent, Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject lightObj = new GameObject($"WarmLight_{i + 1}");
            lightObj.transform.SetParent(parent.transform);
            lightObj.transform.position = positions[i];
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1f, 0.8f, 0.4f, 1f);
            light.intensity = 2f;
            light.range = 10f;
        }
    }

    static void CreateAtmosphericLights(GameObject parent)
    {
        // Spotlight
        GameObject spotlight = new GameObject("AtmosphericSpotlight");
        spotlight.transform.SetParent(parent.transform);
        spotlight.transform.position = new Vector3(0f, 2f, 0f);
        Light spot = spotlight.AddComponent<Light>();
        spot.type = LightType.Spot;
        spot.color = new Color(1f, 0.7f, 0.3f, 1f);
        spot.intensity = 1.5f;
        spot.range = 8f;
        spot.spotAngle = 45f;

        // Ambient fill light
        GameObject ambient = new GameObject("AmbientLight");
        ambient.transform.SetParent(parent.transform);
        ambient.transform.position = new Vector3(0f, 1f, 0f);
        Light ambLight = ambient.AddComponent<Light>();
        ambLight.type = LightType.Point;
        ambLight.color = new Color(0.8f, 0.6f, 0.3f, 0.5f);
        ambLight.intensity = 0.5f;
        ambLight.range = 6f;
    }
}