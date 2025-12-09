using UnityEngine;

public class GlowOrbPulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;

    // Updated to match your desired scale range (4 -> 5)
    public float minScale = 4f;
    public float maxScale = 5f;

    [Header("Glow Settings")]
    public bool pulseBrightness = true;
    public float minBrightness = 0.7f;
    public float maxBrightness = 1.5f;

    [Header("Rotation")]
    public bool rotate = false;
    public float rotationSpeed = 30f;

    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private bool isPulsing = true;   // Allows stopping the pulse

    void Start()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (!isPulsing)
            return;

        // Sin wave for smooth pulsing (0 to 1)
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // Scale pulse
        float scale = Mathf.Lerp(minScale, maxScale, pulse);
        transform.localScale = new Vector3(scale, scale, scale);

        // Brightness pulse
        if (pulseBrightness && spriteRenderer != null)
        {
            float brightness = Mathf.Lerp(minBrightness, maxBrightness, pulse);
            Color newColor = originalColor * brightness;
            newColor.a = originalColor.a;
            spriteRenderer.color = newColor;
        }

        // Rotation (optional)
        if (rotate)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    // Call this when the player interacts with the rune
    public void StopPulse()
    {
        isPulsing = false;

        // Reset to normal size and brightness
        transform.localScale = new Vector3(minScale, minScale, minScale);

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }
}
