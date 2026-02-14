using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

[Serializable]
public class GameSessionData
{
    public string sessionId;
    public DateTime startTime;
    public DateTime lastSaveTime;
    public string currentScene;
    public string currentPhase;
    public int playerLevel;
    public int playerExperience;
    public Dictionary<string, object> gameState;
    public List<string> completedObjectives;
    public List<string> collectedItems;

    public GameSessionData()
    {
        sessionId = System.Guid.NewGuid().ToString();
        startTime = DateTime.Now;
        lastSaveTime = DateTime.Now;
        gameState = new Dictionary<string, object>();
        completedObjectives = new List<string>();
        collectedItems = new List<string>();
    }
}

public class GameFlow : MonoBehaviour
{
    [Header("Save System")]
    [SerializeField] private string saveFileName = "GameSave";
    [SerializeField] private float autoSaveInterval = 60.0f; // 1 minute
    [SerializeField] private bool enableAutoSave = true;

    [Header("Game State")]
    [SerializeField] private GameSessionData currentSession;
    [SerializeField] private bool isLoadingGame = false;

    private SceneTransitionManager sceneTransitionManager;
    private float autoSaveTimer;

    public static GameFlow Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGameSession();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sceneTransitionManager = SceneTransitionManager.Instance;
        autoSaveTimer = 0f;

        // Initialize systems
        InitializeGameSystems();
    }

    private void InitializeGameSession()
    {
        currentSession = new GameSessionData();
        Debug.Log($"Game session started: {currentSession.sessionId}");
    }

    private void InitializeGameSystems()
    {
        // Find or create essential systems
        var inputManager = FindObjectOfType<InputManager>();
        if (inputManager == null)
        {
            new GameObject("InputManager").AddComponent<InputManager>();
        }

        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            new GameObject("AudioManager").AddComponent<AudioManager>();
        }

        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            new GameObject("UIManager").AddComponent<UIManager>();
        }
    }

    private void Update()
    {
        if (enableAutoSave && !isLoadingGame)
        {
            autoSaveTimer += Time.deltaTime;
            if (autoSaveTimer >= autoSaveInterval)
            {
                AutoSave();
                autoSaveTimer = 0f;
            }
        }
    }

    public void StartNewGame()
    {
        isLoadingGame = false;
        currentSession = new GameSessionData();

        // Save initial state
        SaveGame();

        // Transition to character creation
        sceneTransitionManager.LoadSceneAsync("CharacterCreation", () =>
        {
            FindObjectOfType<NewGameFlow>()?.StartCharacterCreation();
        });
    }

    public void LoadGame()
    {
        isLoadingGame = true;
        bool loadSuccess = LoadGameData();

        if (loadSuccess)
        {
            // Restore scene
            if (!string.IsNullOrEmpty(currentSession.currentScene))
            {
                sceneTransitionManager.LoadSceneAsync(currentSession.currentScene, () =>
                {
                    isLoadingGame = false;
                    ResumeGame();
                });
            }
            else
            {
                // No scene saved, load main menu
                sceneTransitionManager.LoadSceneAsync("MainMenu");
                isLoadingGame = false;
            }
        }
        else
        {
            // Load failed, go to main menu
            sceneTransitionManager.LoadSceneAsync("MainMenu");
            isLoadingGame = false;
        }
    }

    public void SaveGame()
    {
        // Update session data
        currentSession.lastSaveTime = DateTime.Now;
        currentSession.currentScene = SceneManager.GetActiveScene().name;
        currentSession.currentPhase = GetCurrentPhase();

        // Serialize and save
        string jsonData = JsonUtility.ToJson(currentSession, true);
        PlayerPrefs.SetString(saveFileName + "_" + currentSession.sessionId, jsonData);
        PlayerPrefs.SetInt("SaveExists", 1);
        PlayerPrefs.Save();

        Debug.Log("Game saved successfully");
    }

    private bool LoadGameData()
    {
        // Get the most recent save
        string jsonData = PlayerPrefs.GetString(saveFileName + "_" + currentSession.sessionId, "");

        if (string.IsNullOrEmpty(jsonData))
        {
            // Try to find any save
            string[] allKeys = PlayerPrefs.GetAllKeys();
            string latestSave = null;
            DateTime latestTime = DateTime.MinValue;

            foreach (string key in allKeys)
            {
                if (key.StartsWith(saveFileName + "_"))
                {
                    string saveData = PlayerPrefs.GetString(key, "");
                    if (!string.IsNullOrEmpty(saveData))
                    {
                        GameSessionData tempSession = JsonUtility.FromJson<GameSessionData>(saveData);
                        if (tempSession.lastSaveTime > latestTime)
                        {
                            latestTime = tempSession.lastSaveTime;
                            latestSave = saveData;
                            currentSession.sessionId = key.Replace(saveFileName + "_", "");
                        }
                    }
                }
            }

            if (latestSave != null)
            {
                currentSession = JsonUtility.FromJson<GameSessionData>(latestSave);
                return true;
            }
        }
        else
        {
            currentSession = JsonUtility.FromJson<GameSessionData>(jsonData);
            return true;
        }

        return false;
    }

    private void AutoSave()
    {
        if (HasActiveGame())
        {
            SaveGame();
            Debug.Log("Auto-saved game");
        }
    }

    public void ResumeGame()
    {
        isLoadingGame = false;

        // Restore game state
        RestoreGameState();

        // Show notification
        ShowNotification("Jogo resumido");
    }

    private void RestoreGameState()
    {
        // Restore player position and stats
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.RestoreFromSession(currentSession);
        }

        // Restore game systems
        var questManager = FindObjectOfType<QuestManager>();
        if (questManager != null)
        {
            questManager.RestoreQuests(currentSession.completedObjectives);
        }

        var inventory = FindObjectOfType<InventorySystem>();
        if (inventory != null)
        {
            inventory.RestoreInventory(currentSession.collectedItems);
        }
    }

    public void QuitGame()
    {
        if (HasActiveGame())
        {
            SaveGame();
        }

        PlayerPrefs.SetInt("SaveExists", 0);
        PlayerPrefs.Save();

        // Quit application
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public bool HasActiveGame()
    {
        return PlayerPrefs.GetInt("SaveExists") == 1;
    }

    public void TransitionToPhase(string phaseName, System.Action onComplete = null)
    {
        currentSession.currentPhase = phaseName;
        SaveGame();

        // Load phase scene
        string phaseScene = $"Phase_{phaseName}";
        sceneTransitionManager.LoadSceneAsync(phaseScene, () =>
        {
            // Initialize phase-specific content
            var phaseManager = FindObjectOfType<PhaseManager>();
            if (phaseManager != null)
            {
                phaseManager.InitializePhase(phaseName);
            }

            onComplete?.Invoke();
        });
    }

    private string GetCurrentPhase()
    {
        // Try to get phase from scene name or active phase manager
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name.StartsWith("Phase_"))
        {
            return currentScene.name.Replace("Phase_", "");
        }

        var phaseManager = FindObjectOfType<PhaseManager>();
        if (phaseManager != null)
        {
            return phaseManager.GetCurrentPhase();
        }

        return "Unknown";
    }

    public void UpdateGameState(string key, object value)
    {
        if (currentSession.gameState.ContainsKey(key))
        {
            currentSession.gameState[key] = value;
        }
        else
        {
            currentSession.gameState.Add(key, value);
        }
    }

    public T GetGameState<T>(string key)
    {
        if (currentSession.gameState.ContainsKey(key) && currentSession.gameState[key] is T value)
        {
            return value;
        }
        return default(T);
    }

    public void AddCompletedObjective(string objectiveId)
    {
        if (!currentSession.completedObjectives.Contains(objectiveId))
        {
            currentSession.completedObjectives.Add(objectId);
            SaveGame();
        }
    }

    public void AddCollectedItem(string itemId)
    {
        if (!currentSession.collectedItems.Contains(itemId))
        {
            currentSession.collectedItems.Add(itemId);
            SaveGame();
        }
    }

    private void ShowNotification(string message)
    {
        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowNotification(message);
        }
    }

    private void OnApplicationQuit()
    {
        // Save on quit
        if (HasActiveGame())
        {
            SaveGame();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        // Save on pause
        if (pauseStatus && HasActiveGame())
        {
            SaveGame();
        }
    }
}