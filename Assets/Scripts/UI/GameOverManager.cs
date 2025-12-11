using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI runeCountText;

    [Header("Button Sounds")]
    public AudioClip buttonClickSound;

    void Start()
    {
        // Display rune count
        if (runeCountText != null)
        {
            runeCountText.text = $"Runes Collected: {Rune.runesCollected}/{Rune.totalRunesNeeded}";
        }

        // Optional: Customize game over message
        if (gameOverText != null)
        {
            gameOverText.text = "YOU DIED";
        }
    }

    public void RetryLevel()
    {
        PlayButtonSound();

        // Reset rune counter for new attempt
        Rune.runesCollected = 0;

        // Load main level
        SceneManager.LoadScene("MainLevel");
    }

    public void ReturnToMainMenu()
    {
        PlayButtonSound();

        // Reset rune counter
        Rune.runesCollected = 0;

        // Load main menu
        SceneManager.LoadScene("MainMenu");
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

    void PlayButtonSound()
    {
        if (AudioManager.Instance != null && buttonClickSound != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSound);
        }
    }
}