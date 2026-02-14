using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;

[TestFixture]
public class GameplayTests
{
    private GameObject player;
    private PlayerController playerController;

    [SetUp]
    public void Setup()
    {
        // Load test scene or create player
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);

        // Find player in scene or create test player
        player = GameObject.Find("Player");
        if (player == null)
        {
            player = new GameObject("Player");
            playerController = player.AddComponent<PlayerController>();
            player.AddComponent<Rigidbody2D>();
            player.AddComponent<BoxCollider2D>();
            player.transform.position = Vector3.zero;
        }
        else
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player);
        player = null;
        playerController = null;

        // Load empty scene to clean up
        SceneManager.LoadScene("EmptyScene", LoadSceneMode.Single);
    }

    [Test]
    public void TestPlayerCanMove()
    {
        // Arrange
        Vector3 initialPosition = player.transform.position;

        // Act
        playerController.Move(Vector2.right);

        // Wait for movement
        yield return null;

        // Assert
        Vector3 finalPosition = player.transform.position;
        Assert.AreNotEqual(initialPosition, finalPosition, "Player should be able to move");
    }

    [UnityTest]
    public IEnumerator TestPlayerCanRun()
    {
        // Arrange
        Vector3 initialPosition = player.transform.position;
        float runSpeedMultiplier = 2.0f;

        // Act
        playerController.Run(true);
        playerController.Move(Vector2.right);

        // Wait for movement
        yield return new WaitForSeconds(0.1f);

        // Assert
        Vector3 finalPosition = player.transform.position;
        float expectedDistance = Time.deltaTime * playerController.moveSpeed * runSpeedMultiplier;
        float actualDistance = Vector3.Distance(initialPosition, finalPosition);

        Assert.Greater(actualDistance, 0, "Player should move when running");
        Assert.Greater(actualDistance, expectedDistance * 0.9f, "Player should move faster when running");
        Assert.Less(actualDistance, expectedDistance * 1.1f, "Player should move at reasonable running speed");
    }

    [Test]
    public void TestPlayerStaysInBounds()
    {
        // Arrange
        Vector3 testPosition = new Vector3(999, 0, 0); // Out of bounds position
        player.transform.position = testPosition;

        // Act
        Vector3 clampedPosition = playerController.ClampToBounds(testPosition);

        // Assert
        Assert.IsTrue(clampedPosition.x < 100, "Player X position should be clamped");
        Assert.IsTrue(clampedPosition.x > -100, "Player X position should be clamped");
        Assert.IsTrue(clampedPosition.y < 100, "Player Y position should be clamped");
        Assert.IsTrue(clampedPosition.y > -100, "Player Y position should be clamped");
    }
}