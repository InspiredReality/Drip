using UnityEngine;
using System.Collections.Generic;

namespace Drip.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;

        [HideInInspector]
        public AudioSource source;
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Sound Effects")]
        [SerializeField] private Sound[] soundEffects;

        [Header("Music")]
        [SerializeField] private Sound[] musicTracks;

        [Header("Settings")]
        [SerializeField] private float masterVolume = 1f;
        [SerializeField] private float sfxVolume = 1f;
        [SerializeField] private float musicVolume = 0.5f;

        private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
        private AudioSource currentMusicSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudio();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeAudio()
        {
            // Initialize sound effects
            foreach (Sound sound in soundEffects)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume * sfxVolume * masterVolume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;

                soundDictionary[sound.name] = sound;
            }

            // Initialize music tracks
            foreach (Sound music in musicTracks)
            {
                music.source = gameObject.AddComponent<AudioSource>();
                music.source.clip = music.clip;
                music.source.volume = music.volume * musicVolume * masterVolume;
                music.source.pitch = music.pitch;
                music.source.loop = music.loop;

                soundDictionary[music.name] = music;
            }
        }

        public void PlaySound(string soundName)
        {
            if (soundDictionary.TryGetValue(soundName, out Sound sound))
            {
                sound.source.PlayOneShot(sound.clip);
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' not found!");
            }
        }

        public void PlayMusic(string musicName)
        {
            if (currentMusicSource != null && currentMusicSource.isPlaying)
            {
                currentMusicSource.Stop();
            }

            if (soundDictionary.TryGetValue(musicName, out Sound music))
            {
                currentMusicSource = music.source;
                currentMusicSource.Play();
            }
            else
            {
                Debug.LogWarning($"Music '{musicName}' not found!");
            }
        }

        public void StopMusic()
        {
            if (currentMusicSource != null)
            {
                currentMusicSource.Stop();
            }
        }

        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateAllVolumes();
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            UpdateAllVolumes();
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            UpdateAllVolumes();
        }

        private void UpdateAllVolumes()
        {
            foreach (Sound sound in soundEffects)
            {
                if (sound.source != null)
                    sound.source.volume = sound.volume * sfxVolume * masterVolume;
            }

            foreach (Sound music in musicTracks)
            {
                if (music.source != null)
                    music.source.volume = music.volume * musicVolume * masterVolume;
            }
        }
    }
}
