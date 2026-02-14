using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CristoAdventure.Localization
{
    /// <summary>
    /// Localization dictionary for Cristo Adventure
    /// Provides runtime access to localized strings with caching and fallback support
    /// Created by: Agent-12 (Narrative Designer)
    /// Date: 14/02/2026
    /// </summary>
    public class LocalizationDictionary : MonoBehaviour
    {
        [Header("Current Language Settings")]
        [SerializeField] private SystemLanguage currentLanguage = SystemLanguage.English;

        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = false;
        [SerializeField] private int cacheSize = 0;

        // Cache for frequently accessed strings
        private Dictionary<string, string> stringCache = new Dictionary<string, string>();

        // Event fired when language changes
        public static event Action<SystemLanguage> OnLanguageChanged;

        // Singleton instance
        private static LocalizationDictionary instance;
        public static LocalizationDictionary Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("LocalizationDictionary");
                    instance = go.AddComponent<LocalizationDictionary>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize with system language
            currentLanguage = Application.systemLanguage;

            // Validate language is supported
            if (!IsSupportedLanguage(currentLanguage))
            {
                Debug.LogWarning($"System language {currentLanguage} is not supported. Falling back to English.");
                currentLanguage = SystemLanguage.English;
            }
        }

        private void Update()
        {
            if (showDebugInfo)
            {
                cacheSize = stringCache.Count;
            }
        }

        /// <summary>
        /// Check if a language is supported
        /// </summary>
        public bool IsSupportedLanguage(SystemLanguage language)
        {
            return language == SystemLanguage.Portuguese ||
                   language == SystemLanguage.English ||
                   language == SystemLanguage.Spanish;
        }

        /// <summary>
        /// Set the current language
        /// </summary>
        public void SetLanguage(SystemLanguage language)
        {
            if (!IsSupportedLanguage(language))
            {
                Debug.LogWarning($"Language {language} is not supported.");
                return;
            }

            if (currentLanguage != language)
            {
                currentLanguage = language;
                stringCache.Clear(); // Clear cache when language changes
                OnLanguageChanged?.Invoke(language);
                Debug.Log($"Language changed to {language}");
            }
        }

        /// <summary>
        /// Get the current language
        /// </summary>
        public SystemLanguage GetCurrentLanguage()
        {
            return currentLanguage;
        }

        /// <summary>
        /// Get localized string by key
        /// </summary>
        public string Get(LocalizationTables.Key key)
        {
            return Get(key, currentLanguage);
        }

        /// <summary>
        /// Get localized string by key and language
        /// </summary>
        public string Get(LocalizationTables.Key key, SystemLanguage language)
        {
            // Generate cache key
            string cacheKey = $"{key}_{language}";

            // Check cache first
            if (stringCache.ContainsKey(cacheKey))
            {
                return stringCache[cacheKey];
            }

            // Get localized string
            LocalizationTables.Language locLanguage = ConvertToLocalizationLanguage(language);
            string localizedString = LocalizationTables.Get(key, locLanguage);

            // Cache the result
            if (!string.IsNullOrEmpty(localizedString))
            {
                stringCache[cacheKey] = localizedString;
            }

            return localizedString;
        }

        /// <summary>
        /// Get localized string by enum key with format support
        /// </summary>
        public string GetFormatted(LocalizationTables.Key key, params object[] args)
        {
            string formatString = Get(key);
            return string.Format(formatString, args);
        }

        /// <summary>
        /// Get multi-line content for POI plaques
        /// </summary>
        public string[] GetContentLines(
            LocalizationTables.Key line1Key,
            LocalizationTables.Key line2Key,
            LocalizationTables.Key line3Key)
        {
            return LocalizationTables.GetContentLines(
                line1Key,
                line2Key,
                line3Key,
                ConvertToLocalizationLanguage(currentLanguage)
            );
        }

        /// <summary>
        /// Get all puzzle events
        /// </summary>
        public string[] GetPuzzleEvents()
        {
            return LocalizationTables.GetPuzzleEvents(ConvertToLocalizationLanguage(currentLanguage));
        }

        /// <summary>
        /// Get all puzzle hints
        /// </summary>
        public string[] GetPuzzleHints()
        {
            return LocalizationTables.GetPuzzleHints(ConvertToLocalizationLanguage(currentLanguage));
        }

        /// <summary>
        /// Get NPC dialogue options
        /// </summary>
        public string[] GetNPCOptions()
        {
            return LocalizationTables.GetNPCOptions(ConvertToLocalizationLanguage(currentLanguage));
        }

        /// <summary>
        /// Get NPC responses
        /// </summary>
        public string[] GetNPCResponses()
        {
            return LocalizationTables.GetNPCResponses(ConvertToLocalizationLanguage(currentLanguage));
        }

        /// <summary>
        /// Convert SystemLanguage to LocalizationTables.Language
        /// </summary>
        private LocalizationTables.Language ConvertToLocalizationLanguage(SystemLanguage systemLanguage)
        {
            switch (systemLanguage)
            {
                case SystemLanguage.Portuguese:
                    return LocalizationTables.Language.Portuguese;
                case SystemLanguage.Spanish:
                    return LocalizationTables.Language.Spanish;
                default:
                    return LocalizationTables.Language.English;
            }
        }

        /// <summary>
        /// Preload strings for a phase into cache
        /// </summary>
        public void PreloadPhase(LocalizationTables.Key[] keys)
        {
            foreach (var key in keys)
            {
                Get(key); // This will cache the result
            }
            Debug.Log($"Preloaded {keys.Length} strings for current language.");
        }

        /// <summary>
        /// Clear the string cache
        /// </summary>
        public void ClearCache()
        {
            stringCache.Clear();
            Debug.Log("Localization cache cleared.");
        }

        /// <summary>
        /// Get cache statistics
        /// </summary>
        public string GetCacheStats()
        {
            return $"Cached strings: {stringCache.Count}";
        }

        /// <summary>
        /// Get all supported languages
        /// </summary>
        public static List<SystemLanguage> GetSupportedLanguages()
        {
            return new List<SystemLanguage>
            {
                SystemLanguage.Portuguese,
                SystemLanguage.English,
                SystemLanguage.Spanish
            };
        }

        /// <summary>
        /// Get language display name
        /// </summary>
        public static string GetLanguageDisplayName(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.Portuguese:
                    return "Português";
                case SystemLanguage.English:
                    return "English";
                case SystemLanguage.Spanish:
                    return "Español";
                default:
                    return language.ToString();
            }
        }

        /// <summary>
        /// Get language native name (in the language itself)
        /// </summary>
        public static string GetLanguageNativeName(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.Portuguese:
                    return "Português";
                case SystemLanguage.English:
                    return "English";
                case SystemLanguage.Spanish:
                    return "Español";
                default:
                    return language.ToString();
            }
        }
    }

    /// <summary>
    /// Static helper class for quick localization access
    /// </summary>
    public static class L
    {
        /// <summary>
        /// Quick access to localized string
        /// </summary>
        public static string Get(LocalizationTables.Key key)
        {
            if (LocalizationDictionary.Instance != null)
            {
                return LocalizationDictionary.Instance.Get(key);
            }
            return LocalizationTables.Get(key);
        }

        /// <summary>
        /// Quick access to formatted localized string
        /// </summary>
        public static string GetFormatted(LocalizationTables.Key key, params object[] args)
        {
            if (LocalizationDictionary.Instance != null)
            {
                return LocalizationDictionary.Instance.GetFormatted(key, args);
            }
            string formatString = LocalizationTables.Get(key);
            return string.Format(formatString, args);
        }

        /// <summary>
        /// Get current language
        /// </summary>
        public static SystemLanguage CurrentLanguage
        {
            get
            {
                if (LocalizationDictionary.Instance != null)
                {
                    return LocalizationDictionary.Instance.GetCurrentLanguage();
                }
                return Application.systemLanguage;
            }
        }

        // Convenient shortcuts for common UI strings
        public static string Interact => Get(LocalizationTables.Key.UI_Interact);
        public static string Backpack => Get(LocalizationTables.Key.UI_Backpack);
        public static string Pause => Get(LocalizationTables.Key.UI_Pause);
        public static string Settings => Get(LocalizationTables.Key.UI_Settings);
        public static string PhaseComplete => Get(LocalizationTables.Key.UI_PhaseComplete);
        public static string StarsEarned => Get(LocalizationTables.Key.UI_StarsEarned);
    }
}
