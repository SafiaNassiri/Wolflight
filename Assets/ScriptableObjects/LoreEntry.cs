using UnityEngine;

[CreateAssetMenu(fileName = "NewLoreEntry", menuName = "Journal/LoreEntry")]
public class LoreEntry : ScriptableObject
{
    public string entryTitle;
    [TextArea(2, 6)]
    public string loreText;
    public Sprite icon;
    [HideInInspector] public bool isCollected = false;
    public string id; // unique id if you want to persist
}
