using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace CristoAdventure.Gameplay
{
    /// <summary>
    /// Manages individual phase logic, POI tracking, and phase completion
    /// </summary>
    public class PhaseManager : MonoBehaviour
    {
        #region Phase Data

        [Header("Phase Identification")]
        [SerializeField] private string _phaseId = "1.1";
        [SerializeField] private string _phaseNameKey = "Phase.1.1.Name";

        [Header("Phase Settings")]
        [SerializeField] private int _requiredStars = 2;
        [SerializeField] private bool _autoSaveOnComplete = true;
        [SerializeField] private string _nextPhaseId = "1.2";

        [Header("POI Tracking")]
        [SerializeField] private List<string> _poiIds = new List<string>();
        [SerializeField] private int _totalPOIs = 6;

        #endregion

        #region State

        private int _poisVisited = 0;
        private int _puzzlesCompleted = 0;
        private float _phaseStartTime;
        private bool _phaseActive = false;

        #endregion

        #region Events

        public event System.Action<int> OnPOIVisited;
        public event System.Action<int> OnPuzzleCompleted;
        public event System.Action<int, float> OnPhaseComplete;

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            _phaseStartTime = Time.time;
            _phaseActive = true;

            // Log phase start
            FirebaseManager.Instance?.LogPhaseStarted(_phaseId);

            // Initialize POI list from scene
            InitializePOIs();

            Debug.Log($"[PhaseManager] Phase {_phaseId} started");
        }

        private void OnDestroy()
        {
            if (_phaseActive)
            {
                // Auto-save on exit
                if (_autoSaveOnComplete)
                {
                    SavePhaseProgress();
                }
            }
        }

        #endregion

        #region Initialization

        private void InitializePOIs()
        {
            // Find all POIs in the scene
            var pois = FindObjectsOfType<Interactable>();

            _poiIds.Clear();
            foreach (var poi in pois)
            {
                if (!string.IsNullOrEmpty(poi.name))
                {
                    _poiIds.Add(poi.name);
                }
            }

            Debug.Log($"[PhaseManager] Found {_poiIds.Count} POIs in scene");
        }

        #endregion

        #region POI Tracking

        public void RegisterPOIVisit(string poiId)
        {
            if (!_poiIds.Contains(poiId))
            {
                Debug.LogWarning($"[PhaseManager] Unknown POI: {poiId}");
                return;
            }

            // Check if already visited
            var playerData = GameManager.Instance?.GetPlayerData();
            string saveKey = $"{_phaseId}_poi_{poiId}";

            if (playerData != null && HasVisitedPOI(saveKey))
            {
                return; // Already visited
            }

            // Mark as visited
            _poisVisited++;
            MarkPOIAsVisited(saveKey);

            // Notify listeners
            OnPOIVisited?.Invoke(_poisVisited);

            // Log analytics
            FirebaseManager.Instance?.LogEvent("poi_visited",
                new Firebase.Analytics.Parameter("phase_id", _phaseId),
                new Firebase.Analytics.Parameter("poi_id", poiId)
            );

            Debug.Log($"[PhaseManager] POI visited: {poiId} ({_poisVisited}/{_totalPOIs})");

            // Check if all POIs visited
            if (_poisVisited >= _totalPOIs)
            {
                Debug.Log($"[PhaseManager] All POIs visited in phase {_phaseId}!");
            }
        }

        private bool HasVisitedPOI(string saveKey)
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData != null)
            {
                return playerData.CompletedPhases.Contains(saveKey);
            }
            return false;
        }

        private void MarkPOIAsVisited(string saveKey)
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData != null)
            {
                playerData.CompletedPhases.Add(saveKey);
            }
        }

        #endregion

        #region Puzzle Tracking

        public void RegisterPuzzleComplete(string puzzleId, int stars)
        {
            _puzzlesCompleted++;

            // Notify listeners
            OnPuzzleCompleted?.Invoke(_puzzlesCompleted);

            // Log analytics
            FirebaseManager.Instance?.LogPuzzleCompleted(puzzleId, true);

            Debug.Log($"[PhaseManager] Puzzle completed: {puzzleId} ({stars} stars)");
        }

        #endregion

        #region Phase Completion

        public void CompletePhase()
        {
            float playTime = Time.time - _phaseStartTime;
            int stars = CalculateStars();

            // Save completion
            if (_autoSaveOnComplete)
            {
                SavePhaseCompletion(stars, playTime);
            }

            // Notify listeners
            OnPhaseComplete?.Invoke(stars, playTime);

            _phaseActive = false;

            Debug.Log($"[PhaseManager] Phase {_phaseId} completed with {stars} stars in {playTime:F2} seconds");
        }

        private int CalculateStars()
        {
            // Calculate stars based on POIs visited and puzzles completed
            float poiProgress = (float)_poisVisited / _totalPOIs;
            float puzzleProgress = (float)_puzzlesCompleted / 1f; // Assuming 1 main puzzle per phase

            float overallProgress = (poiProgress + puzzleProgress) / 2f;

            if (overallProgress >= 0.8f) return 3;
            if (overallProgress >= 0.5f) return 2;
            return 1;
        }

        #endregion

        #region Save/Load

        private void SavePhaseProgress()
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData == null) return;

            // Save current phase
            playerData.CurrentPhase = _phaseId;

            GameManager.Instance?.SaveGame();
        }

        private void SavePhaseCompletion(int stars, float playTime)
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData == null) return;

            // Add phase to completed list
            if (!playerData.CompletedPhases.Contains(_phaseId))
            {
                playerData.CompletedPhases.Add(_phaseId);
            }

            // Add phase statistics
            var phaseStats = new PhaseStatistics
            {
                PhaseId = _phaseId,
                CompletionTime = playTime,
                StarsEarned = stars,
                CompletedDate = System.DateTime.UtcNow
            };

            playerData.Statistics.CompletedPhases.Add(phaseStats);

            // Unlock next phase
            if (stars >= _requiredStars)
            {
                Debug.Log($"[PhaseManager] Phase {_phaseId} completed with {stars} stars. Next phase: {_nextPhaseId}");
            }

            GameManager.Instance?.SaveGame();
        }

        #endregion

        #region Phase Transition

        public void LoadNextPhase()
        {
            if (!string.IsNullOrEmpty(_nextPhaseId))
            {
                GameManager.Instance?.LoadPhase(_nextPhaseId);
            }
            else
            {
                // Return to main menu
                SceneManager.LoadScene("MainMenu");
            }
        }

        public void RestartPhase()
        {
            SceneManager.LoadScene(_phaseId);
        }

        #endregion

        #region Public API

        public int GetProgressPercentage()
        {
            float poiProgress = (float)_poisVisited / _totalPOIs;
            return Mathf.RoundToInt(poiProgress * 100f);
        }

        public int POIsVisited => _poisVisited;
        public int TotalPOIs => _totalPOIs;
        public int PuzzlesCompleted => _puzzlesCompleted;
        public string PhaseId => _phaseId;
        public float PlayTime => Time.time - _phaseStartTime;

        #endregion
    }

    #region Phase Statistics

    [System.Serializable]
    public class PhaseStatistics
    {
        public string PhaseId;
        public float CompletionTime;
        public int StarsEarned;
        public System.DateTime CompletedDate;
        public int POIsVisited;
        public int PuzzlesCompleted;
        public int CoinsCollected;
    }

    #endregion
}
