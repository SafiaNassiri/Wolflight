using UnityEngine;

public class WolfSectionMarker : MonoBehaviour
{
    [TextArea] public string loreText;
    public bool triggersCheckpoint = true;
    public bool hasTriggered = false;
    public float leadDistance = 1.5f; // optional short lead
    public bool doShortLead = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        if (!string.IsNullOrEmpty(loreText))
            UIManager.Instance.ShowLorePopup(loreText);

        if (triggersCheckpoint)
            SaveManager.Instance?.SetCheckpoint(transform.position);

        if (doShortLead)
            StartCoroutine(ShortLead());
    }

    System.Collections.IEnumerator ShortLead()
    {
        Vector3 start = transform.position;
        Vector3 target = transform.position + Vector3.right * leadDistance; // example; could be directional
        float t = 0f;
        float dur = 0.4f;
        while (t < dur)
        {
            transform.position = Vector3.Lerp(start, target, t / dur);
            t += Time.deltaTime;
            yield return null;
        }
        // return to start
        t = 0f;
        while (t < dur)
        {
            transform.position = Vector3.Lerp(target, start, t / dur);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
