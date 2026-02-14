using UnityEngine;
using UnityEngine.UI;

public class LoadingUIPrefabExample : MonoBehaviour
{
    [Header("Example Setup")]
    public GameObject loadingUIPrefab;
    public LoadingManager loadingManager;

    [Header("Phase Art Sprites")]
    public Sprite entranceSprite;
    public Sprite hallwaySprite;
    public Sprite chamberSprite;

    void Start()
    {
        // Example of how to set up loading UI
        SetupLoadingUI();
    }

    private void SetupLoadingUI()
    {
        if (loadingManager == null)
        {
            Debug.LogError("LoadingManager not assigned!");
            return;
        }

        // Find or create the loading UI prefab
        if (loadingUIPrefab == null)
        {
            loadingUIPrefab = Resources.Load<GameObject>("Prefabs/LoadingUI");
            if (loadingUIPrefab == null)
            {
                Debug.LogWarning("LoadingUI prefab not found in Resources. Make sure to create it.");
                return;
            }
        }

        // Instantiate the prefab
        GameObject loadingUI = Instantiate(loadingUIPrefab);
        DontDestroyOnLoad(loadingUI);

        // Assign to loading manager
        loadingManager.loadingUI = loadingUI;

        // Set up phase splash sprites
        if (loadingManager.phaseSplashImage != null)
        {
            // This would be set up in the LoadingScreenConfig
            // Here we're just showing how to assign them
            var config = Resources.Load<LoadingScreenConfig>("LoadingScreenConfig");
            if (config != null)
            {
                // Assign sprites to phase splashes
                if (config.phaseSplashes.Count > 0 && config.phaseSplashes[0].phaseArt == null)
                {
                    config.phaseSplashes[0].phaseArt = entranceSprite;
                }
                if (config.phaseSplashes.Count > 1 && config.phaseSplashes[1].phaseArt == null)
                {
                    config.phaseSplashes[1].phaseArt = hallwaySprite;
                }
                if (config.phaseSplashes.Count > 2 && config.phaseSplashes[2].phaseArt == null)
                {
                    config.phaseSplashes[2].phaseArt = chamberSprite;
                }
            }
        }

        // Set up the loading manager references
        SetupLoadingManagerReferences();
    }

    private void SetupLoadingManagerReferences()
    {
        // Find all UI elements in the prefab
        Transform loadingCanvas = loadingUIPrefab.transform;

        // Find progress bar
        progressBar = loadingCanvas.Find("ProgressBar/ProgressBarFill")?.GetComponent<Image>();
        loadingManager.progressBar = progressBar;

        // Find progress text
        progressText = loadingCanvas.Find("ProgressText")?.GetComponent<TMPro.TextMeshProUGUI>();
        loadingManager.progressText = progressText;

        // Find tip text
        tipText = loadingCanvas.Find("TipsArea/TipText")?.GetComponent<TMPro.TextMeshProUGUI>();
        loadingManager.tipText = tipText;

        // Find phase splash elements
        phaseSplashImage = loadingCanvas.Find("PhaseSplash/PhaseSplashImage")?.GetComponent<Image>();
        loadingManager.phaseSplashImage = phaseSplashImage;

        phaseNameText = loadingCanvas.Find("PhaseSplash/PhaseNameText")?.GetComponent<TMPro.TextMeshProUGUI>();
        loadingManager.phaseNameText = phaseNameText;

        chapterText = loadingCanvas.Find("PhaseSplash/ChapterText")?.GetComponent<TMPro.TextMeshProUGUI>();
        loadingManager.chapterText = chapterText;

        // Find rotating icon
        rotatingIcon = loadingCanvas.Find("LoadingIcon/rotatingIcon")?.GetComponent<Image>();
        loadingManager.rotatingIcon = rotatingIcon;

        Debug.Log("Loading UI setup complete!");
    }

    // Example UI elements
    private Image progressBar;
    private TMPro.TextMeshProUGUI progressText;
    private TMPro.TextMeshProUGUI tipText;
    private Image phaseSplashImage;
    private TMPro.TextMeshProUGUI phaseNameText;
    private TMPro.TextMeshProUGUI chapterText;
    private Image rotatingIcon;

    // Example usage in other scripts
    public void ExampleUsage()
    {
        // Load a scene with chapter number
        loadingManager.LoadScene("Level_1", 1);

        // Switch language
        LocalizationManager.Instance.SetLanguage("es");

        // Show loading error
        loadingManager.ShowLoadingError("Failed to load scene");
    }
}