using UnityEngine;
using UnityEngine.SceneManagement;

// Central audio manager handling music, ambience, SFX, and special sources like elf singing.
// Provides volume control, persistence via PlayerPrefs, and scene-specific music handling.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource musicSource;      // Main background music
    public AudioSource ambienceSource;   // Environmental loops
    public AudioSource sfxSource;        // Player and event sound effects
    public AudioSource elfSource;        // Special source for elf singing
    public AudioClip menuMusic;
    public AudioClip mainLevelMusic;
    public AudioClip gameOverMusic;
    public AudioClip goodEndingMusic;
    public AudioClip badEndingMusic;
    public AudioClip elfClip;
    public AudioClip openingCutsceneMusic;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip wallSlideSound;
    public AudioClip deathSound;
    public AudioClip runningLoopSound;
    public AudioClip runeSound;
    public AudioClip shrineSound;
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float elfVolume = 0.3f;
    [Range(0f, 1f)] public float cutsceneMusicVolume = 0.5f;
    [Range(0f, 1f)] public float jumpVolume = 0.9f;
    [Range(0f, 1f)] public float landVolume = 0.7f;
    [Range(0f, 1f)] public float wallSlideVolume = 0.5f;
    [Range(0f, 1f)] public float deathVolume = 1f;
    [Range(0f, 1f)] public float footstepVolume = 0.3f;
    [Range(0f, 1f)] public float runeVolume = 0.8f;
    [Range(0f, 1f)] public float shrineVolume = 0.8f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            InitializeAudioSources();
            LoadVolumes();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Creates AudioSource components if not assigned and sets defaults.
    void InitializeAudioSources()
    {
        if (musicSource == null) musicSource = CreateSource("MusicSource", loop: true);
        if (ambienceSource == null) ambienceSource = CreateSource("AmbienceSource", loop: true);
        if (sfxSource == null) sfxSource = CreateSource("SFXSource", loop: false);
        if (elfSource == null) elfSource = CreateSource("ElfSource", loop: true);

        ApplyVolumes(); // Apply saved volume settings on start
    }

    AudioSource CreateSource(string name, bool loop)
    {
        GameObject g = new GameObject(name);
        g.transform.parent = transform;
        AudioSource src = g.AddComponent<AudioSource>();
        src.loop = loop;
        src.playOnAwake = false;
        return src;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        // Stop elf source by default
        elfSource.Stop();

        // Play music depending on scene
        if (scene.name == "MainMenu")
        {
            PlayMusic(menuMusic);
            elfSource.clip = elfClip;
            elfSource.Play();
        }
        else if (scene.name == "MainLevel")
            PlayMusic(mainLevelMusic);
        else if (scene.name == "GameOver")
            PlayMusic(gameOverMusic);
        else if (scene.name == "GoodEnding")
            PlayMusic(goodEndingMusic);
        else if (scene.name == "BadEnding")
            PlayMusic(badEndingMusic);
        else if (scene.name == "OpeningCutscene")
            PlayMusic(openingCutsceneMusic);

        ApplyVolumes(); // Ensure volume is correct after scene load
    }

    public void SetMasterVolume(float value) { masterVolume = Mathf.Clamp01(value); ApplyVolumes(); SaveVolume("MasterVolume", masterVolume); }
    public void SetMusicVolume(float value) { musicVolume = Mathf.Clamp01(value); ApplyVolumes(); SaveVolume("MusicVolume", musicVolume); }
    public void SetSFXVolume(float value) { sfxVolume = Mathf.Clamp01(value); SaveVolume("SFXVolume", sfxVolume); }
    public void SetCutsceneMusicVolume(float value) { cutsceneMusicVolume = Mathf.Clamp01(value); ApplyVolumes(); SaveVolume("CutsceneMusicVolume", cutsceneMusicVolume); }

    /// Applies all volume multipliers to AudioSources based on saved values.
    /// Handles cutscene music differently if desired.
    void ApplyVolumes()
    {
        if (!musicSource) return;

        bool isCutscene = musicSource.clip == openingCutsceneMusic || musicSource.clip == goodEndingMusic || musicSource.clip == badEndingMusic;
        float musicVol = isCutscene ? musicVolume * cutsceneMusicVolume : musicVolume;
        musicSource.volume = masterVolume * musicVol;

        if (elfSource) elfSource.volume = masterVolume * musicVolume * elfVolume;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
        ApplyVolumes();
    }

    public void StopMusic() { if (musicSource) musicSource.Stop(); }

    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, masterVolume * sfxVolume * volumeMultiplier);
    }

    // Shortcuts for common player/environment sounds
    public void PlayJumpSound() => PlaySFX(jumpSound, jumpVolume);
    public void PlayLandSound() => PlaySFX(landSound, landVolume);
    public void PlayDeathSound() => PlaySFX(deathSound, deathVolume);
    public void PlayRuneSFX() => PlaySFX(runeSound, runeVolume);
    public void PlayShrineSFX() => PlaySFX(shrineSound, shrineVolume);

    void SaveVolume(string key, float value) { PlayerPrefs.SetFloat(key, value); PlayerPrefs.Save(); }
    void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        cutsceneMusicVolume = PlayerPrefs.GetFloat("CutsceneMusicVolume", 0.5f);
        ApplyVolumes();
    }
    
    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
    public float GetFootstepVolume() => footstepVolume;
    public float GetWallSlideVolume() => wallSlideVolume;
    public float GetCutsceneMusicVolume() => cutsceneMusicVolume;

    void OnDestroy() { SceneManager.sceneLoaded -= OnSceneLoaded; }
}
