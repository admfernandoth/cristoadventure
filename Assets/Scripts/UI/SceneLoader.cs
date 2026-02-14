using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class SceneLoader : MonoBehaviour
{
    [Header("References")]
    public LoadingManager loadingManager;
    public CanvasGroup loadingCanvasGroup;

    [Header("Loading Settings")]
    public float fadeDuration = 0.5f;
    public float minLoadingTime = 1f; // Minimum time to show loading screen

    private string currentScene;
    private int currentChapter;
    private bool isLoading = false;

    private void Start()
    {
        // Ensure we have references
        if (loadingManager == null)
        {
            loadingManager = FindObjectOfType<LoadingManager>();
            if (loadingManager == null)
            {
                Debug.LogError("LoadingManager not found in scene!");
                return;
            }
        }

        if (loadingCanvasGroup == null && gameObject.transform.Find("LoadingCanvas") != null)
        {
            loadingCanvasGroup = gameObject.transform.Find("LoadingCanvas").GetComponent<CanvasGroup>();
        }
    }

    public void LoadScene(string sceneName, int chapter = 1)
    {
        if (isLoading || string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Already loading or scene name is empty");
            return;
        }

        currentScene = sceneName;
        currentChapter = chapter;
        isLoading = true;

        StartCoroutine(LoadSceneRoutine(sceneName, chapter));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, int chapter)
    {
        // Show loading screen
        if (loadingCanvasGroup != null)
        {
            loadingCanvasGroup.alpha = 1f;
            loadingCanvasGroup.blocksRaycasts = true;
        }

        // Start loading
        StartCoroutine(AsyncLoadScene(sceneName, chapter));
    }

    private IEnumerator AsyncLoadScene(string sceneName, int chapter)
    {
        float startTime = Time.time;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Check if loading manager is available
        if (loadingManager != null)
        {
            loadingManager.LoadScene(sceneName, chapter);
        }
        else
        {
            // Fallback: Simple loading display
            Debug.LogWarning("LoadingManager not found, using fallback display");
            StartCoroutine(FallbackLoadingDisplay());
        }

        // Wait for load to complete
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Wait minimum loading time
                float elapsedTime = Time.time - startTime;
                if (elapsedTime < minLoadingTime)
                {
                    yield return new WaitForSeconds(minLoadingTime - elapsedTime);
                }

                // Allow scene activation
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        isLoading = false;
    }

    private IEnumerator FallbackLoadingDisplay()
    {
        float startTime = Time.time;
        float progress = 0f;

        // Simple loading display
        while (progress < 1f)
        {
            progress = Mathf.Clamp01((Time.time - startTime) / minLoadingTime);

            // Update UI if available
            if (loadingCanvasGroup != null)
            {
                // This is a fallback, you might want to add a simple progress text here
            }

            yield return null;
        }
    }

    // Error handling
    public void HandleLoadingError(string errorMessage)
    {
        if (loadingManager != null)
        {
            loadingManager.ShowLoadingError(errorMessage);
        }

        Debug.LogError($"Loading Error: {errorMessage}");
    }

    // Public methods for scene loading with error handling
    public void LoadMainMenu()
    {
        LoadScene("MainMenu", 0);
    }

    public void LoadGameLevel(int levelNumber)
    {
        LoadScene($"Level_{levelNumber}", levelNumber);
    }

    public void LoadCredits()
    {
        LoadScene("Credits", 0);
    }

    // Scene validation
    private bool ValidateScene(string sceneName)
    {
        // This is a simple validation - you might want to add more sophisticated checks
        if (string.IsNullOrEmpty(sceneName))
        {
            HandleLoadingError("Scene name is empty");
            return false;
        }

        // Check if scene exists (simplified check)
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameOnly = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameOnly == sceneName)
            {
                return true;
            }
        }

        HandleLoadingError($"Scene '{sceneName}' not found in build settings");
        return false;
    }

    // Preload utility for common scenes
    public void PreloadScene(string sceneName)
    {
        if (ValidateScene(sceneName))
        {
            StartCoroutine(PreloadSceneAsync(sceneName));
        }
    }

    private IEnumerator PreloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitUntil(() => asyncLoad.isDone || asyncLoad.progress >= 0.9f);
    }

    // Get current loading status
    public bool IsLoading()
    {
        return isLoading;
    }

    // Get current loading progress
    public float GetLoadingProgress()
    {
        if (loadingManager != null)
        {
            // This would require adding a public progress property to LoadingManager
            return 0f; // Placeholder
        }
        return 0f;
    }
}

// Optional: Scene Loader Manager for global access
public class SceneLoaderManager
{
    private static SceneLoader instance;

    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneLoader>();
                if (instance == null)
                {
                    Debug.LogError("SceneLoader not found in scene!");
                }
            }
            return instance;
        }
    }

    public static void LoadScene(string sceneName, int chapter = 1)
    {
        if (Instance != null)
        {
            Instance.LoadScene(sceneName, chapter);
        }
        else
        {
            Debug.LogError("SceneLoader instance not available!");
        }
    }

    public static bool IsLoading
    {
        get { return Instance != null ? Instance.IsLoading() : false; }
    }
}