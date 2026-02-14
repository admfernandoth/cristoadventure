using UnityEngine;
using UnityEngine.Events;

public class AmbientZoneManager : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private string zoneName = "Default";
    [SerializeField] private float transitionDistance = 5f;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool loopAmbient = true;

    [Header("Audio References")]
    [SerializeField] private AudioClip ambientClip;
    [SerializeField] private AudioClip[] additionalLayers;
    [SerializeField] private AudioClip windSound;

    [Header("Events")]
    public UnityEvent onZoneEnter;
    public UnityEvent onZoneExit;

    private bool isInZone = false;
    private Vector3 playerPosition;
    private float lastDistance;

    private void Start()
    {
        playerPosition = GetPlayerPosition();
        lastDistance = Vector3.Distance(transform.position, playerPosition);
    }

    private void Update()
    {
        if (isInZone) return;

        playerPosition = GetPlayerPosition();
        float currentDistance = Vector3.Distance(transform.position, playerPosition);

        // Check if player is entering the zone
        if (currentDistance <= transitionDistance && lastDistance > transitionDistance)
        {
            EnterZone();
        }
        // Check if player is leaving the zone
        else if (currentDistance > transitionDistance && lastDistance <= transitionDistance)
        {
            ExitZone();
        }

        lastDistance = currentDistance;
    }

    private Vector3 GetPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            return player.transform.position;
        }
        return Vector3.zero;
    }

    private void EnterZone()
    {
        isInZone = true;
        onZoneEnter?.Invoke();

        // Set ambient zone through AudioPolish
        AudioPolish audioPolish = FindObjectOfType<AudioPolish>();
        if (audioPolish != null)
        {
            audioPolish.SetAmbientZone(zoneName);
        }

        // Play additional layers if any
        if (additionalLayers != null && additionalLayers.Length > 0)
        {
            foreach (var layer in additionalLayers)
            {
                PlayAmbientLayer(layer, volume * 0.5f); // Lower volume for layers
            }
        }

        // Play wind sound for cave zones
        if (windSound != null && zoneName.ToLower().Contains("cave"))
        {
            PlayAmbientLayer(windSound, volume * 0.3f);
        }
    }

    private void ExitZone()
    {
        isInZone = false;
        onZoneExit?.Invoke();

        // Stop ambient through AudioPolish
        AudioPolish audioPolish = FindObjectOfType<AudioPolish>();
        if (audioPolish != null)
        {
            // Set to default ambient zone
            audioPolish.SetAmbientZone("Default");
        }

        // Stop all layers
        StopAllAmbientLayers();
    }

    private void PlayAmbientLayer(AudioClip clip, float layerVolume)
    {
        // This would need additional implementation for multi-layer ambience
        // For now, we'll just play through the main ambient system
        SoundManager.Instance.PlayAmbient(clip, loopAmbient);
    }

    private void StopAllAmbientLayers()
    {
        SoundManager.Instance.StopAmbient();
    }

    // Gizmos for visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, transitionDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        // Draw zone name
        Gizmos.color = Color.white;
        Gizmos.DrawText(zoneName, transform.position + Vector3.up * 2f);
    }
}

// Extension for Gizmos text drawing
public static class GizmosExtensions
{
    public static void DrawText(string text, Vector3 position)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.Label(position, text);
#endif
    }
}