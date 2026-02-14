using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateDialogueAsset
{
    [MenuItem("Assets/Create Dialogue/Father Elias Asset")]
    public static void CreateFatherEliasAsset()
    {
        // Create path for dialogue asset
        string path = "Assets/Phases/Chapter1/Dialogue/FatherElias_Dialogue.asset";

        // Check if asset already exists
        if (AssetDatabase.LoadAssetAtPath<FatherEliasDialogues>(path) != null)
        {
            EditorUtility.DisplayDialog("Asset Exists", "Father Elias dialogue asset already exists!", "OK");
            return;
        }

        // Create new asset
        FatherEliasDialogues dialogue = ScriptableObject.CreateInstance<FatherEliasDialogues>();
        AssetDatabase.CreateAsset(dialogue, path);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Asset Created", $"Father Elias dialogue asset created at:\n{path}", "OK");
    }

    [MenuItem("Assets/Create Dialogue/Import Father Elias Dialogue")]
    public static void ImportFatherEliasDialogue()
    {
        // Get the script file path
        string scriptPath = "Assets/Phases/Chapter1/Dialogue/FatherEliasDialogues.cs";

        if (!File.Exists(scriptPath))
        {
            EditorUtility.DisplayDialog("Error", "FatherEliasDialogues.cs not found!", "OK");
            return;
        }

        // Read and display the content
        string content = File.ReadAllText(scriptPath);
        EditorUtility.DisplayDialog("Father Elias Dialogue Script", content, "OK");
    }

    [MenuItem("Tools/Dialogue/Export Dialogue Data")]
    public static void ExportDialogueData()
    {
        string path = EditorUtility.SaveFilePanel(
            "Export Dialogue Data",
            Application.dataPath,
            "dialogue_export",
            "json"
        );

        if (!string.IsNullOrEmpty(path))
        {
            // Export dialogue system configuration
            string exportData = @"{
                ""DialogueSystem"": {
                    ""version"": ""1.0"",
                    ""supportedLanguages"": [""Portuguese"", ""English"", ""Spanish""],
                    ""nodesCount"": 4,
                    ""nodes"": [
                        {
                            ""id"": 0,
                            ""name"": ""Initial_Greeting"",
                            ""type"": ""Root""
                        },
                        {
                            ""id"": 1,
                            ""name"": ""Special_Place_Response"",
                            ""type"": ""Information""
                        },
                        {
                            ""id"": 2,
                            ""name"": ""Star_Information"",
                            ""type"": ""Information""
                        },
                        {
                            ""id"": 3,
                            ""name"": ""Bethlehem_History"",
                            ""type"": ""Information""
                        }
                    ],
                    ""features"": [
                        ""Multi-language support"",
                        ""Dialogue tree structure"",
                        ""Choice-based progression"",
                        ""Voice narration support"",
                        ""Typing animation""
                    ]
                }
            }";

            File.WriteAllText(path, exportData);
            Debug.Log($"Dialogue data exported to: {path}");
        }
    }
}

// Custom editor for FatherEliasDialogues
#if UNITY_EDITOR
[CustomEditor(typeof(FatherEliasDialogues))]
public class FatherEliasDialoguesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FatherEliasDialogues dialogue = (FatherEliasDialogues)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dialogue Tree Information", EditorStyles.boldLabel);

        // Display dialogue tree structure
        if (dialogue.dialogueNodes != null && dialogue.dialogueNodes.Count > 0)
        {
            EditorGUILayout.LabelField($"Total Nodes: {dialogue.dialogueNodes.Count}");
            EditorGUILayout.LabelField($"Root Node: {dialogue.rootNodeIndex}");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Node Structure:", EditorStyles.boldLabel);

            foreach (var node in dialogue.dialogueNodes)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField($"Node {dialogue.dialogueNodes.IndexOf(node)}:");
                EditorGUILayout.LabelField($"  Choices: {node.choices?.Length ?? 0}");
                EditorGUILayout.LabelField($"  PT: {node.dialogueTextPT.Substring(0, Mathf.Min(50, node.dialogueTextPT.Length))}...");
                EditorGUILayout.LabelField($"  EN: {node.dialogueTextEN.Substring(0, Mathf.Min(50, node.dialogueTextEN.Length))}...");
                EditorGUILayout.LabelField($"  ES: {node.dialogueTextES.Substring(0, Mathf.Min(50, node.dialogueTextES.Length))}...");
                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.Space();

        // Action buttons
        if (GUILayout.Button("Generate ScriptableObject"))
        {
            CreateScriptableObjectAsset(dialogue);
        }

        if (GUILayout.Button("Validate All Nodes"))
        {
            ValidateAllNodes(dialogue);
        }

        if (GUILayout.Button("Preview Dialogue"))
        {
            PreviewDialogue(dialogue);
        }
    }

    private void CreateScriptableObjectAsset(FatherEliasDialogues dialogue)
    {
        // Create a new ScriptableObject from the current dialogue
        string path = EditorUtility.SaveFilePanel(
            "Save Dialogue Asset",
            Application.dataPath,
            "FatherElias_Dialogue",
            "asset"
        );

        if (!string.IsNullOrEmpty(path))
        {
            // Ensure path is within Assets folder
            if (path.Contains("/Assets/"))
            {
                string assetPath = path.Substring(path.IndexOf("/Assets/") + "/Assets/".Length);
                FatherEliasDialogues newAsset = ScriptableObject.CreateInstance<FatherEliasDialogues>();

                // Copy data
                newAsset.dialogueNodes = new System.Collections.Generic.List<DialogueNode>(dialogue.dialogueNodes);
                newAsset.rootNodeIndex = dialogue.rootNodeIndex;

                AssetDatabase.CreateAsset(newAsset, assetPath);
                AssetDatabase.SaveAssets();
                EditorUtility.DisplayDialog("Success", "Dialogue asset created successfully!", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Asset must be saved within the Assets folder!", "OK");
            }
        }
    }

    private void ValidateAllNodes(FatherEliasDialogues dialogue)
    {
        if (dialogue.dialogueNodes == null)
        {
            Debug.LogError("Dialogue nodes list is null!");
            return;
        }

        bool isValid = true;
        for (int i = 0; i < dialogue.dialogueNodes.Count; i++)
        {
            var node = dialogue.dialogueNodes[i];
            if (node == null)
            {
                Debug.LogError($"Node {i} is null!");
                isValid = false;
                continue;
            }

            // Check if node has text in at least one language
            if (string.IsNullOrEmpty(node.dialogueTextPT) &&
                string.IsNullOrEmpty(node.dialogueTextEN) &&
                string.IsNullOrEmpty(node.dialogueTextES))
            {
                Debug.LogError($"Node {i} has no text in any language!");
                isValid = false;
            }

            // Validate choices
            if (node.choices != null)
            {
                for (int j = 0; j < node.choices.Length; j++)
                {
                    var choice = node.choices[j];
                    if (choice == null)
                    {
                        Debug.LogError($"Node {i}, Choice {j} is null!");
                        isValid = false;
                    }
                    else if (choice.nextNodeIndex < 0 || choice.nextNodeIndex >= dialogue.dialogueNodes.Count)
                    {
                        Debug.LogError($"Node {i}, Choice {j} has invalid next node index: {choice.nextNodeIndex}");
                        isValid = false;
                    }
                }
            }
        }

        if (isValid)
        {
            EditorUtility.DisplayDialog("Validation Complete", "All dialogue nodes are valid!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Validation Failed", "Please check the console for errors.", "OK");
        }
    }

    private void PreviewDialogue(FatherEliasDialogues dialogue)
    {
        // Create a preview window
        var window = EditorWindow.GetWindow<DialoguePreviewWindow>("Dialogue Preview");
        if (window != null)
        {
            window.Initialize(dialogue);
        }
    }
}

// Preview window for dialogue
public class DialoguePreviewWindow : EditorWindow
{
    private FatherEliasDialogues dialogue;
    private int currentNodeIndex = 0;
    private SystemLanguage previewLanguage = SystemLanguage.English;

    public void Initialize(FatherEliasDialogues dialogueData)
    {
        dialogue = dialogueData;
        currentNodeIndex = dialogue.rootNodeIndex;
    }

    void OnGUI()
    {
        if (dialogue == null)
        {
            EditorGUILayout.LabelField("No dialogue data loaded!");
            return;
        }

        // Language selection
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Preview Language:");
        previewLanguage = (SystemLanguage)EditorGUILayout.EnumPopup(previewLanguage);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Preview Dialogue", EditorStyles.boldLabel);

        // Current node
        var currentNode = dialogue.dialogueNodes[currentNodeIndex];
        if (currentNode != null)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // Speaker name
            EditorGUILayout.LabelField($"Speaker: {currentNode.GetLocalizedSpeakerName(previewLanguage)}", EditorStyles.boldLabel);

            // Dialogue text
            EditorGUILayout.TextArea(currentNode.GetLocalizedDialogueText(previewLanguage), GUILayout.Height(100));

            // Choices
            if (currentNode.choices != null && currentNode.choices.Length > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Choices:", EditorStyles.boldLabel);

                foreach (var choice in currentNode.choices)
                {
                    if (GUILayout.Button(choice.GetLocalizedChoiceText(previewLanguage)))
                    {
                        // Navigate to next node
                        currentNodeIndex = choice.nextNodeIndex;
                        Repaint();
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }

        // Navigation buttons
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Start Dialogue"))
        {
            currentNodeIndex = dialogue.rootNodeIndex;
            Repaint();
        }

        if (GUILayout.Button("End Dialogue") && currentNodeIndex >= 0)
        {
            currentNodeIndex = -1;
        }

        EditorGUILayout.EndHorizontal();
    }
}
#endif