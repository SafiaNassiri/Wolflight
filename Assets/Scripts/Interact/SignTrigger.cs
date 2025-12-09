using UnityEngine;

public class SignTrigger : MonoBehaviour
{
    [Header("UI Panel")]
    [Tooltip("The panel that will show when player enters the trigger")]
    public GameObject signPanel;

    [Header("Settings")]
    [Tooltip("Fade in/out duration")]
    public float fadeDuration = 0.2f;

    [Tooltip("Optional: Play sound when showing panel")]
    public AudioClip showSound;

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool isPlayerInside = false;
    private float currentAlpha = 0f;

    void Start()
    {
        // Make sure the panel starts hidden
        if (signPanel != null)
        {
            // Add CanvasGroup for smooth fading
            canvasGroup = signPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = signPanel.AddComponent<CanvasGroup>();
            }

            // Start invisible
            canvasGroup.alpha = 0f;
            currentAlpha = 0f;
            signPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("SignTrigger: Sign Panel is not assigned!", this);
        }

        // Setup audio if sound is provided
        if (showSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = showSound;
        }
    }

    void Update()
    {
        // Smooth fade in/out
        if (signPanel != null && canvasGroup != null)
        {
            float targetAlpha = isPlayerInside ? 1f : 0f;

            if (Mathf.Abs(currentAlpha - targetAlpha) > 0.01f)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime / fadeDuration * 10f);
                canvasGroup.alpha = currentAlpha;

                // Deactivate panel when fully faded out
                if (currentAlpha < 0.01f && !isPlayerInside)
                {
                    signPanel.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (signPanel != null)
            {
                signPanel.SetActive(true);

                // Play sound if assigned
                if (audioSource != null && showSound != null)
                {
                    audioSource.Play();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player left the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    // Draw the trigger area in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(boxCollider.offset, boxCollider.size);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.6f);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
        }
    }
}