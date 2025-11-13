using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Popups")]
    public GameObject lorePopupPrefab; // small popup prefab to show lore temporarily
    public Transform canvasTransform;

    [Header("Journal UI")]
    public GameObject journalPanel; // main panel (inactive by default)
    public Transform journalListContainer; // parent for list items
    public GameObject journalListItemPrefab; // button prefab with image and text
    public Image journalDetailIcon;
    public Text journalDetailTitle;
    public Text journalDetailText;

    List<LoreEntry> collectedLore = new List<LoreEntry>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal();
        }
    }

    public void ShowLorePopup(string text)
    {
        // simple popup: instantiate prefab and set text; destroy after delay
        if (lorePopupPrefab && canvasTransform)
        {
            GameObject go = Instantiate(lorePopupPrefab, canvasTransform);
            var txt = go.GetComponentInChildren<Text>();
            if (txt != null) txt.text = text;
            Destroy(go, 4f);
        }
    }

    public void AddLoreToJournal(LoreEntry entry)
    {
        if (collectedLore.Contains(entry)) return;
        collectedLore.Add(entry);
        // also make a UI list button
        if (journalListItemPrefab && journalListContainer)
        {
            GameObject item = Instantiate(journalListItemPrefab, journalListContainer);
            var img = item.transform.Find("Icon")?.GetComponent<Image>();
            var txt = item.transform.Find("Title")?.GetComponent<Text>();
            if (img != null) img.sprite = entry.icon;
            if (txt != null) txt.text = entry.entryTitle;
            var button = item.GetComponent<Button>();
            button.onClick.AddListener(() => ShowJournalDetail(entry));
        }
    }

    public void AddRelicToJournal(Relic relic)
    {
        if (relic == null) return;
        if (relic.linkedLore != null)
            AddLoreToJournal(relic.linkedLore);
        // Optionally separate relics list
    }

    public void ShowJournalDetail(LoreEntry entry)
    {
        journalDetailIcon.sprite = entry.icon;
        journalDetailTitle.text = entry.entryTitle;
        journalDetailText.text = entry.loreText;
    }

    public void ToggleJournal()
    {
        if (journalPanel == null) return;
        journalPanel.SetActive(!journalPanel.activeSelf);
        Time.timeScale = journalPanel.activeSelf ? 0f : 1f; // pause when journal open
    }

    public void ShowSmallMessage(string msg)
    {
        // small, temporary UI element - reuse ShowLorePopup or implement toast system
        ShowLorePopup(msg);
    }
}
