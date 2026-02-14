using UnityEngine;
using System.Collections;
using TMPro;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Manages NPC dialogue system with educational content
    /// Supports multi-language dialogues with branching choices
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton

        private static DialogueManager _instance;
        public static DialogueManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_DialogueManager");
                    _instance = go.AddComponent<DialogueManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region UI References

        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private TextMeshProUGUI _speakerNameText;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private Image _speakerPortrait;
        [SerializeField] private Transform _choicesContainer;
        [SerializeField] private GameObject _choiceButtonPrefab;

        #endregion

        #region State

        private DialogueNode _currentNode;
        private bool _isDialogueActive = false;
        private Coroutine _typingCoroutine;

        #endregion

        #region Settings

        [Header("Settings")]
        [SerializeField] private float _typingSpeed = 0.05f;
        [SerializeField] private bool _skipTypingOnClick = true;

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

        private void Update()
        {
            // Handle click to advance or skip typing
            if (_isDialogueActive && Input.GetMouseButtonDown(0))
            {
                if (_skipTypingClicked()) return;

                // Check for choices
                if (_currentNode != null && _currentNode.HasChoices)
                {
                    // Choices are already shown, don't advance
                }
                else
                {
                    AdvanceDialogue();
                }
            }
        }

        #endregion

        #region Dialogue Control

        public void StartDialogue(string speakerName, DialogueNode startingNode)
        {
            if (startingNode == null)
            {
                Debug.LogError("[DialogueManager] Cannot start dialogue - node is null");
                return;
            }

            _currentNode = startingNode;
            _isDialogueActive = true;

            // Show panel
            if (_dialoguePanel != null)
                _dialoguePanel.SetActive(true);

            // Display first node
            DisplayNode(_currentNode);

            // Pause game
            GameManager.Instance?.SetState(GameManager.GameState.InDialogue);
        }

        public void EndDialogue()
        {
            _isDialogueActive = false;
            _currentNode = null;

            // Stop typing if active
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }

            // Hide panel
            if (_dialoguePanel != null)
                _dialoguePanel.SetActive(false);

            // Clear choices
            ClearChoices();

            // Resume game
            if (GameManager.Instance?.CurrentGameState == GameManager.GameState.InDialogue)
            {
                GameManager.Instance?.SetState(GameManager.GameState.Playing);
            }
        }

        private void AdvanceDialogue()
        {
            if (_currentNode == null)
            {
                EndDialogue();
                return;
            }

            // If has choices, wait for player input
            if (_currentNode.HasChoices)
            {
                return;
            }

            // Move to next node (if single path)
            // For simple linear dialogue, you can add a "nextNode" field to DialogueNode
            // For now, just end
            EndDialogue();
        }

        #endregion

        #region Display

        private void DisplayNode(DialogueNode node)
        {
            if (node == null) return;

            // Set speaker name
            if (_speakerNameText != null)
            {
                _speakerNameText.text = GetLocalizedString(node.speakerName);
            }

            // Set portrait
            if (_speakerPortrait != null && node.speakerPortrait != null)
            {
                _speakerPortrait.sprite = node.speakerPortrait;
            }

            // Type out dialogue text
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }
            _typingCoroutine = StartCoroutine(TypeText(node.dialogueText));

            // Show choices if available
            if (node.HasChoices)
            {
                ShowChoices(node.choices);
            }
        }

        private IEnumerator TypeText(string text)
        {
            string localizedText = GetLocalizedString(text);
            _dialogueText.text = "";

            foreach (char c in localizedText)
            {
                _dialogueText.text += c;
                yield return new WaitForSeconds(_typingSpeed);
            }

            _typingCoroutine = null;
        }

        private bool _typingSkipped = false;
        private bool _skipTypingClicked()
        {
            if (_typingCoroutine != null && _skipTypingClicked && !_typingSkipped)
            {
                // Complete typing immediately
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;

                string localizedText = GetLocalizedString(_currentNode.dialogueText);
                _dialogueText.text = localizedText;

                _typingSkipped = true;
                return true;
            }

            _typingSkipped = false;
            return false;
        }

        #endregion

        #region Choices

        private void ShowChoices(DialogueChoice[] choices)
        {
            ClearChoices();

            foreach (var choice in choices)
            {
                GameObject buttonObj = Instantiate(_choiceButtonPrefab, _choicesContainer);
                var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    buttonText.text = GetLocalizedString(choice.choiceText);
                }

                // Add click handler
                var button = buttonObj.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    string capturedChoice = choice.choiceText; // Capture for closure
                    button.onClick.AddListener(() => OnChoiceSelected(choice));
                }
            }
        }

        private void ClearChoices()
        {
            foreach (Transform child in _choicesContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnChoiceSelected(DialogueChoice choice)
        {
            Debug.Log($"[DialogueManager] Player selected: {choice.choiceText}");

            // Apply affection change
            // TODO: Implement NPC affection system

            // Move to next node
            if (choice.nextNode != null)
            {
                _currentNode = choice.nextNode;
                DisplayNode(_currentNode);
            }
            else
            {
                EndDialogue();
            }
        }

        #endregion

        #region Localization

        private string GetLocalizedString(string key)
        {
            // Check if key is a localization key (format: "Key.SubKey")
            if (key.Contains("."))
            {
                return LocalizationManager.Instance?.GetLocalizedString(key) ?? key;
            }

            // Return as-is if not a localization key
            return key;
        }

        #endregion

        #region Properties

        public bool IsDialogueActive => _isDialogueActive;

        #endregion
    }

    #region Dialogue Data Classes

    [System.Serializable]
    public class DialogueNode
    {
        public string speakerName;
        [Multiline(5)] public string dialogueText;
        public Sprite speakerPortrait;
        public DialogueChoice[] choices;

        public bool HasChoices => choices != null && choices.Length > 0;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public DialogueNode nextNode;
        public int affectionChange = 0;
    }

    #endregion
}
