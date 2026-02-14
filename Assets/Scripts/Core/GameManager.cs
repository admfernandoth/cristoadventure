using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace CristoAdventure.Core
{
    /// <summary>
    /// Main game manager - controls game state, initialization, and core game loop
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_GameManager");
                    _instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region Game State

        public enum GameState
        {
            MainMenu,
            Loading,
            Playing,
            Paused,
            InDialogue,
            InPuzzle,
            InBackpack,
            GameOver
        }

        private GameState _currentGameState = GameState.MainMenu;
        public GameState CurrentGameState
        {
            get => _currentGameState;
            private set
            {
                if (_currentGameState != value)
                {
                    _currentGameState = value;
                    OnGameStateChanged?.Invoke(value);
                }
            }
        }

        public event Action<GameState> OnGameStateChanged;

        #endregion

        #region Game Data

        [Header("Game Settings")]
        [SerializeField] private string _gameVersion = "0.1.0";
        [SerializeField] private bool _debugMode = true;

        public string GameVersion => _gameVersion;
        public bool IsDebugMode => _debugMode;

        // Player progress data
        private PlayerData _playerData;

        #endregion

        #region Managers

        public SaveManager SaveManager { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public UIManager UIManager { get; private set; }
        public LocalizationManager LocalizationManager { get; private set; }
        public FirebaseManager FirebaseManager { get; private set; }

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

            InitializeManagers();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;

            LoadGameData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                // Auto-save when game is paused/minimized
                SaveGame();
            }
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        #endregion

        #region Initialization

        private void InitializeManagers()
        {
            // Create or find managers
            SaveManager = GetComponent<SaveManager>();
            if (SaveManager == null)
            {
                SaveManager = gameObject.AddComponent<SaveManager>();
            }

            AudioManager = FindObjectOfType<AudioManager>();
            if (AudioManager == null)
            {
                GameObject audioManagerGO = new GameObject("_AudioManager");
                AudioManager = audioManagerGO.AddComponent<AudioManager>();
                DontDestroyOnLoad(audioManagerGO);
            }

            // Initialize Firebase if available
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
#if FIREBASE_ENABLED
            FirebaseManager = GetComponent<FirebaseManager>();
            if (FirebaseManager == null)
            {
                FirebaseManager = gameObject.AddComponent<FirebaseManager>();
            }
            FirebaseManager.Initialize();
#endif
        }

        private void LoadGameData()
        {
            _playerData = SaveManager.LoadPlayerData();

            if (_playerData == null)
            {
                // New player - create initial data
                _playerData = CreateNewPlayerData();
                SaveGame();
            }

            Debug.Log($"[GameManager] Player data loaded. Level: {_playerData.Level}, Coins: {_playerData.Coins}");
        }

        private PlayerData CreateNewPlayerData()
        {
            return new PlayerData
            {
                PlayerId = System.Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow,
                LastPlayedDate = DateTime.UtcNow,
                Level = 1,
                Experience = 0,
                Coins = 100, // Starting coins
                CurrentPhase = "1.1",
                CompletedPhases = new System.Collections.Generic.List<string>(),
                UnlockedCosmetics = new System.Collections.Generic.List<string>(),
                InventoryItems = new System.Collections.Generic.List<InventoryItem>(),
                GameSettings = new GameSettings
                {
                    Language = SystemLanguage.Portuguese.ToString(),
                    MusicVolume = 0.8f,
                    SfxVolume = 0.8f,
                    VoiceoverVolume = 1.0f,
                    ShowSubtitles = true,
                    AutoSave = true
                },
                Statistics = new PlayerStatistics()
            };
        }

        #endregion

        #region State Control

        public void SetState(GameState newState)
        {
            // Validate state transition
            if (!IsValidStateTransition(_currentGameState, newState))
            {
                Debug.LogWarning($"[GameManager] Invalid state transition: {_currentGameState} -> {newState}");
                return;
            }

            Debug.Log($"[GameManager] State changing: {_currentGameState} -> {newState}");
            CurrentGameState = newState;

            // Handle state-specific logic
            HandleStateEnter(newState);
        }

        private bool IsValidStateTransition(GameState from, GameState to)
        {
            // Define valid state transitions
            switch (from)
            {
                case GameState.MainMenu:
                    return to == GameState.Loading;

                case GameState.Loading:
                    return to == GameState.Playing;

                case GameState.Playing:
                    return to == GameState.Paused ||
                           to == GameState.InDialogue ||
                           to == GameState.InPuzzle ||
                           to == GameState.InBackpack ||
                           to == GameState.MainMenu;

                case GameState.Paused:
                    return to == GameState.Playing || to == GameState.MainMenu;

                case GameState.InDialogue:
                case GameState.InPuzzle:
                case GameState.InBackpack:
                    return to == GameState.Playing || to == GameState.Paused;

                default:
                    return false;
            }
        }

        private void HandleStateEnter(GameState state)
        {
            switch (state)
            {
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;

                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;

                default:
                    Time.timeScale = 1f;
                    break;
            }
        }

        #endregion

        #region Save/Load

        public void SaveGame()
        {
            if (_playerData != null)
            {
                _playerData.LastPlayedDate = DateTime.UtcNow;
                SaveManager.SavePlayerData(_playerData);
                Debug.Log("[GameManager] Game saved successfully");
            }
        }

        public void LoadGame(string saveSlot = "auto")
        {
            _playerData = SaveManager.LoadPlayerData(saveSlot);

            if (_playerData != null)
            {
                Debug.Log($"[GameManager] Game loaded from slot: {saveSlot}");
                // Trigger game loaded event
            }
            else
            {
                Debug.LogWarning($"[GameManager] No save data found in slot: {saveSlot}");
            }
        }

        public void NewGame()
        {
            _playerData = CreateNewPlayerData();
            SaveGame();
            LoadPhase("1.1");
        }

        #endregion

        #region Phase Management

        public void LoadPhase(string phaseId)
        {
            Debug.Log($"[GameManager] Loading phase: {phaseId}");
            SetState(GameState.Loading);

            // Load scene async
            StartCoroutine(LoadPhaseAsync(phaseId));
        }

        private System.Collections.IEnumerator LoadPhaseAsync(string phaseId)
        {
            // TODO: Implement async scene loading with loading screen
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(phaseId);

            while (!asyncLoad.isDone)
            {
                // Update loading progress
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                // TODO: Update loading screen progress bar
                yield return null;
            }

            SetState(GameState.Playing);
            Debug.Log($"[GameManager] Phase {phaseId} loaded successfully");
        }

        #endregion

        #region Player Data Access

        public PlayerData GetPlayerData() => _playerData;

        public void AddCoins(int amount)
        {
            _playerData.Coins += amount;
            Debug.Log($"[GameManager] Added {amount} coins. Total: {_playerData.Coins}");
        }

        public void SpendCoins(int amount)
        {
            if (_playerData.Coins >= amount)
            {
                _playerData.Coins -= amount;
                Debug.Log($"[GameManager] Spent {amount} coins. Remaining: {_playerData.Coins}");
            }
            else
            {
                Debug.LogWarning($"[GameManager] Not enough coins. Have: {_playerData.Coins}, Need: {amount}");
            }
        }

        public void CompletePhase(string phaseId, int stars, float completionTime)
        {
            if (!_playerData.CompletedPhases.Contains(phaseId))
            {
                _playerData.CompletedPhases.Add(phaseId);
                _playerData.Experience += CalculatePhaseExperience(phaseId, stars);
                CheckLevelUp();
            }

            // Update phase statistics
            var phaseStats = new PhaseStatistics
            {
                PhaseId = phaseId,
                CompletionTime = completionTime,
                StarsEarned = stars,
                CompletedDate = DateTime.UtcNow
            };

            _playerData.Statistics.CompletedPhases.Add(phaseStats);
            SaveGame();
        }

        private int CalculatePhaseExperience(string phaseId, int stars)
        {
            // Base experience + star bonus
            int baseExp = 100;
            int starBonus = stars * 50;
            return baseExp + starBonus;
        }

        private void CheckLevelUp()
        {
            int requiredExp = CalculateRequiredExperience(_playerData.Level);

            if (_playerData.Experience >= requiredExp)
            {
                _playerData.Level++;
                _playerData.Experience -= requiredExp;
                Debug.Log($"[GameManager] Level up! New level: {_playerData.Level}");

                // TODO: Trigger level up event/reward
            }
        }

        private int CalculateRequiredExperience(int level)
        {
            // Exponential curve: 100 * level^1.5
            return Mathf.RoundToInt(100 * Mathf.Pow(level, 1.5f));
        }

        #endregion

        #region Debug

        private void Update()
        {
            // Debug shortcuts
            if (_debugMode)
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    AddCoins(100);
                }

                if (Input.GetKeyDown(KeyCode.F2))
                {
                    SaveGame();
                }

                if (Input.GetKeyDown(KeyCode.F5))
                {
                    SetState(CurrentGameState == GameState.Playing
                        ? GameState.Paused
                        : GameState.Playing);
                }
            }
        }

        #endregion
    }
}
