using UnityEngine;
using UnityEngine.SceneManagement;

public class Shrine : MonoBehaviour, IInteractable
{
    [Header("Ending Scenes")]
    public int goodEndingIndex = SceneIndex.GOOD_ENDING;
    public int badEndingIndex = SceneIndex.BAD_ENDING;

    public void Interact()
    {
        // Play shrine sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayShrineSFX();

        // Check if all runes collected
        bool allRunesCollected = Rune.runesCollected >= Rune.totalRunesNeeded;

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