using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector2 offset = new Vector2(0f, 1.5f);

    [Header("Horizontal Follow")]
    public float smoothSpeedX = 8f;

    [Header("Vertical Follow")]
    [Tooltip("Smooth time when moving up or slowly")]
    public float smoothTimeYNormal = 0.25f;

    [Tooltip("Smooth time when falling fast")]
    public float smoothTimeYFalling = 0.1f;

    [Tooltip("Falling velocity threshold to use faster smoothing")]
    public float fallThreshold = -2f;

    private float yVelocity = 0f;
    private Rigidbody2D targetRb;

    void Start()
    {
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Determine smooth time based on falling speed
        float currentSmoothTime = smoothTimeYNormal;

        if (targetRb != null)
        {
            // If player is falling fast, use faster camera follow
            if (targetRb.linearVelocity.y < fallThreshold)
            {
                currentSmoothTime = smoothTimeYFalling;
            }
        }

        // Smooth horizontal follow
        float targetX = Mathf.Lerp(transform.position.x, target.position.x + offset.x, smoothSpeedX * Time.deltaTime);

        // Smooth vertical follow (faster when falling)
        float targetY = Mathf.SmoothDamp(transform.position.y, target.position.y + offset.y, ref yVelocity, currentSmoothTime);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}