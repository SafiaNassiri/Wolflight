using UnityEngine;
using UnityEngine.SceneManagement;

// Represents the shrine where the player can trigger the game ending.
// Implements IInteractable so the player can interact with it.
// Chooses good or bad ending based on collected runes.
public class Shrine : MonoBehaviour, IInteractable
{
    [Header("Ending Scenes")]
    public int goodEndingIndex = SceneIndex.GOOD_ENDING;  // Scene index for good ending
    public int badEndingIndex = SceneIndex.BAD_ENDING;    // Scene index for bad ending

    public void Interact()
    {
        // Play shrine interaction sound if AudioManager exists
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayShrineSFX();

        // Determine if player collected all runes
        bool allRunesCollected = Rune.runesCollected >= Rune.totalRunesNeeded;

        // Load ending scene accordingly
        if (allRunesCollected)
        {
            Debug.Log("Loading Good Ending...");
            SceneManager.LoadScene(goodEndingIndex);
        }
        else
        {
            Debug.Log($"Loading Bad Ending... (Only {Rune.runesCollected}/{Rune.totalRunesNeeded} runes collected)");
            SceneManager.LoadScene(badEndingIndex);
        }
    }
}
