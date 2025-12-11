using UnityEngine;

// Handles automatic background scrolling for parallax effects or moving skies, fog, etc. 
public class AutoScrollBackground : MonoBehaviour
{
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0f;
    public bool infiniteScroll = true;
    public float spriteWidth;
    public float spriteHeight;

    private Vector3 startPos;                   // The original starting position of the background
    private Material material;                  // Material reference if texture offset scrolling is enabled
    private bool useMaterialScroll = false;     // Switch between material or position-based scrolling

    void Start()
    {
        // Store the starting position so we know where to loop back to
        startPos = transform.position;

        // Material scrolling disabled
        useMaterialScroll = false;

        // Auto-detect sprite size if using a SpriteRenderer
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (spriteWidth == 0) spriteWidth = sr.bounds.size.x;
            if (spriteHeight == 0) spriteHeight = sr.bounds.size.y;
        }
    }

    void Update()
    {
        // If material scrolling is ever re-enabled, use texture offset instead of transform movement
        if (useMaterialScroll)
        {
            float offsetX = Time.time * scrollSpeedX * 0.1f;
            float offsetY = Time.time * scrollSpeedY * 0.1f;
            material.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
        else if (infiniteScroll)
        {
            // Move the background based on scroll speeds
            float newX = transform.position.x + scrollSpeedX * Time.deltaTime;
            float newY = transform.position.y + scrollSpeedY * Time.deltaTime;

            transform.position = new Vector3(newX, newY, transform.position.z);

            // Horizontal infinite loop
            if (spriteWidth > 0)
            {
                // Background moving right
                if (scrollSpeedX > 0 && transform.position.x > startPos.x + spriteWidth)
                {
                    transform.position = new Vector3(startPos.x, transform.position.y, transform.position.z);
                }
                // Background moving left
                else if (scrollSpeedX < 0 && transform.position.x < startPos.x - spriteWidth)
                {
                    transform.position = new Vector3(startPos.x, transform.position.y, transform.position.z);
                }
            }

            // Vertical infinite loop
            if (spriteHeight > 0)
            {
                // Moving up
                if (scrollSpeedY > 0 && transform.position.y > startPos.y + spriteHeight)
                {
                    transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
                }
                // Moving down
                else if (scrollSpeedY < 0 && transform.position.y < startPos.y - spriteHeight)
                {
                    transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
                }
            }
        }
        else
        {
            // Simple scroll with no looping
            float newX = transform.position.x + scrollSpeedX * Time.deltaTime;
            float newY = transform.position.y + scrollSpeedY * Time.deltaTime;
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }
}
