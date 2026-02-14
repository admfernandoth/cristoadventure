using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class AudioPolish : MonoBehaviour
{
    [Header("Interaction SFX")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip collectibleChime;
    [SerializeField] private AudioClip successStinger;
    [SerializeField] private AudioClip failureStinger;
    [SerializeField] private AudioClip coinCollect;

    [Range(0f, 1f)]
    [SerializeField] private float clickVolume = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float hoverVolume = 0.3f;
    [Range(0f, 1f)]
    [SerializeField] private float collectibleVolume = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float stingerVolume = 0.8f;
    [Range(0f, 1f)]
    [SerializeField] private float coinVolume = 0.6f;

    [Header("Ambient Zones")]
    [SerializeField] private AudioClip basilicaAmbient;
    [SerializeField] private AudioClip caveAmbient;
    [SerializeField] private AudioClip courtyardAmbient;
    [SerializeField] private AudioClip windSound;

    [Range(0f, 1f)]
    [SerializeField] private float ambientVolume = 0.3f;
    [Range(0f, 1f)]
    [SerializeField] private float windVolume = 0.2f;

    [Header("Music Transitions")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource dialogueSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;
    [SerializeField] private AudioMixerGroup ambientMixer;

    [Range(0f, 2f)]
    [SerializeField] private float musicFadeDuration = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float dialogueVolume = 0.8f;
    [SerializeField] private float victoryDelay = 0.5f;

    // Audio players
    private AudioSource clickPlayer;
    private AudioSource hoverPlayer;
    private AudioSource collectiblePlayer;
    private AudioSource stingerPlayer;
    private AudioSource coinPlayer;
    private AudioSource ambientPlayer;
    private AudioSource windPlayer;

    // Audio state
    private bool isTransitioning = false;
    private Dictionary<string, float> zoneVolumes = new Dictionary<string, float>();

    // Events
    public UnityEvent onPlayClick;
    public UnityEvent onPlayHover;
    public UnityEvent onPlayCollectible;
    public UnityEvent onPlaySuccess;
    public UnityEvent onPlayFailure;
    public UnityEvent onPlayCoin;
    public UnityEvent onMusicTransitionStart;
    public UnityEvent onMusicTransitionEnd;

    private void Awake()
    {
        InitializeAudioPlayers();
        SetupAudioSources();
    }

    private void InitializeAudioPlayers()
    {
        // Create dedicated audio players for each sound type
        clickPlayer = CreateAudioPlayer("ClickPlayer", sfxMixer);
        hoverPlayer = CreateAudioPlayer("HoverPlayer", sfxMixer);
        collectiblePlayer = CreateAudioPlayer("CollectiblePlayer", sfxMixer);
        stingerPlayer = CreateAudioPlayer("StingerPlayer", sfxMixer);
        coinPlayer = CreateAudioPlayer("CoinPlayer", sfxMixer);
        ambientPlayer = CreateAudioPlayer("AmbientPlayer", ambientMixer);
        windPlayer = CreateAudioPlayer("WindPlayer", ambientMixer);
    }

    private AudioSource CreateAudioPlayer(string name, AudioMixerGroup mixer)
    {
        GameObject audioObj = new GameObject(name);
        audioObj.transform.SetParent(transform);
        audioObj.hideFlags = HideFlags.HideAndDontSave;

        AudioSource source = audioObj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.outputAudioMixerGroup = mixer;

        return source;
    }

    private void SetupAudioSources()
    {
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
            musicSource.outputAudioMixerGroup = musicMixer;
        }

        if (dialogueSource == null)
        {
            GameObject dialogueObj = new GameObject("DialogueSource");
            dialogueObj.transform.SetParent(transform);
            dialogueSource = dialogueObj.AddComponent<AudioSource>();
            dialogueSource.playOnAwake = false;
            dialogueSource.loop = false;
            dialogueSource.outputAudioMixerGroup = sfxMixer;
        }

        if (ambientSource == null)
        {
            GameObject ambientObj = new GameObject("AmbientSource");
            ambientObj.transform.SetParent(transform);
            ambientSource = ambientObj.AddComponent<AudioSource>();
            ambientSource.playOnAwake = false;
            ambientSource.loop = true;
            ambientSource.outputAudioMixerGroup = ambientMixer;
        }
    }

    #region Interaction SFX

    public void PlayClickSound()
    {
        if (clickSound != null && clickPlayer != null)
        {
            clickPlayer.volume = clickVolume;
            clickPlayer.PlayOneShot(clickSound);
            onPlayClick?.Invoke();
        }
    }

    public void PlayHoverSound()
    {
        if (hoverSound != null && hoverPlayer != null)
        {
            hoverPlayer.volume = hoverVolume;
            hoverPlayer.PlayOneShot(hoverSound);
            onPlayHover?.Invoke();
        }
    }

    public void PlayCollectibleSound(Vector3 position)
    {
        if (collectibleChime != null && collectiblePlayer != null)
        {
            collectiblePlayer.transform.position = position;
            collectiblePlayer.volume = collectibleVolume;
            collectiblePlayer.PlayOneShot(collectibleChime);
            onPlayCollectible?.Invoke();
        }
    }

    public void PlaySuccessSound()
    {
        if (successStinger != null && stingerPlayer != null)
        {
            stingerPlayer.volume = stingerVolume;
            stingerPlayer.PlayOneShot(successStinger);
            onPlaySuccess?.Invoke();
        }
    }

    public void PlayFailureSound()
    {
        if (failureStinger != null && stingerPlayer != null)
        {
            stingerPlayer.volume = stingerVolume;
            stingerPlayer.PlayOneShot(failureStinger);
            onPlayFailure?.Invoke();
        }
    }

    public void PlayCoinSound(Vector3 position)
    {
        if (coinCollect != null && coinPlayer != null)
        {
            coinPlayer.transform.position = position;
            coinPlayer.volume = coinVolume;
            coinPlayer.PlayOneShot(coinCollect);
            onPlayCoin?.Invoke();
        }
    }

    #endregion

    #region Ambient Zones

    public void SetAmbientZone(string zoneName)
    {
        StartCoroutine(TransitionAmbientZone(zoneName));
    }

    private IEnumerator TransitionAmbientZone(string zoneName)
    {
        AudioClip targetClip = null;
        float targetVolume = ambientVolume;

        switch (zoneName.ToLower())
        {
            case "basilica":
                targetClip = basilicaAmbient;
                targetVolume = ambientVolume;
                break;
            case "cave":
                targetClip = caveAmbient;
                targetVolume = ambientVolume * 0.5f; // Quieter in caves
                break;
            case "courtyard":
                targetClip = courtyardAmbient;
                targetVolume = ambientVolume;
                break;
        }

        // Fade out current ambient
        float currentVolume = ambientSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < musicFadeDuration)
        {
            ambientSource.volume = Mathf.Lerp(currentVolume, 0f, elapsedTime / musicFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Change clip and fade in
        if (targetClip != null)
        {
            ambientSource.clip = targetClip;
            ambientSource.Play();

            elapsedTime = 0f;
            while (elapsedTime < musicFadeDuration)
            {
                ambientSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / musicFadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            ambientSource.Stop();
        }

        // Update wind sound for cave zone
        if (zoneName.ToLower() == "cave" && windSound != null)
        {
            windPlayer.clip = windSound;
            windPlayer.volume = windVolume;
            windPlayer.loop = true;
            windPlayer.Play();
        }
        else
        {
            windPlayer.Stop();
        }
    }

    #endregion

    #region Music Transitions

    public void PlayMusic(AudioClip musicClip)
    {
        StartCoroutine(FadeMusic(musicClip));
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        if (isTransitioning) yield break;

        isTransitioning = true;
        onMusicTransitionStart?.Invoke();

        // Fade out current music
        float startVolume = musicSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < musicFadeDuration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / musicFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Change clip and fade in
        if (newClip != null)
        {
            musicSource.clip = newClip;
            musicSource.Play();

            elapsedTime = 0f;
            while (elapsedTime < musicFadeDuration)
            {
                musicSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / musicFadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        isTransitioning = false;
        onMusicTransitionEnd?.Invoke();
    }

    public void PlayDialogue(AudioClip dialogueClip)
    {
        if (dialogueClip != null && dialogueSource != null)
        {
            dialogueSource.volume = dialogueVolume;
            dialogueSource.PlayOneShot(dialogueClip);
        }
    }

    public void PlayVictoryMusic(AudioClip victoryMusic)
    {
        if (victoryMusic != null)
        {
            StartCoroutine(VictorySequence(victoryMusic));
        }
    }

    private IEnumerator VictorySequence(AudioClip victoryMusic)
    {
        // Wait a moment before victory fanfare
        yield return new WaitForSeconds(victoryDelay);

        // Play victory music with a dramatic fade
        musicSource.clip = victoryMusic;
        musicSource.volume = 0f;
        musicSource.Play();

        float elapsedTime = 0f;
        float fadeDuration = 2f;

        while (elapsedTime < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Keep playing victory music
        musicSource.loop = true;
    }

    #endregion

    #region Utility Methods

    public void StopAllSound()
    {
        musicSource.Stop();
        dialogueSource.Stop();
        ambientSource.Stop();
        windPlayer.Stop();
    }

    public void SetMixerVolumes(float musicVolume, float sfxVolume, float ambientVolume)
    {
        if (musicMixer != null)
        {
            musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        }
        if (sfxMixer != null)
        {
            sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        }
        if (ambientMixer != null)
        {
            ambientMixer.audioMixer.SetFloat("AmbientVolume", Mathf.Log10(ambientVolume) * 20);
        }
    }

    public bool IsPlayingMusic()
    {
        return musicSource.isPlaying;
    }

    #endregion
}