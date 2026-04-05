using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnPortal : MonoBehaviour
{
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private Vector2 spawnFacingDirection;
    [SerializeField] private string spawnSoundName;

    private static Vector2 pendingPos;
    private static Vector2 pendingFacing;
    private static bool shouldMovePlayer = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pendingPos = spawnPosition;
            pendingFacing = spawnFacingDirection;
            shouldMovePlayer = true;

            if (!string.IsNullOrEmpty(spawnSoundName) && AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(spawnSoundName);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;

            if (TrashProgressManager.instance != null)
            {
                SceneManager.LoadScene(TrashProgressManager.instance.GetOverworldSceneName());
            }
            else
            {
                Debug.LogWarning("TrashProgressManager instance not found!");
            }
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
                var moveScript = player.GetComponent<PlayerController>();

                if (moveScript != null)
                {
                    // We must set the script's internal variable, not just the Animator!
                    moveScript.SyncFacingDirection(pendingFacing);
                    moveScript.FreezeInput(0.2f);
                }
            }

            shouldMovePlayer = false;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}