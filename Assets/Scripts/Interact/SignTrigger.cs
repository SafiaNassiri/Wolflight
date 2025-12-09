using UnityEngine;

public class SignTrigger : MonoBehaviour
{
    public GameObject signPanel;
    public float fadeDuration = 0.2f;
    public AudioClip showSound;

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool isPlayerInside = false;
    private float currentAlpha = 0f;

    void Start()
    {
        if (signPanel != null)
        {
            canvasGroup = signPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = signPanel.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;
            currentAlpha = 0f;
            signPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("SignTrigger: Sign Panel is not assigned!", this);
        }

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

            if (Mathf.Abs(currentAlpha - targetAlpha) > 0.01f)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime / fadeDuration * 10f);
                canvasGroup.alpha = currentAlpha;

                if (currentAlpha < 0.01f && !isPlayerInside)
                {
                    signPanel.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (signPanel != null)
            {
                signPanel.SetActive(true);

                if (audioSource != null && showSound != null)
                {
                    audioSource.Play();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

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