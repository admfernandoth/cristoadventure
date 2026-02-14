using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [Header("Dialogue References")]
    [Tooltip("Dialogue scriptable object containing the conversation tree")]
    public FatherEliasDialogues dialogueData;

    [Header("UI References")]
    [Tooltip("UI panel for dialogue display")]
    public GameObject dialoguePanel;

    [Tooltip("Text component for speaker name")]
    public TextMeshProUGUI speakerNameText;

    [Tooltip("Text component for dialogue text")]
    public TextMeshProUGUI dialogueText;

    [Tooltip("Container for choice buttons")]
    public Transform choicesContainer;

    [Tooltip("Button prefab for dialogue choices")]
    public GameObject choiceButtonPrefab;

    [Header("Audio Settings")]
    [Tooltip("Audio source for voice narration")]
    public AudioSource audioSource;

    [Tooltip("Voice volume")]
    [Range(0f, 1f)]
    public float voiceVolume = 0.8f;

    [Header("Settings")]
    [Tooltip("Time between characters in typing effect (seconds)")]
    public float typingSpeed = 0.03f;

    [Tooltip("Whether to play voice narration")]
    public bool playVoice = true;

    private DialogueNode currentNode;
    private List<GameObject> choiceButtons = new List<GameObject>();
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private SystemLanguage currentLanguage;

    // Event for dialogue state changes
    public System.Action<DialogueNode> OnDialogueStart;
    public System.Action<DialogueNode> OnDialogueEnd;
    public System.Action OnChoiceMade;

    private void Awake()
    {
        currentLanguage = Application.systemLanguage;
    }

    private void Start()
    {
        // Hide dialogue panel initially
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    // Start the dialogue
    public void StartDialogue()
    {
        if (dialogueData == null)
        {
            Debug.LogError("Dialogue data not assigned!");
            return;
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        currentNode = dialogueData.GetStartingNode();
        DisplayCurrentNode();
        OnDialogueStart?.Invoke(currentNode);
    }

    // End the dialogue
    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // Clear choice buttons
        foreach (var button in choiceButtons)
        {
            Destroy(button);
        }
        choiceButtons.Clear();

        currentNode = null;
        OnDialogueEnd?.Invoke(currentNode);
    }

    // Display current dialogue node
    private void DisplayCurrentNode()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        // Update speaker name
        if (speakerNameText != null)
        {
            speakerNameText.text = currentNode.GetLocalizedSpeakerName(currentLanguage);
        }

        // Start typing effect for dialogue text
        if (dialogueText != null)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            string text = currentNode.GetLocalizedDialogueText(currentLanguage);
            typingCoroutine = StartCoroutine(TypeText(text));

            // Play voice narration if enabled
            if (playVoice && audioSource != null)
            {
                // Note: You would need actual audio clips for voice narration
                // This is a placeholder implementation
                PlayVoiceNarration(text);
            }
        }

        // Display choices
        DisplayChoices();
    }

    // Typing effect coroutine
    private System.Collections.IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        int characterIndex = 0;

        while (characterIndex < text.Length)
        {
            dialogueText.text += text[characterIndex];
            characterIndex++;

            // Add slight pause for punctuation
            if (text[characterIndex - 1] == '.' || text[characterIndex - 1] == '!' || text[characterIndex - 1] == '?')
            {
                yield return new WaitForSeconds(typingSpeed * 3);
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
    }

    // Display dialogue choices
    private void DisplayChoices()
    {
        // Clear existing choice buttons
        foreach (var button in choiceButtons)
        {
            Destroy(button);
        }
        choiceButtons.Clear();

        // Create new choice buttons
        if (currentNode.choices != null && currentNode.choices.Length > 0)
        {
            for (int i = 0; i < currentNode.choices.Length; i++)
            {
                GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer);
                Button button = buttonObj.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    buttonText.text = currentNode.choices[i].GetLocalizedChoiceText(currentLanguage);
                }

                int choiceIndex = i; // Capture the index for the lambda
                button.onClick.AddListener(() => MakeChoice(choiceIndex));

                choiceButtons.Add(buttonObj);
            }

            // Enable/disable choices based on typing state
            UpdateChoiceInteractable();
        }
    }

    // Make a dialogue choice
    private void MakeChoice(int choiceIndex)
    {
        if (choiceIndex >= 0 && choiceIndex < currentNode.choices.Length)
        {
            DialogueChoice choice = currentNode.choices[choiceIndex];
            currentNode = dialogueData.GetNode(choice.nextNodeIndex);

            // Clear choice buttons immediately
            foreach (var button in choiceButtons)
            {
                Destroy(button);
            }
            choiceButtons.Clear();

            OnChoiceMade?.Invoke();

            if (currentNode != null)
            {
                DisplayCurrentNode();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    // Update choice button interactability
    private void UpdateChoiceInteractable()
    {
        foreach (var button in choiceButtons)
        {
            Button btn = button.GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = !isTyping;
            }
        }
    }

    // Play voice narration (placeholder)
    private void PlayVoiceNarration(string text)
    {
        // In a real implementation, you would:
        // 1. Map text to appropriate audio clips
        // 2. Play the corresponding audio clip
        // 3. Handle text-to-speech if clips aren't available

        // For now, just set the volume
        if (audioSource != null)
        {
            audioSource.volume = voiceVolume;
        }
    }

    // Change language during dialogue
    public void ChangeLanguage(SystemLanguage language)
    {
        currentLanguage = language;
        if (currentNode != null)
        {
            // Re-display current node with new language
            DisplayCurrentNode();
        }
    }

    // Check if dialogue is active
    public bool IsDialogueActive()
    {
        return currentNode != null;
    }

    // Get current dialogue node
    public DialogueNode GetCurrentNode()
    {
        return currentNode;
    }
}