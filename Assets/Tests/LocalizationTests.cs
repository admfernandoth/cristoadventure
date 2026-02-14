using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[TestFixture]
public class LocalizationTests
{
    private GameObject localizationManagerObject;
    private LocalizationManager localizationManager;
    private GameObject uiManagerObject;
    private UIManager uiManager;

    [SetUp]
    public void Setup()
    {
        // Load test scene
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);

        // Create Localization Manager
        localizationManagerObject = new GameObject("LocalizationManager");
        localizationManager = localizationManagerObject.AddComponent<LocalizationManager>();

        // Create UI Manager
        uiManagerObject = new GameObject("UIManager");
        uiManager = uiManagerObject.AddComponent<UIManager>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(localizationManagerObject);
        Object.Destroy(uiManagerObject);
        localizationManagerObject = null;
        localizationManager = null;
        uiManagerObject = null;
        uiManager = null;

        SceneManager.LoadScene("EmptyScene", LoadSceneMode.Single);
    }

    [Test]
    public void TestLanguageSwitch()
    {
        // Arrange
        string currentLanguage = "en";
        string targetLanguage = "pt";

        // Act
        localizationManager.SetLanguage(targetLanguage);
        currentLanguage = localizationManager.GetCurrentLanguage();

        // Assert
        Assert.AreEqual(targetLanguage, currentLanguage, "Language should be switched");
    }

    [Test]
    public void TestPortugueseLoads()
    {
        // Arrange
        string testKey = "hello_world";
        Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "pt", new Dictionary<string, string>
                {
                    { "hello_world", "Olá Mundo" }
                }
            },
            {
                "en", new Dictionary<string, string>
                {
                    { "hello_world", "Hello World" }
                }
            }
        };

        // Act
        localizationManager.LoadTranslations(translations);
        localizationManager.SetLanguage("pt");
        string translatedText = localizationManager.GetLocalizedText(testKey);

        // Assert
        Assert.AreEqual("Olá Mundo", translatedText, "Portuguese translation should load correctly");
    }

    [Test]
    public void TestEnglishLoads()
    {
        // Arrange
        string testKey = "hello_world";
        Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "en", new Dictionary<string, string>
                {
                    { "hello_world", "Hello World" }
                }
            },
            {
                "pt", new Dictionary<string, string>
                {
                    { "hello_world", "Olá Mundo" }
                }
            }
        };

        // Act
        localizationManager.LoadTranslations(translations);
        localizationManager.SetLanguage("en");
        string translatedText = localizationManager.GetLocalizedText(testKey);

        // Assert
        Assert.AreEqual("Hello World", translatedText, "English translation should load correctly");
    }

    [Test]
    public void TestSpanishLoads()
    {
        // Arrange
        string testKey = "hello_world";
        Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "es", new Dictionary<string, string>
                {
                    { "hello_world", "Hola Mundo" }
                }
            },
            {
                "en", new Dictionary<string, string>
                {
                    { "hello_world", "Hello World" }
                }
            }
        };

        // Act
        localizationManager.LoadTranslations(translations);
        localizationManager.SetLanguage("es");
        string translatedText = localizationManager.GetLocalizedText(testKey);

        // Assert
        Assert.AreEqual("Hola Mundo", translatedText, "Spanish translation should load correctly");
    }

    [Test]
    public void TestMissingKeyFallback()
    {
        // Arrange
        Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "en", new Dictionary<string, string>
                {
                    { "existing_key", "Existing Text" }
                }
            }
        };

        // Act
        localizationManager.LoadTranslations(translations);
        localizationManager.SetLanguage("en");
        string translatedText = localizationManager.GetLocalizedText("missing_key");

        // Assert
        Assert.AreEqual("[MISSING KEY: missing_key]", translatedText, "Should return fallback for missing key");
    }

    [Test]
    public void TestLanguagePersistence()
    {
        // Arrange
        string targetLanguage = "pt";

        // Act
        localizationManager.SetLanguage(targetLanguage);
        string savedLanguage = PlayerPrefs.GetString("Language", "en");

        // Assert
        Assert.AreEqual(targetLanguage, savedLanguage, "Language preference should be saved");
    }

    [UnityTest]
    public IEnumerator TestUILocalizationUpdate()
    {
        // Arrange
        Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "en", new Dictionary<string, string>
                {
                    { "menu_title", "Main Menu" }
                }
            },
            {
                "pt", new Dictionary<string, string>
                {
                    { "menu_title", "Menu Principal" }
                }
            }
        };

        // Act
        localizationManager.LoadTranslations(translations);
        localizationManager.SetLanguage("pt");
        yield return null;

        string localizedText = uiManager.GetLocalizedText("menu_title");

        // Assert
        Assert.AreEqual("Menu Principal", localizedText, "UI should update with localized text");
    }
}