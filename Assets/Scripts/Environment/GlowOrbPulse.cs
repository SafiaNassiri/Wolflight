using UnityEngine;

public class GlowOrbPulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    [Tooltip("How fast the orb pulses")]
    public float pulseSpeed = 2f;

    [Tooltip("Minimum scale (0.8 = 80% of original size)")]
    public float minScale = 0.8f;

    [Tooltip("Maximum scale (1.2 = 120% of original size)")]
    public float maxScale = 1.2f;

    [Header("Glow Settings")]
    [Tooltip("Should the brightness pulse too?")]
    public bool pulseBrightness = true;

    [Tooltip("Minimum brightness")]
    public float minBrightness = 0.7f;

    [Tooltip("Maximum brightness")]
    public float maxBrightness = 1.5f;

    [Header("Rotation")]
    [Tooltip("Should the orb rotate?")]
    public bool rotate = false;

    [Tooltip("Rotation speed (degrees per second)")]
    public float rotationSpeed = 30f;

    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        // Calculate pulse value (oscillates between 0 and 1)
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // Scale pulse
        float scale = Mathf.Lerp(minScale, maxScale, pulse);
        transform.localScale = originalScale * scale;

        // Brightness pulse
        if (pulseBrightness && spriteRenderer != null)
        {
            float brightness = Mathf.Lerp(minBrightness, maxBrightness, pulse);
            Color newColor = originalColor * brightness;
            newColor.a = originalColor.a; // Keep original alpha
            spriteRenderer.color = newColor;
        }

        // Rotation
        if (rotate)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}