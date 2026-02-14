using UnityEngine;

public class BasicMaterialsCreation : MonoBehaviour
{
    [Header("Basic Materials Creation Script")]
    [Space(10)]

    // Stone Material
    public static Material CreateStoneMaterial()
    {
        Material stoneMaterial = new Material(Shader.Find("Standard"));
        stoneMaterial.name = "StoneMaterial";
        stoneMaterial.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        stoneMaterial.SetFloat("_Metallic", 0.1f);
        stoneMaterial.SetFloat("_Glossiness", 0.3f);

        // Add subtle noise texture (placeholder for procedural generation)
        Texture2D noiseTexture = new Texture2D(256, 256);
        for (int y = 0; y < noiseTexture.height; y++)
        {
            for (int x = 0; x < noiseTexture.width; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f) * 0.1f;
                noiseTexture.SetPixel(x, y, new Color(noise, noise, noise));
            }
        }
        noiseTexture.Apply();
        stoneMaterial.mainTexture = noiseTexture;

        return stoneMaterial;
    }

    // Wood Material
    public static Material CreateWoodMaterial()
    {
        Material woodMaterial = new Material(Shader.Find("Standard"));
        woodMaterial.name = "WoodMaterial";
        woodMaterial.color = new Color(0.45f, 0.25f, 0.1f, 1f);
        woodMaterial.SetFloat("_Metallic", 0f);
        woodMaterial.SetFloat("_Glossiness", 0.4f);

        // Simple wood grain texture
        Texture2D woodTexture = new Texture2D(256, 256);
        for (int y = 0; y < woodTexture.height; y++)
        {
            float grain = Mathf.Sin(y * 0.2f) * 0.1f + 0.5f;
            for (int x = 0; x < woodTexture.width; x++)
            {
                woodTexture.SetPixel(x, y, new Color(grain * 0.8f, grain * 0.5f, grain * 0.2f));
            }
        }
        woodTexture.Apply();
        woodMaterial.mainTexture = woodTexture;

        return woodMaterial;
    }

    // Gold Material
    public static Material CreateGoldMaterial()
    {
        Material goldMaterial = new Material(Shader.Find("Standard"));
        goldMaterial.name = "GoldMaterial";
        goldMaterial.color = new Color(1f, 0.84f, 0f, 1f);
        goldMaterial.SetFloat("_Metallic", 1f);
        goldMaterial.SetFloat("_Glossiness", 0.8f);

        // Metallic gold texture
        Texture2D goldTexture = new Texture2D(256, 256);
        for (int y = 0; y < goldTexture.height; y++)
        {
            for (int x = 0; x < goldTexture.width; x++)
            {
                goldTexture.SetPixel(x, y, Color.yellow);
            }
        }
        goldTexture.Apply();
        goldMaterial.mainTexture = goldTexture;

        return goldMaterial;
    }

    // Silver Material
    public static Material CreateSilverMaterial()
    {
        Material silverMaterial = new Material(Shader.Find("Standard"));
        silverMaterial.name = "SilverMaterial";
        silverMaterial.color = new Color(0.9f, 0.9f, 0.95f, 1f);
        silverMaterial.SetFloat("_Metallic", 1f);
        silverMaterial.SetFloat("_Glossiness", 0.9f);

        // Metallic silver texture
        Texture2D silverTexture = new Texture2D(256, 256);
        for (int y = 0; y < silverTexture.height; y++)
        {
            for (int x = 0; x < silverTexture.width; x++)
            {
                silverTexture.SetPixel(x, y, Color.white);
            }
        }
        silverTexture.Apply();
        silverMaterial.mainTexture = silverTexture;

        return silverMaterial;
    }
}