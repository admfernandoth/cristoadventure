using UnityEngine;
using System.Collections;
using CristoAdventure.Core;
using CristoAdventure.Systems;
using CristoAdventure.Gameplay;
using CristoAdventure.Network;

namespace CristoAdventure.Core
{
    /// <summary>
    /// Master Game Initializer - Controls system initialization order
    /// Ensures all managers are initialized in the correct dependency order
    /// </summary>
    public class MasterGameInitializer : MonoBehaviour
    {
        #region Singleton

        private static MasterGameInitializer _instance;
        public static MasterGameInitializer Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_MasterGameInitializer");
                    _instance = go.AddComponent<MasterGameInitializer>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region Initialization Order

        // System initialization order (must be followed strictly)
        private enum InitializationStep
        {
            GameManager,           // 1 - Core manager, must be first
            SaveManager,           // 2 - Handles save data
            AudioManager,          // 3 - Audio system
            UIManager,             // 4 - UI system
            LocalizationManager,   // 5 - Localization
            FirebaseManager,       // 6 - Cloud services (optional)
            DialogueManager,       // 7 - Dialogue system
            PuzzleManager,         // 8 - Puzzle system
            PhaseManager,          // 9 - Per-scene manager
            Complete               // 10 - All systems ready
        }

        #endregion

        #region State

        private InitializationStep _currentStep = InitializationStep.GameManager;
        private bool _isInitializing = false;
        private bool _isInitialized = false;
        private float _initializationStartTime;

        #endregion

        #region Events

        public event System.Action OnInitializationComplete;
        public event System.Action<InitializationStep, float> OnInitializationProgress;

        #endregion

        #region Settings

        [Header("Settings")]
        [SerializeField] private float _timeoutPerStep = 10f;
        [SerializeField] private bool _verboseLogging = true;

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

            // Start initialization immediately
            StartCoroutine(InitializationSequence());
        }

        #endregion

        #region Initialization Sequence

        private IEnumerator InitializationSequence()
        {
            if (_isInitializing) yield break;

            _isInitializing = true;
            _initializationStartTime = Time.time;

            Debug.Log("[MasterGameInitializer] Starting game initialization sequence...");

            // Step 1: Initialize GameManager (MUST BE FIRST)
            yield return InitializeStep(InitializationStep.GameManager, InitializeGameManager);

            // Step 2: Initialize SaveManager
            yield return InitializeStep(InitializationStep.SaveManager, InitializeSaveManager);

            // Step 3: Initialize AudioManager
            yield return InitializeStep(InitializationStep.AudioManager, InitializeAudioManager);

            // Step 4: Initialize UIManager
            yield return InitializeStep(InitializationStep.UIManager, InitializeUIManager);

            // Step 5: Initialize LocalizationManager
            yield return InitializeStep(InitializationStep.LocalizationManager, InitializeLocalizationManager);

            // Step 6: Initialize FirebaseManager (optional, may fail)
            yield return InitializeStep(InitializationStep.FirebaseManager, InitializeFirebaseManager, true);

            // Step 7: Initialize DialogueManager
            yield return InitializeStep(InitializationStep.DialogueManager, InitializeDialogueManager);

            // Step 8: Initialize PuzzleManager
            yield return InitializeStep(InitializationStep.PuzzleManager, InitializePuzzleManager);

            // Step 9: PhaseManager is per-scene, will be initialized by scene

            // Complete
            _currentStep = InitializationStep.Complete;
            _isInitialized = true;
            _isInitializing = false;

            float totalTime = Time.time - _initializationStartTime;
            Debug.Log($"[MasterGameInitializer] Initialization complete in {totalTime:F2} seconds");

            // Auto-wire all connections
            AutoWireConnections();

            // Initialize game runtime
            GameRuntime.Instance?.Initialize();

            OnInitializationComplete?.Invoke();
            OnInitializationProgress?.Invoke(InitializationStep.Complete, 1f);
        }

        private IEnumerator InitializeStep(InitializationStep step, System.Func<bool> initFunc, bool isOptional = false)
        {
            _currentStep = step;
            float stepStartTime = Time.time;

            if (_verboseLogging)
            {
                Debug.Log($"[MasterGameInitializer] Initializing: {step}...");
            }

            // Wait for initialization or timeout
            float elapsed = 0f;
            bool success = false;

            while (elapsed < _timeoutPerStep)
            {
                success = initFunc();
                if (success) break;

                yield return new WaitForSeconds(0.1f);
                elapsed += 0.1f;
            }

            if (!success && !isOptional)
            {
                Debug.LogError($"[MasterGameInitializer] Failed to initialize: {step} (timeout after {_timeoutPerStep}s)");
            }
            else if (!success && isOptional)
            {
                Debug.LogWarning($"[MasterGameInitializer] Optional system failed to initialize: {step} (continuing...)");
            }
            else
            {
                float stepTime = Time.time - stepStartTime;
                if (_verboseLogging)
                {
                    Debug.Log($"[MasterGameInitializer] {step} initialized in {stepTime:F2}s");
                }
            }

            // Report progress
            float progress = (int)step / (float)System.Enum.GetValues(typeof(InitializationStep)).Length;
            OnInitializationProgress?.Invoke(step, progress);

            yield return new WaitForSeconds(0.05f); // Small delay between steps
        }

        #endregion

        #region System Initialization Functions

        private bool InitializeGameManager()
        {
            try
            {
                // Get or create GameManager instance
                var gameManager = GameManager.Instance;

                // Verify it's ready
                if (gameManager == null)
                {
                    Debug.LogError("[MasterGameInitializer] GameManager instance is null!");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[MasterGameInitializer] GameManager initialization failed: {e.Message}");
                return false;
            }
        }

        private bool InitializeSaveManager()
        {
            try
            {
                // SaveManager is initialized by GameManager, just verify
                var saveManager = GameManager.Instance?.SaveManager;

                if (saveManager == null)
                {
                    Debug.LogError("[MasterGameInitializer] SaveManager not found!");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[MasterGameInitializer] SaveManager initialization failed: {e.Message}");
                return false;
            }
        }

        private bool InitializeAudioManager()
        {
            try
            {
                // Get or create AudioManager instance
                var audioManager = AudioManager.Instance;

                if (audioManager == null)
                {
                    Debug.LogError("[MasterGameInitializer] AudioManager not created!");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[MasterGameInitializer] AudioManager initialization failed: {e.Message}");
                return false;
            }
        }

        private bool InitializeUIManager()
        {
            try
            {
                // Get or create UIManager instance
                var uiManager = UIManager.Instance;

                if (uiManager == null)
                {
                    Debug.LogError("[MasterGameInitializer] UIManager not created!");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[MasterGameInitializer] UIManager initialization failed: {e.Message}");
                return false;
            }
        }

        private bool InitializeLocalizationManager()
        {
            try
            {
                // Get or create LocalizationManager instance
                var locManager = LocalizationManager.Instance;

                if (locManager == null)
                {
                    Debug.LogError("[MasterGameInitializer] LocalizationManager not created!");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[MasterGameInitializer] LocalizationManager initialization failed: {e.Message}");
                return false;
            }
        }

        private bool InitializeFirebaseManager()
        {
            try
            {
#if FIREBASE_ENABLED
                // Get FirebaseManager instance (it may not be available)
                var firebaseManager = FirebaseManager.Instance;

                if (firebaseManager != null)
                {
                    // Start Firebase initialization (it's async)
                    firebaseManager.Initialize();
                    Debug.Log("[MasterGameInitializer] FirebaseManager initialization started (async)");
                }
                else
                {
                    Debug.LogWarning("[MasterGameInitializer] FirebaseManager not available (this is OK)");
                }

                return true; // Firebase is optional, always return true
#else
                Debug.Log("[MasterGameInitializer] Firebase not enabled in this build");
                return true;
#endif
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[MasterGameInitializer] FirebaseManager initialization failed (non-critical): {e.Message}");
                return true; // Firebase is optional
            }
        }

        private bool InitializeDialogueManager()
        {
            try
            {
                // Get or create DialogueManager instance
                var dialogueManager = DialogueManager.Instance;

                if (dialogueManager == null)
                {
                    Debug.LogError("[MasterGameInitializer] DialogueManager not created!");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[MasterGameInitializer] DialogueManager initialization failed: {e.Message}");
                return false;
            }
        }

        private bool InitializePuzzleManager()
        {
            try
            {
                // Get or create PuzzleManager instance
                var puzzleManager = PuzzleManager.Instance;

                if (puzzleManager == null)
                {
                    Debug.LogError("[MasterGameInitializer] PuzzleManager not created!");
                    return false;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[MasterGameInitializer] PuzzleManager initialization failed: {e.Message}");
                return false;
            }
        }

        #endregion

        #region Auto-Wiring Connections

        private void AutoWireConnections()
        {
            Debug.Log("[MasterGameInitializer] Auto-wiring system connections...");

            // GameManager → All other managers
            WireGameManager();

            // UIManager → GameManager for state changes
            WireUIManager();

            // POI → PhaseManager for visit tracking
            WirePOISystem();

            // PuzzleManager → GameManager for rewards
            WirePuzzleManager();

            // SaveManager → Firebase for cloud sync
            WireSaveSystem();

            Debug.Log("[MasterGameInitializer] System connections wired successfully");
        }

        private void WireGameManager()
        {
            // GameManager is already connected to all managers through singleton instances
            // This is just for verification
            var gameManager = GameManager.Instance;
            if (gameManager == null) return;

            SystemDiagnostics.LogSystemAccess("GameManager", "All managers connected");
        }

        private void WireUIManager()
        {
            // UIManager already calls GameManager.Instance for state changes
            // This is just verification
            var uiManager = UIManager.Instance;
            var gameManager = GameManager.Instance;

            if (uiManager != null && gameManager != null)
            {
                // Subscribe to game state changes
                gameManager.OnGameStateChanged += (state) =>
                {
                    SystemDiagnostics.LogSystemEvent("UIManager", $"Game state changed to: {state}");
                };

                SystemDiagnostics.LogSystemConnection("UIManager", "GameManager", "State changes");
            }
        }

        private void WirePOISystem()
        {
            // Find all Interactable POIs in the scene
            var pois = FindObjectsOfType<Interactable>();

            foreach (var poi in pois)
            {
                // POIs already access PhaseManager through singleton
                SystemDiagnostics.LogSystemConnection($"POI ({poi.name})", "PhaseManager", "Visit tracking");
            }

            Debug.Log($"[MasterGameInitializer] Wired {pois.Length} POIs to PhaseManager");
        }

        private void WirePuzzleManager()
        {
            // PuzzleManager already calls GameManager.Instance for rewards
            // This is just verification
            var puzzleManager = PuzzleManager.Instance;
            var gameManager = GameManager.Instance;

            if (puzzleManager != null && gameManager != null)
            {
                SystemDiagnostics.LogSystemConnection("PuzzleManager", "GameManager", "Rewards");
            }
        }

        private void WireSaveSystem()
        {
            // SaveManager already calls FirebaseManager through GameManager
            // This is just verification
            var saveManager = GameManager.Instance?.SaveManager;
            var firebaseManager = FirebaseManager.Instance;

            if (saveManager != null && firebaseManager != null)
            {
                SystemDiagnostics.LogSystemConnection("SaveManager", "FirebaseManager", "Cloud sync");
            }
        }

        #endregion

        #region Phase Manager Initialization

        public void InitializePhaseManager()
        {
            // Called by each scene to initialize the PhaseManager
            var phaseManager = FindObjectOfType<PhaseManager>();

            if (phaseManager != null)
            {
                Debug.Log($"[MasterGameInitializer] PhaseManager initialized for scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
                SystemDiagnostics.LogSystemEvent("PhaseManager", $"Scene loaded: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            }
            else
            {
                Debug.LogWarning($"[MasterGameInitializer] No PhaseManager found in scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
            }
        }

        #endregion

        #region Properties

        public bool IsInitialized => _isInitialized;
        public InitializationStep CurrentStep => _currentStep;

        #endregion

        #region Debug

        private void OnGUI()
        {
            if (!_verboseLogging) return;

            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Box("System Initialization Status");

            GUILayout.Label($"Initialized: {_isInitialized}");
            GUILayout.Label($"Current Step: {_currentStep}");

            if (!_isInitialized)
            {
                GUILayout.Label("Initializing...");
            }

            GUILayout.EndArea();
        }

        #endregion
    }
}
