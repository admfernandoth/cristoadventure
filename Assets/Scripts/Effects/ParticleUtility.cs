using UnityEngine;

public class ParticleUtility : MonoBehaviour
{
    [Header("Sacred Light Particle Settings")]
    [SerializeField] private Color sacredLightColor = new Color(1f, 0.9f, 0.7f, 1f);
    [SerializeField] private float sacredLightSize = 0.1f;
    [SerializeField] private float sacredLightSpeed = 0.5f;
    [SerializeField] private float sacredLifeTime = 3f;

    [Header("Pickup Burst Settings")]
    [SerializeField] private Color pickupColor = Color.yellow;
    [SerializeField] private float pickupSize = 0.05f;
    [SerializeField] private float pickupSpeed = 3f;
    [SerializeField] private float pickupLifeTime = 1f;

    [Header("Success Flash Settings")]
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private float successSize = 0.2f;
    [SerializeField] private int particleCount = 20;

    [Header("Button Press Settings")]
    [SerializeField] private Color buttonPressColor = new Color(0.8f, 0.8f, 1f, 1f);
    [SerializeField] private float buttonPressSize = 0.02f;
    [SerializeField] private int buttonPressCount = 10;

    // Create a sacred light particle system
    public static ParticleSystem CreateSacredLightParticle()
    {
        GameObject particleObj = new GameObject("SacredLightParticles");
        ParticleSystem particles = particleObj.AddComponent<ParticleSystem>();

        // Main module
        var main = particles.main;
        main.duration = 2f;
        main.startLifetime = 3f;
        main.startSpeed = 0.5f;
        main.startSize = 0.1f;
        main.particleSystem.loop = false;
        main.gravityModifier = -0.1f;

        // Emission module
        var emission = particles.emission;
        emission.rateOverTime = 20f;

        // Shape module
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.radius = 0.5f;
        shape.angle = 25f;

        // Color module
        var color = particles.color;
        color.color = new Color(1f, 0.9f, 0.7f, 1f);

        // Renderer
        var renderer = particles.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        return particles;
    }

    // Create a pickup burst particle system
    public static ParticleSystem CreatePickupBurstParticle()
    {
        GameObject particleObj = new GameObject("PickupBurst");
        ParticleSystem particles = particleObj.AddComponent<ParticleSystem>();

        // Main module
        var main = particles.main;
        main.duration = 1f;
        main.startLifetime = 1f;
        main.startSpeed = 3f;
        main.startSize = 0.05f;
        main.particleSystem.loop = false;

        // Emission module
        var emission = particles.emission;
        emission.rateOverTime = 50f;

        // Shape module
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f;

        // Color module
        var color = particles.color;
        color.color = new Color(1f, 1f, 0f, 1f);

        // Renderer
        var renderer = particles.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        return particles;
    }

    // Create a success flash particle system
    public static ParticleSystem CreateSuccessFlashParticle()
    {
        GameObject particleObj = new GameObject("SuccessFlash");
        ParticleSystem particles = particleObj.AddComponent<ParticleSystem>();

        // Main module
        var main = particles.main;
        main.duration = 0.5f;
        main.startLifetime = 0.5f;
        main.startSpeed = 2f;
        main.startSize = 0.2f;
        main.particleSystem.loop = false;

        // Emission module
        var emission = particles.emission;
        emission.rateOverTime = 100f;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, 20)
        });

        // Shape module
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;

        // Color module
        var color = particles.color;
        color.color = new Color(0f, 1f, 0f, 1f);

        // Renderer
        var renderer = particles.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        return particles;
    }

    // Create a button press particle system
    public static ParticleSystem CreateButtonPressParticle()
    {
        GameObject particleObj = new GameObject("ButtonPress");
        ParticleSystem particles = particleObj.AddComponent<ParticleSystem>();

        // Main module
        var main = particles.main;
        main.duration = 0.3f;
        main.startLifetime = 0.3f;
        main.startSpeed = 1f;
        main.startSize = 0.02f;
        main.particleSystem.loop = false;

        // Emission module
        var emission = particles.emission;
        emission.rateOverTime = 50f;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, 10)
        });

        // Shape module
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;

        // Color module
        var color = particles.color;
        color.color = new Color(0.8f, 0.8f, 1f, 1f);

        // Renderer
        var renderer = particles.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        return particles;
    }

    // Create a generic sparkle particle
    public static ParticleSystem CreateSparkleParticle(Color color, float size, float speed)
    {
        GameObject particleObj = new GameObject("Sparkle");
        ParticleSystem particles = particleObj.AddComponent<ParticleSystem>();

        // Main module
        var main = particles.main;
        main.duration = 1f;
        main.startLifetime = 1f;
        main.startSpeed = speed;
        main.startSize = size;
        main.particleSystem.loop = false;

        // Emission module
        var emission = particles.emission;
        emission.rateOverTime = 20f;

        // Shape module
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Point;

        // Color module
        var colorModule = particles.color;
        colorModule.color = color;

        // Renderer
        var renderer = particles.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        return particles;
    }

    // Method to create all particle prefabs at once
    public static void CreateAllParticlePrefabs()
    {
        // Create Sacred Light Prefab
        GameObject sacredLightObj = new GameObject("SacredLight_Particle");
        ParticleSystem sacredLight = CreateSacredLightParticle();
        sacredLight.transform.SetParent(sacredLightObj.transform);
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(sacredLightObj, "Assets/Prefabs/Particles/SacredLight.prefab");
        DestroyImmediate(sacredLightObj);

        // Create Pickup Burst Prefab
        GameObject pickupObj = new GameObject("PickupBurst_Particle");
        ParticleSystem pickup = CreatePickupBurstParticle();
        pickup.transform.SetParent(pickupObj.transform);
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(pickupObj, "Assets/Prefabs/Particles/PickupBurst.prefab");
        DestroyImmediate(pickupObj);

        // Create Success Flash Prefab
        GameObject successObj = new GameObject("SuccessFlash_Particle");
        ParticleSystem success = CreateSuccessFlashParticle();
        success.transform.SetParent(successObj.transform);
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(successObj, "Assets/Prefabs/Particles/SuccessFlash.prefab");
        DestroyImmediate(successObj);

        // Create Button Press Prefab
        GameObject buttonObj = new GameObject("ButtonPress_Particle");
        ParticleSystem button = CreateButtonPressParticle();
        button.transform.SetParent(buttonObj.transform);
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(buttonObj, "Assets/Prefabs/Particles/ButtonPress.prefab");
        DestroyImmediate(buttonObj);
    }
}