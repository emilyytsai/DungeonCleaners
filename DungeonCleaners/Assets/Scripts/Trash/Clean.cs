using UnityEngine;

public class Clean : MonoBehaviour
{
    public float cleanRange = 1.3f;
    public KeyCode cleanKey = KeyCode.E;
    [SerializeField] private string sweepSound;
    [SerializeField] private string munchSound;
    [SerializeField] [Range(0f, 1f)] private float munchSoundChance = 0.1f;

    // Pivot offset for 1x1 player starting at (0,0)

    // List of tags to look for
    public string[] trashTags = { "solidTrash", "softTrash" };

    void Update()
    {
        if (Input.GetKeyDown(cleanKey))
        {
            CleanNearbyTrash();
        }
    }

    void CleanNearbyTrash()
    {
        Vector2 playerCenter = (Vector2)transform.position;

        // Loop through each tag type (Soft and Solid)
        foreach (string tag in trashTags)
        {
            GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject trash in foundObjects)
            {
                // Calculate distance from player center to trash center
                // Note: Ensure your trash sprites/prefabs have 'Center' pivots!
                float distance = Vector2.Distance(playerCenter, trash.transform.position);

                if (distance <= cleanRange)
                {
                    ProcessCleanup(trash);
                }
            }
        }
    }

    void ProcessCleanup(GameObject trash)
    {
        Debug.Log("Sweeping up: " + trash.name);

        if (AudioManager.instance != null)
        {
            if (!string.IsNullOrEmpty(sweepSound))
            {
                AudioManager.instance.PlaySFX(sweepSound);
            }

            if (!string.IsNullOrEmpty(munchSound) && Random.value <= munchSoundChance)
            {
                AudioManager.instance.PlaySFX(munchSound);
            }
        }

        TrashObject trashObject = trash.GetComponent<TrashObject>();
        if (trashObject != null && TrashProgressManager.instance != null)
        {
            TrashProgressManager.instance.CollectTrash(trashObject.TrashID);
        }

        Destroy(trash);

        // Logic for healing the land goes here!
    }
}