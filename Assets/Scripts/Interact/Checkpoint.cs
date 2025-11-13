using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated = false;
    public Vector3 spawnPositionOffset = new Vector3(0, 0.5f, 0);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;
        activated = true;
        Vector3 playerSpawn = transform.position + spawnPositionOffset;
        SaveManager.Instance.SetCheckpoint(playerSpawn);
        UIManager.Instance.ShowSmallMessage("Checkpoint reached");
        // optional: play animation / sound
    }
}
