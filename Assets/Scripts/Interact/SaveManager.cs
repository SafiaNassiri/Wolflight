using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public Vector3 checkpointPosition;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void SetCheckpoint(Vector3 pos)
    {
        checkpointPosition = pos;
        PlayerPrefs.SetFloat("checkpoint_x", pos.x);
        PlayerPrefs.SetFloat("checkpoint_y", pos.y);
        PlayerPrefs.Save();
    }

    public Vector3 GetCheckpoint()
    {
        if (PlayerPrefs.HasKey("checkpoint_x"))
        {
            return new Vector3(PlayerPrefs.GetFloat("checkpoint_x"), PlayerPrefs.GetFloat("checkpoint_y"), 0f);
        }
        return checkpointPosition;
    }
}
