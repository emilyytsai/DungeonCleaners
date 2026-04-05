using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashObject : MonoBehaviour
{
    [SerializeField] private string trashID;

    public string TrashID => trashID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(trashID))
        {
            GenerateID();
        }

        if (TrashProgressManager.instance != null)
        {
            TrashProgressManager.instance.RegisterTrash(trashID);

            if (TrashProgressManager.instance.HasCollectedTrash(trashID))
            {
                Destroy(gameObject);
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(trashID))
        {
            GenerateID();
        }
    }
#endif

    private void GenerateID()
    {
        Vector3 pos = transform.position;
        string sceneName = SceneManager.GetActiveScene().name;
        trashID = sceneName + "_" + gameObject.name + "_" + pos.x.ToString("F2") + "_" + pos.y.ToString("F2");
    }
}