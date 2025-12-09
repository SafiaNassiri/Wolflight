using UnityEngine;

public class Shrine : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        bool allRunesCollected = Rune.runesCollected >= Rune.totalRunesNeeded;

        if (allRunesCollected)
        {
            Debug.Log("GOOD ENDING CUTSCENE");
            // trigger your cutscene here
        }
        else
        {
            Debug.Log("BAD ENDING - Not all runes collected");
            // trigger bad ending here
        }
    }
}
