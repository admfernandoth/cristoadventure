using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LoadingManager : MonoBehaviour
{
    [Header("Loading Screen Settings")]
    public GameObject loadingUI;
    public Image progressBar;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI tipText;
    public Image phaseSplashImage;
    public TextMeshProUGUI phaseNameText;
    public TextMeshProUGUI chapterText;
    public Image rotatingIcon;

    [Header("Animation Settings")]
    public float rotationSpeed = 180f;
    public float fadeDuration = 0.5f;
    public float tipRotationInterval = 5f;

    [Header("Colors")]
    public Color primaryColor = new Color(0.2f, 0.6f, 1f);
    public Color backgroundColor = new Color(0.1f, 0.1f, 0.2f);
    public Color textColor = Color.white;

    private LoadingScreenConfig config;
    private int currentTipIndex = 0;
    private int currentPhaseIndex = 0;
    private bool isLoading = false;
    private Coroutine rotationCoroutine;
    private Coroutine tipCoroutine;

    // Multilingual support
    private string currentLanguage = "en";
    private Dictionary<string, Dictionary<string, string>> localizedTips = new Dictionary<string, Dictionary<string, string>>();

    private void Awake()
    {
        config = Resources.Load<LoadingScreenConfig>("LoadingScreenConfig");
        if (config == null)
        {
            Debug.LogError("LoadingScreenConfig not found! Please create one.");
            return;
        }

        InitializeLocalization();
        SetupUI();
    }

    private void InitializeLocalization()
    {
        // Initialize tip translations
        foreach (var tip in config.tips)
        {
            if (!localizedTips.ContainsKey(tip.id))
            {
                localizedTips[tip.id] = new Dictionary<string, string>();
            }

            localizedTips[tip.id]["en"] = tip.english;
            localizedTips[tip.id]["es"] = tip.spanish;
            localizedTips[tip.id]["fr"] = tip.french;
        }
    }

    private void SetupUI()
    {
        if (loadingUI != null)
        {
            loadingUI.SetActive(false);
        }

        // Set colors
        if (progressBar != null)
        {
            progressBar.color = primaryColor;
        }

        if (progressText != null)
        {
            progressText.color = textColor;
        }

        if (tipText != null)
        {
            tipText.color = textColor;
        }

        if (phaseNameText != null)
        {
            phaseNameText.color = textColor;
        }

        if (chapterText != null)
        {
            chapterText.color = textColor;
        }
    }

    public void LoadScene(string sceneName, int chapter = 1)
    {
        if (isLoading) return;

        isLoading = true;
        currentPhaseIndex = 0;

        // Show loading screen
        if (loadingUI != null)
        {
            loadingUI.SetActive(true);
            FadeUI(1f, 0f);
        }

        // Reset UI elements
        ResetUI();

        // Start loading
        StartCoroutine(LoadSceneAsync(sceneName, chapter));
    }

    private IEnumerator LoadSceneAsync(string sceneName, int chapter)
    {
        // Show phase splash
        ShowPhaseSplash(chapter);

        // Start rotating icon
        if (rotatingIcon != null)
        {
            rotationCoroutine = StartCoroutine(RotateIcon());
        }

        // Start tip rotation
        if (tipText != null)
        {
            tipCoroutine = StartCoroutine(RotateTips());
        }

        // Start loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float progress = 0;

        while (!asyncLoad.isDone)
        {
            // Calculate progress (0.9 to account for scene activation)
            progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Update progress bar
            UpdateProgressBar(progress);

            // Update activity text
            if (progressText != null)
            {
                progressText.text = $"Loading... {Mathf.Round(progress * 100)}%";
            }

            // Check if loading is complete
            if (progress >= 1f)
            {
                break;
            }

            yield return null;
        }

        // Handle scene activation
        if (asyncLoad.isDone)
        {
            // Fade out loading screen
            FadeUI(0f, 1f, () => {
                if (loadingUI != null)
                {
                    loadingUI.SetActive(false);
                }
            });

            // Stop coroutines
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }

            if (tipCoroutine != null)
            {
                StopCoroutine(tipCoroutine);
            }

            isLoading = false;
        }
    }

    private void ShowPhaseSplash(int chapter)
    {
        if (currentPhaseIndex < config.phaseSplashes.Count)
        {
            var phase = config.phaseSplashes[currentPhaseIndex];

            // Update phase text
            if (phaseNameText != null)
            {
                phaseNameText.text = phase.phaseName;
            }

            if (chapterText != null)
            {
                chapterText.text = $"Chapter {chapter}";
            }

            // Update phase image (placeholder)
            if (phaseSplashImage != null)
            {
                phaseSplashImage.sprite = phase.phaseArt;
            }

            // Fade in splash
            FadeUI(1f, 0f);

            currentPhaseIndex++;
        }
    }

    private IEnumerator RotateIcon()
    {
        while (true)
        {
            rotatingIcon.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator RotateTips()
    {
        yield return new WaitForSeconds(tipRotationInterval);

        while (true)
        {
            currentTipIndex = (currentTipIndex + 1) % config.tips.Count;
            UpdateTip();

            yield return new WaitForSeconds(tipRotationInterval);
        }
    }

    private void UpdateTip()
    {
        if (tipText != null && config.tips.Count > 0)
        {
            var tip = config.tips[currentTipIndex];

            // Get localized text
            if (localizedTips.ContainsKey(tip.id) && localizedTips[tip.id].ContainsKey(currentLanguage))
            {
                tipText.text = localizedTips[tip.id][currentLanguage];
            }
            else
            {
                tipText.text = tip.english; // Fallback to English
            }

            // Fade in new tip
            FadeUI(1f, 0f);
        }
    }

    private void UpdateProgressBar(float progress)
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = progress;
        }
    }

    private void ResetUI()
    {
        currentTipIndex = 0;
        UpdateTip();

        if (progressBar != null)
        {
            progressBar.fillAmount = 0f;
        }

        if (progressText != null)
        {
            progressText.text = "Loading... 0%";
        }
    }

    private void FadeUI(float targetAlpha, float duration, System.Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(targetAlpha, duration, onComplete));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, System.Action onComplete)
    {
        float startAlpha = 1f - targetAlpha;
        float currentAlpha = startAlpha;
        float elapsedTime = 0f;

        // Set initial alpha
        SetCanvasGroupAlpha(currentAlpha);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            SetCanvasGroupAlpha(currentAlpha);
            yield return null;
        }

        onComplete?.Invoke();
    }

    private void SetCanvasGroupAlpha(float alpha)
    {
        CanvasGroup canvasGroup = loadingUI.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }

    // Language switching
    public void SetLanguage(string language)
    {
        currentLanguage = language;
        UpdateTip();
    }

    // Error handling
    public void ShowLoadingError(string errorMessage)
    {
        if (progressText != null)
        {
            progressText.text = $"Error: {errorMessage}";
            progressText.color = Color.red;
        }
    }
}