using UnityEngine;
using UnityEngine.UI;

namespace CristoAdventure.Localization
{
    /// <summary>
    /// Example implementations showing how to use the localization system
    /// Created by: Agent-12 (Narrative Designer)
    /// Date: 14/02/2026
    /// </summary>

    // ========== EXAMPLE 1: Localized Text Component ==========
    /// <summary>
    /// Attach this component to any Text/TMP_Text component to automatically localize it
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {
        [Header("Localization Settings")]
        [SerializeField] private LocalizationTables.Key localizationKey;
        [SerializeField] private bool updateOnLanguageChange = true;

        private Text textComponent;
        private string[] formatArgs;

        private void Awake()
        {
            textComponent = GetComponent<Text>();
        }

        private void OnEnable()
        {
            if (updateOnLanguageChange)
            {
                LocalizationDictionary.OnLanguageChanged += OnLanguageChanged;
            }
            UpdateText();
        }

        private void OnDisable()
        {
            if (updateOnLanguageChange)
            {
                LocalizationDictionary.OnLanguageChanged -= OnLanguageChanged;
            }
        }

        private void OnLanguageChanged(SystemLanguage newLanguage)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (textComponent != null)
            {
                textComponent.text = L.Get(localizationKey);
            }
        }

        /// <summary>
        /// Set format arguments for formatted strings
        /// </summary>
        public void SetFormatArgs(params object[] args)
        {
            formatArgs = args;
            if (textComponent != null)
            {
                textComponent.text = L.GetFormatted(localizationKey, args);
            }
        }
    }

    // ========== EXAMPLE 2: POI Content Display ==========
    /// <summary>
    /// Displays multi-line content for POI plaques
    /// </summary>
    public class POIContentDisplay : MonoBehaviour
    {
        [Header("Content Keys")]
        [SerializeField] private LocalizationTables.Key titleKey;
        [SerializeField] private LocalizationTables.Key line1Key;
        [SerializeField] private LocalizationTables.Key line2Key;
        [SerializeField] private LocalizationTables.Key line3Key;

        [Header("UI References")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text contentText;
        [SerializeField] private string lineSeparator = "\n\n";

        private void Start()
        {
            DisplayContent();
        }

        public void DisplayContent()
        {
            if (titleText != null)
            {
                titleText.text = L.Get(titleKey);
            }

            if (contentText != null && LocalizationDictionary.Instance != null)
            {
                string[] lines = LocalizationDictionary.Instance.GetContentLines(
                    line1Key,
                    line2Key,
                    line3Key
                );
                contentText.text = string.Join(lineSeparator, lines);
            }
        }
    }

    // ========== EXAMPLE 3: NPC Dialogue System ==========
    /// <summary>
    /// Manages NPC dialogue with localization
    /// </summary>
    public class NPCDialogueSystem : MonoBehaviour
    {
        [Header("NPC Info")]
        [SerializeField] private LocalizationTables.Key npcNameKey;

        [Header("UI References")]
        [SerializeField] private Text nameText;
        [SerializeField] private Text dialogueText;
        [SerializeField] private GameObject[] optionButtons;

        private int currentOptionIndex = 0;

        private void Start()
        {
            ShowGreeting();
        }

        private void ShowGreeting()
        {
            if (nameText != null)
            {
                nameText.text = L.Get(npcNameKey);
            }

            // Show greeting and options
            if (dialogueText != null)
            {
                dialogueText.text = L.Get(LocalizationTables.Key.NPC001_Greeting);
            }

            ShowDialogueOptions();
        }

        private void ShowDialogueOptions()
        {
            string[] options = LocalizationDictionary.Instance.GetNPCOptions();

            for (int i = 0; i < optionButtons.Length && i < options.Length; i++)
            {
                if (optionButtons[i] != null)
                {
                    Text buttonText = optionButtons[i].GetComponentInChildren<Text>();
                    if (buttonText != null)
                    {
                        buttonText.text = options[i];
                    }
                    optionButtons[i].SetActive(true);
                }
            }
        }

        public void OnOptionSelected(int optionIndex)
        {
            currentOptionIndex = optionIndex;
            string[] responses = LocalizationDictionary.Instance.GetNPCResponses();

            if (dialogueText != null && optionIndex < responses.Length)
            {
                dialogueText.text = responses[optionIndex];
            }

            // Hide options after selection
            foreach (var button in optionButtons)
            {
                if (button != null)
                {
                    button.SetActive(false);
                }
            }
        }
    }

    // ========== EXAMPLE 4: Puzzle System Integration ==========
    /// <summary>
    /// Manages localized puzzle content
    /// </summary>
    public class LocalizedPuzzle : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text titleText;
        [SerializeField] private Text instructionsText;
        [SerializeField] private Text hintText;

        [Header("Puzzle Elements")]
        [SerializeField] private DraggablePuzzleElement[] puzzleElements;

        private string[] events;
        private string[] hints;
        private int hintIndex = 0;

        private void Start()
        {
            InitializePuzzle();
        }

        private void InitializePuzzle()
        {
            if (titleText != null)
            {
                titleText.text = L.Get(LocalizationTables.Key.Puzzle_Title);
            }

            if (instructionsText != null)
            {
                instructionsText.text = L.Get(LocalizationTables.Key.Puzzle_Instructions);
            }

            // Get puzzle events
            events = LocalizationDictionary.Instance.GetPuzzleEvents();

            // Initialize draggable elements
            if (puzzleElements != null)
            {
                for (int i = 0; i < puzzleElements.Length && i < events.Length; i++)
                {
                    if (puzzleElements[i] != null)
                    {
                        puzzleElements[i].SetText(events[i]);
                    }
                }
            }

            hints = LocalizationDictionary.Instance.GetPuzzleHints();
        }

        public void ShowHint()
        {
            if (hintText != null && hintIndex < hints.Length)
            {
                hintText.text = hints[hintIndex];
                hintIndex++;
            }
        }

        public void OnPuzzleComplete()
        {
            if (hintText != null)
            {
                hintText.text = L.Get(LocalizationTables.Key.Puzzle_Success);
            }
        }
    }

    // ========== EXAMPLE 5: Language Settings UI ==========
    /// <summary>
    /// Language selector dropdown implementation
    /// </summary>
    public class LanguageSelector : MonoBehaviour
    {
        [SerializeField] private Dropdown languageDropdown;

        private void Start()
        {
            if (languageDropdown != null)
            {
                PopulateDropdown();
                languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            }
        }

        private void PopulateDropdown()
        {
            languageDropdown.ClearOptions();

            var languages = LocalizationDictionary.GetSupportedLanguages();
            var options = new System.Collections.Generic.List<string>();

            foreach (var lang in languages)
            {
                options.Add(LocalizationDictionary.GetLanguageNativeName(lang));
            }

            languageDropdown.AddOptions(options);

            // Set current language
            SystemLanguage current = LocalizationDictionary.Instance.GetCurrentLanguage();
            languageDropdown.value = languages.IndexOf(current);
        }

        private void OnLanguageChanged(int index)
        {
            var languages = LocalizationDictionary.GetSupportedLanguages();
            if (index >= 0 && index < languages.Count)
            {
                LocalizationDictionary.Instance.SetLanguage(languages[index]);
            }
        }
    }

    // ========== HELPER CLASS: Draggable Puzzle Element ==========
    public class DraggablePuzzleElement : MonoBehaviour
    {
        [SerializeField] private Text textComponent;

        public void SetText(string text)
        {
            if (textComponent != null)
            {
                textComponent.text = text;
            }
        }
    }
}
