using UnityEngine;
using System.Collections.Generic;
using System;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [Header("Language Settings")]
    public string currentLanguage = "en";
    public List<Language> availableLanguages = new List<Language>();

    private Dictionary<string, Dictionary<string, string>> localizedTexts = new Dictionary<string, Dictionary<string, string>>();

    [Serializable]
    public class Language
    {
        public string code;
        public string name;
        public string nativeName;
        public Sprite flagIcon;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize default languages
        InitializeLanguages();
    }

    private void InitializeLanguages()
    {
        if (availableLanguages.Count == 0)
        {
            availableLanguages.AddRange(new List<Language>
            {
                new Language { code = "en", name = "English", nativeName = "English" },
                new Language { code = "es", name = "Spanish", nativeName = "Español" },
                new Language { code = "fr", name = "French", nativeName = "Français" }
            });
        }
    }

    public void SetLanguage(string languageCode)
    {
        foreach (var language in availableLanguages)
        {
            if (language.code == languageCode)
            {
                currentLanguage = languageCode;

                // Notify all LoadingManagers to update
                LoadingManager[] loadingManagers = FindObjectsOfType<LoadingManager>();
                foreach (var loadingManager in loadingManagers)
                {
                    loadingManager.SetLanguage(languageCode);
                }

                Debug.Log($"Language changed to: {language.nativeName}");
                return;
            }
        }

        Debug.LogWarning($"Language {languageCode} not found. Using current language: {currentLanguage}");
    }

    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }

    public List<Language> GetAvailableLanguages()
    {
        return availableLanguages;
    }

    public Language GetCurrentLanguageData()
    {
        foreach (var language in availableLanguages)
        {
            if (language.code == currentLanguage)
            {
                return language;
            }
        }
        return availableLanguages[0]; // Fallback to first language
    }

    // Add localized text to the manager
    public void AddLocalizedText(string id, string english, string spanish, string french)
    {
        if (!localizedTexts.ContainsKey(id))
        {
            localizedTexts[id] = new Dictionary<string, string>();
        }

        localizedTexts[id]["en"] = english;
        localizedTexts[id]["es"] = spanish;
        localizedTexts[id]["fr"] = french;
    }

    // Get localized text by ID
    public string GetLocalizedText(string id)
    {
        if (localizedTexts.ContainsKey(id) && localizedTexts[id].ContainsKey(currentLanguage))
        {
            return localizedTexts[id][currentLanguage];
        }

        // Fallback to English if current language not found
        if (localizedTexts.ContainsKey(id) && localizedTexts[id].ContainsKey("en"))
        {
            return localizedTexts[id]["en"];
        }

        return $"[Missing: {id}]";
    }

    // Check if text exists for current language
    public bool HasLocalizedText(string id)
    {
        return localizedTexts.ContainsKey(id) && localizedTexts[id].ContainsKey(currentLanguage);
    }

    // Get all available text IDs
    public List<string> GetAllTextIDs()
    {
        return new List<string>(localizedTexts.Keys);
    }
}