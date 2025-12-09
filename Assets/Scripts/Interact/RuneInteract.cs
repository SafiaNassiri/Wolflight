using UnityEngine;

public class Rune : MonoBehaviour, IInteractable
{
    public static int runesCollected = 0;
    public static int totalRunesNeeded = 14;

    public GlowOrbPulse glowPulse;
    private bool collected = false;

    public void Interact()
    {
        if (collected) return;

        collected = true;
        runesCollected++;

        // stop glow pulse
        if (glowPulse != null)
            glowPulse.enabled = false;

        Debug.Log($"Rune collected! Total: {runesCollected}/{totalRunesNeeded}");

        // destroy rune light after collecting
        Destroy(gameObject);
    }
}
