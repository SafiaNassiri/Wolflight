using UnityEngine;

public class Rune : MonoBehaviour, IInteractable
{
    public static int runesCollected = 0;
    public static int totalRunesNeeded = 14;

    [Header("Rune Settings")]
    public int runeNumber = 1;

    [Header("Lore Text")]
    [TextArea(3, 10)]
    public string loreText = "Ancient power flows through this rune...";

    [Header("References")]
    public GlowOrbPulse glowPulse;

    [Header("Optional Audio")]
    public AudioClip collectSound;

    private bool collected = false;

    public void Interact()
    {
        if (collected) return;

        collected = true;
        runesCollected++;

        // Play collect sound
        if (collectSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(collectSound);
        }

        // Show dialogue with lore
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowDialogue(loreText);
        }

        // Stop glow pulse
        if (glowPulse != null)
        {
            glowPulse.enabled = false;
        }

        Debug.Log($"Rune collected! Total: {runesCollected}/{totalRunesNeeded}");

        // Hide the glow (which is the child)
        if (glowPulse != null)
        {
            glowPulse.gameObject.SetActive(false);
        }

        // Destroy after a short delay (give time for dialogue to appear)
        Destroy(gameObject, 0.5f);
    }
}