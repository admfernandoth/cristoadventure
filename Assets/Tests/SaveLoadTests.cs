using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.IO;
using System;
using UnityEngine.SceneManagement;

[TestFixture]
public class SaveLoadTests
{
    private GameObject player;
    private PlayerController playerController;
    private SaveLoadManager saveLoadManager;
    private string testSavePath;

    [SetUp]
    public void Setup()
    {
        // Load test scene
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);

        // Create test player
        player = new GameObject("Player");
        playerController = player.AddComponent<PlayerController>();
        player.AddComponent<Rigidbody2D>();
        player.AddComponent<BoxCollider2D>();
        player.transform.position = Vector3.zero;

        // Create SaveLoadManager
        saveLoadManager = new GameObject("SaveLoadManager").AddComponent<SaveLoadManager>();

        // Set test save path
        testSavePath = Path.Combine(Application.persistentDataPath, "test_save.sav");
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up test files
        if (File.Exists(testSavePath))
        {
            File.Delete(testSavePath);
        }

        Object.Destroy(player);
        Object.Destroy(saveLoadManager.gameObject);
        player = null;
        playerController = null;
        saveLoadManager = null;

        SceneManager.LoadScene("EmptyScene", LoadSceneMode.Single);
    }

    [Test]
    public void TestNewGameCreatesData()
    {
        // Arrange
        saveLoadManager.NewGame();

        // Act
        GameData gameData = saveLoadManager.GetCurrentGameData();

        // Assert
        Assert.IsNotNull(gameData, "Game data should be created");
        Assert.AreEqual(0, gameData.score, "New game should start with score 0");
        Assert.AreEqual(0, gameData.puzzlesSolved, "New game should have 0 puzzles solved");
        Assert.IsNotNull(gameData.playerPosition, "Player position should be initialized");
        Assert.IsNotNull(gameData.saveTimestamp, "Save timestamp should be set");
    }

    [Test]
    public void TestSavePersists()
    {
        // Arrange
        GameData originalData = new GameData
        {
            playerName = "TestPlayer",
            score = 100,
            puzzlesSolved = 3,
            playerPosition = new Vector3(10, 5, 0),
            saveTimestamp = DateTime.Now.ToString()
        };

        // Act
        saveLoadManager.SaveGame(originalData);
        GameData loadedData = saveLoadManager.LoadGame();

        // Assert
        Assert.IsNotNull(loadedData, "Loaded data should not be null");
        Assert.AreEqual(originalData.playerName, loadedData.playerName, "Player name should persist");
        Assert.AreEqual(originalData.score, loadedData.score, "Score should persist");
        Assert.AreEqual(originalData.puzzlesSolved, loadedData.puzzlesSolved, "Puzzles solved should persist");
        Assert.AreEqual(originalData.playerPosition, loadedData.playerPosition, "Player position should persist");
    }

    [UnityTest]
    public IEnumerator TestLoadRestores()
    {
        // Arrange
        GameData originalData = new GameData
        {
            playerName = "TestPlayer",
            score = 200,
            puzzlesSolved = 5,
            playerPosition = new Vector3(20, 10, 0),
            saveTimestamp = DateTime.Now.ToString()
        };

        saveLoadManager.SaveGame(originalData);

        // Move player to different position
        playerController.transform.position = new Vector3(30, 15, 0);
        yield return null;

        // Act
        saveLoadManager.LoadGame();
        yield return null;

        // Assert
        GameData currentData = saveLoadManager.GetCurrentGameData();
        Assert.AreEqual(originalData.playerName, currentData.playerName, "Player name should be restored");
        Assert.AreEqual(originalData.score, currentData.score, "Score should be restored");
        Assert.AreEqual(originalData.puzzlesSolved, currentData.puzzlesSolved, "Puzzles solved should be restored");
        Assert.AreEqual(originalData.playerPosition, currentData.playerPosition, "Player position should be restored");
    }

    [Test]
    public void TestMultipleSaveSlots()
    {
        // Arrange
        GameData data1 = new GameData { playerName = "Player1", score = 100 };
        GameData data2 = new GameData { playerName = "Player2", score = 200 };

        // Act
        saveLoadManager.SaveGame(data1, "slot1");
        saveLoadManager.SaveGame(data2, "slot2");

        // Assert
        GameData loaded1 = saveLoadManager.LoadGame("slot1");
        GameData loaded2 = saveLoadManager.LoadGame("slot2");

        Assert.AreEqual("Player1", loaded1.playerName);
        Assert.AreEqual("Player2", loaded2.playerName);
        Assert.AreEqual(100, loaded1.score);
        Assert.AreEqual(200, loaded2.score);
    }

    [Test]
    public void TestSaveFileExists()
    {
        // Arrange
        GameData data = new GameData { playerName = "TestPlayer", score = 50 };

        // Act
        saveLoadManager.SaveGame(data);

        // Assert
        Assert.IsTrue(File.Exists(testSavePath), "Save file should exist");
        Assert.IsTrue(new FileInfo(testSavePath).Length > 0, "Save file should not be empty");
    }

    [Test]
    public void TestSaveErrorHandling()
    {
        // Arrange
        string invalidPath = "/invalid/path/save.sav";

        // Act & Assert
        Assert.Throws<Exception>(() =>
        {
            saveLoadManager.SaveGame(new GameData(), invalidPath);
        }, "Should throw exception for invalid save path");
    }
}