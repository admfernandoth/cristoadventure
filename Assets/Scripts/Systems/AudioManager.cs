using UnityEngine;
using System.Collections.Generic;

namespace CristoAdventure.Systems
{
    /// <summary>
    /// Manages all audio playback - music, SFX, and voiceovers
    /// Supports 3D spatial audio for immersive exploration
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("_AudioManager");
                    _instance = go.AddComponent<AudioManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        #endregion

        #region Audio Sources

        private AudioSource _musicSource;
        private AudioSource _sfxSource;
        private AudioSource _voiceSource;
        private AudioSource _ambientSource;

        // Object pooling for SFX
        private List<AudioSource> _sfxPool = new List<AudioSource>();
        private const int SFX_POOL_SIZE = 20;

        #endregion

        #region Settings

        [Header("Volume Settings")]
        [Range(0f, 1f)] private float _musicVolume = 0.8f;
        [Range(0f, 1f)] private float _sfxVolume = 0.8f;
        [Range(0f, 1f)] private float _voiceVolume = 1f;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip _mainMenuMusic;
        [SerializeField] private AudioClip[] _explorationMusic;
        [SerializeField] private AudioClip[] _phaseCompleteStingers;

        // Current playing music
        private AudioClip _currentMusic;

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

            InitializeAudioSources();
            InitializeSFXPool();
        }

        private void Start()
        {
            PlayMusic(_mainMenuMusic);
        }

        #endregion

        #region Initialization

        private void InitializeAudioSources()
        {
            // Music source
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;

            // SFX source
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.playOnAwake = false;

            // Voiceover source
            _voiceSource = gameObject.AddComponent<AudioSource>();
            _voiceSource.playOnAwake = false;

            // Ambient source
            _ambientSource = gameObject.AddComponent<AudioSource>();
            _ambientSource.loop = true;
            _ambientSource.playOnAwake = false;

            UpdateVolumes();
        }

        private void InitializeSFXPool()
        {
            for (int i = 0; i < SFX_POOL_SIZE; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                _sfxPool.Add(source);
            }
        }

        #endregion

        #region Music Control

        public void PlayMusic(AudioClip clip, float fadeDuration = 1f)
        {
            if (clip == null) return;

            if (_currentMusic == clip && _musicSource.isPlaying)
                return;

            StartCoroutine(FadeMusic(clip, fadeDuration));
        }

        private System.Collections.IEnumerator FadeMusic(AudioClip newClip, float duration)
        {
            // Fade out current music
            float timer = 0f;
            float startVolume = _musicSource.volume;

            while (timer < duration / 2f)
            {
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / (duration / 2f));
                timer += Time.deltaTime;
                yield return null;
            }

            // Switch clip
            _musicSource.clip = newClip;
            _musicSource.Play();
            _currentMusic = newClip;

            // Fade in new music
            timer = 0f;
            while (timer < duration / 2f)
            {
                _musicSource.volume = Mathf.Lerp(0f, _musicVolume * 0.5f, timer / (duration / 2f));
                timer += Time.deltaTime;
                yield return null;
            }
        }

        public void PlayExplorationMusic()
        {
            if (_explorationMusic != null && _explorationMusic.Length > 0)
            {
                AudioClip clip = _explorationMusic[Random.Range(0, _explorationMusic.Length)];
                PlayMusic(clip);
            }
        }

        public void StopMusic(float fadeDuration = 1f)
        {
            StartCoroutine(FadeOutMusic(fadeDuration));
        }

        private System.Collections.IEnumerator FadeOutMusic(float duration)
        {
            float timer = 0f;
            float startVolume = _musicSource.volume;

            while (timer < duration)
            {
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            _musicSource.Stop();
            _currentMusic = null;
        }

        #endregion

        #region SFX Control

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;

            AudioSource source = GetAvailableSFXSource();
            if (source != null)
            {
                source.clip = clip;
                source.volume = _sfxVolume;
                source.Play();
            }
        }

        public void PlaySFX(AudioClip clip, Vector3 position)
        {
            if (clip == null) return;

            AudioSource source = GetAvailableSFXSource();
            if (source != null)
            {
                source.transform.position = position;
                source.clip = clip;
                source.volume = _sfxVolume;
                source.spatialBlend = 1f; // Full 3D
                source.Play();
            }
        }

        private AudioSource GetAvailableSFXSource()
        {
            // Find first available source
            foreach (var source in _sfxPool)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            // All sources busy, create temporary one
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.playOnAwake = false;
            _sfxPool.Add(tempSource);
            return tempSource;
        }

        #endregion

        #region Voiceover Control

        public void PlayVoiceover(AudioClip clip)
        {
            if (clip == null) return;

            _voiceSource.clip = clip;
            _voiceSource.volume = _voiceVolume;
            _voiceSource.Play();
        }

        public void StopVoiceover()
        {
            _voiceSource.Stop();
        }

        public bool IsPlayingVoiceover => _voiceSource.isPlaying;

        #endregion

        #region Ambient Control

        public void PlayAmbient(AudioClip clip)
        {
            if (clip == null) return;

            _ambientSource.clip = clip;
            _ambientSource.volume = _sfxVolume * 0.3f;
            _ambientSource.Play();
        }

        public void StopAmbient()
        {
            _ambientSource.Stop();
        }

        #endregion

        #region Volume Control

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            _musicSource.volume = _musicVolume * 0.5f;
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
        }

        public void SetVoiceVolume(float volume)
        {
            _voiceVolume = Mathf.Clamp01(volume);
            _voiceSource.volume = _voiceVolume;
        }

        public void UpdateVolumes()
        {
            var settings = GameManager.Instance?.GetPlayerData()?.GameSettings;
            if (settings != null)
            {
                SetMusicVolume(settings.MusicVolume);
                SetSFXVolume(settings.SfxVolume);
                SetVoiceVolume(settings.VoiceoverVolume);
            }
        }

        #endregion

        #region Phase Complete

        public void PlayPhaseCompleteStinger()
        {
            if (_phaseCompleteStingers != null && _phaseCompleteStingers.Length > 0)
            {
                AudioClip clip = _phaseCompleteStingers[Random.Range(0, _phaseCompleteStingers.Length)];
                PlaySFX(clip);
            }
        }

        #endregion
    }
}
