using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI versionText;

    [Header("Configuration")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string gameplayScene = "Gameplay";
    [SerializeField] private string settingsScene = "Settings";
    [SerializeField] private string characterCreationScene = "CharacterCreation";

    [Header("Audio")]
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioSource audioSource;

    private SceneTransitionManager sceneTransitionManager;
    private GameFlow gameFlow;

    private void Awake()
    {
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        gameFlow = FindObjectOfType<GameFlow>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        SetupVersionText();
        SetupButtons();
        CheckForExistingSaves();
    }

    private void SetupVersionText()
    {
        if (versionText != null)
        {
            versionText.text = $"v{Application.version}";
        }
    }

    private void SetupButtons()
    {
        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(OnNewGameClick);
            SetupButtonHover(newGameButton);
        }

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClick);
            SetupButtonHover(continueButton);
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsClick);
            SetupButtonHover(settingsButton);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClick);
            SetupButtonHover(quitButton);
        }
    }

    private void SetupButtonHover(Button button)
    {
        EventTriggerListener listener = button.gameObject.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = button.gameObject.AddComponent<EventTriggerListener>();
        }

        listener.onPointerEnter += () => PlaySound(buttonHoverSound);
        listener.onPointerClick += () => PlaySound(buttonClickSound);
    }

    private void CheckForExistingSaves()
    {
        if (continueButton != null)
        {
            bool hasSave = PlayerPrefs.HasKey("SaveExists") && PlayerPrefs.GetInt("SaveExists") == 1;
            continueButton.interactable = hasSave;

            // Visual feedback
            Image continueImage = continueButton.GetComponent<Image>();
            if (continueImage != null)
            {
                ColorBlock colors = continueButton.colors;
                colors.normalColor = hasSave ? new Color(0.2f, 0.6f, 1f, 1f) : new Color(0.5f, 0.5f, 0.5f, 1f);
                colors.pressedColor = hasSave ? new Color(0.1f, 0.4f, 0.8f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f);
                continueButton.colors = colors;
            }
        }
    }

    private void OnNewGameClick()
    {
        PlaySound(buttonClickSound);

        // Clear any existing save data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Start character creation flow
        sceneTransitionManager.LoadSceneAsync(characterCreationScene, () =>
        {
            FindObjectOfType<NewGameFlow>()?.StartCharacterCreation();
        });
    }

    private void OnContinueClick()
    {
        PlaySound(buttonClickSound);

        // Load the saved game data
        if (gameFlow != null)
        {
            gameFlow.LoadGame();
        }

        // Transition to gameplay
        sceneTransitionManager.LoadSceneAsync(gameplayScene, () =>
        {
            // Initialize loaded game state
            if (gameFlow != null)
            {
                gameFlow.ResumeGame();
            }
        });
    }

    private void OnSettingsClick()
    {
        PlaySound(buttonClickSound);

        // Fade to settings
        sceneTransitionManager.LoadSceneAsync(settingsScene, () =>
        {
            // Initialize settings screen
            FindObjectOfType<SettingsManager>()?.LoadCurrentSettings();
        });
    }

    private void OnQuitClick()
    {
        PlaySound(buttonClickSound);

        // Save game state if there's an active game
        if (gameFlow != null && gameFlow.HasActiveGame())
        {
            gameFlow.SaveGame();
        }

        // Quit application
        QuitApplication();
    }

    private void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Debug helper
    private void Update()
    {
        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F1))
            {
                // Force enable continue button for testing
                continueButton.interactable = true;
                CheckForExistingSaves();
            }
        #endif
    }
}

// Helper class for button events
public class EventTriggerListener : MonoBehaviour
{
    public event System.Action onPointerEnter;
    public event System.Action onPointerExit;
    public event System.Action onPointerClick;

    public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        onPointerEnter?.Invoke();
    }

    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }

    public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        onPointerClick?.Invoke();
    }
}