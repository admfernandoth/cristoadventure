using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingAnimations : MonoBehaviour
{
    [Header("Animation Settings")]
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    public float iconRotationSpeed = 180f;
    public float tipPulseScale = 1.1f;
    public float tipPulseDuration = 0.5f;

    [Header("References")]
    public Image rotatingIcon;
    public TextMeshProUGUI tipText;
    public Image progressBar;
    public TextMeshProUGUI progressText;

    private CanvasGroup canvasGroup;
    private Coroutine currentAnimation;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        // Set initial state
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    // Public methods for triggering animations
    public void FadeIn(System.Action onComplete = null)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(FadeInRoutine(onComplete));
    }

    public void FadeOut(System.Action onComplete = null)
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(FadeOutRoutine(onComplete));
    }

    public void PulseTipText()
    {
        if (tipText != null)
        {
            StartCoroutine(TipPulseRoutine());
        }
    }

    public void AnimateProgress(float targetProgress, float duration = 0.3f)
    {
        StartCoroutine(ProgressAnimationRoutine(targetProgress, duration));
    }

    private IEnumerator FadeInRoutine(System.Action onComplete)
    {
        float startTime = Time.time;
        float startAlpha = canvasGroup.alpha;

        while (Time.time - startTime < fadeInDuration)
        {
            float progress = (Time.time - startTime) / fadeInDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, progress);
            canvasGroup.blocksRaycasts = true;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        onComplete?.Invoke();
    }

    private IEnumerator FadeOutRoutine(System.Action onComplete)
    {
        float startTime = Time.time;
        float startAlpha = canvasGroup.alpha;

        while (Time.time - startTime < fadeOutDuration)
        {
            float progress = (Time.time - startTime) / fadeOutDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, progress);
            canvasGroup.blocksRaycasts = false;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        onComplete?.Invoke();
    }

    private IEnumerator TipPulseRoutine()
    {
        Vector3 originalScale = tipText.transform.localScale;
        float startTime = Time.time;
        float duration = tipPulseDuration;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            float scale = Mathf.Lerp(1f, tipPulseScale, Mathf.Sin(progress * Mathf.PI));
            tipText.transform.localScale = originalScale * scale;
            yield return null;
        }

        tipText.transform.localScale = originalScale;
    }

    private IEnumerator ProgressAnimationRoutine(float targetProgress, float duration)
    {
        float startTime = Time.time;
        float startProgress = progressBar.fillAmount;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            float currentProgress = Mathf.Lerp(startProgress, targetProgress, progress);

            progressBar.fillAmount = currentProgress;

            if (progressText != null)
            {
                progressText.text = $"Loading... {Mathf.Round(currentProgress * 100)}%";
            }

            yield return null;
        }

        // Ensure final value is exact
        progressBar.fillAmount = targetProgress;
        if (progressText != null)
        {
            progressText.text = $"Loading... {Mathf.Round(targetProgress * 100)}%";
        }
    }

    // Animation for phase splash
    public void AnimatePhaseSplash(System.Action onComplete = null)
    {
        StartCoroutine(PhaseSplashRoutine(onComplete));
    }

    private IEnumerator PhaseSplashRoutine(System.Action onComplete)
    {
        // Fade in
        canvasGroup.alpha = 0f;
        float startTime = Time.time;

        while (Time.time - startTime < fadeInDuration)
        {
            float progress = (Time.time - startTime) / fadeInDuration;
            canvasGroup.alpha = progress;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Wait a bit for display
        yield return new WaitForSeconds(1f);

        // Fade out
        startTime = Time.time;
        while (Time.time - startTime < fadeOutDuration)
        {
            float progress = (Time.time - startTime) / fadeOutDuration;
            canvasGroup.alpha = 1f - progress;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        onComplete?.Invoke();
    }

    private void Update()
    {
        // Continuous icon rotation
        if (rotatingIcon != null)
        {
            rotatingIcon.transform.Rotate(0, 0, iconRotationSpeed * Time.deltaTime);
        }
    }

    // Stop all animations
    public void StopAllAnimations()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }
    }

    // Reset animations
    public void ResetAnimations()
    {
        StopAllAnimations();

        // Reset UI states
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }

        if (tipText != null)
        {
            tipText.transform.localScale = Vector3.one;
        }

        if (progressBar != null)
        {
            progressBar.fillAmount = 0f;
        }

        if (progressText != null)
        {
            progressText.text = "Loading... 0%";
        }
    }
}