using UnityEngine;

[System.Serializable]
public class SoundLibrary : ScriptableObject
{
    [Header("Interaction SFX")]
    public AudioClip[] clickSounds;
    public AudioClip[] hoverSounds;
    public AudioClip[] collectibleChimes;
    public AudioClip[] successStingers;
    public AudioClip[] failureStingers;
    public AudioClip[] coinCollects;

    [Header("Ambient Zones")]
    public AudioClip basilicaAmbient;
    public AudioClip caveAmbient;
    public AudioClip courtyardAmbient;
    public AudioClip windSound;

    [Header("Music Themes")]
    public AudioClip explorationMusic;
    public AudioClip puzzleMusic;
    public AudioClip victoryMusic;
    public AudioClip[] dialogueMusic;

    [Header("Ambience Layers")]
    public AudioClip[] backgroundLayers;
    public AudioClip[] foregroundLayers;

    // Get random sound from array
    public AudioClip GetRandomSound(AudioClip[] sounds)
    {
        if (sounds == null || sounds.Length == 0)
            return null;

        return sounds[Random.Range(0, sounds.Length)];
    }

    // Get specific sound by index
    public AudioClip GetSound(AudioClip[] sounds, int index)
    {
        if (sounds == null || index < 0 || index >= sounds.Length)
            return null;

        return sounds[index];
    }

    // Check if any sounds are loaded
    public bool HasInteractionSounds()
    {
        return (clickSounds != null && clickSounds.Length > 0) ||
               (hoverSounds != null && hoverSounds.Length > 0) ||
               (collectibleChimes != null && collectibleChimes.Length > 0);
    }

    public bool HasStingers()
    {
        return (successStingers != null && successStingers.Length > 0) ||
               (failureStingers != null && failureStingers.Length > 0);
    }

    public bool HasMusic()
    {
        return explorationMusic != null || puzzleMusic != null || victoryMusic != null;
    }
}

// Helper class for runtime sound management
public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource dialogueSource;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;
    [SerializeField] private AudioMixerGroup ambientMixer;

    [Header("Sound Library")]
    [SerializeField] private SoundLibrary soundLibrary;

    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SoundManager");
                    instance = go.AddComponent<SoundManager>();
                    DontDestroyOnLoad(go);
                }
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

        SetupAudioSources();
    }

    private void SetupAudioSources()
    {
        // Assign mixer groups
        if (musicSource != null && musicMixer != null)
            musicSource.outputAudioMixerGroup = musicMixer;

        if (sfxSource != null && sfxMixer != null)
            sfxSource.outputAudioMixerGroup = sfxMixer;

        if (ambientSource != null && ambientMixer != null)
            ambientSource.outputAudioMixerGroup = ambientMixer;

        if (dialogueSource != null && sfxMixer != null)
            dialogueSource.outputAudioMixerGroup = sfxMixer;
    }

    // Music playback
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicClip == null || musicSource == null) return;

        if (musicSource.clip == musicClip && musicSource.isPlaying)
            return;

        StartCoroutine(FadeMusic(musicClip, loop));
    }

    private System.Collections.IEnumerator FadeMusic(AudioClip newClip, bool loop)
    {
        float fadeTime = 1f;
        float startVolume = musicSource.volume;

        // Fade out
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Change clip and fade in
        musicSource.clip = newClip;
        musicSource.loop = loop;
        musicSource.Play();

        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            musicSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // SFX playback
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayRandomInteractionSound(SoundLibrary.SoundType type, float volume = 1f)
    {
        if (soundLibrary == null) return;

        AudioClip clip = null;
        switch (type)
        {
            case SoundLibrary.SoundType.Click:
                clip = soundLibrary.GetRandomSound(soundLibrary.clickSounds);
                break;
            case SoundLibrary.SoundType.Hover:
                clip = soundLibrary.GetRandomSound(soundLibrary.hoverSounds);
                break;
            case SoundLibrary.SoundType.Collectible:
                clip = soundLibrary.GetRandomSound(soundLibrary.collectibleChimes);
                break;
            case SoundLibrary.SoundType.Success:
                clip = soundLibrary.GetRandomSound(soundLibrary.successStingers);
                break;
            case SoundLibrary.SoundType.Failure:
                clip = soundLibrary.GetRandomSound(soundLibrary.failureStingers);
                break;
            case SoundLibrary.SoundType.Coin:
                clip = soundLibrary.GetRandomSound(soundLibrary.coinCollects);
                break;
        }

        if (clip != null)
        {
            PlaySFX(clip, volume);
        }
    }

    // Ambient playback
    public void PlayAmbient(AudioClip ambientClip, bool loop = true)
    {
        if (ambientClip == null || ambientSource == null) return;

        if (ambientSource.clip == ambientClip && ambientSource.isPlaying)
            return;

        ambientSource.Stop();
        ambientSource.clip = ambientClip;
        ambientSource.loop = loop;
        ambientSource.Play();
    }

    // Dialogue playback
    public void PlayDialogue(AudioClip dialogueClip, float volume = 1f)
    {
        if (dialogueClip == null || dialogueSource == null) return;

        dialogueSource.volume = volume;
        dialogueSource.PlayOneShot(dialogueClip);
    }

    // Stop functions
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void StopAmbient()
    {
        if (ambientSource != null)
            ambientSource.Stop();
    }

    public void StopAllAudio()
    {
        StopMusic();
        StopAmbient();
        if (sfxSource != null)
            sfxSource.Stop();
        if (dialogueSource != null)
            dialogueSource.Stop();
    }

    // Volume control
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
            sfxSource.volume = Mathf.Clamp01(volume);
    }

    public void SetAmbientVolume(float volume)
    {
        if (ambientSource != null)
            ambientSource.volume = Mathf.Clamp01(volume);
    }

    public void SetDialogueVolume(float volume)
    {
        if (dialogueSource != null)
            dialogueSource.volume = Mathf.Clamp01(volume);
    }

    // Getters
    public bool IsMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }

    public bool IsDialoguePlaying()
    {
        return dialogueSource != null && dialogueSource.isPlaying;
    }
}

// Extension for SoundLibrary
public static class SoundLibraryExtensions
{
    public static SoundLibrary.SoundType ToSoundType(this string type)
    {
        switch (type.ToLower())
        {
            case "click":
                return SoundLibrary.SoundType.Click;
            case "hover":
                return SoundLibrary.SoundType.Hover;
            case "collectible":
                return SoundLibrary.SoundType.Collectible;
            case "success":
                return SoundLibrary.SoundType.Success;
            case "failure":
                return SoundLibrary.SoundType.Failure;
            case "coin":
                return SoundLibrary.SoundType.Coin;
            default:
                return SoundLibrary.SoundType.Click;
        }
    }
}

// Enum for sound types
public static class SoundLibrary
{
    public enum SoundType
    {
        Click,
        Hover,
        Collectible,
        Success,
        Failure,
        Coin
    }
}