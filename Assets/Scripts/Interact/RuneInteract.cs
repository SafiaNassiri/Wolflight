using UnityEngine;

public class Rune : MonoBehaviour, IInteractable
{
    public static int runesCollected = 0;
    public static int totalRunesNeeded = 14;

    public GlowOrbPulse glowPulse;
    [TextArea(3, 10)]
    public string loreText = "Ancient power flows through this rune...";

    private bool collected = false;

    public void Interact()
    {
        if (collected) return;

        collected = true;
        runesCollected++;

        // Play rune collect sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayRuneSFX();

        // Show dialogue
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.ShowDialogue(loreText);

        // Hide glow
        if (glowPulse != null)
        {
            glowPulse.enabled = false;
            glowPulse.gameObject.SetActive(false);
        }

        Debug.Log($"Rune collected! Total: {runesCollected}/{totalRunesNeeded}");
        Destroy(gameObject, 0.5f);
    }
}