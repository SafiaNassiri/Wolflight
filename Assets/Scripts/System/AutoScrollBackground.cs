using UnityEngine;

public class AutoScrollBackground : MonoBehaviour
{
    [Header("Scroll Settings")]
    [Tooltip("Horizontal scroll speed (negative = left, positive = right)")]
    public float scrollSpeedX = 0.5f;

    [Tooltip("Vertical scroll speed (negative = down, positive = up)")]
    public float scrollSpeedY = 0f;

    [Header("Infinite Scrolling")]
    [Tooltip("Enable seamless looping")]
    public bool infiniteScroll = true;

    [Tooltip("Width of sprite (auto-detected if using SpriteRenderer)")]
    public float spriteWidth;

    [Tooltip("Height of sprite (for vertical scrolling)")]
    public float spriteHeight;

    private Vector3 startPos;
    private Material material;
    private bool useMaterialScroll = false;

    void Start()
    {
        startPos = transform.position;

        // DISABLED material scrolling - use position scrolling instead
        useMaterialScroll = false;

        // Auto-detect sprite size
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (spriteWidth == 0) spriteWidth = sr.bounds.size.x;
            if (spriteHeight == 0) spriteHeight = sr.bounds.size.y;
        }
    }

    void Update()
    {
        // DEBUG
        //Debug.Log($"Scrolling: Speed={scrollSpeedX}, Pos={transform.position.x:F2}, UseMaterial={useMaterialScroll}");

        if (useMaterialScroll)
        {
            // Scroll using texture offset (best for tiling textures)
            float offsetX = Time.time * scrollSpeedX * 0.1f;
            float offsetY = Time.time * scrollSpeedY * 0.1f;
            material.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
        else if (infiniteScroll)
        {
            // Scroll by moving the object and resetting position
            float newX = transform.position.x + (scrollSpeedX * Time.deltaTime);
            float newY = transform.position.y + (scrollSpeedY * Time.deltaTime);

            transform.position = new Vector3(newX, newY, transform.position.z);

            // Reset position for infinite loop (horizontal)
            if (spriteWidth > 0)
            {
                if (scrollSpeedX > 0 && transform.position.x > startPos.x + spriteWidth)
                {
                    transform.position = new Vector3(startPos.x, transform.position.y, transform.position.z);
                }
                else if (scrollSpeedX < 0 && transform.position.x < startPos.x - spriteWidth)
                {
                    transform.position = new Vector3(startPos.x, transform.position.y, transform.position.z);
                }
            }

            // Reset position for infinite loop (vertical)
            if (spriteHeight > 0)
            {
                if (scrollSpeedY > 0 && transform.position.y > startPos.y + spriteHeight)
                {
                    transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
                }
                else if (scrollSpeedY < 0 && transform.position.y < startPos.y - spriteHeight)
                {
                    transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
                }
            }
        }
        else
        {
            // Simple continuous scroll (no looping)
            float newX = transform.position.x + (scrollSpeedX * Time.deltaTime);
            float newY = transform.position.y + (scrollSpeedY * Time.deltaTime);
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }
}