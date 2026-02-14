using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class VisualPolish : MonoBehaviour
{
    [Header("Sacred Light Effect")]
    [SerializeField] private ParticleSystem sacredLightParticles;
    [SerializeField] private Color lightColor = new Color(1f, 0.9f, 0.7f, 1f);
    [SerializeField] private float particleLifetime = 3f;
    [SerializeField] private float particleSpeed = 0.5f;
    [SerializeField] private float emissionRate = 10f;

    [Header("Interaction Feedback")]
    [SerializeField] private ParticleSystem pickupBurst;
    [SerializeField] private float scaleDuration = 0.3f;
    [SerializeField] private float scaleAmount = 1.2f;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.2f;

    [Header("Camera Transitions")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float zoomDuration = 2f;
    [SerializeField] private float zoomAmount = 1.5f;
    [SerializeField] private float followSmoothTime = 0.3f;

    private Volume volume;
    private Bloom bloom;
    private Camera mainCamera;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    // Event subscriptions
    public UnityEvent onSacredLightTrigger;
    public UnityEvent onPickup;
    public UnityEvent onPuzzleComplete;

    private void Awake()
    {
        mainCamera = Camera.main;
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;

        // Setup volume for bloom effect
        volume = FindObjectOfType<Volume>();
        if (volume.profile.TryGet(out bloom))
        {
            bloom.intensity.value = 0f;
            bloom.threshold.value = 0.8f;
            bloom.color.value = lightColor;
        }

        // Initialize particle systems
        if (sacredLightParticles != null)
        {
            var main = sacredLightParticles.main;
            main.startColor = lightColor;
            main.startLifetime = particleLifetime;
            main.startSpeed = particleSpeed;
        }

        if (pickupBurst != null)
        {
            pickupBurst.gameObject.SetActive(false);
        }
    }

    #region Sacred Light Effect

    public void TriggerSacredLight(Vector3 position)
    {
        if (sacredLightParticles != null)
        {
            sacredLightParticles.transform.position = position;
            sacredLightParticles.Play();

            // Enable bloom effect
            StartCoroutine(AnimateBloom());
        }

        onSacredLightTrigger?.Invoke();
    }

    private IEnumerator AnimateBloom()
    {
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            bloom.intensity.value = Mathf.Lerp(0f, 2f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fade out bloom
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            bloom.intensity.value = Mathf.Lerp(2f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    #region Interaction Feedback

    public void PlayPickupEffect(Vector3 position)
    {
        if (pickupBurst != null)
        {
            pickupBurst.transform.position = position;
            pickupBurst.gameObject.SetActive(true);
            pickupBurst.Play();

            // Auto-disable after particles finish
            StartCoroutine(DisablePickupBurst());
        }

        onPickup?.Invoke();
    }

    private IEnumerator DisablePickupBurst()
    {
        yield return new WaitForSeconds(pickupBurst.main.duration);
        pickupBurst.gameObject.SetActive(false);
    }

    public void AnimateScale(GameObject target)
    {
        StartCoroutine(ScaleAnimation(target));
    }

    private IEnumerator ScaleAnimation(GameObject target)
    {
        Vector3 originalScale = target.transform.localScale;
        Vector3 targetScale = originalScale * scaleAmount;

        // Scale up
        float elapsedTime = 0f;
        while (elapsedTime < scaleDuration)
        {
            target.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Scale down
        elapsedTime = 0f;
        while (elapsedTime < scaleDuration)
        {
            target.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void TriggerFlash(GameObject target)
    {
        StartCoroutine(FlashEffect(target));
    }

    private IEnumerator FlashEffect(GameObject target)
    {
        SpriteRenderer renderer = target.GetComponent<SpriteRenderer>();
        if (renderer == null) yield break;

        Color originalColor = renderer.color;
        renderer.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        renderer.color = originalColor;
    }

    #endregion

    #region Camera Transitions

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        CanvasGroup fadeCanvas = FindObjectOfType<CanvasGroup>();
        if (fadeCanvas == null)
        {
            fadeCanvas = new GameObject("FadeCanvas").AddComponent<CanvasGroup>();
            DontDestroyOnLoad(fadeCanvas.gameObject);
            fadeCanvas.blocksRaycasts = false;
        }

        fadeCanvas.alpha = 1f;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvas.alpha = 1f - (elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvas.alpha = 0f;
    }

    public void RevealSilverStar(GameObject silverStar)
    {
        StartCoroutine(RevealStarCoroutine(silverStar));
    }

    private IEnumerator RevealStarCoroutine(GameObject silverStar)
    {
        silverStar.SetActive(true);
        Vector3 originalScale = silverStar.transform.localScale;
        silverStar.transform.localScale = Vector3.zero;

        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime / zoomDuration;
            silverStar.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
            silverStar.transform.rotation = Quaternion.Euler(0f, 0f, t * 360f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void ZoomOnComplete(Transform target)
    {
        StartCoroutine(ZoomCoroutine(target));
    }

    private IEnumerator ZoomCoroutine(Transform target)
    {
        Vector3 originalPosition = mainCamera.transform.position;
        Vector3 targetPosition = target.position + Vector3.back * 5f;
        float originalSize = mainCamera.orthographicSize;
        float targetSize = originalSize / zoomAmount;

        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime / zoomDuration;
            mainCamera.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(originalSize, targetSize, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hold for a moment
        yield return new WaitForSeconds(1f);

        // Return to original position
        elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime / zoomDuration;
            mainCamera.transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
            mainCamera.orthographicSize = Mathf.Lerp(targetSize, originalSize, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void SmoothFollow(Transform target)
    {
        StartCoroutine(SmoothFollowCoroutine(target));
    }

    private IEnumerator SmoothFollowCoroutine(Transform target)
    {
        while (true)
        {
            if (target == null) yield break;

            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                followSmoothTime * Time.deltaTime
            );

            yield return null;
        }
    }

    #endregion
}