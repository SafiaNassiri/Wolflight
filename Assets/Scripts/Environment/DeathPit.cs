using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathPit : MonoBehaviour
{
    public float deathDelay = 1.0f;
    public string gameOverSceneName = "GameOver";
    public GameObject deathAnimationPrefab;

    private bool isDying = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDying)
        {
            isDying = true;
            Vector3 deathPosition = other.transform.position;

            // Play death sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayDeathSound();

            // Spawn death animation
            if (deathAnimationPrefab != null)
                Instantiate(deathAnimationPrefab, deathPosition, Quaternion.identity);

            // Hide player
            other.gameObject.SetActive(false);

            // Reload after delay
            Invoke(nameof(LoadNextScene), deathDelay);
        }
    }

    void LoadNextScene()
    {
        if (string.IsNullOrEmpty(gameOverSceneName))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        else
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}