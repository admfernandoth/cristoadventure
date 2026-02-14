using UnityEngine;
using System.Collections;
using CristoAdventure.Core;
using CristoAdventure.Systems;
using CristoAdventure.Gameplay;
using CristoAdventure.Network;

namespace CristoAdventure.Core
{
    /// <summary>
    /// Game Runtime - Main runtime coordinator
    /// Initializes all systems on startup, loads player data, sets up event connections
    /// Handles cleanup on quit
    /// </summary>
    public class GameRuntime : MonoBehaviour
    {
        #region Singleton

        private static GameRuntime _instance;
        public static GameRuntime Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_GameRuntime");
                    _instance = go.AddComponent<GameRuntime>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region State

        private bool _isRuntimeInitialized = false;
        private float _sessionStartTime;
        private int _sessionNumber = 1;

        #endregion

        #region Events

        public event System.Action OnRuntimeInitialized;
        public event System.Action<float> OnSessionTimeUpdated;

        #endregion

        #region Settings

        [Header("Runtime Settings")]
        [SerializeField] private bool _autoLoadLastSave = true;
        [SerializeField] private float _sessionTimeUpdateInterval = 1f;

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

            // Register for application events
            RegisterApplicationEvents();
        }

        private void Start()
        {
            // Session start time
            _sessionStartTime = Time.time;
        }

        private void Update()
        {
            if (!_isRuntimeInitialized) return;

            // Update session time
            float sessionTime = Time.time - _sessionStartTime;
            OnSessionTimeUpdated?.Invoke(sessionTime);

            // Track play time in player data
            UpdatePlayTime();
        }

        #endregion

        #region Initialization

        public void Initialize()
        {
            if (_isRuntimeInitialized)
            {
                Debug.LogWarning("[GameRuntime] Already initialized, skipping...");
                return;
            }

            Debug.Log("[GameRuntime] Initializing game runtime...");
            SystemDiagnostics.LogSystemEvent("GameRuntime", "Runtime initialization started");

            StartCoroutine(InitializeRuntime());
        }

        private IEnumerator InitializeRuntime()
        {
            // Step 1: Wait for MasterGameInitializer to complete
            yield return new WaitUntil(() => MasterGameInitializer.Instance?.IsInitialized == true);

            Debug.Log("[GameRuntime] All systems initialized, setting up runtime...");
            SystemDiagnostics.LogSystemEvent("GameRuntime", "All systems ready");

            // Step 2: Load player data
            yield return LoadPlayerData();

            // Step 3: Setup event connections
            SetupEventConnections();

            // Step 4: Initialize per-session data
            InitializeSessionData();

            // Step 5: Subscribe to system events
            SubscribeToSystemEvents();

            // Step 6: Auto-load last save if enabled
            if (_autoLoadLastSave)
            {
                yield return AutoLoadGame();
            }

            _isRuntimeInitialized = true;
            Debug.Log("[GameRuntime] Game runtime initialization complete!");
            SystemDiagnostics.LogSystemEvent("GameRuntime", "Runtime initialization complete");

            OnRuntimeInitialized?.Invoke();
        }

        #endregion

        #region Player Data Loading

        private IEnumerator LoadPlayerData()
        {
            Debug.Log("[GameRuntime] Loading player data...");

            var saveManager = GameManager.Instance?.SaveManager;
            if (saveManager == null)
            {
                Debug.LogError("[GameRuntime] SaveManager not available!");
                yield break;
            }

            // Load player data
            var playerData = saveManager.LoadPlayerData();

            if (playerData == null)
            {
                Debug.Log("[GameRuntime] No save data found, creating new player...");
                SystemDiagnostics.LogSystemEvent("GameRuntime", "New player data created");

                // GameManager will create new player data if needed
                yield break;
            }

            Debug.Log($"[GameRuntime] Player data loaded: Level {playerData.Level}, {playerData.Coins} coins");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"Player loaded: Level {playerData.Level}");

            // Apply player settings
            ApplyPlayerSettings(playerData);

            yield return null;
        }

        private void ApplyPlayerSettings(PlayerData playerData)
        {
            // Apply audio settings
            if (playerData.GameSettings != null)
            {
                var audioManager = AudioManager.Instance;
                if (audioManager != null)
                {
                    audioManager.SetMusicVolume(playerData.GameSettings.MusicVolume);
                    audioManager.SetSFXVolume(playerData.GameSettings.SfxVolume);
                    audioManager.SetVoiceVolume(playerData.GameSettings.VoiceoverVolume);
                    Debug.Log($"[GameRuntime] Applied audio settings: Music={playerData.GameSettings.MusicVolume}, SFX={playerData.GameSettings.SfxVolume}");
                }

                // Apply language settings
                var localizationManager = LocalizationManager.Instance;
                if (localizationManager != null && playerData.GameSettings.Language != null)
                {
                    if (System.Enum.TryParse<LocalizationManager.Language>(playerData.GameSettings.Language, out var language))
                    {
                        localizationManager.SetLanguage(language);
                        Debug.Log($"[GameRuntime] Applied language: {language}");
                    }
                }
            }
        }

        #endregion

        #region Event Connections

        private void SetupEventConnections()
        {
            Debug.Log("[GameRuntime] Setting up event connections...");

            // Connect GameManager events
            ConnectGameManagerEvents();

            // Connect UIManager events
            ConnectUIManagerEvents();

            // Connect PhaseManager events (if in scene)
            ConnectPhaseManagerEvents();

            SystemDiagnostics.LogSystemEvent("GameRuntime", "Event connections established");
        }

        private void ConnectGameManagerEvents()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null) return;

            // Subscribe to game state changes
            gameManager.OnGameStateChanged += OnGameStateChanged;

            SystemDiagnostics.LogSystemConnection("GameRuntime", "GameManager", "State change events");
        }

        private void ConnectUIManagerEvents()
        {
            // UIManager already connects to GameManager internally
            // This is for any GameRuntime-specific UI handling
            SystemDiagnostics.LogSystemConnection("GameRuntime", "UIManager", "Runtime events");
        }

        private void ConnectPhaseManagerEvents()
        {
            var phaseManager = FindObjectOfType<PhaseManager>();
            if (phaseManager == null) return;

            // Subscribe to phase events
            phaseManager.OnPhaseComplete += OnPhaseComplete;
            phaseManager.OnPOIVisited += OnPOIVisited;
            phaseManager.OnPuzzleCompleted += OnPuzzleCompleted;

            SystemDiagnostics.LogSystemConnection("GameRuntime", "PhaseManager", "Phase events");
        }

        private void SubscribeToSystemEvents()
        {
            // Subscribe to Firebase events
            var firebaseManager = FirebaseManager.Instance;
            if (firebaseManager != null)
            {
                firebaseManager.OnCloudSaveComplete += OnCloudSaveComplete;
                firebaseManager.OnCloudSaveFailed += OnCloudSaveFailed;
                SystemDiagnostics.LogSystemConnection("GameRuntime", "FirebaseManager", "Cloud events");
            }

            // Subscribe to Localization events
            var localizationManager = LocalizationManager.Instance;
            if (localizationManager != null)
            {
                localizationManager.OnLanguageChanged += OnLanguageChanged;
                SystemDiagnostics.LogSystemConnection("GameRuntime", "LocalizationManager", "Language events");
            }
        }

        #endregion

        #region Event Handlers

        private void OnGameStateChanged(GameManager.GameState newState)
        {
            Debug.Log($"[GameRuntime] Game state changed to: {newState}");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"State: {newState}");

            // Handle state-specific logic
            switch (newState)
            {
                case GameManager.GameState.Playing:
                    // Resume gameplay
                    break;

                case GameManager.GameState.Paused:
                    // Pause gameplay
                    break;

                case GameManager.GameState.InDialogue:
                    // Pause game time
                    break;

                case GameManager.GameState.InPuzzle:
                    // Enter puzzle mode
                    break;
            }
        }

        private void OnPhaseComplete(int stars, float playTime)
        {
            Debug.Log($"[GameRuntime] Phase completed: {stars} stars, {playTime:F2}s");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"Phase complete: {stars} stars");

            // Auto-save on phase completion
            GameManager.Instance?.SaveGame();
        }

        private void OnPOIVisited(int count)
        {
            Debug.Log($"[GameRuntime] POI visited: {count}");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"POI visited: {count}");
        }

        private void OnPuzzleCompleted(int count)
        {
            Debug.Log($"[GameRuntime] Puzzle completed: {count}");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"Puzzle completed: {count}");
        }

        private void OnCloudSaveComplete()
        {
            Debug.Log("[GameRuntime] Cloud save complete");
            SystemDiagnostics.LogSystemEvent("GameRuntime", "Cloud save successful");
        }

        private void OnCloudSaveFailed(string error)
        {
            Debug.LogWarning($"[GameRuntime] Cloud save failed: {error}");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"Cloud save failed: {error}");
        }

        private void OnLanguageChanged(LocalizationManager.Language newLanguage)
        {
            Debug.Log($"[GameRuntime] Language changed to: {newLanguage}");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"Language: {newLanguage}");

            // Save language preference
            GameManager.Instance?.SaveGame();
        }

        #endregion

        #region Session Data

        private void InitializeSessionData()
        {
            _sessionNumber = PlayerPrefs.GetInt("SessionNumber", 1);
            Debug.Log($"[GameRuntime] Session #{_sessionNumber} started");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"Session #{_sessionNumber} started");
        }

        private void UpdatePlayTime()
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData != null)
            {
                // Update total play time (add delta time)
                playerData.Statistics.TotalPlayTime += Time.deltaTime;

                // Save every 60 seconds of play time
                if (Mathf.FloorToInt(playerData.Statistics.TotalPlayTime) % 60 == 0)
                {
                    // Note: Don't save every frame, this is just tracking
                    // Actual save happens at specific intervals
                }
            }
        }

        #endregion

        #region Auto Load

        private IEnumerator AutoLoadGame()
        {
            // Check if there's a save file
            var saveManager = GameManager.Instance?.SaveManager;
            if (saveManager == null) yield break;

            var slots = saveManager.GetSaveSlots();
            if (slots.Count > 0 && slots.Contains("auto"))
            {
                Debug.Log("[GameRuntime] Auto-loading last save...");
                SystemDiagnostics.LogSystemEvent("GameRuntime", "Auto-loading save");

                // Load the auto-save
                GameManager.Instance?.LoadGame("auto");
            }
            else
            {
                Debug.Log("[GameRuntime] No save file found, starting new game");
                SystemDiagnostics.LogSystemEvent("GameRuntime", "No save file - new game");
            }

            yield return null;
        }

        #endregion

        #region Cleanup

        private void RegisterApplicationEvents()
        {
            // These are already registered in GameManager, but we add runtime-specific cleanup
        }

        private void OnApplicationQuit()
        {
            Debug.Log("[GameRuntime] Application quitting, performing cleanup...");

            // Save session data
            CleanupSession();

            // Save game one last time
            GameManager.Instance?.SaveGame();

            // Log session end
            float sessionDuration = Time.time - _sessionStartTime;
            Debug.Log($"[GameRuntime] Session #{_sessionNumber} ended. Duration: {sessionDuration:F2}s");
            SystemDiagnostics.LogSystemEvent("GameRuntime", $"Session #{_sessionNumber} ended: {sessionDuration:F2}s");

            // Increment session number for next run
            PlayerPrefs.SetInt("SessionNumber", _sessionNumber + 1);
            PlayerPrefs.Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Debug.Log("[GameRuntime] Application paused");
                SystemDiagnostics.LogSystemEvent("GameRuntime", "Application paused");

                // Auto-save on pause
                GameManager.Instance?.SaveGame();
            }
            else
            {
                Debug.Log("[GameRuntime] Application resumed");
                SystemDiagnostics.LogSystemEvent("GameRuntime", "Application resumed");
            }
        }

        private void CleanupSession()
        {
            Debug.Log("[GameRuntime] Cleaning up session data...");
            SystemDiagnostics.LogSystemEvent("GameRuntime", "Session cleanup");

            // Unsubscribe from events
            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= OnGameStateChanged;
            }

            var phaseManager = FindObjectOfType<PhaseManager>();
            if (phaseManager != null)
            {
                phaseManager.OnPhaseComplete -= OnPhaseComplete;
                phaseManager.OnPOIVisited -= OnPOIVisited;
                phaseManager.OnPuzzleCompleted -= OnPuzzleCompleted;
            }

            var firebaseManager = FirebaseManager.Instance;
            if (firebaseManager != null)
            {
                firebaseManager.OnCloudSaveComplete -= OnCloudSaveComplete;
                firebaseManager.OnCloudSaveFailed -= OnCloudSaveFailed;
            }

            var localizationManager = LocalizationManager.Instance;
            if (localizationManager != null)
            {
                localizationManager.OnLanguageChanged -= OnLanguageChanged;
            }
        }

        #endregion

        #region Public API

        public float GetSessionTime()
        {
            return Time.time - _sessionStartTime;
        }

        public int GetCurrentSessionNumber()
        {
            return _sessionNumber;
        }

        public bool IsRuntimeInitialized => _isRuntimeInitialized;

        #endregion

        #region Debug

        private void OnGUI()
        {
            if (!GameManager.Instance?.IsDebugMode ?? false) return;

            GUILayout.BeginArea(new Rect(10, 220, 300, 150));
            GUILayout.Box("Runtime Status");

            GUILayout.Label($"Session: #{_sessionNumber}");
            GUILayout.Label($"Session Time: {GetSessionTime():F2}s");
            GUILayout.Label($"Runtime Ready: {_isRuntimeInitialized}");

            if (GameManager.Instance?.GetPlayerData() != null)
            {
                var playerData = GameManager.Instance.GetPlayerData();
                GUILayout.Label($"Total Play Time: {playerData.Statistics.TotalPlayTime:F2}s");
            }

            GUILayout.EndArea();
        }

        #endregion
    }
}
