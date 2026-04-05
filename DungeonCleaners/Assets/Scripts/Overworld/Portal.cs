using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName;

    [SerializeField]
    private Vector2 spawnPositionInTargetScene;

    private bool hasTriggered = false;

    private static Vector2 pendingSpawnPosition;
    private static bool shouldSetSpawn = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        pendingSpawnPosition = spawnPositionInTargetScene;
        shouldSetSpawn = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (shouldSetSpawn)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = pendingSpawnPosition;
            }
            shouldSetSpawn = false;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}