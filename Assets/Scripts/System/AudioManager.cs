using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource ambienceSource;
    public AudioSource sfxSource;
    public AudioSource elfSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip mainLevelMusic;
    public AudioClip gameOverMusic;
    public AudioClip goodEndingMusic;
    public AudioClip badEndingMusic;
    public AudioClip elfClip;
    public AudioClip openingCutsceneMusic;

    [Header("Player SFX")]
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip wallSlideSound;
    public AudioClip deathSound;
    public AudioClip runningLoopSound;

    [Header("Environment SFX")]
    public AudioClip runeSound;
    public AudioClip shrineSound;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float elfVolume = 0.3f;
    // ----------  NEW  ----------
    [Range(0f, 1f)] public float cutsceneMusicVolume = 0.5f; // inspector tweak
    // ----------  END NEW  ----------

    [Header("Individual SFX Volumes")]
    [Range(0f, 1f)] public float jumpVolume = 0.9f;
    [Range(0f, 1f)] public float landVolume = 0.7f;
    [Range(0f, 1f)] public float wallSlideVolume = 0.5f;
    [Range(0f, 1f)] public float deathVolume = 1f;
    [Range(0f, 1f)] public float footstepVolume = 0.3f;
    [Range(0f, 1f)] public float runeVolume = 0.8f;
    [Range(0f, 1f)] public float shrineVolume = 0.8f;

    /* ---------------------------------------------------------- */
    /*  LIFECYCLE                                                 */
    /* ---------------------------------------------------------- */

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            LoadVolumes();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else { Destroy(gameObject); }
    }

    void InitializeAudioSources()
    {
        if (musicSource == null)
        {
            var g = new GameObject("MusicSource");
            g.transform.parent = transform;
            musicSource = g.AddComponent<AudioSource>();
        }
        if (ambienceSource == null)
        {
            var g = new GameObject("AmbienceSource");
            g.transform.parent = transform;
            ambienceSource = g.AddComponent<AudioSource>();
        }
        if (sfxSource == null)
        {
            var g = new GameObject("SFXSource");
            g.transform.parent = transform;
            sfxSource = g.AddComponent<AudioSource>();
        }
        if (elfSource == null)
        {
            var g = new GameObject("ElfSource");
            g.transform.parent = transform;
            elfSource = g.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        ambienceSource.loop = true;
        ambienceSource.playOnAwake = false;
        sfxSource.playOnAwake = false;
        elfSource.loop = true;
        elfSource.playOnAwake = false;

        // ----------  NEW  ----------
        ApplyVolumes(); // first-frame volume
        // ----------  END NEW  ----------
    }

    /* ---------------------------------------------------------- */
    /*  SCENE-CHANGE HANDLER   (volume lines removed)             */
    /* ---------------------------------------------------------- */

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        /*  MAIN MENU  */
        if (scene.name == "MainMenu")
        {
            if (menuMusic != null)
            {
                musicSource.Stop();
                musicSource.clip = menuMusic;
                // musicSource.volume = masterVolume * musicVolume;  // REMOVED
                musicSource.Play();
                Debug.Log("Playing menu music");
            }
            if (elfClip != null)
            {
                elfSource.Stop();
                elfSource.clip = elfClip;
                // elfSource.volume = masterVolume * musicVolume * elfVolume;  // REMOVED
                elfSource.Play();
                Debug.Log("Playing elf singing");
            }
        }

        /*  MAIN LEVEL  */
        else if (scene.name == "MainLevel")
        {
            musicSource.Stop();
            elfSource.Stop();
            if (mainLevelMusic != null)
            {
                musicSource.clip = mainLevelMusic;
                // musicSource.volume = masterVolume * musicVolume;  // REMOVED
                musicSource.Play();
                Debug.Log("Playing main level music");
            }
        }

        /*  GAME OVER  */
        else if (scene.name == "GameOver")
        {
            musicSource.Stop();
            elfSource.Stop();
            if (gameOverMusic != null)
            {
                musicSource.clip = gameOverMusic;
                // musicSource.volume = masterVolume * musicVolume;  // REMOVED
                musicSource.Play();
                Debug.Log("Playing game over music");
            }
        }

        /*  GOOD ENDING  */
        else if (scene.name == "GoodEnding")
        {
            musicSource.Stop();
            elfSource.Stop();
            if (goodEndingMusic != null)
            {
                musicSource.clip = goodEndingMusic;
                // musicSource.volume = masterVolume * musicVolume;  // REMOVED
                musicSource.Play();
            }
        }

        /*  BAD ENDING  */
        else if (scene.name == "BadEnding")
        {
            musicSource.Stop();
            elfSource.Stop();
            if (badEndingMusic != null)
            {
                musicSource.clip = badEndingMusic;
                // musicSource.volume = masterVolume * musicVolume;  // REMOVED
                musicSource.Play();
            }
        }

        /*  OPENING CUT-SCENE  */
        else if (scene.name == "OpeningCutscene")
        {
            musicSource.Stop();
            elfSource.Stop();
            if (openingCutsceneMusic != null)
            {
                musicSource.clip = openingCutsceneMusic;
                // musicSource.volume = masterVolume * cutsceneMusicVolume;  // REMOVED
                musicSource.Play();
                Debug.Log("Playing opening-cutscene music");
            }
        }

        // ----------  NEW  ----------
        ApplyVolumes(); // set correct volume after clip change
        // ----------  END NEW  ----------
    }

    /* ---------------------------------------------------------- */
    /*  PUBLIC VOLUME API  (single music slider drives everything) */
    /* ---------------------------------------------------------- */

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        ApplyVolumes();
        SaveVolume("MasterVolume", masterVolume);
    }

    public void SetMusicVolume(float value) /* <-- your UI slider calls this */
    {
        musicVolume = Mathf.Clamp01(value);
        ApplyVolumes();
        SaveVolume("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        SaveVolume("SFXVolume", sfxVolume);
    }

    // ----------  NEW  ----------
    /* optional: let designers change the RELATIVE trim at runtime */
    public void SetCutsceneMusicVolume(float value)
    {
        cutsceneMusicVolume = Mathf.Clamp01(value);
        ApplyVolumes();
        SaveVolume("CutsceneMusicVolume", cutsceneMusicVolume);
    }
    // ----------  END NEW  ----------

    /* ---------------------------------------------------------- */
    /*  APPLY VOLUMES  (single source of truth)                   */
    /* ---------------------------------------------------------- */

    void ApplyVolumes()
    {
        if (!musicSource) return;

        /* decide which multiplier to use */
        bool isCutscene =
            musicSource.clip == openingCutsceneMusic ||
            musicSource.clip == goodEndingMusic ||
            musicSource.clip == badEndingMusic;

        float musicVol = isCutscene ? musicVolume * cutsceneMusicVolume
                                    : musicVolume;

        musicSource.volume = masterVolume * musicVol;

        if (elfSource)
            elfSource.volume = masterVolume * musicVolume * elfVolume;
    }

    /* ---------------------------------------------------------- */
    /*  MUSIC / SFX  PLAY-HELPERS                                 */
    /* ---------------------------------------------------------- */

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.Stop();
        musicSource.clip = clip;
        // musicSource.volume = masterVolume * musicVolume;  // REMOVED
        musicSource.Play();
        ApplyVolumes(); // <-- make sure volume is correct
    }

    public void StopMusic()
    {
        if (musicSource) musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, masterVolume * sfxVolume * volumeMultiplier);
    }

    // ----------  shortcuts  ----------
    public void PlayJumpSound() => PlaySFX(jumpSound, jumpVolume);
    public void PlayLandSound() => PlaySFX(landSound, landVolume);
    public void PlayDeathSound() => PlaySFX(deathSound, deathVolume);
    public void PlayRuneSFX() => PlaySFX(runeSound, runeVolume);
    public void PlayShrineSFX() => PlaySFX(shrineSound, shrineVolume);

    /* ---------------------------------------------------------- */
    /*  SAVE / LOAD                                               */
    /* ---------------------------------------------------------- */

    void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        // ----------  NEW  ----------
        cutsceneMusicVolume = PlayerPrefs.GetFloat("CutsceneMusicVolume", 0.5f);
        // ----------  END NEW  ----------
        ApplyVolumes();
    }

    /* ---------------------------------------------------------- */
    /*  GETTERS  (kept for UI or other scripts)                   */
    /* ---------------------------------------------------------- */

    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
    public float GetFootstepVolume() => footstepVolume;
    public float GetWallSlideVolume() => wallSlideVolume;
    public float GetCutsceneMusicVolume() => cutsceneMusicVolume;

    /* ---------------------------------------------------------- */
    /*  CLEAN-UP                                                  */
    /* ---------------------------------------------------------- */

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}