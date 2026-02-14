using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace CristoAdventure.Systems
{
    /// <summary>
    /// Manages game localization across Portuguese, English, and Spanish
    /// Uses Unity Localization package
    /// </summary>
    public class LocalizationManager : MonoBehaviour
    {
        #region Singleton

        private static LocalizationManager _instance;
        public static LocalizationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_LocalizationManager");
                    _instance = go.AddComponent<LocalizationManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region Supported Languages

        public enum Language
        {
            Portuguese,
            English,
            Spanish
        }

        #endregion

        #region State

        private Language _currentLanguage = Language.Portuguese;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved language preference
            LoadLanguagePreference();
        }

        private void Start()
        {
            // Initialize Unity Localization
            InitializeLocalization();
        }

        #endregion

        #region Initialization

        private void InitializeLocalization()
        {
#if LOCALIZATION_ENABLED
            if (LocalizationSettings.Instance != null)
            {
                // Set available locales
                var locales = LocalizationSettings.Instance.GetAvailableLocales();

                // Find matching locale for current language
                foreach (var locale in locales.Locales)
                {
                    if (locale.Identifier.Code == GetLanguageCode(_currentLanguage))
                    {
                        LocalizationSettings.Instance.SetSelectedLocale(locale);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("[LocalizationManager] Unity Localization not configured. Using fallback.");
            }
#endif
        }

        #endregion

        #region Language Control

        public void SetLanguage(Language language)
        {
            if (_currentLanguage == language) return;

            _currentLanguage = language;

            // Update Unity Localization
#if LOCALIZATION_ENABLED
            if (LocalizationSettings.Instance != null)
            {
                var locales = LocalizationSettings.Instance.GetAvailableLocales();
                foreach (var locale in locales.Locales)
                {
                    if (locale.Identifier.Code == GetLanguageCode(language))
                    {
                        LocalizationSettings.Instance.SetSelectedLocale(locale);
                        break;
                    }
                }
            }
#endif

            // Save preference
            SaveLanguagePreference();

            // Notify listeners
            OnLanguageChanged?.Invoke(language);

            Debug.Log($"[LocalizationManager] Language changed to: {language}");
        }

        public Language GetCurrentLanguage()
        {
            return _currentLanguage;
        }

        public string GetCurrentLanguageCode()
        {
            return GetLanguageCode(_currentLanguage);
        }

        #endregion

        #region String Retrieval

        /// <summary>
        /// Get localized string by key
        /// </summary>
        public string GetLocalizedString(string key)
        {
#if LOCALIZATION_ENABLED
            // Use Unity Localization
            var stringTable = LocalizationSettings.Instance.GetStringTable();
            if (stringTable != null)
            {
                var entry = stringTable.GetEntry(key);
                if (entry != null)
                {
                    return entry.GetLocalizedString();
                }
            }
#endif

            // Fallback to dictionary
            return GetFallbackString(key);
        }

        /// <summary>
        /// Get localized string asynchronously
        /// </summary>
        public void GetLocalizedStringAsync(string key, System.Action<string> onComplete)
        {
#if LOCALIZATION_ENABLED
            var stringTable = LocalizationSettings.Instance.GetStringTable();
            if (stringTable != null)
            {
                var entry = stringTable.GetEntry(key);
                if (entry != null)
                {
                    entry.GetLocalizedStringAsync().Completed += (operation) =>
                    {
                        onComplete?.Invoke(operation.Result);
                    };
                    return;
                }
            }
#endif

            // Fallback
            onComplete?.Invoke(GetFallbackString(key));
        }

        #endregion

        #region Fallback Dictionary

        private Dictionary<string, Dictionary<Language, string>> _fallbackStrings = new Dictionary<string, Dictionary<Language, string>>()
        {
            // Main Menu
            ["MainMenu.Title"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Cristo Adventure" },
                { Language.English, "Christ Adventure" },
                { Language.Spanish, "Cristo Aventura" }
            },
            ["MainMenu.NewGame"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Novo Jogo" },
                { Language.English, "New Game" },
                { Language.Spanish, "Nueva Partida" }
            },
            ["MainMenu.Continue"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Continuar" },
                { Language.English, "Continue" },
                { Language.Spanish, "Continuar" }
            },
            ["MainMenu.Settings"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Configurações" },
                { Language.English, "Settings" },
                { Language.Spanish, "Configuración" }
            },
            ["MainMenu.Quit"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Sair" },
                { Language.English, "Quit" },
                { Language.Spanish, "Salir" }
            },

            // Gameplay
            ["Gameplay.Backpack"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Mochila" },
                { Language.English, "Backpack" },
                { Language.Spanish, "Mochila" }
            },
            ["Gameplay.Interact"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Pressione E para interagir" },
                { Language.English, "Press E to interact" },
                { Language.Spanish, "Presiona E para interactuar" }
            },

            // Settings
            ["Settings.Language"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Idioma" },
                { Language.English, "Language" },
                { Language.Spanish, "Idioma" }
            },
            ["Settings.Music"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Música" },
                { Language.English, "Music" },
                { Language.Spanish, "Música" }
            },
            ["Settings.SFX"] = new Dictionary<Language, string>
            {
                { Language.Portuguese, "Efeitos Sonoros" },
                { Language.English, "Sound Effects" },
                { Language.Spanish, "Efectos de Sonido" }
            }
        };

        private string GetFallbackString(string key)
        {
            if (_fallbackStrings.TryGetValue(key, out var langDict))
            {
                if (langDict.TryGetValue(_currentLanguage, out string value))
                {
                    return value;
                }
            }

            Debug.LogWarning($"[LocalizationManager] No translation found for key: {key}");
            return key;
        }

        #endregion

        #region Preferences

        private void LoadLanguagePreference()
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData != null)
            {
                _currentLanguage = ParseLanguage(playerData.GameSettings.Language);
            }
        }

        private void SaveLanguagePreference()
        {
            var playerData = GameManager.Instance?.GetPlayerData();
            if (playerData != null)
            {
                playerData.GameSettings.Language = _currentLanguage.ToString();
                GameManager.Instance?.SaveGame();
            }
        }

        #endregion

        #region Utility

        private string GetLanguageCode(Language language)
        {
            switch (language)
            {
                case Language.Portuguese: return "pt-BR";
                case Language.English: return "en-US";
                case Language.Spanish: return "es-MX";
                default: return "en-US";
            }
        }

        private Language ParseLanguage(string languageString)
        {
            if (System.Enum.TryParse<Language>(languageString, out Language result))
            {
                return result;
            }
            return Language.Portuguese;
        }

        #endregion

        #region Events

        public event System.Action<Language> OnLanguageChanged;

        #endregion
    }
}
