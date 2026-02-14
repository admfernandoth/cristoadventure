using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Groups")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;

    [Header "Music Tracks"]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;

    [Header "Sound Effects"]
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip hurtSound;

    private float masterVolume = 1f;
    private float musicVolume = 0.8f;
    private float sfxVolume = 0.8f;

    private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeSoundEffects();
        LoadVolumes();
    }

    private void InitializeSoundEffects()
    {
        soundEffects["ButtonHover"] = buttonHoverSound;
        soundEffects["ButtonClick"] = buttonClickSound;
        soundEffects["Interact"] = interactSound;
        soundEffects["Collect"] = collectSound;
        soundEffects["Hurt"] = hurtSound;
    }

    private void Start()
    {
        // Play main menu music if we're in the main menu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayMusic(mainMenuMusic);
        }
    }

    private void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = masterVolume * musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = masterVolume * sfxVolume;
        }

        if (ambientSource != null)
        {
            ambientSource.volume = masterVolume * musicVolume;
        }
    }

    #region Music Controls

    public void PlayMusic(AudioClip music)
    {
        if (musicSource == null || music == null) return;

        if (musicSource.clip != music)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void FadeMusic(float duration, float targetVolume, System.Action onComplete = null)
    {
        StartCoroutine(FadeMusicRoutine(duration, targetVolume, onComplete));
    }

    private IEnumerator FadeMusicRoutine(float duration, float targetVolume, System.Action onComplete)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentVolume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            musicSource.volume = currentVolume;
            yield return null;
        }

        musicSource.volume = targetVolume;
        onComplete?.Invoke();
    }

    #endregion

    #region Sound Effect Controls

    public void PlaySound(string soundName)
    {
        if (soundEffects.TryGetValue(soundName, out AudioClip clip) && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject soundGO = new GameObject("3DSound");
        soundGO.transform.position = position;

        AudioSource audioSource = soundGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = masterVolume * sfxVolume;
        audioSource.Play();

        Destroy(soundGO, clip.length);
    }

    public void PlayRandomSound(string soundNamePrefix, int min, int max)
    {
        int randomIndex = Random.Range(min, max + 1);
        string soundName = $"{soundNamePrefix}{randomIndex}";

        if (soundEffects.TryGetValue(soundName, out AudioClip clip) && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    #endregion

    #region Volume Controls

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    #endregion

    #region Ambient Audio

    public void PlayAmbient(AudioClip ambient)
    {
        if (ambientSource == null || ambient == null) return;

        if (ambientSource.clip != ambient)
        {
            ambientSource.clip = ambient;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    public void StopAmbient()
    {
        if (ambientSource != null)
        {
            ambientSource.Stop();
        }
    }

    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handle scene-specific music
        if (scene.name == "MainMenu")
        {
            PlayMusic(mainMenuMusic);
        }
        else if (scene.name != "MainMenu" && scene.name != "Settings")
        {
            PlayMusic(gameMusic);
        }
    }
}