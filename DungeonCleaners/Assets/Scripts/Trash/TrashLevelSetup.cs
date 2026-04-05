using UnityEngine;

public class TrashLevelSetup : MonoBehaviour
{
    [SerializeField] private string[] trashTags = { "solidTrash", "softTrash" };

    private void Start()
    {
        int total = 0;

        foreach (string tag in trashTags)
        {
            total += GameObject.FindGameObjectsWithTag(tag).Length;
        }

        if (TrashProgressManager.instance != null)
        {
            TrashProgressManager.instance.RegisterTotalTrash(total);
        }
    }
}