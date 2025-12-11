using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;     // Main menu UI panel
    public GameObject settingsPanel;     // Settings UI panel
    public GameObject creditsPanel;      // Credits UI panel

    public Slider musicSlider;           // Slider controlling music volume
    public Slider sfxSlider;             // Slider controlling sound effect volume

    public AudioClip buttonClickSound;   // Generic button click sound
    public AudioClip playButtonSound;    // (Unused) unique sound for Play button if desired

    private void Start()
    {
        // Ensure the main menu shows first.
        ShowMainMenu();

        // Initialize audio sliders using current values from AudioManager.
        if (AudioManager.Instance != null)
        {
            if (musicSlider != null)
            {
                musicSlider.value = AudioManager.Instance.GetMusicVolume();
                musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }

            if (sfxSlider != null)
            {
                sfxSlider.value = AudioManager.Instance.GetSFXVolume();
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
        }
    }

    public void PlayGame()
    {
        // Play special button sound
        PlayButtonSound();

        // Load the Opening Cutscene scene
        SceneManager.LoadScene(SceneIndex.OPENING);
    }

    public void OpenSettings()
    {
        PlayButtonSound();

        // Switch from main menu to settings screen
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        PlayButtonSound();

        // Switch to credits screen
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        PlayButtonSound();

        // Make the main menu the active panel
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        PlayButtonSound();

        // Quit the application. Works differently in Editor vs Build.
        Debug.Log("Quitting game...");
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void OnMusicVolumeChanged(float value)
    {
        // Forward slider adjustment to AudioManager
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        // Update SFX volume and give audio feedback
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
            PlayButtonSound(); // small "bing" when moving slider
        }
    }

    private void PlayButtonSound()
    {
        // Play a click noise through the AudioManager
        if (AudioManager.Instance != null && buttonClickSound != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSound);
        }
    }
}
