using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[TestFixture]
public class PuzzleTests
{
    private GameObject player;
    private PlayerController playerController;
    private GameObject puzzleObject;
    private PuzzleController puzzleController;
    private UIManager uiManager;

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

        // Create test puzzle
        puzzleObject = new GameObject("Puzzle");
        puzzleController = puzzleObject.AddComponent<PuzzleController>();
        puzzleController.puzzleID = "test_puzzle";

        // Create UI Manager
        uiManager = new GameObject("UIManager").AddComponent<UIManager>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player);
        Object.Destroy(puzzleObject);
        Object.Destroy(uiManager.gameObject);
        player = null;
        playerController = null;
        puzzleObject = null;
        puzzleController = null;
        uiManager = null;

        SceneManager.LoadScene("EmptyScene", LoadSceneMode.Single);
    }

    [Test]
    public void TestPuzzleStartup()
    {
        // Arrange
        puzzleController.question = "What is 2 + 2?";
        puzzleController.correctAnswer = "4";
        puzzleController.puzzleCompleted = false;

        // Act
        puzzleController.InitializePuzzle();

        // Assert
        Assert.IsNotNull(puzzleController.question, "Puzzle should have a question");
        Assert.IsNotNull(puzzleController.correctAnswer, "Puzzle should have a correct answer");
        Assert.IsFalse(puzzleController.puzzleCompleted, "New puzzle should not be completed");
        Assert.IsTrue(puzzleController.answers.Count > 0, "Puzzle should have answer options");
    }

    [Test]
    public void TestCorrectAnswer()
    {
        // Arrange
        puzzleController.question = "What is 2 + 2?";
        puzzleController.correctAnswer = "4";
        puzzleController.answers = new List<string> { "3", "4", "5", "6" };
        puzzleController.puzzleCompleted = false;

        // Act
        bool result = puzzleController.CheckAnswer("4");

        // Assert
        Assert.IsTrue(result, "Correct answer should return true");
        Assert.IsTrue(puzzleController.puzzleCompleted, "Puzzle should be marked as completed");
    }

    [Test]
    public void TestWrongAnswer()
    {
        // Arrange
        puzzleController.question = "What is 2 + 2?";
        puzzleController.correctAnswer = "4";
        puzzleController.answers = new List<string> { "3", "4", "5", "6" };
        puzzleController.puzzleCompleted = false;

        // Act
        bool result = puzzleController.CheckAnswer("3");

        // Assert
        Assert.IsFalse(result, "Wrong answer should return false");
        Assert.IsFalse(puzzleController.puzzleCompleted, "Puzzle should not be completed");
    }

    [Test]
    public void TestPuzzleCompletion()
    {
        // Arrange
        puzzleController.question = "What is 2 + 2?";
        puzzleController.correctAnswer = "4";
        puzzleController.answers = new List<string> { "3", "4", "5", "6" };
        puzzleController.puzzleCompleted = false;
        bool completionTriggered = false;

        puzzleController.OnPuzzleCompleted += () => completionTriggered = true;

        // Act
        bool answerResult = puzzleController.CheckAnswer("4");

        // Assert
        Assert.IsTrue(answerResult, "Correct answer should trigger completion");
        Assert.IsTrue(puzzleController.puzzleCompleted, "Puzzle should be completed");
        Assert.IsTrue(completionTriggered, "Puzzle completion event should be triggered");
    }

    [UnityTest]
    public IEnumerator TestHintSystem()
    {
        // Arrange
        puzzleController.question = "What is 2 + 2?";
        puzzleController.correctAnswer = "4";
        puzzleController.hint = "Think about basic addition";
        string hint = null;

        // Act
        yield return null;
        hint = puzzleController.GetHint();

        // Assert
        Assert.IsNotNull(hint, "Hint should be available");
        Assert.AreEqual("Think about basic addition", hint, "Hint should match expected text");
    }
}