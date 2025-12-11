using UnityEngine;

// Represents a collectible rune in the game.
// Implements IInteractable so the player can interact with it.
// Keeps track of the total collected runes and triggers effects like glow, dialogue, and sounds.
public class Rune : MonoBehaviour, IInteractable
{
    public static int runesCollected = 0;          // Number of runes collected globally
    public static int totalRunesNeeded = 14;       // Total runes in the game
    public GlowOrbPulse glowPulse;                 // Optional glow effect to disable on collection
    [TextArea(3, 10)]
    public string loreText = "Ancient power flows through this rune..."; // Text shown in dialogue on collection
    private bool collected = false;               // Has this rune been collected already?

    public void Interact()
    {
        if (collected) return;  // Prevent double collection

        collected = true;
        runesCollected++;       // Increment global rune counter

        // Play rune collection sound if AudioManager exists
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayRuneSFX();

        // Show lore text in dialogue with bold formatting
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.ShowDialogue($"<b>{loreText}</b>");

        // Disable glow effect
        if (glowPulse != null)
        {
            glowPulse.enabled = false;
            glowPulse.gameObject.SetActive(false);
        }

        // Debug log for developers
        Debug.Log($"Rune collected! Total: {runesCollected}/{totalRunesNeeded}");

        // Destroy the rune object after a short delay to allow any final effects
        Destroy(gameObject, 0.5f);
    }
}
