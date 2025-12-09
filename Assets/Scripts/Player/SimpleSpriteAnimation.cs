using UnityEngine;

public class SimpleSpriteAnimation : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 10f;
    public bool loop = false;
    public bool destroyOnComplete = true;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SimpleSpriteAnimation requires a SpriteRenderer component!");
            enabled = false;
            return;
        }

        if (frames.Length == 0)
        {
            Debug.LogError("No animation frames assigned!");
            enabled = false;
            return;
        }
        spriteRenderer.sprite = frames[0];
    }

    void Update()
    {
        if (!isPlaying || frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    isPlaying = false;

                    if (destroyOnComplete)
                    {
                        Destroy(gameObject);
                    }
                    return;
                }
            }

            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    public void Play()
    {
        currentFrame = 0;
        timer = 0f;
        isPlaying = true;
        if (frames.Length > 0)
            spriteRenderer.sprite = frames[0];
    }

    public void Stop()
    {
        isPlaying = false;
    }
}