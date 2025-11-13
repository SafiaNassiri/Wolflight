using UnityEngine;

public class LoreObject : MonoBehaviour
{
    public LoreEntry loreEntry; // assign ScriptableObject
    public Relic relic; // optional

    public void Interact()
    {
        if (loreEntry != null && !loreEntry.isCollected)
        {
            loreEntry.isCollected = true;
            UIManager.Instance.AddLoreToJournal(loreEntry);
            UIManager.Instance.ShowLorePopup(loreEntry.loreText);
        }
        if (relic != null && !relic.isCollected)
        {
            relic.isCollected = true;
            UIManager.Instance.AddRelicToJournal(relic);
        }
        // optional: destroy or play pickup animation
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Interact(); // auto-pickup; or require button press
        }
    }
}
