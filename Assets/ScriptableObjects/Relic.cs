using UnityEngine;

[CreateAssetMenu(fileName = "NewRelic", menuName = "Journal/Relic")]
public class Relic : ScriptableObject
{
    public string relicName;
    public Sprite icon;
    public LoreEntry linkedLore;
    [HideInInspector] public bool isCollected = false;
    public string id;
}
