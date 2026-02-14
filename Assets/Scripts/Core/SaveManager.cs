using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace CristoAdventure.Core
{
    /// <summary>
    /// Manages save/load operations for player data
    /// Supports both local JSON storage and Firebase cloud sync
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        #region Settings

        [Header("Save Settings")]
        [SerializeField] private string _saveFolder = "Saves";
        [SerializeField] private int _maxAutoSaves = 3;
        [SerializeField] private float _autoSaveInterval = 300f; // 5 minutes

        private string _savePath;
        private float _autoSaveTimer;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _savePath = Path.Combine(Application.persistentDataPath, _saveFolder);

            // Create save directory if it doesn't exist
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
                Debug.Log($"[SaveManager] Created save directory: {_savePath}");
            }
        }

        private void Update()
        {
            // Auto-save timer
            _autoSaveTimer += Time.deltaTime;
            if (_autoSaveTimer >= _autoSaveInterval)
            {
                _autoSaveTimer = 0f;
                if (GameManager.Instance?.GetPlayerData()?.GameSettings.AutoSave ?? true)
                {
                    AutoSave();
                }
            }
        }

        #endregion

        #region Save/Load - Local

        public void SavePlayerData(PlayerData data, string slotName = "auto")
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                string filePath = GetSaveFilePath(slotName);
                File.WriteAllText(filePath, json);

                Debug.Log($"[SaveManager] Saved player data to: {filePath}");

                // Trigger cloud save if Firebase is available
                SaveToCloud(data, slotName);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to save player data: {e.Message}");
            }
        }

        public PlayerData LoadPlayerData(string slotName = "auto")
        {
            try
            {
                string filePath = GetSaveFilePath(slotName);

                if (!File.Exists(filePath))
                {
                    Debug.Log($"[SaveManager] No save file found at: {filePath}");
                    return null;
                }

                string json = File.ReadAllText(filePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                Debug.Log($"[SaveManager] Loaded player data from: {filePath}");
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to load player data: {e.Message}");
                return null;
            }
        }

        public void DeleteSave(string slotName)
        {
            try
            {
                string filePath = GetSaveFilePath(slotName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log($"[SaveManager] Deleted save file: {filePath}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to delete save: {e.Message}");
            }
        }

        #endregion

        #region Auto Save

        private void AutoSave()
        {
            // Rotate auto saves
            string[] autoSaves = new string[_maxAutoSaves];
            for (int i = _maxAutoSaves - 1; i > 0; i--)
            {
                string oldFile = GetSaveFilePath($"auto_{i}");
                string newFile = GetSaveFilePath($"auto_{i + 1}");

                if (File.Exists(oldFile))
                {
                    File.Move(oldFile, newFile);
                }
            }

            // Save current as auto_1
            PlayerData data = GameManager.Instance?.GetPlayerData();
            if (data != null)
            {
                SavePlayerData(data, "auto_1");
                Debug.Log("[SaveManager] Auto-saved game");
            }
        }

        #endregion

        #region Save Slots

        public List<string> GetSaveSlots()
        {
            List<string> slots = new List<string>();

            try
            {
                string[] files = Directory.GetFiles(_savePath, "*.json");

                foreach (string file in files)
                {
                    string slotName = Path.GetFileNameWithoutExtension(file);
                    slots.Add(slotName);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to get save slots: {e.Message}");
            }

            return slots;
        }

        public SaveSlotInfo GetSaveSlotInfo(string slotName)
        {
            try
            {
                string filePath = GetSaveFilePath(slotName);

                if (!File.Exists(filePath))
                {
                    return null;
                }

                string json = File.ReadAllText(filePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                return new SaveSlotInfo
                {
                    SlotName = slotName,
                    PlayerLevel = data.Level,
                    CurrentPhase = data.CurrentPhase,
                    LastPlayed = data.LastPlayedDate,
                    PlayTime = data.Statistics.TotalPlayTime
                };
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to get save slot info: {e.Message}");
                return null;
            }
        }

        #endregion

        #region Cloud Save (Firebase)

        private void SaveToCloud(PlayerData data, string slotName)
        {
#if FIREBASE_ENABLED
            // Trigger Firebase cloud save
            FirebaseManager firebaseManager = GameManager.Instance?.FirebaseManager;
            if (firebaseManager != null)
            {
                firebaseManager.SaveToCloud(data, slotName);
            }
#endif
        }

        public void LoadFromCloud(string slotName, Action<PlayerData> onComplete)
        {
#if FIREBASE_ENABLED
            FirebaseManager firebaseManager = GameManager.Instance?.FirebaseManager;
            if (firebaseManager != null)
            {
                firebaseManager.LoadFromCloud(slotName, onComplete);
            }
            else
            {
                onComplete?.Invoke(null);
            }
#else
            onComplete?.Invoke(null);
#endif
        }

        #endregion

        #region Utility

        private string GetSaveFilePath(string slotName)
        {
            return Path.Combine(_savePath, $"{slotName}.json");
        }

        public long GetSaveFileSize(string slotName)
        {
            try
            {
                string filePath = GetSaveFilePath(slotName);
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    return fileInfo.Length;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Failed to get save file size: {e.Message}");
            }

            return 0;
        }

        #endregion
    }

    #region Data Classes

    [System.Serializable]
    public class PlayerData
    {
        // Player identification
        public string PlayerId;
        public DateTime CreatedDate;
        public DateTime LastPlayedDate;

        // Progression
        public int Level;
        public int Experience;
        public int Coins;
        public string CurrentPhase;
        public List<string> CompletedPhases;

        // Customization
        public List<string> UnlockedCosmetics;
        public string EquippedCosmeticId;

        // Inventory
        public List<InventoryItem> InventoryItems;

        // Settings
        public GameSettings GameSettings;

        // Statistics
        public PlayerStatistics Statistics;
    }

    [System.Serializable]
    public class GameSettings
    {
        public string Language;
        public float MusicVolume;
        public float SfxVolume;
        public float VoiceoverVolume;
        public bool ShowSubtitles;
        public bool AutoSave;
        public int GraphicsQuality;
    }

    [System.Serializable]
    public class PlayerStatistics
    {
        public float TotalPlayTime;
        public int TotalDistanceWalked;
        public int TotalInteractions;
        public int PuzzlesCompleted;
        public int TotalStarsEarned;
        public List<PhaseStatistics> CompletedPhases;
        public List<Achievement> UnlockedAchievements;

        public PlayerStatistics()
        {
            CompletedPhases = new List<PhaseStatistics>();
            UnlockedAchievements = new List<Achievement>();
        }
    }

    [System.Serializable]
    public class PhaseStatistics
    {
        public string PhaseId;
        public float CompletionTime;
        public int StarsEarned;
        public DateTime CompletedDate;
    }

    [System.Serializable]
    public class InventoryItem
    {
        public string ItemId;
        public string ItemType;
        public DateTime AcquiredDate;
        public bool IsNew;
    }

    [System.Serializable]
    public class Achievement
    {
        public string AchievementId;
        public DateTime UnlockedDate;
    }

    [System.Serializable]
    public class SaveSlotInfo
    {
        public string SlotName;
        public int PlayerLevel;
        public string CurrentPhase;
        public DateTime LastPlayed;
        public float PlayTime;
    }

    #endregion
}
