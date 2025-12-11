using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Handles the Game Over screen: displays rune count, shows messages,and provides options to retry, return to main menu, or quit.

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;   // Main "YOU DIED" message
    public TextMeshProUGUI runeCountText;  // Displays how many runes were collected
    public AudioClip buttonClickSound;     // Sound played when pressing buttons

    void Start()
    {
        // Update the rune count display
        if (runeCountText != null)
        {
            runeCountText.text = $"Runes Collected: {Rune.runesCollected}/{Rune.totalRunesNeeded}";
        }

        // Optionally customize the game over text
        if (gameOverText != null)
        {
            gameOverText.text = "YOU DIED";
        }
    }

    // Retry the current main level.
    // Resets the rune counter to 0.
    public void RetryLevel()
    {
        PlayButtonSound();

        // Reset runes so the player starts fresh
        Rune.runesCollected = 0;

        // Load the main gameplay scene
        SceneManager.LoadScene(SceneIndex.MAIN_LEVEL);
    }

    // Return to the main menu from game over.
    // Also resets rune counter.
    public void ReturnToMainMenu()
    {
        PlayButtonSound();

        Rune.runesCollected = 0;

        SceneManager.LoadScene(SceneIndex.MAIN_MENU);
    }

    // Quit the game completely.
    // Works in Editor and Build.
    public void QuitGame()
    {
        PlayButtonSound();

        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Plays the assigned button click sound via AudioManager.
    void PlayButtonSound()
    {
        if (AudioManager.Instance != null && buttonClickSound != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSound);
        }
    }
}
