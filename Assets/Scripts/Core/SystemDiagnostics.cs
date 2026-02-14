using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CristoAdventure.Core
{
    /// <summary>
    /// System Diagnostics - Comprehensive logging, performance monitoring, and error tracking
    /// Provides debug commands for runtime system inspection
    /// </summary>
    public class SystemDiagnostics : MonoBehaviour
    {
        #region Singleton

        private static SystemDiagnostics _instance;
        public static SystemDiagnostics Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_SystemDiagnostics");
                    _instance = go.AddComponent<SystemDiagnostics>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region Log Categories

        public enum LogCategory
        {
            System,
            Initialization,
            Event,
            Connection,
            Performance,
            Error,
            Warning,
            Debug
        }

        #endregion

        #region Log Entry

        [System.Serializable]
        public class LogEntry
        {
            public DateTime Timestamp;
            public LogCategory Category;
            public string System;
            public string Message;
            public string StackTrace;
            public float FrameTime;

            public LogEntry(LogCategory category, string system, string message, string stackTrace = null)
            {
                Timestamp = DateTime.UtcNow;
                Category = category;
                System = system;
                Message = message;
                StackTrace = stackTrace;
                FrameTime = Time.unscaledDeltaTime;
            }

            public override string ToString()
            {
                return $"[{Timestamp:HH:mm:ss.fff}] [{Category}] {System}: {Message}";
            }
        }

        #endregion

        #region State

        private List<LogEntry> _logHistory = new List<LogEntry>();
        private const int MAX_LOG_ENTRIES = 1000;
        private int _errorCount = 0;
        private int _warningCount = 0;

        // Performance tracking
        private float _fpsUpdateInterval = 0.5f;
        private float _fpsTimer = 0f;
        private int _frameCount = 0;
        private float _currentFPS = 0f;

        // Memory tracking
        private long _lastMemoryUsage = 0;
        private float _memoryUpdateInterval = 1f;
        private float _memoryTimer = 0f;

        // System health tracking
        private Dictionary<string, DateTime> _lastSystemActivity = new Dictionary<string, DateTime>();

        #endregion

        #region Settings

        [Header("Logging Settings")]
        [SerializeField] private bool _enableConsoleLogging = true;
        [SerializeField] private bool _enableFileLogging = false;
        [SerializeField] private LogCategory _minConsoleLogLevel = LogCategory.Debug;

        [Header("Performance Monitoring")]
        [SerializeField] private bool _enablePerformanceMonitoring = true;
        [SerializeField] private float _lowFPSWarning = 30f;
        [SerializeField] private float _criticalFPSWarning = 15f;

        [Header("Debug Display")]
        [SerializeField] private bool _showInGameDebug = true;
        [SerializeField] private KeyCode _debugToggleKey = KeyCode.F10;
        [SerializeField] private KeyCode _consoleToggleKey = KeyCode.F11;

        private bool _showDebugUI = true;
        private Vector2 _debugScrollPosition;
        private bool _showConsole = false;
        private string _consoleInput = "";
        private List<string> _consoleOutput = new List<string>();

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

            LogSystemEvent("SystemDiagnostics", "Diagnostics system initialized");
        }

        private void Update()
        {
            // Performance monitoring
            if (_enablePerformanceMonitoring)
            {
                UpdatePerformanceMetrics();
            }

            // Memory tracking
            UpdateMemoryMetrics();

            // Debug UI toggle
            if (Input.GetKeyDownUp(_debugToggleKey))
            {
                _showDebugUI = !_showDebugUI;
            }

            // Console toggle
            if (Input.GetKeyDown(_consoleToggleKey))
            {
                _showConsole = !_showConsole;
            }

            // Console input
            if (_showConsole && Input.GetKeyDown(KeyCode.Return))
            {
                ProcessConsoleCommand(_consoleInput);
                _consoleInput = "";
            }
        }

        private void OnApplicationQuit()
        {
            LogSystemEvent("SystemDiagnostics", "Application shutting down");
            SaveLogToFile();
        }

        #endregion

        #region Logging API

        public static void LogSystemEvent(string systemName, string message)
        {
            Instance?.AddLogEntry(LogCategory.Event, systemName, message);
        }

        public static void LogSystemAccess(string systemName, string target)
        {
            Instance?.AddLogEntry(LogCategory.System, systemName, $"Accessed: {target}");
        }

        public static void LogSystemConnection(string fromSystem, string toSystem, string purpose)
        {
            Instance?.AddLogEntry(LogCategory.Connection, fromSystem, $"Connected to {toSystem} for {purpose}");
        }

        public static void LogSystemWarning(string systemName, string message)
        {
            Instance?.AddLogEntry(LogCategory.Warning, systemName, message);
        }

        public static void LogSystemError(string systemName, string message, string stackTrace = null)
        {
            Instance?.AddLogEntry(LogCategory.Error, systemName, message, stackTrace);
        }

        public static void LogPerformance(string systemName, float duration, string operation)
        {
            string message = $"{operation} took {duration:F4}ms";
            Instance?.AddLogEntry(LogCategory.Performance, systemName, message);
        }

        public static void LogInitialization(string systemName, string status)
        {
            Instance?.AddLogEntry(LogCategory.Initialization, systemName, status);
        }

        #endregion

        #region Log Entry Management

        private void AddLogEntry(LogCategory category, string system, string message, string stackTrace = null)
        {
            var entry = new LogEntry(category, system, message, stackTrace);

            // Add to history
            _logHistory.Add(entry);

            // Trim history if needed
            if (_logHistory.Count > MAX_LOG_ENTRIES)
            {
                _logHistory.RemoveAt(0);
            }

            // Track error/warning counts
            if (category == LogCategory.Error) _errorCount++;
            if (category == LogCategory.Warning) _warningCount++;

            // Update last activity
            _lastSystemActivity[system] = DateTime.UtcNow;

            // Log to console
            if (_enableConsoleLogging && ShouldLogToConsole(category))
            {
                string logMessage = entry.ToString();
                switch (category)
                {
                    case LogCategory.Error:
                        Debug.LogError(logMessage);
                        break;
                    case LogCategory.Warning:
                        Debug.LogWarning(logMessage);
                        break;
                    default:
                        Debug.Log(logMessage);
                        break;
                }
            }

            // Log to file
            if (_enableFileLogging)
            {
                // File logging is handled in SaveLogToFile periodically
            }
        }

        private bool ShouldLogToConsole(LogCategory category)
        {
            // Check if category meets minimum log level
            int categoryLevel = GetLogLevelPriority(category);
            int minLevel = GetLogLevelPriority(_minConsoleLogLevel);
            return categoryLevel >= minLevel;
        }

        private int GetLogLevelPriority(LogCategory category)
        {
            switch (category)
            {
                case LogCategory.Debug: return 1;
                case LogCategory.Event: return 2;
                case LogCategory.System: return 3;
                case LogCategory.Connection: return 4;
                case LogCategory.Initialization: return 5;
                case LogCategory.Performance: return 6;
                case LogCategory.Warning: return 7;
                case LogCategory.Error: return 8;
                default: return 0;
            }
        }

        #endregion

        #region Performance Monitoring

        private void UpdatePerformanceMetrics()
        {
            _frameCount++;
            _fpsTimer += Time.unscaledDeltaTime;

            if (_fpsTimer >= _fpsUpdateInterval)
            {
                _currentFPS = _frameCount / _fpsTimer;
                _frameCount = 0;
                _fpsTimer = 0f;

                // Check for low FPS
                if (_currentFPS < _criticalFPSWarning)
                {
                    LogSystemWarning("Performance", $"CRITICAL FPS: {_currentFPS:F1}");
                }
                else if (_currentFPS < _lowFPSWarning)
                {
                    LogSystemWarning("Performance", $"Low FPS: {_currentFPS:F1}");
                }
            }
        }

        private void UpdateMemoryMetrics()
        {
            _memoryTimer += Time.unscaledDeltaTime;

            if (_memoryTimer >= _memoryUpdateInterval)
            {
                _memoryTimer = 0f;

                long currentMemory = GC.GetTotalMemory(false);
                long memoryDelta = currentMemory - _lastMemoryUsage;
                _lastMemoryUsage = currentMemory;

                // Log if memory increased significantly
                if (memoryDelta > 10 * 1024 * 1024) // 10 MB
                {
                    LogSystemPerformance("Memory", memoryDelta / (1024f * 1024f), "Memory increase");
                }
            }
        }

        #endregion

        #region Console Commands

        private void ProcessConsoleCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return;

            _consoleOutput.Add($"> {command}");
            LogSystemEvent("Console", $"Command: {command}");

            string[] parts = command.Split(' ');
            string cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "help":
                    ShowHelp();
                    break;

                case "list":
                case "ls":
                    ListSystems();
                    break;

                case "status":
                    ShowSystemStatus(parts.Length > 1 ? parts[1] : "");
                    break;

                case "logs":
                    ShowLogs(parts.Length > 1 ? int.Parse(parts[1]) : 20);
                    break;

                case "errors":
                    ShowErrors();
                    break;

                case "warnings":
                    ShowWarnings();
                    break;

                case "clear":
                    _consoleOutput.Clear();
                    _consoleOutput.Add("Console cleared");
                    break;

                case "save":
                    SaveLogToFile();
                    _consoleOutput.Add("Logs saved to file");
                    break;

                case "fps":
                    _consoleOutput.Add($"Current FPS: {_currentFPS:F1}");
                    break;

                case "memory":
                    long memory = GC.GetTotalMemory(false) / (1024 * 1024);
                    _consoleOutput.Add($"Memory Usage: {memory} MB");
                    break;

                case "phase":
                    ShowPhaseInfo();
                    break;

                case "player":
                    ShowPlayerInfo();
                    break;

                default:
                    _consoleOutput.Add($"Unknown command: {cmd}. Type 'help' for available commands.");
                    break;
            }
        }

        private void ShowHelp()
        {
            _consoleOutput.Add("=== Available Commands ===");
            _consoleOutput.Add("help         - Show this help message");
            _consoleOutput.Add("list / ls    - List all systems");
            _consoleOutput.Add("status [sys] - Show system status (optional: specific system)");
            _consoleOutput.Add("logs [n]     - Show last n log entries (default: 20)");
            _consoleOutput.Add("errors       - Show all errors");
            _consoleOutput.Add("warnings     - Show all warnings");
            _consoleOutput.Add("clear        - Clear console");
            _consoleOutput.Add("save         - Save logs to file");
            _consoleOutput.Add("fps          - Show current FPS");
            _consoleOutput.Add("memory       - Show memory usage");
            _consoleOutput.Add("phase        - Show current phase info");
            _consoleOutput.Add("player       - Show player info");
        }

        private void ListSystems()
        {
            _consoleOutput.Add("=== Active Systems ===");
            _consoleOutput.Add($"GameManager: {GameManager.Instance != null}");
            _consoleOutput.Add($"SaveManager: {GameManager.Instance?.SaveManager != null}");
            _consoleOutput.Add($"AudioManager: {Systems.AudioManager.Instance != null}");
            _consoleOutput.Add($"UIManager: {Systems.UIManager.Instance != null}");
            _consoleOutput.Add($"LocalizationManager: {Systems.LocalizationManager.Instance != null}");
            _consoleOutput.Add($"FirebaseManager: {Network.FirebaseManager.Instance != null}");
            _consoleOutput.Add($"DialogueManager: {Gameplay.DialogueManager.Instance != null}");
            _consoleOutput.Add($"PuzzleManager: {Gameplay.PuzzleManager.Instance != null}");
        }

        private void ShowSystemStatus(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
            {
                _consoleOutput.Add("Usage: status <system_name>");
                return;
            }

            _consoleOutput.Add($"=== {systemName} Status ===");

            switch (systemName.ToLower())
            {
                case "gamemanager":
                    var gm = GameManager.Instance;
                    _consoleOutput.Add($"Exists: {gm != null}");
                    if (gm != null)
                    {
                        _consoleOutput.Add($"State: {gm.CurrentGameState}");
                        _consoleOutput.Add($"Version: {gm.GameVersion}");
                    }
                    break;

                case "audiomanager":
                    _consoleOutput.Add($"Exists: {Systems.AudioManager.Instance != null}");
                    break;

                case "firebase":
                    var fm = Network.FirebaseManager.Instance;
                    _consoleOutput.Add($"Exists: {fm != null}");
                    if (fm != null)
                    {
                        _consoleOutput.Add($"Initialized: {fm.IsInitialized}");
                    }
                    break;

                default:
                    _consoleOutput.Add($"Unknown system: {systemName}");
                    break;
            }
        }

        private void ShowLogs(int count)
        {
            _consoleOutput.Add($"=== Last {count} Log Entries ===");

            int startIndex = Mathf.Max(0, _logHistory.Count - count);
            for (int i = startIndex; i < _logHistory.Count; i++)
            {
                _consoleOutput.Add(_logHistory[i].ToString());
            }
        }

        private void ShowErrors()
        {
            _consoleOutput.Add($"=== Errors ({_errorCount}) ===");

            foreach (var entry in _logHistory)
            {
                if (entry.Category == LogCategory.Error)
                {
                    _consoleOutput.Add(entry.ToString());
                }
            }
        }

        private void ShowWarnings()
        {
            _consoleOutput.Add($"=== Warnings ({_warningCount}) ===");

            foreach (var entry in _logHistory)
            {
                if (entry.Category == LogCategory.Warning)
                {
                    _consoleOutput.Add(entry.ToString());
                }
            }
        }

        private void ShowPhaseInfo()
        {
            var phaseManager = FindObjectOfType<Gameplay.PhaseManager>();
            if (phaseManager != null)
            {
                _consoleOutput.Add($"=== Phase Info ===");
                _consoleOutput.Add($"Phase ID: {phaseManager.PhaseId}");
                _consoleOutput.Add($"POIs: {phaseManager.POIsVisited}/{phaseManager.TotalPOIs}");
                _consoleOutput.Add($"Puzzles: {phaseManager.PuzzlesCompleted}");
                _consoleOutput.Add($"Play Time: {phaseManager.PlayTime:F2}s");
            }
            else
            {
                _consoleOutput.Add("No PhaseManager in current scene");
            }
        }

        private void ShowPlayerInfo()
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData != null)
            {
                _consoleOutput.Add($"=== Player Info ===");
                _consoleOutput.Add($"Level: {playerData.Level}");
                _consoleOutput.Add($"Experience: {playerData.Experience}");
                _consoleOutput.Add($"Coins: {playerData.Coins}");
                _consoleOutput.Add($"Current Phase: {playerData.CurrentPhase}");
                _consoleOutput.Add($"Completed Phases: {playerData.CompletedPhases.Count}");
                _consoleOutput.Add($"Play Time: {playerData.Statistics.TotalPlayTime:F2}s");
            }
            else
            {
                _consoleOutput.Add("No player data available");
            }
        }

        #endregion

        #region File Logging

        private void SaveLogToFile()
        {
            try
            {
                string logPath = System.IO.Path.Combine(Application.persistentDataPath, "Logs");
                if (!System.IO.Directory.Exists(logPath))
                {
                    System.IO.Directory.CreateDirectory(logPath);
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string filePath = System.IO.Path.Combine(logPath, $"game_log_{timestamp}.txt");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=== Cristo Adventure System Log ===");
                sb.AppendLine($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
                sb.AppendLine($"Total Entries: {_logHistory.Count}");
                sb.AppendLine($"Errors: {_errorCount}");
                sb.AppendLine($"Warnings: {_warningCount}");
                sb.AppendLine();

                foreach (var entry in _logHistory)
                {
                    sb.AppendLine(entry.ToString());
                    if (!string.IsNullOrEmpty(entry.StackTrace))
                    {
                        sb.AppendLine("Stack Trace:");
                        sb.AppendLine(entry.StackTrace);
                    }
                }

                System.IO.File.WriteAllText(filePath, sb.ToString());
                Debug.Log($"[SystemDiagnostics] Log saved to: {filePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SystemDiagnostics] Failed to save log: {e.Message}");
            }
        }

        #endregion

        #region Debug UI

        private void OnGUI()
        {
            if (!_showInGameDebug || !_showDebugUI) return;

            // Performance info box
            GUILayout.BeginArea(new Rect(Screen.width - 310, 10, 300, 150));
            GUILayout.Box("System Performance");

            GUILayout.Label($"FPS: {_currentFPS:F1}");
            GUILayout.Label($"Memory: {_lastMemoryUsage / (1024 * 1024)} MB");

            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData != null)
            {
                GUILayout.Label($"Level: {playerData.Level}");
                GUILayout.Label($"Coins: {playerData.Coins}");
            }

            if (GameManager.Instance != null)
            {
                GUILayout.Label($"State: {GameManager.Instance.CurrentGameState}");
            }

            GUILayout.Label($"Errors: {_errorCount} | Warnings: {_warningCount}");
            GUILayout.Label($"Logs: {_logHistory.Count}/{MAX_LOG_ENTRIES}");

            GUILayout.EndArea();

            // Console
            if (_showConsole)
            {
                DrawDebugConsole();
            }
        }

        private void DrawDebugConsole()
        {
            float consoleWidth = Screen.width * 0.8f;
            float consoleHeight = 300f;
            float consoleX = (Screen.width - consoleWidth) / 2f;
            float consoleY = Screen.height - consoleHeight - 10f;

            GUILayout.BeginArea(new Rect(consoleX, consoleY, consoleWidth, consoleHeight));
            GUILayout.Box("Debug Console (F11 to close)");

            // Output area
            _debugScrollPosition = GUILayout.BeginScrollView(_debugScrollPosition, GUILayout.Height(consoleHeight - 50));
            foreach (var line in _consoleOutput)
            {
                GUILayout.Label(line);
            }
            GUILayout.EndScrollView();

            // Input field
            GUILayout.BeginHorizontal();
            GUILayout.Label(">");
            _consoleInput = GUILayout.TextField(_consoleInput);
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        #endregion

        #region Public API

        public List<LogEntry> GetLogHistory() => new List<LogEntry>(_logHistory);
        public int ErrorCount => _errorCount;
        public int WarningCount => _warningCount;
        public float CurrentFPS => _currentFPS;

        public DateTime GetLastSystemActivity(string systemName)
        {
            if (_lastSystemActivity.TryGetValue(systemName, out DateTime lastActivity))
            {
                return lastActivity;
            }
            return DateTime.MinValue;
        }

        #endregion
    }
}
