using UnityEngine;
using UnityEngine.SceneManagement;

public class Shrine : MonoBehaviour, IInteractable
{
    [Header("Ending Scenes")]
    public string goodEndingScene = "GoodEnding";
    public string badEndingScene = "BadEnding";

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
            SceneManager.LoadScene(goodEndingScene);
        }
        else
        {
            Debug.Log($"Loading Bad Ending... (Only {Rune.runesCollected}/{Rune.totalRunesNeeded} runes collected)");
            SceneManager.LoadScene(badEndingScene);
        }
    }
}