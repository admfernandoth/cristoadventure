using System;
using UnityEngine;
using System.Collections.Generic;

// Custom attribute to mark dialogue data for localization
public class LocalizedDialogue : Attribute
{
    public string key;
    public LocalizedDialogue(string key) { this.key = key; }
}

// Dialogue choice structure
[Serializable]
public class DialogueChoice
{
    [Localized("choice")]
    public string choiceText;

    [HideInInspector]
    public string choiceTextKey;

    public int nextNodeIndex;
}

// Dialogue node structure
[Serializable]
public class DialogueNode
{
    [Localized("speaker")]
    public string speakerName;

    [HideInInspector]
    public string speakerNameKey;

    [Localized("dialogue")]
    public string dialogueText;

    [HideInInspector]
    public string dialogueTextKey;

    public DialogueChoice[] choices;

    // Language-specific versions
    public string speakerNamePT;
    public string speakerNameEN;
    public string speakerNameES;

    public string dialogueTextPT;
    public string dialogueTextEN;
    public string dialogueTextES;

    // Method to get localized text based on current language
    public string GetLocalizedSpeakerName(SystemLanguage language = SystemLanguage.Unknown)
    {
        if (language == SystemLanguage.Unknown)
            language = Application.systemLanguage;

        switch (language)
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

    public string GetLocalizedDialogueText(SystemLanguage language = SystemLanguage.Unknown)
    {
        if (language == SystemLanguage.Unknown)
            language = Application.systemLanguage;

        switch (language)
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
}

// Main dialogue system for Father Elias
[CreateAssetMenu(fileName = "FatherEliasDialogues", menuName = "Dialogue/Father Elias Dialogues")]
public class FatherEliasDialogues : ScriptableObject
{
    public List<DialogueNode> dialogueNodes;

    // Root node (starting point)
    public int rootNodeIndex = 0;

    // Get current language
    private SystemLanguage GetCurrentLanguage()
    {
        return Application.systemLanguage;
    }

    // Get dialogue node by index
    public DialogueNode GetNode(int index)
    {
        if (index >= 0 && index < dialogueNodes.Count)
        {
            return dialogueNodes[index];
        }
        return null;
    }

    // Get starting node
    public DialogueNode GetStartingNode()
    {
        return GetNode(rootNodeIndex);
    }

    // Initialize dialogue nodes with default values
    private void OnEnable()
    {
        if (dialogueNodes == null || dialogueNodes.Count == 0)
        {
            InitializeDialogue();
        }
    }

    // Initialize the dialogue tree
    private void InitializeDialogue()
    {
        dialogueNodes = new List<DialogueNode>();

        // Node 0: Initial greeting
        DialogueNode greetingNode = new DialogueNode
        {
            speakerName = "Padre Elias",
            speakerNamePT = "Padre Elias",
            speakerNameEN = "Father Elias",
            speakerNameES = "Padre Elías",

            dialogueText = "Paz esteja com você, peregrino. Bem-vindo à Basílica da Natividade.",
            dialogueTextPT = "Paz esteja com você, peregrino. Bem-vindo à Basílica da Natividade.",
            dialogueTextEN = "Peace be with you, pilgrim. Welcome to the Church of the Nativity.",
            dialogueTextES = "La paz esté contigo, peregrino. Bienvenido a la Basílica de la Natividad.",

            choices = new DialogueChoice[]
            {
                new DialogueChoice
                {
                    choiceText = "O que há de especial neste lugar?",
                    choiceTextKey = "choice_1",
                    nextNodeIndex = 1
                },
                new DialogueChoice
                {
                    choiceText = "Conte-me sobre a estrela.",
                    choiceTextKey = "choice_2",
                    nextNodeIndex = 2
                },
                new DialogueChoice
                {
                    choiceText = "Por que Belém é importante?",
                    choiceTextKey = "choice_3",
                    nextNodeIndex = 3
                }
            }
        };

        dialogueNodes.Add(greetingNode);

        // Node 1: Response to "What's special about this place?"
        DialogueNode specialPlaceNode = new DialogueNode
        {
            speakerName = "Padre Elias",
            speakerNamePT = "Padre Elias",
            speakerNameEN = "Father Elias",
            speakerNameES = "Padre Elías",

            dialogueText = "Este é um dos lugares mais sagrados do cristianismo. Aqui, nesta gruta, Deus se fez homem e habitou entre nós. Sinta a presença sagrada deste local.",
            dialogueTextPT = "Este é um dos lugares mais sagrados do cristianismo. Aqui, nesta gruta, Deus se fez homem e habitou entre nós. Sinta a presença sagrada deste local.",
            dialogueTextEN = "This is one of the most sacred places in Christianity. Here, in this grotto, God became man and dwelt among us. Feel the sacred presence of this place.",
            dialogueTextES = "Este es uno de los lugares más sagrados del cristianismo. Aquí, en esta gruta, Dios se hizo hombre y habitó entre nosotros. Siente la presencia sagrada de este lugar.",

            choices = new DialogueChoice[0] // End of dialogue branch
        };

        dialogueNodes.Add(specialPlaceNode);

        // Node 2: Response to "Tell me about the star"
        DialogueNode starNode = new DialogueNode
        {
            speakerName = "Padre Elias",
            speakerNamePT = "Padre Elias",
            speakerNameEN = "Father Elias",
            speakerNameES = "Padre Elías",

            dialogueText = "A Estrela de Prata tem 14 pontas e marca o local exato do nascimento. Peregrinos de todo o mundo vêm aqui para orar e reverenciar este santo lugar.",
            dialogueTextPT = "A Estrela de Prata tem 14 pontas e marca o local exato do nascimento. Peregrinos de todo o mundo vêm aqui para orar e reverenciar este santo lugar.",
            dialogueTextEN = "The Silver Star has 14 points and marks the exact location of the birth. Pilgrims from all over the world come here to pray and venerate this holy place.",
            dialogueTextES = "La Estrella de Plata tiene 14 puntas y marca el lugar exacto del nacimiento. Peregrinos de todo el mundo vienen aquí para rezar y venerar este santo lugar.",

            choices = new DialogueChoice[0] // End of dialogue branch
        };

        dialogueNodes.Add(starNode);

        // Node 3: Response to "Why is Bethlehem important?"
        DialogueNode bethlehemNode = new DialogueNode
        {
            speakerName = "Padre Elias",
            speakerNamePT = "Padre Elias",
            speakerNameEN = "Father Elias",
            speakerNameES = "Padre Elías",

            dialogueText = "Belém significa 'Casa de Pão' em hebraico. É aqui que nasceu Aquele que se chamaria o Pão da Vida. Profecias de séculos antes previram este local.",
            dialogueTextPT = "Belém significa 'Casa de Pão' em hebraico. É aqui que nasceu Aquele que se chamaria o Pão da Vida. Profecias de séculos antes previram este local.",
            dialogueTextEN = "Bethlehem means 'House of Bread' in Hebrew. It is here that was born He who would be called the Bread of Life. Prophecies centuries before foretold this place.",
            dialogueTextES = "Belén significa 'Casa del Pan' en hebreo. Es aquí donde nació Aquel que sería llamado el Pan de Vida. Profecías de siglos antes predijeron este lugar.",

            choices = new DialogueChoice[0] // End of dialogue branch
        };

        dialogueNodes.Add(bethlehemNode);
    }
}

// Dialogue manager for controlling conversation flow
public class DialogueManager : MonoBehaviour
{
    public FatherEliasDialogues fatherEliasDialogues;

    private DialogueNode currentNode;
    private SystemLanguage currentLanguage;

    void Start()
    {
        currentLanguage = Application.systemLanguage;
    }

    public void StartDialogue()
    {
        currentNode = fatherEliasDialogues.GetStartingNode();
        DisplayCurrentNode();
    }

    public void DisplayCurrentNode()
    {
        if (currentNode != null)
        {
            Debug.Log(currentNode.GetLocalizedSpeakerName(currentLanguage) + ": " +
                     currentNode.GetLocalizedDialogueText(currentLanguage));

            // Display choices if available
            if (currentNode.choices != null && currentNode.choices.Length > 0)
            {
                Debug.Log("Choose your response:");
                for (int i = 0; i < currentNode.choices.Length; i++)
                {
                    Debug.Log((i + 1) + ". " + currentNode.choices[i].choiceText);
                }
            }
            else
            {
                Debug.Log("End of dialogue.");
                currentNode = null;
            }
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (currentNode != null &&
            choiceIndex >= 0 &&
            choiceIndex < currentNode.choices.Length)
        {
            DialogueChoice choice = currentNode.choices[choiceIndex];
            currentNode = fatherEliasDialogues.GetNode(choice.nextNodeIndex);
            DisplayCurrentNode();
        }
    }

    public void SetLanguage(SystemLanguage language)
    {
        currentLanguage = language;
        if (currentNode != null)
        {
            DisplayCurrentNode();
        }
    }
}

// Helper class for localization
public static class LocalizationSystem
{
    public static SystemLanguage GetCurrentLanguage()
    {
        return Application.systemLanguage;
    }

    public static string GetLocalizedText(string baseText, string ptText, string enText, string esText, SystemLanguage language = SystemLanguage.Unknown)
    {
        if (language == SystemLanguage.Unknown)
            language = GetCurrentLanguage();

        switch (language)
        {
            case SystemLanguage.Portuguese:
                return ptText;
            case SystemLanguage.English:
                return enText;
            case SystemLanguage.Spanish:
                return esText;
            default:
                return enText; // Default to English
        }
    }
}