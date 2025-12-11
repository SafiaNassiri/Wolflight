using UnityEngine;

public class GlowOrbPulse : MonoBehaviour
{
    // How fast the orb pulses (affects the speed of the sine wave)
    public float pulseSpeed = 2f;

    // Minimum and maximum scale values during pulsing
    public float minScale = 4f;
    public float maxScale = 5f;

    // Optional brightness pulsing
    public bool pulseBrightness = true;
    public float minBrightness = 0.7f;
    public float maxBrightness = 1.5f;

    // Optional rotation
    public bool rotate = false;
    public float rotationSpeed = 30f;

    private Vector3 originalScale;          // Scale the orb starts with
    private SpriteRenderer spriteRenderer;  // Cached SpriteRenderer
    private Color originalColor;            // Starting color of the orb
    private bool isPulsing = true;          // Whether the pulsing is active

    void Start()
    {
        // Store initial scale
        originalScale = transform.localScale;

        // Grab the SpriteRenderer for brightness control
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store its original color if found
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // If the rune was collected, pulsing stops completely
        if (!isPulsing)
            return;

        // Smooth 0->1 pulse value using a sine wave
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // SCALE PULSE
        float scale = Mathf.Lerp(minScale, maxScale, pulse);
        transform.localScale = new Vector3(scale, scale, scale);

        // BRIGHTNESS PULSE
        if (pulseBrightness && spriteRenderer != null)
        {
            float brightness = Mathf.Lerp(minBrightness, maxBrightness, pulse);

            // Multiply original color by brightness (keeping same alpha)
            Color newColor = originalColor * brightness;
            newColor.a = originalColor.a;

            spriteRenderer.color = newColor;
        }

        // OPTIONAL ROTATION
        if (rotate)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    // Called by Rune.cs when the player collects the rune
    public void StopPulse()
    {
        isPulsing = false;

        // Freeze the orb at minimum scale
        transform.localScale = new Vector3(minScale, minScale, minScale);

        // Restore original color (stops brightness effect)
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }
}
