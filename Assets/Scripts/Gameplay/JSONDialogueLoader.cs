using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Loads dialogue data from JSON files instead of ScriptableObjects
    /// This allows content to be created without Unity Editor
    /// </summary>
    public static class JSONDialogueLoader
    {
        private static Dictionary<string, DialogueDataJSON> loadedDialogues = new Dictionary<string, DialogueDataJSON>();
        private static readonly string DIALOGUE_DATA_PATH = "Assets/Phases/Chapter1/Dialogue/";

        [System.Serializable]
        public class DialogueDataJSON
        {
            public string dialogueId;
            public string npcName;
            public string npcId;
            public string location;
            public string portraitSpritePath;
            public DialogueTreeData dialogueTree;
        }

        [System.Serializable]
        public class DialogueTreeData
        {
            public string startNodeId;
            public DialogueNode[] nodes;
        }

        [System.Serializable]
        public class DialogueNode
        {
            public string id;
            public string type;
            public string speaker;
            public string[] lines_PT;
            public string[] lines_EN;
            public string[] lines_ES;
            public DialogueChoice[] choices;
            public bool endsDialogue;
        }

        [System.Serializable]
        public class DialogueChoice
        {
            public string text_PT;
            public string text_EN;
            public string text_ES;
            public string nextNode;
        }

        /// <summary>
        /// Load dialogue by its ID
        /// </summary>
        public static DialogueDataJSON LoadDialogue(string dialogueId)
        {
            if (loadedDialogues.ContainsKey(dialogueId))
            {
                return loadedDialogues[dialogueId];
            }

            string filePath = Path.Combine(DIALOGUE_DATA_PATH, $"{dialogueId}.json");

            if (!File.Exists(filePath))
            {
                Debug.LogError($"Dialogue data file not found: {filePath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var dialogueData = JsonUtility.FromJson<DialogueDataJSON>(json);
                loadedDialogues[dialogueId] = dialogueData;
                return dialogueData;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load dialogue data: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get the starting node for a dialogue
        /// </summary>
        public static DialogueNode GetStartNode(string dialogueId)
        {
            var dialogue = LoadDialogue(dialogueId);
            if (dialogue == null || dialogue.dialogueTree == null) return null;

            return GetNodeById(dialogue, dialogue.dialogueTree.startNodeId);
        }

        /// <summary>
        /// Get a specific node by ID
        /// </summary>
        public static DialogueNode GetNodeById(string dialogueId, string nodeId)
        {
            var dialogue = LoadDialogue(dialogueId);
            if (dialogue == null) return null;

            return GetNodeById(dialogue, nodeId);
        }

        private static DialogueNode GetNodeById(DialogueDataJSON dialogue, string nodeId)
        {
            if (dialogue.dialogueTree?.nodes == null) return null;

            foreach (var node in dialogue.dialogueTree.nodes)
            {
                if (node.id == nodeId)
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// Get dialogue lines for a node in the specified language
        /// </summary>
        public static string[] GetNodeLines(string dialogueId, string nodeId, string languageCode = "PT")
        {
            var node = GetNodeById(dialogueId, nodeId);
            if (node == null) return new string[0];

            switch (languageCode.ToUpper())
            {
                case "PT":
                case "PT-BR":
                    return node.lines_PT;
                case "EN":
                    return node.lines_EN;
                case "ES":
                    return node.lines_ES;
                default:
                    return node.lines_EN;
            }
        }

        /// <summary>
        /// Get choices for a node in the specified language
        /// </summary>
        public static List<(string text, string nextNode)> GetNodeChoices(string dialogueId, string nodeId, string languageCode = "PT")
        {
            var node = GetNodeById(dialogueId, nodeId);
            if (node?.choices == null) return new List<(string, string)>();

            var choices = new List<(string, string)>();
            foreach (var choice in node.choices)
            {
                string text;
                switch (languageCode.ToUpper())
                {
                    case "PT":
                    case "PT-BR":
                        text = choice.text_PT;
                        break;
                    case "EN":
                        text = choice.text_EN;
                        break;
                    case "ES":
                        text = choice.text_ES;
                        break;
                    default:
                        text = choice.text_EN;
                        break;
                }
                choices.Add((text, choice.nextNode));
            }
            return choices;
        }

        /// <summary>
        /// Check if a node ends the dialogue
        /// </summary>
        public static bool NodeEndsDialogue(string dialogueId, string nodeId)
        {
            var node = GetNodeById(dialogueId, nodeId);
            return node?.endsDialogue ?? false;
        }

        /// <summary>
        /// Get NPC name from dialogue
        /// </summary>
        public static string GetNPCName(string dialogueId)
        {
            var dialogue = LoadDialogue(dialogueId);
            return dialogue?.npcName ?? "Unknown";
        }

        /// <summary>
        /// Get portrait path for dialogue
        /// </summary>
        public static string GetPortraitPath(string dialogueId)
        {
            var dialogue = LoadDialogue(dialogueId);
            return dialogue?.portraitSpritePath ?? "";
        }

        /// <summary>
        /// Clear loaded dialogues cache
        /// </summary>
        public static void ClearCache()
        {
            loadedDialogues.Clear();
        }

        /// <summary>
        /// Reload a dialogue from disk
        /// </summary>
        public static DialogueDataJSON ReloadDialogue(string dialogueId)
        {
            if (loadedDialogues.ContainsKey(dialogueId))
            {
                loadedDialogues.Remove(dialogueId);
            }
            return LoadDialogue(dialogueId);
        }
    }
}
