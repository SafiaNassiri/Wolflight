using UnityEngine;

// Handles showing and fading a UI sign when the player enters a trigger area.
// Uses CanvasGroup for smooth fade-in/out and optionally plays a sound.
public class SignTrigger : MonoBehaviour
{
    public GameObject signPanel;            // The UI panel being shown/hidden
    public float fadeDuration = 0.2f;       // Time it takes to fade fully
    public AudioClip showSound;             // Audio feedback when shown for later use

    private CanvasGroup canvasGroup;        // Controls UI transparency
    private AudioSource audioSource;        // Plays the show sound
    private bool isPlayerInside = false;    // Whether the player is inside the trigger
    private float currentAlpha = 0f;        // Tracks fade progress

    void Start()
    {
        // Setup CanvasGroup for fading
        if (signPanel != null)
        {
            canvasGroup = signPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                // Automatically add CanvasGroup if missing
                canvasGroup = signPanel.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;
            currentAlpha = 0f;
            signPanel.SetActive(false); // Start hidden
        }
        else
        {
            Debug.LogError("SignTrigger: Sign Panel is not assigned!", this);
        }

        // Setup audio source if sound is assigned
        if (showSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = showSound;
        }
    }

    void Update()
    {
        if (signPanel != null && canvasGroup != null)
        {
            float targetAlpha = isPlayerInside ? 1f : 0f;

            // Smoothly fade between current and target alpha
            if (Mathf.Abs(currentAlpha - targetAlpha) > 0.01f)
            {
                // Multiply to speed up fade relative to duration
                currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime / fadeDuration * 10f);
                canvasGroup.alpha = currentAlpha;

                // Hide when fully faded out
                if (currentAlpha < 0.01f && !isPlayerInside)
                {
                    signPanel.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Player has entered the sign area
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (signPanel != null)
            {
                signPanel.SetActive(true);

                // Play sound if available
                if (audioSource != null && showSound != null)
                {
                    audioSource.Play();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Player left the sign area -> fade out
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    // Draw a translucent box to visualize trigger area
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

    // Draw a more visible outline when selected
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
