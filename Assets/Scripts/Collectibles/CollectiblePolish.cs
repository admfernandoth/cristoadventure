using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class CollectiblePolish : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatAmount = 0.5f;
    [SerializeField] private float rotateSpeed = 50f;
    [SerializeField] private float glowPulseSpeed = 2f;

    [Header("Pickup Effects")]
    [SerializeField] private ParticleSystem pickupEffect;
    [SerializeField] private float pickupDuration = 1f;
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private float pickupVolume = 0.7f;

    [Header("Events")]
    public UnityEvent onPickup;
    public UnityEvent onCollectibleHover;
    public UnityEvent onCollectibleLeave;

    private Vector3 originalPosition;
    private float floatTimer = 0f;
    private bool isPickedUp = false;
    private bool isHovering = false;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isPickedUp) return;

        // Floating animation
        floatTimer += Time.deltaTime * floatSpeed;
        float yOffset = Mathf.Sin(floatTimer) * floatAmount;
        transform.position = originalPosition + Vector3.up * yOffset;

        // Rotation
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);

        // Glow pulse (if sprite renderer exists)
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float pulse = (Mathf.Sin(floatTimer * glowPulseSpeed) + 1f) * 0.5f;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.8f + pulse * 0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPickedUp)
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        if (isPickedUp) return;

        isPickedUp = true;

        // Play pickup effect
        if (pickupEffect != null)
        {
            pickupEffect.transform.position = transform.position;
            pickupEffect.Play();
        }

        // Play pickup sound
        if (pickupClip != null)
        {
            AudioSource.PlayClipAtPoint(pickupClip, transform.position, pickupVolume);
        }

        // Visual polish effect
        TriggerPickupVisual();

        // Trigger events
        onPickup?.Invoke();

        // Start pickup sequence
        StartCoroutine(PickupSequence());
    }

    private IEnumerator PickupSequence()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsed = 0f;

        while (elapsed < pickupDuration)
        {
            float t = elapsed / pickupDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 2f, t * 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Destroy collectible
        Destroy(gameObject);
    }

    private void TriggerPickupVisual()
    {
        // Get VisualPolish instance
        VisualPolish visualPolish = FindObjectOfType<VisualPolish>();
        if (visualPolish != null)
        {
            visualPolish.PlayPickupEffect(transform.position);
        }
    }

    // Hover detection
    public void OnHoverEnter()
    {
        if (isPickedUp) return;
        isHovering = true;
        onCollectibleHover?.Invoke();
        TriggerHoverEffect();
    }

    public void OnHoverExit()
    {
        if (isPickedUp) return;
        isHovering = false;
        onCollectibleLeave?.Invoke();
        ResetHoverEffect();
    }

    private void TriggerHoverEffect()
    {
        // Scale up slightly
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 1.2f;

        // Add glow effect
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
        }
    }

    private void ResetHoverEffect()
    {
        // Return to original scale
        transform.localScale = Vector3.one;

        // Reset color
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }

    // Special coin collect
    public static void CollectCoin(Vector3 position)
    {
        // Play coin sound
        AudioPolish audioPolish = FindObjectOfType<AudioPolish>();
        if (audioPolish != null)
        {
            audioPolish.PlayCoinSound(position);
        }

        // Create coin effect
        CreateCoinEffect(position);
    }

    private static void CreateCoinEffect(Vector3 position)
    {
        GameObject coinEffect = new GameObject("CoinEffect");
        coinEffect.transform.position = position;

        // Add particle system for coins
        ParticleSystem particles = coinEffect.AddComponent<ParticleSystem>();
        var main = particles.main;
        main.duration = 1f;
        main.startLifetime = 1f;
        main.startSpeed = 2f;
        main.startSize = 0.1f;
        main.particleSystem.loop = false;

        var emission = particles.emission;
        emission.rateOverTime = 50f;

        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.5f;

        var color = particles.color;
        color.color = new Color(1f, 0.9f, 0.3f);

        // Destroy after effect completes
        Destroy(coinEffect, 1f);
    }

    // Special sacred collectible (relics)
    public static void CollectSacredRelic(Vector3 position)
    {
        // Trigger sacred light effect
        VisualPolish visualPolish = FindObjectOfType<VisualPolish>();
        if (visualPolish != null)
        {
            visualPolish.TriggerSacredLight(position);
        }

        // Play special sound
        AudioPolish audioPolish = FindObjectOfType<AudioPolish>();
        if (audioPolish != null)
        {
            audioPolish.PlayCollectibleSound(position);
        }

        // Create sacred effect
        CreateSacredEffect(position);
    }

    private static void CreateSacredEffect(Vector3 position)
    {
        GameObject sacredEffect = new GameObject("SacredEffect");
        sacredEffect.transform.position = position;

        // Add particle system for sacred effect
        ParticleSystem particles = sacredEffect.AddComponent<ParticleSystem>();
        var main = particles.main;
        main.duration = 2f;
        main.startLifetime = 2f;
        main.startSpeed = 1f;
        main.startSize = 0.2f;
        main.particleSystem.loop = false;

        var emission = particles.emission;
        emission.rateOverTime = 30f;

        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.radius = 0.3f;
        shape.angle = 25f;

        var color = particles.color;
        color.color = new Color(1f, 0.9f, 0.7f, 1f);

        var renderer = particles.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        }

        // Destroy after effect completes
        Destroy(sacredEffect, 2f);
    }
}