using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPit : MonoBehaviour
{
    public float deathDelay = 1.0f;
    public string gameOverSceneName = "";
    public AudioClip deathSound;
    public GameObject deathAnimationPrefab;

    private bool isDying = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player
        if (other.CompareTag("Player") && !isDying)
        {
            isDying = true;

            // Store player position
            Vector3 deathPosition = other.transform.position;

            // Play death sound
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, deathPosition);
            }

            // Spawn death animation at player position
            if (deathAnimationPrefab != null)
            {
                Instantiate(deathAnimationPrefab, deathPosition, Quaternion.identity);
            }

            // Hide the player
            other.gameObject.SetActive(false);

            // Wait for animation to finish, then load scene
            Invoke(nameof(LoadNextScene), deathDelay);
        }
    }

    void LoadNextScene()
    {
        if (string.IsNullOrEmpty(gameOverSceneName))
        {
            // Reload current scene
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        else
        {
            // Load game over scene
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}