using UnityEngine;

public class CreateBasilicaInterior : MonoBehaviour
{
    [Header("Basilica Interior Creator")]
    public GameObject basilicaParent;

    void Start()
    {
        CreateBasilicaInteriorPrefab();
    }

    void CreateBasilicaInteriorPrefab()
    {
        // Create parent object for the basilica
        GameObject basilica = new GameObject("BasilicaInterior");
        basilica.tag = "Environment";

        // Create main nave (long corridor)
        GameObject nave = GameObject.CreatePrimitive(PrimitiveType.Cube);
        nave.name = "MainNave";
        nave.transform.SetParent(basilica.transform);
        nave.transform.localScale = new Vector3(20f, 5f, 10f); // Scale 20x5x10
        nave.transform.position = Vector3.zero;

        // Apply stone material to nave
        MeshRenderer naveRenderer = nave.GetComponent<MeshRenderer>();
        naveRenderer.material = BasicMaterialsCreation.CreateStoneMaterial();

        // Remove collider for interior
        Destroy(nave.GetComponent<BoxCollider>());

        // Create columns along walls (8 on each side)
        for (int i = 0; i < 8; i++)
        {
            // Left side columns
            GameObject leftColumn = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leftColumn.name = $"Column_Left_{i + 1}";
            leftColumn.transform.SetParent(basilica.transform);
            leftColumn.transform.localScale = new Vector3(0.5f, 4f, 0.5f);
            leftColumn.transform.position = new Vector3(-10f + (i * 2.5f), 2f, -5f);
            leftColumn.GetComponent<MeshRenderer>().material = BasicMaterialsCreation.CreateStoneMaterial();
            Destroy(leftColumn.GetComponent<CapsuleCollider>());

            // Right side columns
            GameObject rightColumn = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rightColumn.name = $"Column_Right_{i + 1}";
            rightColumn.transform.SetParent(basilica.transform);
            rightColumn.transform.localScale = new Vector3(0.5f, 4f, 0.5f);
            rightColumn.transform.position = new Vector3(-10f + (i * 2.5f), 2f, 5f);
            rightColumn.GetComponent<MeshRenderer>().material = BasicMaterialsCreation.CreateStoneMaterial();
            Destroy(rightColumn.GetComponent<CapsuleCollider>());
        }

        // Create arched ceiling using scaled cubes
        for (int i = 0; i < 10; i++)
        {
            GameObject ceilingArch = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ceilingArch.name = $"CeilingArch_{i + 1}";
            ceilingArch.transform.SetParent(basilica.transform);
            ceilingArch.transform.localScale = new Vector3(2f, 1f, 10f);
            ceilingArch.transform.position = new Vector3(-10f + (i * 2f), 5.5f, 0f);
            ceilingArch.GetComponent<MeshRenderer>().material = BasicMaterialsCreation.CreateStoneMaterial();
            Destroy(ceilingArch.GetComponent<BoxCollider>());
        }

        // Add warm lighting
        GameObject warmLight1 = new GameObject("WarmLight1");
        warmLight1.transform.SetParent(basilica.transform);
        warmLight1.transform.position = new Vector3(-5f, 3f, -3f);
        Light pointLight1 = warmLight1.AddComponent<Light>();
        pointLight1.type = LightType.Point;
        pointLight1.color = new Color(1f, 0.8f, 0.4f, 1f); // Gold color
        pointLight1.intensity = 2f;
        pointLight1.range = 10f;

        GameObject warmLight2 = new GameObject("WarmLight2");
        warmLight2.transform.SetParent(basilica.transform);
        warmLight2.transform.position = new Vector3(5f, 3f, 3f);
        Light pointLight2 = warmLight2.AddComponent<Light>();
        pointLight2.type = LightType.Point;
        pointLight2.color = new Color(1f, 0.8f, 0.4f, 1f); // Gold color
        pointLight2.intensity = 2f;
        pointLight2.range = 10f;

        // Create stone floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "StoneFloor";
        floor.transform.SetParent(basilica.transform);
        floor.transform.localScale = new Vector3(20f, 0.5f, 10f);
        floor.transform.position = new Vector3(0f, -2.25f, 0f);
        floor.GetComponent<MeshRenderer>().material = BasicMaterialsCreation.CreateStoneMaterial();
        floor.tag = "Ground";

        // Save as prefab (this would need to be handled differently in Unity Editor)
        // Debug.Log("BasilicaInterior prefab created with " + basilica.transform.childCount + " child objects");
    }
}