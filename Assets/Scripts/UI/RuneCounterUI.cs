using UnityEngine;
using TMPro;

/// Handles updating and animating the on-screen rune counter UI.
/// Keeps the display synced with Rune.runesCollected and plays a small
/// "punch" animation whenever the count changes.
public class RuneCounterUI : MonoBehaviour
{
    // Instance so other scripts can easily reference the UI.
    public static RuneCounterUI Instance { get; private set; }

    public TextMeshProUGUI runeCountText; // The "X/Y" text display
    public GameObject runeIcon;           // Icon for runes (unused but available for future code refactoring)

    public bool animateOnCollect = true;  // Toggle the punch animation
    public float punchScale = 1.3f;       // How large the text scales during the punch
    public float animDuration = 0.3f;     // Total duration of the punch animation

    private Vector3 originalScale;        // Rune text's starting scale
    private int lastCount = 0;            // Detect when count changes

    void Awake()
    {
        // Only one instance allowed
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Record the starting scale of the text so we can animate from it
        if (runeCountText != null)
        {
            originalScale = runeCountText.transform.localScale;
        }

        UpdateDisplay();
    }

    void Update()
    {
        // Detect changes in the collected runes count
        if (Rune.runesCollected != lastCount)
        {
            lastCount = Rune.runesCollected;
            UpdateDisplay();

            // Trigger the punch animation if enabled
            if (animateOnCollect)
            {
                AnimatePunch();
            }
        }
    }

    /// Updates the text to show current/required runes.
    void UpdateDisplay()
    {
        if (runeCountText != null)
        {
            runeCountText.text = $"{Rune.runesCollected}/{Rune.totalRunesNeeded}";
        }
    }

    /// Starts the punch animation coroutine.
    void AnimatePunch()
    {
        if (runeCountText == null) return;

        StopAllCoroutines(); // Cancel any ongoing animation so no overlap
        StartCoroutine(PunchAnimation());
    }

    /// Punch animation: scale up, then scale back down.
    System.Collections.IEnumerator PunchAnimation()
    {
        float elapsed = 0f;

        // Scale up
        while (elapsed < animDuration / 2)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animDuration / 2);
            runeCountText.transform.localScale = Vector3.Lerp(originalScale, originalScale * punchScale, progress);
            yield return null;
        }

        elapsed = 0f;

        // Scale down
        while (elapsed < animDuration / 2)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animDuration / 2);
            runeCountText.transform.localScale = Vector3.Lerp(originalScale * punchScale, originalScale, progress);
            yield return null;
        }

        // Make sure final scale is the iriogional scale
        runeCountText.transform.localScale = originalScale;
    }
}
