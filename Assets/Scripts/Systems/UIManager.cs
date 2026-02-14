using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CristoAdventure.Systems
{
    /// <summary>
    /// Manages all UI screens and elements
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Singleton

        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_UIManager");
                    _instance = go.AddComponent<UIManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region UI Panels

        [Header("Main Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _hudPanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _backpackPanel;
        [SerializeField] private GameObject _loadingPanel;

        [Header("Info Panels")]
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private GameObject _relicPanel;
        [SerializeField] private GameObject _versePanel;
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private GameObject _puzzlePanel;

        [Header("HUD Elements")]
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _phaseText;
        [SerializeField] private Slider _loadingBar;
        [SerializeField] private GameObject _interactionPrompt;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ShowMainMenu();
        }

        private void Update()
        {
            // Handle pause menu toggle
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }

        #endregion

        #region Main Screens

        public void ShowMainMenu()
        {
            HideAllPanels();
            SetPanelActive(_mainMenuPanel, true);
        }

        public void ShowHUD()
        {
            SetPanelActive(_hudPanel, true);
        }

        public void HideHUD()
        {
            SetPanelActive(_hudPanel, false);
        }

        public void ShowPauseMenu()
        {
            SetPanelActive(_pausePanel, true);
            Time.timeScale = 0f;
        }

        public void HidePauseMenu()
        {
            SetPanelActive(_pausePanel, false);
            Time.timeScale = 1f;
        }

        public void TogglePauseMenu()
        {
            if (_pausePanel != null && _pausePanel.activeSelf)
            {
                HidePauseMenu();
            }
            else if (GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
            {
                ShowPauseMenu();
            }
        }

        public void ShowSettings()
        {
            SetPanelActive(_settingsPanel, true);
        }

        public void HideSettings()
        {
            SetPanelActive(_settingsPanel, false);
        }

        public void ShowBackpack()
        {
            SetPanelActive(_backpackPanel, true);
            UpdateBackpackContent();
        }

        public void HideBackpack()
        {
            SetPanelActive(_backpackPanel, false);
        }

        #endregion

        #region Loading

        public void ShowLoadingScreen()
        {
            HideAllPanels();
            SetPanelActive(_loadingPanel, true);
        }

        public void SetLoadingProgress(float progress)
        {
            if (_loadingBar != null)
            {
                _loadingBar.value = Mathf.Clamp01(progress);
            }
        }

        #endregion

        #region Info Panels

        public void ShowInfoPanel(string title, string content, Sprite icon = null)
        {
            SetPanelActive(_infoPanel, true);

            // Find and set content
            var titleText = _infoPanel.transform.Find("Title")?.GetComponent<TextMeshProUGUI>();
            var contentText = _infoPanel.transform.Find("Content")?.GetComponent<TextMeshProUGUI>();
            var iconImage = _infoPanel.transform.Find("Icon")?.GetComponent<Image>();

            if (titleText != null) titleText.text = title;
            if (contentText != null) contentText.text = content;
            if (iconImage != null && icon != null) iconImage.sprite = icon;
        }

        public void HideInfoPanel()
        {
            SetPanelActive(_infoPanel, false);
        }

        public void ShowRelicView(string relicName, string description, Sprite image, string verse)
        {
            SetPanelActive(_relicPanel, true);

            // Find and set content
            var nameText = _relicPanel.transform.Find("RelicName")?.GetComponent<TextMeshProUGUI>();
            var descText = _relicPanel.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();
            var relicImage = _relicPanel.transform.Find("RelicImage")?.GetComponent<Image>();
            var verseText = _relicPanel.transform.Find("Verse")?.GetComponent<TextMeshProUGUI>();

            if (nameText != null) nameText.text = relicName;
            if (descText != null) descText.text = description;
            if (relicImage != null && image != null) relicImage.sprite = image;
            if (verseText != null) verseText.text = verse;
        }

        public void HideRelicView()
        {
            SetPanelActive(_relicPanel, false);
        }

        public void ShowVerse(string reference, string text, string translation)
        {
            SetPanelActive(_versePanel, true);

            // Find and set content
            var refText = _versePanel.transform.Find("Reference")?.GetComponent<TextMeshProUGUI>();
            var verseText = _versePanel.transform.Find("VerseText")?.GetComponent<TextMeshProUGUI>();
            var transText = _versePanel.transform.Find("Translation")?.GetComponent<TextMeshProUGUI>();

            if (refText != null) refText.text = reference;
            if (verseText != null) verseText.text = text;
            if (transText != null) transText.text = translation;
        }

        public void HideVerse()
        {
            SetPanelActive(_versePanel, false);
        }

        #endregion

        #region Dialogue

        public void ShowDialogue(string speakerName, string dialogueText, Sprite portrait = null)
        {
            SetPanelActive(_dialoguePanel, true);

            // Find and set content
            var speakerText = _dialoguePanel.transform.Find("SpeakerName")?.GetComponent<TextMeshProUGUI>();
            var textText = _dialoguePanel.transform.Find("DialogueText")?.GetComponent<TextMeshProUGUI>();
            var portraitImage = _dialoguePanel.transform.Find("Portrait")?.GetComponent<Image>();

            if (speakerText != null) speakerText.text = speakerName;
            if (textText != null) textText.text = dialogueText;
            if (portraitImage != null && portrait != null) portraitImage.sprite = portrait;
        }

        public void HideDialogue()
        {
            SetPanelActive(_dialoguePanel, false);
        }

        #endregion

        #region Puzzle

        public void ShowPuzzlePanel()
        {
            SetPanelActive(_puzzlePanel, true);
        }

        public void HidePuzzlePanel()
        {
            SetPanelActive(_puzzlePanel, false);
        }

        #endregion

        #region Photo Mode

        public void ShowPhotoModeUI()
        {
            // Show photo mode overlay
            Debug.Log("[UIManager] Photo mode UI shown");
        }

        public void HidePhotoModeUI()
        {
            Debug.Log("[UIManager] Photo mode UI hidden");
        }

        #endregion

        #region HUD Updates

        public void UpdateCoinCount(int coins)
        {
            if (_coinText != null)
            {
                _coinText.text = coins.ToString();
            }
        }

        public void UpdatePhaseInfo(string phaseName)
        {
            if (_phaseText != null)
            {
                _phaseText.text = phaseName;
            }
        }

        public void ShowInteractionPrompt(string message)
        {
            if (_interactionPrompt != null)
            {
                _interactionPrompt.SetActive(true);

                var text = _interactionPrompt.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    text.text = message;
                }
            }
        }

        public void HideInteractionPrompt()
        {
            if (_interactionPrompt != null)
            {
                _interactionPrompt.SetActive(false);
            }
        }

        #endregion

        #region Backpack

        private void UpdateBackpackContent()
        {
            // TODO: Populate backpack with player data
            Debug.Log("[UIManager] Updating backpack content");
        }

        #endregion

        #region Utility

        private void HideAllPanels()
        {
            if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
            if (_hudPanel != null) _hudPanel.SetActive(false);
            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_settingsPanel != null) _settingsPanel.SetActive(false);
            if (_backpackPanel != null) _backpackPanel.SetActive(false);
            if (_loadingPanel != null) _loadingPanel.SetActive(false);
            if (_infoPanel != null) _infoPanel.SetActive(false);
            if (_relicPanel != null) _relicPanel.SetActive(false);
            if (_versePanel != null) _versePanel.SetActive(false);
            if (_dialoguePanel != null) _dialoguePanel.SetActive(false);
            if (_puzzlePanel != null) _puzzlePanel.SetActive(false);
        }

        private void SetPanelActive(GameObject panel, bool active)
        {
            if (panel != null)
            {
                panel.SetActive(active);
            }
        }

        #endregion

        #region Button Handlers

        public void OnNewGameClicked()
        {
            GameManager.Instance?.NewGame();
        }

        public void OnLoadGameClicked()
        {
            // TODO: Show load game screen
            Debug.Log("[UIManager] Load game clicked");
        }

        public void OnSettingsClicked()
        {
            ShowSettings();
        }

        public void OnResumeClicked()
        {
            HidePauseMenu();
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }

        #endregion
    }
}
