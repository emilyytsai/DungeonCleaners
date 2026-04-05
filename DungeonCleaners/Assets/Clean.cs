using UnityEngine;

public class Clean : MonoBehaviour
{
    public float cleanRange = 1.3f;
    public KeyCode cleanKey = KeyCode.E;

    // Pivot offset for 1x1 player starting at (0,0)
    private Vector2 centerOffset = new Vector2(0.5f, 0.5f);

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
        Vector2 playerCenter = (Vector2)transform.position + centerOffset;

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
        Destroy(trash);

        // Logic for healing the land goes here!
    }
}