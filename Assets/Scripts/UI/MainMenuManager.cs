using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Audio Settings UI")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Button Sound")]
    public AudioClip buttonClickSound;
    public AudioClip playButtonSound; // Special sound for Play button

    private void Start()
    {
        ShowMainMenu();

        // Initialize sliders with saved volumes
        if (AudioManager.Instance != null)
        {
            if (musicSlider != null)
            {
                // Use current music volume
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

    // ===== BUTTON FUNCTIONS =====
    public void PlayGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene(SceneIndex.OPENING);
    }

    public void OpenSettings()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        PlayButtonSound();
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ===== AUDIO SETTINGS =====
    private void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
            PlayButtonSound();
        }
    }

    private void PlayButtonSound()
    {
        if (AudioManager.Instance != null && buttonClickSound != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSound);
        }
    }
}
