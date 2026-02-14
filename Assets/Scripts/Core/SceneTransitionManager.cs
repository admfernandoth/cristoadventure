using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private string loadingTextFormat = "Carregando... {0}%";

    [Header("Memory Management")]
    [SerializeField] private float memoryCheckInterval = 5.0f;
    [SerializeField] private float warningMemoryThreshold = 0.8f;

    private AsyncOperation sceneLoadOperation;
    private List<string> loadedScenes = new List<string>();
    private float targetAlpha = 0f;
    private bool isLoading = false;

    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeFadeOverlay();
    }

    private void Start()
    {
        StartMemoryManagement();
    }

    private void InitializeFadeOverlay()
    {
        if (fadeOverlay == null)
        {
            GameObject fadeObj = new GameObject("FadeOverlay");
            fadeObj.transform.SetParent(transform);
            RectTransform rect = fadeObj.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            fadeOverlay = fadeObj.AddComponent<Image>();
            fadeOverlay.color = new Color(0, 0, 0, 0);
        }

        SetFadeAlpha(0f);
    }

    public void LoadSceneAsync(string sceneName, System.Action onComplete = null)
    {
        if (isLoading)
        {
            Debug.LogWarning("Scene load already in progress!");
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName, onComplete));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, System.Action onComplete)
    {
        isLoading = true;

        // Fade to black
        yield return StartCoroutine(FadeTo(1f));

        // Show loading screen
        ShowLoadingScreen();

        // Unload unused scenes
        UnloadUnusedScenes();

        // Start loading the new scene
        sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        sceneLoadOperation.allowSceneActivation = false;

        // Wait for load to complete
        while (!sceneLoadOperation.isDone)
        {
            UpdateLoadingProgress(sceneLoadOperation.progress);

            // Allow scene activation when it's almost done (90%)
            if (sceneLoadOperation.progress >= 0.9f)
            {
                sceneLoadOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Hide loading screen
        HideLoadingScreen();

        // Add to loaded scenes
        if (!loadedScenes.Contains(sceneName))
        {
            loadedScenes.Add(sceneName);
        }

        // Fade in
        yield return StartCoroutine(FadeTo(0f));

        isLoading = false;
        onComplete?.Invoke();
    }

    private IEnumerator FadeTo(float target)
    {
        float elapsedTime = 0f;
        float startAlpha = fadeOverlay.color.a;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, target, elapsedTime / fadeDuration);
            SetFadeAlpha(currentAlpha);
            yield return null;
        }

        SetFadeAlpha(target);
    }

    private void SetFadeAlpha(float alpha)
    {
        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = alpha;
            fadeOverlay.color = color;
        }
    }

    private void ShowLoadingScreen()
    {
        if (loadingCanvas != null)
        {
            loadingCanvas.gameObject.SetActive(true);
        }

        if (loadingText != null)
        {
            loadingText.text = string.Format(loadingTextFormat, "0");
        }

        if (loadingSlider != null)
        {
            loadingSlider.value = 0f;
        }
    }

    private void HideLoadingScreen()
    {
        if (loadingCanvas != null)
        {
            loadingCanvas.gameObject.SetActive(false);
        }
    }

    private void UpdateLoadingProgress(float progress)
    {
        if (loadingText != null)
        {
            int percent = Mathf.RoundToInt(progress * 100);
            loadingText.text = string.Format(loadingTextFormat, percent);
        }

        if (loadingSlider != null)
        {
            loadingSlider.value = progress;
        }
    }

    public void UnloadUnusedScenes()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            if (i != currentSceneIndex)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                {
                    StartCoroutine(UnloadSceneRoutine(scene));
                }
            }
        }
    }

    private IEnumerator UnloadSceneRoutine(Scene scene)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene);

        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        loadedScenes.Remove(scene.name);
        Debug.Log($"Unloaded scene: {scene.name}");
    }

    private void StartMemoryManagement()
    {
        StartCoroutine(MemoryManagementRoutine());
    }

    private IEnumerator MemoryManagementRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(memoryCheckInterval);

            // Check memory usage
            float memoryUsage = GetMemoryUsage();
            if (memoryUsage > warningMemoryThreshold)
            {
                Debug.LogWarning($"Memory usage high: {memoryUsage * 100}%");

                // Perform cleanup
                System.GC.Collect();
                Resources.UnloadUnusedAssets();
            }
        }
    }

    private float GetMemoryUsage()
    {
        // This is a simplified memory check
        // In a real game, you might want to use more sophisticated methods
        return SystemInfo.graphicsMemorySize / (float)SystemInfo.systemMemorySize;
    }

    // Quick scene load without loading screen
    public void QuickLoadScene(string sceneName)
    {
        StartCoroutine(QuickLoadRoutine(sceneName));
    }

    private IEnumerator QuickLoadRoutine(string sceneName)
    {
        // Quick fade
        yield return StartCoroutine(FadeTo(0.5f));

        SceneManager.LoadScene(sceneName);

        // Fade back in
        yield return StartCoroutine(FadeTo(0f));
    }

    // Emergency stop for scene loading
    public void StopSceneLoad()
    {
        if (sceneLoadOperation != null)
        {
            sceneLoadOperation.allowSceneActivation = false;
            sceneLoadOperation = null;
        }

        isLoading = false;
        HideLoadingScreen();
        StartCoroutine(FadeTo(0f));
    }

    private void OnDestroy()
    {
        // Clean up
        if (instance == this)
        {
            instance = null;
        }
    }
}