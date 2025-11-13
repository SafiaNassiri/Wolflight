using UnityEngine;

public class CameraFollow : MonoBehaviour
{
public Transform target;
public Vector2 offset = new Vector2(0f, 1.5f);
public float smoothSpeedX = 8f;
public float smoothTimeY = 0.25f;

private float yVelocity = 0f;

void LateUpdate()
{
    if (target == null) return;

    float targetX = Mathf.Lerp(transform.position.x, target.position.x + offset.x, smoothSpeedX * Time.deltaTime);
    float targetY = Mathf.SmoothDamp(transform.position.y, target.position.y + offset.y, ref yVelocity, smoothTimeY);

    transform.position = new Vector3(targetX, targetY, transform.position.z);
}
}
