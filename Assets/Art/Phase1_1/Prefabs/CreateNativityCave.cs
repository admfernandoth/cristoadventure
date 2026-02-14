using UnityEngine;

public class CreateNativityCave : MonoBehaviour
{
    [Header("Nativity Cave Creator")]
    public GameObject caveParent;

    void Start()
    {
        CreateNativityCavePrefab();
    }

    void CreateNativityCavePrefab()
    {
        // Create parent object for the cave
        GameObject cave = new GameObject("NativityCave");
        cave.tag = "Environment";

        // Create small cave room (scale 5x3x5)
        GameObject caveRoom = GameObject.CreatePrimitive(PrimitiveType.Cube);
        caveRoom.name = "CaveRoom";
        caveRoom.transform.SetParent(cave.transform);
        caveRoom.transform.localScale = new Vector3(5f, 3f, 5f); // Scale 5x3x5
        caveRoom.transform.position = Vector3.zero;

        // Apply rough stone material to cave walls
        MeshRenderer caveRenderer = caveRoom.GetComponent<MeshRenderer>();
        caveRenderer.material = CreateRoughStoneMaterial();

        // Remove collider for interior
        Destroy(caveRoom.GetComponent<BoxCollider>());

        // Create StarOnFloor
        GameObject star = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        star.name = "StarOnFloor";
        star.transform.SetParent(cave.transform);
        star.transform.localScale = new Vector3(2f, 0.1f, 2f);
        star.transform.position = new Vector3(0f, -1.5f, 0f);

        // Create silver material for star
        MeshRenderer starRenderer = star.GetComponent<MeshRenderer>();
        starRenderer.material = BasicMaterialsCreation.CreateSilverMaterial();

        // Add 14-point star texture (would need custom shader for this)
        // Placeholder: Just use a simple pattern
        Texture2D starTexture = new Texture2D(256, 256);
        starTexture.SetPixel(128, 128, Color.yellow);
        starTexture.Apply();
        starRenderer.material.mainTexture = starTexture;

        Destroy(star.GetComponent<CapsuleCollider>());

        // Create Manger (wood-colored box)
        GameObject manger = GameObject.CreatePrimitive(PrimitiveType.Cube);
        manger.name = "Manger";
        manger.transform.SetParent(cave.transform);
        manger.transform.localScale = new Vector3(2f, 0.5f, 1f);
        manger.transform.position = new Vector3(0f, -1.4f, -1.5f);
        manger.GetComponent<MeshRenderer>().material = BasicMaterialsCreation.CreateWoodMaterial();

        // Add collision to manger
        BoxCollider mangerCollider = manger.AddComponent<BoxCollider>();
        mangerCollider.isTrigger = false;

        // Create rough stone walls (procedural noise material)
        CreateCaveWalls(cave);

        // Add atmospheric lighting
        GameObject spotlight = new GameObject("AtmosphericSpotlight");
        spotlight.transform.SetParent(cave.transform);
        spotlight.transform.position = new Vector3(0f, 2f, 0f);
        Light spotLight = spotlight.AddComponent<Light>();
        spotLight.type = LightType.Spot;
        spotLight.color = new Color(1f, 0.7f, 0.3f, 1f); // Warm golden light
        spotLight.intensity = 1.5f;
        spotLight.range = 8f;
        spotLight.spotAngle = 45f;

        // Add ambient fill light
        GameObject ambientLight = new GameObject("AmbientLight");
        ambientLight.transform.SetParent(cave.transform);
        ambientLight.transform.position = new Vector3(0f, 1f, 0f);
        Light ambient = ambientLight.AddComponent<Light>();
        ambient.type = LightType.Point;
        ambient.color = new Color(0.8f, 0.6f, 0.3f, 0.5f); // Subtle warm light
        ambient.intensity = 0.5f;
        ambient.range = 6f;

        // Debug.Log("NativityCave prefab created with " + cave.transform.childCount + " child objects");
    }

    void CreateCaveWalls(GameObject cave)
    {
        // Create back wall
        GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backWall.name = "BackWall";
        backWall.transform.SetParent(cave.transform);
        backWall.transform.localScale = new Vector3(5f, 3f, 0.5f);
        backWall.transform.position = new Vector3(0f, 0f, -2.5f);
        backWall.GetComponent<MeshRenderer>().material = CreateRoughStoneMaterial();
        Destroy(backWall.GetComponent<BoxCollider>());

        // Create left wall
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftWall.name = "LeftWall";
        leftWall.transform.SetParent(cave.transform);
        leftWall.transform.localScale = new Vector3(0.5f, 3f, 5f);
        leftWall.transform.position = new Vector3(-2.5f, 0f, 0f);
        leftWall.GetComponent<MeshRenderer>().material = CreateRoughStoneMaterial();
        Destroy(leftWall.GetComponent<BoxCollider>());

        // Create right wall
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightWall.name = "RightWall";
        rightWall.transform.SetParent(cave.transform);
        rightWall.transform.localScale = new Vector3(0.5f, 3f, 5f);
        rightWall.transform.position = new Vector3(2.5f, 0f, 0f);
        rightWall.GetComponent<MeshRenderer>().material = CreateRoughStoneMaterial();
        Destroy(rightWall.GetComponent<BoxCollider>());

        // Create ceiling
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.name = "Ceiling";
        ceiling.transform.SetParent(cave.transform);
        ceiling.transform.localScale = new Vector3(5f, 0.5f, 5f);
        ceiling.transform.position = new Vector3(0f, 1.75f, 0f);
        ceiling.GetComponent<MeshRenderer>().material = CreateRoughStoneMaterial();
        Destroy(ceiling.GetComponent<BoxCollider>());
    }

    Material CreateRoughStoneMaterial()
    {
        Material roughStone = new Material(Shader.Find("Standard"));
        roughStone.name = "RoughStoneMaterial";
        roughStone.color = new Color(0.6f, 0.5f, 0.4f, 1f);
        roughStone.SetFloat("_Metallic", 0f);
        roughStone.SetFloat("_Glossiness", 0.1f);

        // Create more aggressive noise texture for rough stone
        Texture2D roughTexture = new Texture2D(256, 256);
        for (int y = 0; y < roughTexture.height; y++)
        {
            for (int x = 0; x < roughTexture.width; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.05f, y * 0.05f) * 0.3f;
                roughTexture.SetPixel(x, y, new Color(0.6f - noise, 0.5f - noise, 0.4f - noise));
            }
        }
        roughTexture.Apply();
        roughStone.mainTexture = roughTexture;

        return roughStone;
    }
}