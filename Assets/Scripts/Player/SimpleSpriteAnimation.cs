using UnityEngine;

// A lightweight frame-by-frame sprite animation system.
// Perfect for simple effects, hitsparks, particles, etc.
// Supports looping and auto-destroy when finished.

public class SimpleSpriteAnimation : MonoBehaviour
{
    public Sprite[] frames;                     // Sequence of sprites for the animation
    public float frameRate = 10f;               // Frames per second
    public bool loop = false;                   // Should the animation repeat?
    public bool destroyOnComplete = true;       // Destroy object after finishing (if not looping)

    private SpriteRenderer spriteRenderer;      // Renderer that displays the frames
    private int currentFrame = 0;               // Index of the current frame
    private float timer = 0f;                   // Tracks time between frame updates
    private bool isPlaying = true;              // True while animation is active

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure a SpriteRenderer is present
        if (spriteRenderer == null)
        {
            Debug.LogError("SimpleSpriteAnimation requires a SpriteRenderer component!");
            enabled = false;
            return;
        }

        // Ensure animation has frames
        if (frames.Length == 0)
        {
            Debug.LogError("No animation frames assigned!");
            enabled = false;
            return;
        }

        // Set initial frame
        spriteRenderer.sprite = frames[0];
    }

    void Update()
    {
        // Stop if animation is paused or empty
        if (!isPlaying || frames.Length == 0) return;

        timer += Time.deltaTime;

        // Advance frame when enough time has passed
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame++;

            // Reached end of animation
            if (currentFrame >= frames.Length)
            {
                if (loop)
                {
                    // Restart from beginning
                    currentFrame = 0;
                }
                else
                {
                    // Stop animation
                    isPlaying = false;

                    // Auto-destroy the object if enabled
                    if (destroyOnComplete)
                    {
                        Destroy(gameObject);
                    }
                    return;
                }
            }

            // Display next frame
            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    // Restart the animation from the beginning.
    public void Play()
    {
        currentFrame = 0;
        timer = 0f;
        isPlaying = true;

        if (frames.Length > 0)
            spriteRenderer.sprite = frames[0];
    }

    // Pause the animation.
    public void Stop()
    {
        isPlaying = false;
    }
}
