using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode_SO : ScriptableObject
{
    // Node identifier
    public int nodeID;
    public string nodeName;

    // Speaker information
    [Header("Speaker Information")]
    [Tooltip("Speaker's name (will be localized)")]
    public string speakerName;

    [Header("Portuguese Text")]
    [Tooltip("Speaker's name in Portuguese")]
    public string speakerNamePT = "Padre Elias";

    [Tooltip("Dialogue text in Portuguese")]
    [TextArea(3, 10)]
    public string dialogueTextPT;

    [Header("English Text")]
    [Tooltip("Speaker's name in English")]
    public string speakerNameEN = "Father Elias";

    [Tooltip("Dialogue text in English")]
    [TextArea(3, 10)]
    public string dialogueTextEN;

    [Header("Spanish Text")]
    [Tooltip("Speaker's name in Spanish")]
    public string speakerNameES = "Padre Elías";

    [Tooltip("Dialogue text in Spanish")]
    [TextArea(3, 10)]
    public string dialogueTextES;

    // Dialogue choices
    [Header("Dialogue Choices")]
    public DialogueChoice_SO[] choices;

    // Method to get localized speaker name
    public string GetLocalizedSpeakerName(SystemLanguage language = SystemLanguage.Unknown)
    {
        SystemLanguage lang = language == SystemLanguage.Unknown ? Application.systemLanguage : language;

        switch (lang)
        {
            case SystemLanguage.Portuguese:
                return speakerNamePT;
            case SystemLanguage.English:
                return speakerNameEN;
            case SystemLanguage.Spanish:
                return speakerNameES;
            default:
                return speakerNameEN; // Default to English
        }
    }

    // Method to get localized dialogue text
    public string GetLocalizedDialogueText(SystemLanguage language = SystemLanguage.Unknown)
    {
        SystemLanguage lang = language == SystemLanguage.Unknown ? Application.systemLanguage : language;

        switch (lang)
        {
            case SystemLanguage.Portuguese:
                return dialogueTextPT;
            case SystemLanguage.English:
                return dialogueTextEN;
            case SystemLanguage.Spanish:
                return dialogueTextES;
            default:
                return dialogueTextEN; // Default to English
        }
    }

    // Method to validate the dialogue node
    public void ValidateNode()
    {
        // Ensure we have at least one language text
        if (string.IsNullOrEmpty(dialogueTextPT) && string.IsNullOrEmpty(dialogueTextEN) && string.IsNullOrEmpty(dialogueTextES))
        {
            Debug.LogWarning($"Dialogue Node {nodeName} has no text in any language!", this);
        }

        // Ensure choices have valid next node indices
        if (choices != null)
        {
            foreach (var choice in choices)
            {
                choice.ValidateChoice();
            }
        }
    }
}

[Serializable]
public class DialogueChoice_SO
{
    [Header("Choice Information")]
    [Tooltip("Choice text (will be localized)")]
    public string choiceText;

    [Header("Portuguese Text")]
    [Tooltip("Choice text in Portuguese")]
    public string choiceTextPT;

    [Header("English Text")]
    [Tooltip("Choice text in English")]
    public string choiceTextEN;

    [Header("Spanish Text")]
    [Tooltip("Choice text in Spanish")]
    public string choiceTextES;

    [Tooltip("Index of the next dialogue node")]
    public int nextNodeIndex = -1;

    // Method to get localized choice text
    public string GetLocalizedChoiceText(SystemLanguage language = SystemLanguage.Unknown)
    {
        SystemLanguage lang = language == SystemLanguage.Unknown ? Application.systemLanguage : language;

        switch (lang)
        {
            case SystemLanguage.Portuguese:
                return choiceTextPT;
            case SystemLanguage.English:
                return choiceTextEN;
            case SystemLanguage.Spanish:
                return choiceTextES;
            default:
                return choiceTextEN; // Default to English
        }
    }

    // Method to validate the choice
    public void ValidateChoice()
    {
        // Ensure we have at least one language text
        if (string.IsNullOrEmpty(choiceTextPT) && string.IsNullOrEmpty(choiceTextEN) && string.IsNullOrEmpty(choiceTextES))
        {
            Debug.LogWarning($"Choice has no text in any language!", this);
        }

        // Ensure next node index is valid
        if (nextNodeIndex < 0)
        {
            Debug.LogWarning($"Choice has invalid next node index: {nextNodeIndex}", this);
        }
    }
}

// Editor script for dialogue node validation
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;

[CustomEditor(typeof(DialogueNode_SO))]
public class DialogueNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueNode_SO dialogueNode = (DialogueNode_SO)target;

        // Validation button
        if (GUILayout.Button("Validate Node"))
        {
            dialogueNode.ValidateNode();
            EditorUtility.SetDirty(dialogueNode);
        }

        // Language preview buttons
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Preview Languages:");

        if (GUILayout.Button("Preview Portuguese"))
        {
            Debug.Log($"PT - Speaker: {dialogueNode.GetLocalizedSpeakerName(SystemLanguage.Portuguese)}");
            Debug.Log($"PT - Text: {dialogueNode.GetLocalizedDialogueText(SystemLanguage.Portuguese)}");
            if (dialogueNode.choices != null)
            {
                foreach (var choice in dialogueNode.choices)
                {
                    Debug.Log($"PT - Choice: {choice.GetLocalizedChoiceText(SystemLanguage.Portuguese)}");
                }
            }
        }

        if (GUILayout.Button("Preview English"))
        {
            Debug.Log($"EN - Speaker: {dialogueNode.GetLocalizedSpeakerName(SystemLanguage.English)}");
            Debug.Log($"EN - Text: {dialogueNode.GetLocalizedDialogueText(SystemLanguage.English)}");
            if (dialogueNode.choices != null)
            {
                foreach (var choice in dialogueNode.choices)
                {
                    Debug.Log($"EN - Choice: {choice.GetLocalizedChoiceText(SystemLanguage.English)}");
                }
            }
        }

        if (GUILayout.Button("Preview Spanish"))
        {
            Debug.Log($"ES - Speaker: {dialogueNode.GetLocalizedSpeakerName(SystemLanguage.Spanish)}");
            Debug.Log($"ES - Text: {dialogueNode.GetLocalizedDialogueText(SystemLanguage.Spanish)}");
            if (dialogueNode.choices != null)
            {
                foreach (var choice in dialogueNode.choices)
                {
                    Debug.Log($"ES - Choice: {choice.GetLocalizedChoiceText(SystemLanguage.Spanish)}");
                }
            }
        }
    }
}
#endif