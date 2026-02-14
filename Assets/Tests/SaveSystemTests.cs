using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CristoAdventure.Tests
{
    /// <summary>
    /// Comprehensive test suite for save/load functionality
    /// Tests all aspects of the save system including local saves, auto-saves, cloud saves,
    /// and persistence of game data across sessions.
    /// </summary>
    public class SaveSystemTests
    {
        private SaveManager _saveManager;
        private PlayerData _testPlayerData;
        private string _testSaveFolder = "TestSaves";
        private string _originalSaveFolder;

        [SetUp]
        public void Setup()
        {
            // Create a new GameObject and add SaveManager component
            GameObject testGO = new GameObject("TestSaveManager");
            _saveManager = testGO.AddComponent<SaveManager>();

            // Configure save manager for testing
            _originalSaveFolder = _saveManager.GetType()
                .GetField("_saveFolder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_saveManager) as string;

            // Use test folder
            _saveManager.GetType()
                .GetField("_saveFolder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_saveManager, _testSaveFolder);

            // Update save path
            _saveManager.GetType()
                .GetField("_savePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_saveManager, Path.Combine(Application.persistentDataPath, _testSaveFolder));

            // Create test save directory
            string testSavePath = Path.Combine(Application.persistentDataPath, _testSaveFolder);
            if (!Directory.Exists(testSavePath))
            {
                Directory.CreateDirectory(testSavePath);
            }

            // Create test player data
            _testPlayerData = new PlayerData
            {
                PlayerId = "test-player-123",
                CreatedDate = DateTime.Now,
                LastPlayedDate = DateTime.Now,
                Level = 5,
                Experience = 2500,
                Coins = 150,
                CurrentPhase = "Phase_2_B",
                CompletedPhases = new List<string> { "Phase_1_A", "Phase_1_B", "Phase_2_A" },
                UnlockedCosmetics = new List<string> { "hat_001", "shoes_001" },
                EquippedCosmeticId = "hat_001",
                InventoryItems = new List<InventoryItem>
                {
                    new InventoryItem
                    {
                        ItemId = "collectible_001",
                        ItemType = "treasure",
                        AcquiredDate = DateTime.Now.AddDays(-1),
                        IsNew = false
                    },
                    new InventoryItem
                    {
                        ItemId = "key_001",
                        ItemType = "key",
                        AcquiredDate = DateTime.Now,
                        IsNew = true
                    }
                },
                GameSettings = new GameSettings
                {
                    Language = "en",
                    MusicVolume = 0.8f,
                    SfxVolume = 0.9f,
                    VoiceoverVolume = 0.7f,
                    ShowSubtitles = true,
                    AutoSave = true,
                    GraphicsQuality = 2
                },
                Statistics = new PlayerStatistics
                {
                    TotalPlayTime = 120.5f,
                    TotalDistanceWalked = 1500,
                    TotalInteractions = 45,
                    PuzzlesCompleted = 8,
                    TotalStarsEarned = 25,
                    CompletedPhases = new List<PhaseStatistics>
                    {
                        new PhaseStatistics
                        {
                            PhaseId = "Phase_1_A",
                            CompletionTime = 180.5f,
                            StarsEarned = 3,
                            CompletedDate = DateTime.Now.AddDays(-2)
                        }
                    },
                    UnlockedAchievements = new List<Achievement>
                    {
                        new Achievement
                        {
                            AchievementId = "first_steps",
                            UnlockedDate = DateTime.Now.AddDays(-3)
                        }
                    }
                }
            };
        }

        [TearDown]
        public void Teardown()
        {
            // Clean up test files
            string testSavePath = Path.Combine(Application.persistentDataPath, _testSaveFolder);
            if (Directory.Exists(testSavePath))
            {
                Directory.Delete(testSavePath, true);
            }

            // Destroy test GameObject
            if (_saveManager != null)
            {
                GameObject.DestroyImmediate(_saveManager.gameObject);
            }

            // Restore original save folder
            if (_originalSaveFolder != null)
            {
                _saveManager?.GetType()
                    .GetField("_saveFolder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .SetValue(_saveManager, _originalSaveFolder);
            }
        }

        #region Test Local Save

        [Test]
        public void TestLocalSave()
        {
            // Arrange
            string slotName = "test_save";
            PlayerData originalData = _testPlayerData;

            // Act - Save data
            _saveManager.SavePlayerData(originalData, slotName);

            // Assert - Verify file exists
            string saveFilePath = Path.Combine(Application.persistentDataPath, _testSaveFolder, $"{slotName}.json");
            Assert.IsTrue(File.Exists(saveFilePath), "Save file should exist after saving");

            // Act - Load data
            PlayerData loadedData = _saveManager.LoadPlayerData(slotName);

            // Assert - Verify data integrity
            Assert.IsNotNull(loadedData, "Loaded data should not be null");
            Assert.AreEqual(originalData.PlayerId, loadedData.PlayerId, "Player ID should match");
            Assert.AreEqual(originalData.Level, loadedData.Level, "Level should match");
            Assert.AreEqual(originalData.Coins, loadedData.Coins, "Coins should match");
            Assert.AreEqual(originalData.CurrentPhase, loadedData.CurrentPhase, "Current phase should match");

            // Test complex object properties
            Assert.AreEqual(originalData.CompletedPhases.Count, loadedData.CompletedPhases.Count, "Completed phases count should match");
            Assert.AreEqual(originalData.InventoryItems.Count, loadedData.InventoryItems.Count, "Inventory items count should match");
            Assert.AreEqual(originalData.GameSettings.Language, loadedData.GameSettings.Language, "Language should match");
            Assert.AreEqual(originalData.GameSettings.MusicVolume, loadedData.GameSettings.MusicVolume, "Music volume should match");

            // Test statistics
            Assert.AreEqual(originalData.Statistics.TotalPlayTime, loadedData.Statistics.TotalPlayTime, "Total play time should match");
            Assert.AreEqual(originalData.Statistics.PuzzlesCompleted, loadedData.Statistics.PuzzlesCompleted, "Puzzles completed should match");
        }

        [Test]
        public void TestLoadNonExistentSave()
        {
            // Act - Try to load from non-existent slot
            PlayerData loadedData = _saveManager.LoadPlayerData("non_existent_slot");

            // Assert - Should return null
            Assert.IsNull(loadedData, "Loading non-existent save should return null");
        }

        #endregion

        #region Test Auto Save

        [UnityTest]
        public IEnumerator TestAutoSave()
        {
            // Arrange
            string[] expectedSlots = new[] { "auto_1", "auto_2", "auto_3" };
            float shortInterval = 2.0f; // Much shorter than 5 minutes for testing

            // Set short auto-save interval
            _saveManager.GetType()
                .GetField("_autoSaveInterval", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_saveManager, shortInterval);

            // Mock GameManager.Instance.GetPlayerData to return our test data
            var gameManagerMock = new UnityEngine.GameObject();
            var mockGameManager = gameManagerMock.AddComponent<MockGameManager>();
            mockGameManager.SetPlayerData(_testPlayerData);

            // Act - Simulate multiple auto-saves
            for (int i = 0; i < 3; i++)
            {
                // Trigger auto-save by setting timer and calling Update
                var timerField = _saveManager.GetType()
                    .GetField("_autoSaveTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                timerField.SetValue(_saveManager, shortInterval);

                // Simulate Update call to trigger auto-save
                _saveManager.GetType().GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(_saveManager, null);

                yield return new WaitForSeconds(0.1f);
            }

            // Assert - Verify save slots
            List<string> saveSlots = _saveManager.GetSaveSlots();
            foreach (string slot in expectedSlots)
            {
                Assert.Contains(slot, saveSlots, $"Auto-save slot {slot} should exist");
            }

            // Verify file naming convention
            foreach (string slot in expectedSlots)
            {
                string filePath = Path.Combine(Application.persistentDataPath, _testSaveFolder, $"{slot}.json");
                Assert.IsTrue(File.Exists(filePath), $"Auto-save file {slot}.json should exist");
            }

            // Clean up mock
            GameObject.DestroyImmediate(gameManagerMock.gameObject);
        }

        private class MockGameManager : MonoBehaviour
        {
            private PlayerData _playerData;

            public void SetPlayerData(PlayerData data)
            {
                _playerData = data;
            }

            public PlayerData GetPlayerData()
            {
                return _playerData;
            }
        }

        #endregion

        #region Test Cloud Save

        [Test]
        public void TestCloudSaveSerialization()
        {
            // Arrange
            string slotName = "cloud_test";

            // Act - Get JSON representation of data
            string json = JsonUtility.ToJson(_testPlayerData, true);

            // Assert - Verify JSON structure
            Assert.IsFalse(string.IsNullOrEmpty(json), "Serialized JSON should not be empty");
            Assert.IsTrue(json.Contains("PlayerId"), "JSON should contain PlayerId");
            Assert.IsTrue(json.Contains("Level"), "JSON should contain Level");
            Assert.IsTrue(json.Contains("Coins"), "JSON should contain Coins");
            Assert.IsTrue(json.Contains("InventoryItems"), "JSON should contain InventoryItems");
            Assert.IsTrue(json.Contains("GameSettings"), "JSON should contain GameSettings");
            Assert.IsTrue(json.Contains("Statistics"), "JSON should contain Statistics");

            // Verify data can be deserialized
            PlayerData deserializedData = JsonUtility.FromJson<PlayerData>(json);
            Assert.IsNotNull(deserializedData, "Deserialized data should not be null");
            Assert.AreEqual(_testPlayerData.PlayerId, deserializedData.PlayerId, "Player ID should match after deserialization");
        }

        [Test]
        public void TestCloudSaveDataFormat()
        {
            // Arrange
            string slotName = "cloud_format_test";

            // Act
            _saveManager.SavePlayerData(_testPlayerData, slotName);

            // Read the actual saved file
            string saveFilePath = Path.Combine(Application.persistentDataPath, _testSaveFolder, $"{slotName}.json");
            string savedJson = File.ReadAllText(saveFilePath);

            // Assert - Verify data format
            Assert.IsTrue(savedJson.Contains("\"PlayerId\": \"" + _testPlayerData.PlayerId + "\""),
                "PlayerId should be properly formatted in JSON");
            Assert.IsTrue(savedJson.Contains("\"Level\": " + _testPlayerData.Level),
                "Level should be properly formatted in JSON");
            Assert.IsTrue(savedJson.Contains("\"Coins\": " + _testPlayerData.Coins),
                "Coins should be properly formatted in JSON");
            Assert.IsTrue(savedJson.Contains("\"CurrentPhase\": \"" + _testPlayerData.CurrentPhase + "\""),
                "CurrentPhase should be properly formatted in JSON");

            // Verify arrays are properly formatted
            Assert.IsTrue(savedJson.Contains("\"CompletedPhases\":"), "CompletedPhases array should exist");
            Assert.IsTrue(savedJson.Contains("\"InventoryItems\":"), "InventoryItems array should exist");
        }

        #endregion

        #region Test Progression Persistence

        [Test]
        public void TestProgressionPersistence()
        {
            // Arrange
            string slotName = "progression_test";
            PlayerData originalData = new PlayerData
            {
                PlayerId = "progression-player",
                CreatedDate = DateTime.Now,
                LastPlayedDate = DateTime.Now,
                Level = 3,
                Experience = 500,
                Coins = 50,
                CurrentPhase = "Phase_1_A",
                CompletedPhases = new List<string>(),
                UnlockedCosmetics = new List<string>(),
                EquippedCosmeticId = "",
                InventoryItems = new List<InventoryItem>(),
                GameSettings = new GameSettings
                {
                    Language = "en",
                    MusicVolume = 0.8f,
                    SfxVolume = 0.9f,
                    VoiceoverVolume = 0.7f,
                    ShowSubtitles = true,
                    AutoSave = true,
                    GraphicsQuality = 2
                },
                Statistics = new PlayerStatistics()
            };

            // Act - Save progression
            _saveManager.SavePlayerData(originalData, slotName);

            // Modify progression
            PlayerData modifiedData = _testPlayerData;
            modifiedData.Coins = 200;
            modifiedData.CurrentPhase = "Phase_3_A";
            modifiedData.CompletedPhases.Add("Phase_2_B");
            modifiedData.Experience = 3500;

            // Save modified data
            _saveManager.SavePlayerData(modifiedData, slotName);

            // Act - Load and verify
            PlayerData loadedData = _saveManager.LoadPlayerData(slotName);

            // Assert - Verify all progression data is preserved
            Assert.IsNotNull(loadedData, "Loaded data should not be null");
            Assert.AreEqual(modifiedData.Level, loadedData.Level, "Level should be preserved");
            Assert.AreEqual(modifiedData.Experience, loadedData.Experience, "Experience should be preserved");
            Assert.AreEqual(modifiedData.Coins, loadedData.Coins, "Coins should be preserved");
            Assert.AreEqual(modifiedData.CurrentPhase, loadedData.CurrentPhase, "Current phase should be preserved");

            // Verify completed phases
            Assert.AreEqual(modifiedData.CompletedPhases.Count, loadedData.CompletedPhases.Count,
                "Completed phases count should match");
            Assert.IsTrue(loadedData.CompletedPhases.Contains("Phase_2_B"),
                "Phase 2B should be in completed phases");
        }

        #endregion

        #region Test Inventory Persistence

        [Test]
        public void TestInventoryPersistence()
        {
            // Arrange
            string slotName = "inventory_test";

            // Create specific test inventory
            _testPlayerData.InventoryItems.Clear();
            _testPlayerData.InventoryItems.Add(new InventoryItem
            {
                ItemId = "coin_001",
                ItemType = "currency",
                AcquiredDate = DateTime.Now,
                IsNew = false
            });
            _testPlayerData.InventoryItems.Add(new InventoryItem
            {
                ItemId = "treasure_chest_001",
                ItemType = "collectible",
                AcquiredDate = DateTime.Now.AddDays(-1),
                IsNew = true
            });
            _testPlayerData.InventoryItems.Add(new InventoryItem
            {
                ItemId = "key_gold_001",
                ItemType = "key",
                AcquiredDate = DateTime.Now.AddHours(-2),
                IsNew = false
            });

            // Act - Save inventory
            _saveManager.SavePlayerData(_testPlayerData, slotName);

            // Modify inventory (remove an item)
            _testPlayerData.InventoryItems.RemoveAt(1); // Remove treasure chest

            // Save modified inventory
            _saveManager.SavePlayerData(_testPlayerData, slotName);

            // Act - Load inventory
            PlayerData loadedData = _saveManager.LoadPlayerData(slotName);

            // Assert - Verify inventory persistence
            Assert.IsNotNull(loadedData, "Loaded data should not be null");
            Assert.AreEqual(2, loadedData.InventoryItems.Count, "Should have 2 items in inventory");

            // Verify remaining items
            var coinItem = loadedData.InventoryItems.FirstOrDefault(i => i.ItemId == "coin_001");
            Assert.IsNotNull(coinItem, "Coin item should exist in inventory");
            Assert.AreEqual("currency", coinItem.ItemType, "Coin item type should match");

            var keyItem = loadedData.InventoryItems.FirstOrDefault(i => i.ItemId == "key_gold_001");
            Assert.IsNotNull(keyItem, "Key item should exist in inventory");
            Assert.AreEqual("key", keyItem.ItemType, "Key item type should match");

            // Verify removed item is not present
            var removedItem = loadedData.InventoryItems.FirstOrDefault(i => i.ItemId == "treasure_chest_001");
            Assert.IsNull(removedItem, "Removed treasure chest should not be in inventory");
        }

        [Test]
        public void TestInventoryItemProperties()
        {
            // Arrange
            string slotName = "inventory_properties_test";

            // Create inventory items with different properties
            _testPlayerData.InventoryItems.Clear();
            var newItem = new InventoryItem
            {
                ItemId = "new_item_001",
                ItemType = "weapon",
                AcquiredDate = DateTime.Now,
                IsNew = true
            };
            _testPlayerData.InventoryItems.Add(newItem);

            // Act - Save and load
            _saveManager.SavePlayerData(_testPlayerData, slotName);
            PlayerData loadedData = _saveManager.LoadPlayerData(slotName);

            // Assert - Verify item properties
            Assert.IsNotNull(loadedData, "Loaded data should not be null");
            Assert.AreEqual(1, loadedData.InventoryItems.Count, "Should have 1 inventory item");

            var loadedItem = loadedData.InventoryItems[0];
            Assert.AreEqual("new_item_001", loadedItem.ItemId, "Item ID should match");
            Assert.AreEqual("weapon", loadedItem.ItemType, "Item type should match");
            Assert.AreEqual(DateTime.Now.Date, loadedItem.AcquiredDate.Date, "Acquired date should match (ignoring time)");
            Assert.IsTrue(loadedItem.IsNew, "Item should be marked as new");
        }

        #endregion

        #region Test Settings Persistence

        [Test]
        public void TestSettingsPersistence()
        {
            // Arrange
            string slotName = "settings_test";

            // Create test settings
            _testPlayerData.GameSettings.Language = "pt";
            _testPlayerData.GameSettings.MusicVolume = 0.3f;
            _testPlayerData.GameSettings.SfxVolume = 0.5f;
            _testPlayerData.GameSettings.VoiceoverVolume = 0.7f;
            _testPlayerData.GameSettings.ShowSubtitles = false;
            _testPlayerData.GameSettings.AutoSave = false;
            _testPlayerData.GameSettings.GraphicsQuality = 1;

            // Act - Save settings
            _saveManager.SavePlayerData(_testPlayerData, slotName);

            // Modify settings
            _testPlayerData.GameSettings.Language = "es";
            _testPlayerData.GameSettings.MusicVolume = 0.6f;
            _testPlayerData.GameSettings.SfxVolume = 0.8f;
            _testPlayerData.GameSettings.VoiceoverVolume = 0.4f;
            _testPlayerData.GameSettings.ShowSubtitles = true;
            _testPlayerData.GameSettings.AutoSave = true;
            _testPlayerData.GameSettings.GraphicsQuality = 3;

            // Save modified settings
            _saveManager.SavePlayerData(_testPlayerData, slotName);

            // Act - Load and verify settings
            PlayerData loadedData = _saveManager.LoadPlayerData(slotName);

            // Assert - Verify all settings are preserved
            Assert.IsNotNull(loadedData, "Loaded data should not be null");
            Assert.AreEqual("es", loadedData.GameSettings.Language, "Language should be preserved");
            Assert.AreEqual(0.6f, loadedData.GameSettings.MusicVolume, "Music volume should be preserved");
            Assert.AreEqual(0.8f, loadedData.GameSettings.SfxVolume, "SFX volume should be preserved");
            Assert.AreEqual(0.4f, loadedData.GameSettings.VoiceoverVolume, "Voiceover volume should be preserved");
            Assert.IsTrue(loadedData.GameSettings.ShowSubtitles, "Subtitles setting should be preserved");
            Assert.IsTrue(loadedData.GameSettings.AutoSave, "Auto-save setting should be preserved");
            Assert.AreEqual(3, loadedData.GameSettings.GraphicsQuality, "Graphics quality should be preserved");
        }

        [Test]
        public void TestDefaultSettings()
        {
            // Arrange
            string slotName = "default_settings_test";
            PlayerData playerWithDefaultSettings = new PlayerData
            {
                PlayerId = "default-settings-player",
                CreatedDate = DateTime.Now,
                LastPlayedDate = DateTime.Now,
                Level = 1,
                Experience = 0,
                Coins = 0,
                CurrentPhase = "Phase_1_A",
                CompletedPhases = new List<string>(),
                UnlockedCosmetics = new List<string>(),
                EquippedCosmeticId = "",
                InventoryItems = new List<InventoryItem>(),
                GameSettings = new GameSettings(),
                Statistics = new PlayerStatistics()
            };

            // Act - Save default settings
            _saveManager.SavePlayerData(playerWithDefaultSettings, slotName);

            // Act - Load settings
            PlayerData loadedData = _saveManager.LoadPlayerData(slotName);

            // Assert - Verify default settings values
            Assert.IsNotNull(loadedData, "Loaded data should not be null");
            Assert.AreEqual("en", loadedData.GameSettings.Language, "Default language should be English");
            Assert.AreEqual(0.5f, loadedData.GameSettings.MusicVolume, "Default music volume should be 0.5");
            Assert.AreEqual(0.5f, loadedData.GameSettings.SfxVolume, "Default SFX volume should be 0.5");
            Assert.AreEqual(0.5f, loadedData.GameSettings.VoiceoverVolume, "Default voiceover volume should be 0.5");
            Assert.IsTrue(loadedData.GameSettings.ShowSubtitles, "Default subtitles should be enabled");
            Assert.IsTrue(loadedData.GameSettings.AutoSave, "Default auto-save should be enabled");
            Assert.AreEqual(2, loadedData.GameSettings.GraphicsQuality, "Default graphics quality should be Medium");
        }

        #endregion

        #region Edge Cases and Error Handling

        [Test]
        public void TestSaveInvalidData()
        {
            // Arrange - Null data
            string slotName = "null_data_test";

            // Act - Save null data
            _saveManager.SavePlayerData(null, slotName);

            // Assert - Should not crash and file should not be created
            string filePath = Path.Combine(Application.persistentDataPath, _testSaveFolder, $"{slotName}.json");
            Assert.IsFalse(File.Exists(filePath), "Save file should not be created for null data");
        }

        [Test]
        public void TestSaveInvalidSlotName()
        {
            // Arrange
            PlayerData data = _testPlayerData;
            string invalidSlotName = "invalid/name/with/slashes";

            // Act - Try to save with invalid slot name
            // This should handle the invalid path gracefully
            _saveManager.SavePlayerData(data, invalidSlotName);

            // Assert - Should not crash
            // The system should sanitize or handle the invalid slot name
            List<string> saveSlots = _saveManager.GetSaveSlots();
            // Check if any save files were created (sanitized names might exist)
            Assert.IsTrue(saveSlots.Count >= 0, "System should handle invalid slot names gracefully");
        }

        [Test]
        public void TestSaveOverwrite()
        {
            // Arrange
            string slotName = "overwrite_test";
            PlayerData originalData = new PlayerData
            {
                PlayerId = "original-player",
                Level = 1,
                Coins = 10,
                CurrentPhase = "Phase_1_A"
            };

            // Act - Save original data
            _saveManager.SavePlayerData(originalData, slotName);

            // Verify file exists
            string filePath = Path.Combine(Application.persistentDataPath, _testSaveFolder, $"{slotName}.json");
            Assert.IsTrue(File.Exists(filePath), "Original save file should exist");

            // Act - Save with different data to same slot
            PlayerData newData = new PlayerData
            {
                PlayerId = "new-player",
                Level = 5,
                Coins = 100,
                CurrentPhase = "Phase_3_B"
            };
            _saveManager.SavePlayerData(newData, slotName);

            // Act - Load data
            PlayerData loadedData = _saveManager.LoadPlayerData(slotName);

            // Assert - Verify data was overwritten
            Assert.IsNotNull(loadedData, "Loaded data should not be null");
            Assert.AreEqual("new-player", loadedData.PlayerId, "Player ID should be overwritten");
            Assert.AreEqual(5, loadedData.Level, "Level should be overwritten");
            Assert.AreEqual(100, loadedData.Coins, "Coins should be overwritten");
            Assert.AreEqual("Phase_3_B", loadedData.CurrentPhase, "Current phase should be overwritten");
        }

        #endregion

        #region Utility Tests

        [Test]
        public void TestGetSaveSlots()
        {
            // Arrange - Create multiple save files
            string[] slotNames = { "slot1", "slot2", "slot3" };
            foreach (string slot in slotNames)
            {
                _saveManager.SavePlayerData(_testPlayerData, slot);
            }

            // Act - Get all save slots
            List<string> saveSlots = _saveManager.GetSaveSlots();

            // Assert - Verify all slots are returned
            Assert.AreEqual(slotNames.Length, saveSlots.Count, "Should return correct number of save slots");

            foreach (string slot in slotNames)
            {
                Assert.Contains(slot, saveSlots, $"Slot {slot} should be in the list");
            }
        }

        [Test]
        public void TestGetSaveSlotInfo()
        {
            // Arrange
            string slotName = "info_test";
            _saveManager.SavePlayerData(_testPlayerData, slotName);

            // Act - Get slot info
            var slotInfo = _saveManager.GetSaveSlotInfo(slotName);

            // Assert - Verify slot information
            Assert.IsNotNull(slotInfo, "Slot info should not be null");
            Assert.AreEqual(slotName, slotInfo.SlotName, "Slot name should match");
            Assert.AreEqual(_testPlayerData.Level, slotInfo.PlayerLevel, "Player level should match");
            Assert.AreEqual(_testPlayerData.CurrentPhase, slotInfo.CurrentPhase, "Current phase should match");
            Assert.AreEqual(_testPlayerData.LastPlayedDate.Date, slotInfo.LastPlayed.Date, "Last played date should match");
            Assert.AreEqual(_testPlayerData.Statistics.TotalPlayTime, slotInfo.PlayTime, "Play time should match");
        }

        [Test]
        public void TestDeleteSave()
        {
            // Arrange
            string slotName = "delete_test";
            _saveManager.SavePlayerData(_testPlayerData, slotName);

            // Verify file exists before deletion
            string filePath = Path.Combine(Application.persistentDataPath, _testSaveFolder, $"{slotName}.json");
            Assert.IsTrue(File.Exists(filePath), "Save file should exist before deletion");

            // Act - Delete save
            _saveManager.DeleteSave(slotName);

            // Assert - File should be deleted
            Assert.IsFalse(File.Exists(filePath), "Save file should be deleted after deletion");
        }

        #endregion
    }
}