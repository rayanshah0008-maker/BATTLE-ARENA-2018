using UnityEngine;

/// <summary>
/// Audio Manager - Handles all sound effects and music
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Background Music")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip victoryMusic;

    [Header("Sound Effects")]
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip victorySound;

    [Header("Volume Settings")]
    public float masterVolume = 1f;
    public float musicVolume = 0.7f;
    public float sfxVolume = 0.8f;

    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AudioManager>();
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
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource != null && gameplayMusic != null)
        {
            musicSource.clip = gameplayMusic;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayMenuMusic()
    {
        if (musicSource != null && menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayVictorySound()
    {
        if (musicSource != null && victoryMusic != null)
        {
            musicSource.clip = victoryMusic;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }
}
