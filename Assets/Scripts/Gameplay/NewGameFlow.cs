using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NewGameFlow : MonoBehaviour
{
    [Header("Character Creation")]
    [SerializeField] private GameObject characterCreationPanel;
    [SerializeField] private TMP_Text playerNameField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button randomizeButton;
    [SerializeField] private Button backButton;

    [Header("Character Appearance")]
    [SerializeField] private Slider skinToneSlider;
    [SerializeField] private Slider bodyTypeSlider;
    [SerializeField] private Toggle hairStyleToggle;
    [SerializeField] private Toggle hairColorToggle;
    [SerializeField] private Toggle genderToggle;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Button tutorialNextButton;
    [SerializeField] private Button tutorialSkipButton;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private Image tutorialImage;

    [Header("Cutscene")]
    [SerializeField] private GameObject cutscenePanel;
    [SerializeField] private TMP_Text cutsceneText;
    [SerializeField] private Image cutsceneImage;
    [SerializeField] private float textTypewriterSpeed = 0.05f;

    [Header("Configuration")]
    [SerializeField] private string tutorialScene = "Tutorial";
    [SerializeField] private string phase11Scene = "Phase_1.1";
    [SerializeField] private string firstCutsceneName = "OpeningCutscene";

    private GameFlow gameFlow;
    private SceneTransitionManager sceneTransitionManager;
    private int currentTutorialStep = 0;
    private List<TutorialStep> tutorialSteps;
    private bool hasPlayerData = false;
    private PlayerCharacterData playerData;

    private void Awake()
    {
        gameFlow = FindObjectOfType<GameFlow>();
        sceneTransitionManager = SceneTransitionManager.Instance;

        InitializeCharacterCreation();
        InitializeTutorial();
        InitializeCutscene();
    }

    private void Start()
    {
        HideAllPanels();
        characterCreationPanel.SetActive(true);
    }

    private void InitializeCharacterCreation()
    {
        confirmButton.onClick.AddListener(ConfirmCharacter);
        randomizeButton.onClick.AddListener(RandomizeCharacter);
        backButton.onClick.AddListener(() =>
        {
            sceneTransitionManager.LoadSceneAsync("MainMenu");
        });

        // Initialize default values
        playerData = new PlayerCharacterData();
    }

    private void InitializeTutorial()
    {
        tutorialSteps = new List<TutorialStep>
        {
            new TutorialStep
            {
                title = "Bem-vindo à CRISTO ADVENTURE",
                description = "Você está prestes a embarcar em uma jornada épica pela floresta misteriosa de Cristo.",
                image = null // Will be set via inspector or loaded
            },
            new TutorialStep
            {
                title = "Seu Personagem",
                description = "Personalize seu personagem para começar sua aventura.",
                image = null
            },
            new TutorialStep
            {
                title = "Controles",
                description = "Use WASD ou setas para se mover. Espaço para interagir.",
                image = null
            },
            new TutorialStep
            {
                title = "Objetivo Principal",
                description = "Descubra os mistérios da floresta e encontre o caminho para fora.",
                image = null
            },
            new TutorialStep
            {
                title = "Boa Sorte!",
                description = "Sua aventura começa agora. Boa sorte, explorador!",
                image = null
            }
        };

        tutorialNextButton.onClick.AddListener(NextTutorialStep);
        tutorialSkipButton.onClick.AddListener(SkipTutorial);
    }

    private void InitializeCutscene()
    {
        cutscenePanel.SetActive(false);
    }

    public void StartCharacterCreation()
    {
        // Reset player data
        playerData = new PlayerCharacterData();
        hasPlayerData = false;

        // Show character creation panel
        HideAllPanels();
        characterCreationPanel.SetActive(true);

        // Reset UI
        playerNameField.text = "";
        skinToneSlider.value = 0.5f;
        bodyTypeSlider.value = 0.5f;
        hairStyleToggle.isOn = false;
        hairColorToggle.isOn = false;
        genderToggle.isOn = false;
    }

    private void ConfirmCharacter()
    {
        if (string.IsNullOrWhiteSpace(playerNameField.text))
        {
            ShowErrorMessage("Por favor, insira um nome para seu personagem.");
            return;
        }

        // Save character data
        playerData.playerName = playerNameField.text;
        playerData.skinTone = skinToneSlider.value;
        playerData.bodyType = bodyTypeSlider.value;
        playerData.hasHairStyle = hairStyleToggle.isOn;
        playerData.hasColoredHair = hairColorToggle.isOn;
        playerData.isMale = genderToggle.isOn;

        hasPlayerData = true;
        PlayerPrefs.SetString("PlayerName", playerData.playerName);
        PlayerPrefs.SetFloat("SkinTone", playerData.skinTone);
        PlayerPrefs.SetFloat("BodyType", playerData.bodyType);
        PlayerPrefs.SetInt("HasHairStyle", playerData.hasHairStyle ? 1 : 0);
        PlayerPrefs.SetInt("HasColoredHair", playerData.hasColoredHair ? 1 : 0);
        PlayerPrefs.SetInt("IsMale", playerData.isMale ? 1 : 0);
        PlayerPrefs.Save();

        // Start tutorial
        ShowTutorial();
    }

    private void RandomizeCharacter()
    {
        playerData = new PlayerCharacterData
        {
            playerName = GetRandomName(),
            skinTone = Random.Range(0f, 1f),
            bodyType = Random.Range(0f, 1f),
            hasHairStyle = Random.value > 0.3f,
            hasColoredHair = Random.value > 0.5f,
            isMale = Random.value > 0.5f
        };

        // Update UI
        playerNameField.text = playerData.playerName;
        skinToneSlider.value = playerData.skinTone;
        bodyTypeSlider.value = playerData.bodyType;
        hairStyleToggle.isOn = playerData.hasHairStyle;
        hairColorToggle.isOn = playerData.hasColoredHair;
        genderToggle.isOn = playerData.isMale;
    }

    private string GetRandomName()
    {
        string[] maleNames = { "João", "Pedro", "Lucas", "Miguel", "Gabriel", "Arthur", "Daniel", "Bruno", "Carlos", "Rafael" };
        string[] femaleNames = { "Maria", "Ana", "Juliana", "Patrícia", "Mariana", "Larissa", "Amanda", "Bruna", "Camila", "Carolina" };

        if (Random.value > 0.5f && maleNames.Length > 0)
        {
            return maleNames[Random.Range(0, maleNames.Length)];
        }
        else if (femaleNames.Length > 0)
        {
            return femaleNames[Random.Range(0, femaleNames.Length)];
        }

        return "Jogador";
    }

    private void ShowTutorial()
    {
        HideAllPanels();
        tutorialPanel.SetActive(true);

        currentTutorialStep = 0;
        UpdateTutorialDisplay();
    }

    private void NextTutorialStep()
    {
        currentTutorialStep++;

        if (currentTutorialStep >= tutorialSteps.Count)
        {
            // Start opening cutscene
            ShowOpeningCutscene();
        }
        else
        {
            UpdateTutorialDisplay();
        }
    }

    private void SkipTutorial()
    {
        // Skip to opening cutscene
        ShowOpeningCutscene();
    }

    private void ShowOpeningCutscene()
    {
        HideAllPanels();
        cutscenePanel.SetActive(true);

        // Play opening cutscene
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Example cutscene content - should be replaced with actual cutscene system
        cutsceneText.text = "Você acorda em uma floresta misteriosa...";
        cutsceneText.gameObject.SetActive(true);

        TypewriterEffect(cutsceneText, "Você acorda em uma floresta misteriosa...");

        yield return new WaitForSeconds(3f);

        TypewriterEffect(cutsceneText, "Não se lembra de como chegou aqui...");

        yield return new WaitForSeconds(3f);

        TypewriterEffect(cutsceneText, "A única coisa que você sabe é precisa encontrar um caminho para fora...");

        yield return new WaitForSeconds(3f);

        // Load the first phase
        StartCoroutine(LoadPhase1_1());
    }

    private void TypewriterEffect(TMP_Text text, string message)
    {
        text.text = "";
        StartCoroutine(TypewriterRoutine(text, message));
    }

    private IEnumerator TypewriterRoutine(TMP_Text text, string message)
    {
        foreach (char c in message)
        {
            text.text += c;
            yield return new WaitForSeconds(textTypewriterSpeed);
        }
    }

    private IEnumerator LoadPhase1_1()
    {
        // Set initial game state
        gameFlow.UpdateGameState("PlayerName", playerData.playerName);
        gameFlow.UpdateGameState("HasTutorialCompleted", true);
        gameFlow.SaveGame();

        // Transition to phase 1.1
        sceneTransitionManager.LoadSceneAsync(phase11Scene, () =>
        {
            var phaseManager = FindObjectOfType<PhaseManager>();
            if (phaseManager != null)
            {
                phaseManager.InitializePhase("1.1");
            }

            // Start initial cutscene in the phase
            var phaseCutscene = FindObjectOfType<PhaseCutscene>();
            if (phaseCutscene != null)
            {
                phaseCutscene.PlayInitialCutscene();
            }
        });
    }

    private void UpdateTutorialDisplay()
    {
        if (currentTutorialStep < tutorialSteps.Count)
        {
            TutorialStep step = tutorialSteps[currentTutorialStep];
            tutorialText.text = step.title + "\n\n" + step.description;

            // Update image if available
            if (tutorialImage != null && step.image != null)
            {
                tutorialImage.sprite = step.image;
                tutorialImage.gameObject.SetActive(true);
            }
            else if (tutorialImage != null)
            {
                tutorialImage.gameObject.SetActive(false);
            }
        }
    }

    private void HideAllPanels()
    {
        characterCreationPanel?.SetActive(false);
        tutorialPanel?.SetActive(false);
        cutscenePanel?.SetActive(false);
    }

    private void ShowErrorMessage(string message)
    {
        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowNotification(message, 3f);
        }
    }

    private class TutorialStep
    {
        public string title;
        public string description;
        public UnityEngine.Sprite image;
    }
}

[System.Serializable]
public class PlayerCharacterData
{
    public string playerName;
    public float skinTone;
    public float bodyType;
    public bool hasHairStyle;
    public bool hasColoredHair;
    public bool isMale;
}