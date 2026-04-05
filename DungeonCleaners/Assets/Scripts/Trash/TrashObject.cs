using UnityEngine;

public class TrashObject : MonoBehaviour
{
    [SerializeField] private string trashID;

    public string TrashID => trashID;

    private void Start()
    {
        if (TrashProgressManager.instance != null &&
            TrashProgressManager.instance.HasCollectedTrash(trashID))
        {
            Destroy(gameObject);
        }
    }
}