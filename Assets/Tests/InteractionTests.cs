using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;

[TestFixture]
public class InteractionTests
{
    private GameObject player;
    private PlayerController playerController;
    private GameObject poi;
    private POI poiController;
    private GameObject poi2;
    private POI poi2Controller;

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
        player.AddComponent<PlayerDetection>();

        // Create test POI 1
        poi = new GameObject("POI1");
        poiController = poi.AddComponent<POI>();
        poi.AddComponent<BoxCollider2D>();
        poi.transform.position = new Vector3(5, 0, 0);

        // Create test POI 2
        poi2 = new GameObject("POI2");
        poi2Controller = poi2.AddComponent<POI>();
        poi2.AddComponent<BoxCollider2D>();
        poi2.transform.position = new Vector3(10, 0, 0);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player);
        Object.Destroy(poi);
        Object.Destroy(poi2);
        player = null;
        playerController = null;
        poi = null;
        poiController = null;
        poi2 = null;
        poi2Controller = null;

        SceneManager.LoadScene("EmptyScene", LoadSceneMode.Single);
    }

    [Test]
    public void TestPOIDetection()
    {
        // Arrange
        player.transform.position = new Vector3(4, 0, 0);

        // Act
        POI detectedPoi = playerController.GetNearestPOI();

        // Assert
        Assert.IsNotNull(detectedPoi, "Should detect a POI when nearby");
        Assert.AreEqual(poi, detectedPoi.gameObject, "Should detect the correct POI");
    }

    [UnityTest]
    public IEnumerator TestPOIInteraction()
    {
        // Arrange
        player.transform.position = new Vector3(4, 0, 0);
        bool interactionCalled = false;
        poiController.OnInteract += () => interactionCalled = true;

        // Act
        playerController.Interact();
        yield return null;

        // Assert
        Assert.IsTrue(interactionCalled, "POI interaction should be called");
        Assert.IsTrue(poiController.wasInteracted, "POI should mark as interacted");
    }

    [Test]
    public void TestMultiplePOIs()
    {
        // Arrange
        player.transform.position = new Vector3(7, 0, 0); // Between POIs

        // Act
        POI nearest = playerController.GetNearestPOI();

        // Assert
        Assert.IsNotNull(nearest, "Should detect nearest POI");
        Assert.AreEqual(poi, nearest.gameObject, "Should detect the closer POI");
        Assert.AreEqual(poiController, nearest, "Should get the correct POI controller");

        // Test distance calculations
        float distToPOI1 = Vector3.Distance(player.transform.position, poi.transform.position);
        float distToPOI2 = Vector3.Distance(player.transform.position, poi2.transform.position);
        float distToNearest = Vector3.Distance(player.transform.position, nearest.transform.position);

        Assert.AreEqual(distToPOI1, distToNearest, "Nearest POI should be the closest");
    }
}