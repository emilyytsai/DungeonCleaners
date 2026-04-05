using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private Vector2 spawnFacingDirection;
    [SerializeField] private string spawnSoundName;

    private static Vector2 pendingPos;
    private static Vector2 pendingFacing;
    private static bool shouldMovePlayer = false;
    private static string pendingSpawnSoundName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pendingPos = spawnPosition;
            pendingFacing = spawnFacingDirection;
            pendingSpawnSoundName = spawnSoundName;
            shouldMovePlayer = true;

            if (!string.IsNullOrEmpty(spawnSoundName) && AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(spawnSoundName);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(targetSceneName);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (shouldMovePlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                // 1. Move the player
                player.transform.position = pendingPos;

                // 2. Force the Animator to use your specific parameters
                Animator anim = player.GetComponent<Animator>();
                var moveScript = player.GetComponent<PlayerController>();

                if (moveScript != null)
                {
                    // We must set the script's internal variable, not just the Animator!
                    moveScript.SyncFacingDirection(pendingFacing);
                    moveScript.FreezeInput(0.2f);
                }
            }
            shouldMovePlayer = false;
            pendingSpawnSoundName = null;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}