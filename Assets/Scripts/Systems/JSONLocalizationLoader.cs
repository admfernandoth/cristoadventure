using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace CristoAdventure.Localization
{
    /// <summary>
    /// Loads localization data from JSON files
    /// Provides fallback to default language if translation is missing
    /// </summary>
    public static class JSONLocalizationLoader
    {
        private static Dictionary<string, Dictionary<string, string>> loadedLanguages = new Dictionary<string, Dictionary<string, string>>();
        private static readonly string LOCALIZATION_PATH = "Assets/Localization/";

        // Supported language codes
        public const string PT = "pt";
        public const string EN = "en";
        public const string ES = "es";

        // Current language (default to Portuguese)
        private static string currentLanguage = PT;

        /// <summary>
        /// Initialize the localization system by loading all language files
        /// </summary>
        public static void Initialize()
        {
            LoadLanguage(PT);
            LoadLanguage(EN);
            LoadLanguage(ES);

            Debug.Log("[JSONLocalizationLoader] Localization initialized with " + loadedLanguages.Count + " languages.");
        }

        /// <summary>
        /// Load a specific language file
        /// </summary>
        private static bool LoadLanguage(string languageCode)
        {
            if (loadedLanguages.ContainsKey(languageCode))
            {
                return true; // Already loaded
            }

            string fileName = languageCode switch
            {
                PT => "Phase1_1_Localization.json",
                EN => "Phase1_1_Localization_EN.json",
                ES => "Phase1_1_Localization_ES.json",
                _ => null
            };

            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError($"[JSONLocalizationLoader] Unsupported language code: {languageCode}");
                return false;
            }

            string filePath = Path.Combine(LOCALIZATION_PATH, fileName);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[JSONLocalizationLoader] Localization file not found: {filePath}");
                return false;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var data = JsonUtility.FromJson<LocalizationFile>(json);

                var strings = new Dictionary<string, string>();
                foreach (var kvp in data.strings)
                {
                    strings[kvp.Key] = kvp.Value;
                }

                loadedLanguages[languageCode] = strings;
                Debug.Log($"[JSONLocalizationLoader] Loaded language: {data.languageName} ({languageCode}) with {strings.Count} strings.");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JSONLocalizationLoader] Failed to load localization file {filePath}: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get a localized string by key
        /// </summary>
        public static string GetString(string key, string languageCode = null)
        {
            // Use current language if not specified
            languageCode = languageCode ?? currentLanguage;

            // Check if language is loaded
            if (!loadedLanguages.ContainsKey(languageCode))
            {
                Debug.LogWarning($"[JSONLocalizationLoader] Language not loaded: {languageCode}, falling back to Portuguese");
                languageCode = PT;
            }

            // Try to get the string
            if (loadedLanguages.TryGetValue(languageCode, out var strings))
            {
                if (strings.TryGetValue(key, out var value))
                {
                    return value;
                }
            }

            // Fall back to Portuguese if current language doesn't have the key
            if (languageCode != PT && loadedLanguages.ContainsKey(PT))
            {
                if (loadedLanguages[PT].TryGetValue(key, out var fallbackValue))
                {
                    Debug.LogWarning($"[JSONLocalizationLoader] Key '{key}' not found in {languageCode}, using Portuguese fallback");
                    return fallbackValue;
                }
            }

            // Final fallback: return the key itself
            Debug.LogWarning($"[JSONLocalizationLoader] Key '{key}' not found in any language");
            return key;
        }

        /// <summary>
        /// Set the current language
        /// </summary>
        public static void SetLanguage(string languageCode)
        {
            if (!loadedLanguages.ContainsKey(languageCode))
            {
                Debug.LogError($"[JSONLocalizationLoader] Cannot set language to {languageCode}: not loaded");
                return;
            }

            currentLanguage = languageCode;
            Debug.Log($"[JSONLocalizationLoader] Language changed to: {GetLanguageName(languageCode)}");
        }

        /// <summary>
        /// Get the current language code
        /// </summary>
        public static string GetCurrentLanguage()
        {
            return currentLanguage;
        }

        /// <summary>
        /// Get the name of a language
        /// </summary>
        public static string GetLanguageName(string languageCode)
        {
            return languageCode switch
            {
                PT => "Portuguese",
                EN => "English",
                ES => "Spanish",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Check if a key exists in the current language
        /// </summary>
        public static bool HasKey(string key, string languageCode = null)
        {
            languageCode = languageCode ?? currentLanguage;
            return loadedLanguages.ContainsKey(languageCode) && loadedLanguages[languageCode].ContainsKey(key);
        }

        /// <summary>
        /// Get all available language codes
        /// </summary>
        public static string[] GetAvailableLanguages()
        {
            var languages = new List<string>();
            foreach (var kvp in loadedLanguages)
            {
                languages.Add(kvp.Key);
            }
            return languages.ToArray();
        }

        /// <summary>
        /// Reload a specific language file
        /// </summary>
        public static bool ReloadLanguage(string languageCode)
        {
            if (loadedLanguages.ContainsKey(languageCode))
            {
                loadedLanguages.Remove(languageCode);
            }
            return LoadLanguage(languageCode);
        }

        /// <summary>
        /// Clear all loaded languages
        /// </summary>
        public static void ClearCache()
        {
            loadedLanguages.Clear();
        }

        // JSON data structure
        [System.Serializable]
        private class LocalizationFile
        {
            public string languageCode;
            public string languageName;
            public Dictionary<string, string> strings;
        }
    }
}
