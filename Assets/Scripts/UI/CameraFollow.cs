using UnityEngine;

// Smooth 2D camera follow that keeps the player on-screen without
// lagging too far behind i.e, especially when falling.
public class CameraFollow : MonoBehaviour
{
    public Transform target;                        // The player or object the camera follows
    public Vector2 offset = new Vector2(0f, 1.5f);  // Screen offset to keep player higher in frame

    [Header("Horizontal Movement")]
    public float smoothSpeedX = 8f;                 // Horizontal follow speed (Lerp)

    [Header("Vertical Movement")]
    public float smoothTimeYNormal = 0.25f;         // Normal vertical smoothing
    public float smoothTimeYFalling = 0.1f;         // Faster smoothing when player is falling
    public float fallThreshold = -2f;               // Speed at which camera switches to fall mode

    private float yVelocity = 0f;                   // Required for SmoothDamp vertical motion
    private Rigidbody2D targetRb;                   // Cached reference to player's Rigidbody2D

    void Start()
    {
        // Grab Rigidbody so we can check player velocity for falling logic
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Pick vertical smoothing based on player's downward velocity
        float currentSmoothTime = smoothTimeYNormal;

        if (targetRb != null)
        {
            // If falling faster than threshold, camera reacts quicker
            if (targetRb.linearVelocity.y < fallThreshold)
            {
                currentSmoothTime = smoothTimeYFalling;
            }
        }

        // Smooth horizontal follow using Lerp
        float targetX = Mathf.Lerp(
            transform.position.x,
            target.position.x + offset.x,
            smoothSpeedX * Time.deltaTime
        );

        // Smooth vertical follow using SmoothDamp
        float targetY = Mathf.SmoothDamp(
            transform.position.y,
            target.position.y + offset.y,
            ref yVelocity,
            currentSmoothTime
        );

        // Apply final camera position 
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
