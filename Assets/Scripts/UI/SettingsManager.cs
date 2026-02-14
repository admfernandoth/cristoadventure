using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Graphics Settings")]
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Slider resolutionSlider;

    [Header("Controls")]
    [SerializeField] private TMP_Text controlSchemeText;
    [SerializeField] private Button changeControlsButton;

    [Header("Save/Load")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button resetButton;

    [Header("Back Button")]
    [SerializeField] private Button backButton;

    private Dictionary<string, float> audioSettings = new Dictionary<string, float>();
    private Dictionary<string, int> graphicSettings = new Dictionary<string, int>();
    private List<Resolution> resolutions;

    private void Awake()
    {
        // Initialize settings if they don't exist
        InitializeSettings();

        // Setup UI elements
        SetupUI();
    }

    private void Start()
    {
        LoadCurrentSettings();
        PopulateResolutions();
    }

    private void InitializeSettings()
    {
        // Initialize audio settings
        audioSettings["Master"] = PlayerPrefs.GetFloat("MasterVolume", 1f);
        audioSettings["Music"] = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        audioSettings["SFX"] = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        // Initialize graphic settings
        graphicSettings["Quality"] = PlayerPrefs.GetInt("QualityLevel", QualitySettings.names.Length - 1);
        graphicSettings["Fullscreen"] = PlayerPrefs.GetInt("Fullscreen", 1);
        graphicSettings["VSync"] = PlayerPrefs.GetInt("VSync", 1);
        graphicSettings["Resolution"] = PlayerPrefs.GetInt("ResolutionIndex", 0);
    }

    private void SetupUI()
    {
        // Audio sliders
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        // Graphic settings
        if (qualityDropdown != null)
        {
            qualityDropdown.onValueChanged.AddListener(SetQualityLevel);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }

        if (vsyncToggle != null)
        {
            vsyncToggle.onValueChanged.AddListener(SetVSync);
        }

        if (resolutionSlider != null)
        {
            resolutionSlider.onValueChanged.AddListener(SetResolution);
        }

        // Controls
        if (changeControlsButton != null)
        {
            changeControlsButton.onClick.AddListener(ChangeControls);
        }

        // Save/Load buttons
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveSettings);
        }

        if (loadButton != null)
        {
            loadButton.onClick.AddListener(LoadSettings);
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetSettings);
        }

        // Back button
        if (backButton != null)
        {
            backButton.onClick.AddListener(() =>
            {
                FindObjectOfType<SceneTransitionManager>()?.LoadSceneAsync("MainMenu");
            });
        }
    }

    public void LoadCurrentSettings()
    {
        // Load audio settings
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = audioSettings["Master"];
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = audioSettings["Music"];
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = audioSettings["SFX"];
        }

        // Load graphic settings
        if (qualityDropdown != null)
        {
            qualityDropdown.value = graphicSettings["Quality"];
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = graphicSettings["Fullscreen"] == 1;
        }

        if (vsyncToggle != null)
        {
            vsyncToggle.isOn = graphicSettings["VSync"] == 1;
        }

        if (resolutionSlider != null)
        {
            resolutionSlider.value = graphicSettings["Resolution"];
        }

        // Apply settings
        ApplyAllSettings();
    }

    private void PopulateResolutions()
    {
        resolutions = Screen.resolutions;
        if (resolutionSlider != null)
        {
            resolutionSlider.maxValue = resolutions.Count - 1;
        }
    }

    private void SetMasterVolume(float value)
    {
        audioSettings["Master"] = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();

        // Apply to audio manager
        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.SetMasterVolume(value);
        }
    }

    private void SetMusicVolume(float value)
    {
        audioSettings["Music"] = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        // Apply to audio manager
        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(value);
        }
    }

    private void SetSFXVolume(float value)
    {
        audioSettings["SFX"] = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();

        // Apply to audio manager
        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.SetSFXVolume(value);
        }
    }

    private void SetQualityLevel(int value)
    {
        graphicSettings["Quality"] = value;
        QualitySettings.SetQualityLevel(value);
    }

    private void SetFullscreen(bool value)
    {
        graphicSettings["Fullscreen"] = value ? 1 : 0;
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetVSync(bool value)
    {
        graphicSettings["VSync"] = value ? 1 : 0;
        QualitySettings.vSyncCount = value ? 1 : 0;
        PlayerPrefs.SetInt("VSync", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetResolution(float value)
    {
        int index = Mathf.RoundToInt(value);
        if (index < resolutions.Count)
        {
            Resolution res = resolutions[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", index);
            PlayerPrefs.Save();
        }
    }

    private void ChangeControls()
    {
        // Show control customization menu
        // This would typically open a separate UI panel
        Debug.Log("Control customization not implemented yet");
    }

    private void SaveSettings()
    {
        // Save all settings to PlayerPrefs
        foreach (var setting in audioSettings)
        {
            PlayerPrefs.SetFloat(setting.Key + "Volume", setting.Value);
        }

        foreach (var setting in graphicSettings)
        {
            PlayerPrefs.SetInt(setting.Key, setting.Value);
        }

        PlayerPrefs.Save();

        // Show confirmation
        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowNotification("Configurações salvas!");
        }
    }

    private void LoadSettings()
    {
        // Reload settings from PlayerPrefs
        LoadCurrentSettings();

        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowNotification("Configurações carregadas!");
        }
    }

    private void ResetSettings()
    {
        // Reset to default settings
        audioSettings["Master"] = 1f;
        audioSettings["Music"] = 0.8f;
        audioSettings["SFX"] = 0.8f;

        graphicSettings["Quality"] = QualitySettings.names.Length - 1;
        graphicSettings["Fullscreen"] = 1;
        graphicSettings["VSync"] = 1;
        graphicSettings["Resolution"] = 0;

        LoadCurrentSettings();

        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowNotification("Configurações resetadas para o padrão!");
        }
    }

    private void ApplyAllSettings()
    {
        // Apply all current settings
        SetMasterVolume(audioSettings["Master"]);
        SetMusicVolume(audioSettings["Music"]);
        SetSFXVolume(audioSettings["SFX"]);

        QualitySettings.SetQualityLevel(graphicSettings["Quality"]);
        Screen.fullScreen = graphicSettings["Fullscreen"] == 1;
        QualitySettings.vSyncCount = graphicSettings["VSync"];
    }
}