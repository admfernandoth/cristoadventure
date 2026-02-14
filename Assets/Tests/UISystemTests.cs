using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.SceneManagement;

[TestFixture]
public class UISystemTests
{
    private GameObject uiManagerObject;
    private UIManager uiManager;
    private GameObject player;

    [SetUp]
    public void Setup()
    {
        // Load test scene
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);

        // Create UI Manager
        uiManagerObject = new GameObject("UIManager");
        uiManager = uiManagerObject.AddComponent<UIManager>();

        // Create test player
        player = new GameObject("Player");
        player.AddComponent<PlayerController>();
        player.AddComponent<Rigidbody2D>();
        player.AddComponent<BoxCollider2D>();
        player.transform.position = Vector3.zero;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(uiManagerObject);
        Object.Destroy(player);
        uiManagerObject = null;
        uiManager = null;
        player = null;

        SceneManager.LoadScene("EmptyScene", LoadSceneMode.Single);
    }

    [Test]
    public void TestHUDShows()
    {
        // Arrange
        bool hudInitialized = false;

        // Act
        uiManager.InitializeHUD();
        hudInitialized = uiManager.IsHUDInitialized();

        // Assert
        Assert.IsTrue(hudInitialized, "HUD should be initialized");
    }

    [UnityTest]
    public IEnumerator TestInfoPanelOpens()
    {
        // Arrange
        bool panelOpened = false;

        // Act
        uiManager.OpenInfoPanel();
        yield return null;

        // Assert
        panelOpened = uiManager.IsPanelOpen("InfoPanel");
        Assert.IsTrue(panelOpened, "Info Panel should be open");
    }

    [UnityTest]
    public IEnumerator TestSettingsPanelWorks()
    {
        // Arrange
        bool settingsPanelOpened = false;
        float volumeBefore = 0.5f;
        float volumeAfter = 0.5f;

        // Set initial volume
        PlayerPrefs.SetFloat("MasterVolume", volumeBefore);

        // Act
        uiManager.OpenSettingsPanel();
        yield return null;

        // Test volume change
        uiManager.SetVolume(0.8f);
        volumeAfter = PlayerPrefs.GetFloat("MasterVolume", 0.5f);

        // Assert
        settingsPanelOpened = uiManager.IsPanelOpen("SettingsPanel");
        Assert.IsTrue(settingsPanelOpened, "Settings Panel should be open");
        Assert.AreEqual(0.8f, volumeAfter, 0.01f, "Volume should be updated");
    }

    [UnityTest]
    public IEnumerator TestPauseMenu()
    {
        // Arrange
        bool gamePaused = false;
        bool menuOpened = false;

        // Act
        uiManager.TogglePauseMenu();
        yield return null;

        // Assert
        gamePaused = Time.timeScale < 1f;
        menuOpened = uiManager.IsPanelOpen("PauseMenu");

        Assert.IsTrue(gamePaused, "Game should be paused");
        Assert.IsTrue(menuOpened, "Pause Menu should be open");

        // Test unpausing
        uiManager.TogglePauseMenu();
        yield return null;

        gamePaused = Time.timeScale < 1f;
        Assert.IsFalse(gamePaused, "Game should be unpaused");
    }

    [Test]
    public void TestLocalizationUI()
    {
        // Arrange
        string currentLanguage = "en";
        string[] availableLanguages = { "en", "pt", "es" };

        // Act
        currentLanguage = uiManager.GetCurrentLanguage();

        // Assert
        Assert.IsNotNull(currentLanguage, "Current language should be set");
        Assert.Contains(currentLanguage, availableLanguages, "Language should be from available options");
    }

    [Test]
    public void TestScoreDisplay()
    {
        // Arrange
        int score = 100;

        // Act
        uiManager.UpdateScore(score);

        // Assert
        Assert.AreEqual(score, uiManager.GetCurrentScore(), "Score should be updated correctly");
    }
}