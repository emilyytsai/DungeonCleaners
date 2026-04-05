using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TrashProgressManager : MonoBehaviour
{
    public static TrashProgressManager instance;

    [Header("Progress")]
    [SerializeField] private int totalTrashCount;
    [SerializeField] private int collectedTrashCount;

    [Header("Overworld Scene Names")]
    [SerializeField] private string level1SceneName;
    [SerializeField] private string level2SceneName;
    [SerializeField] private string level3SceneName;
    [SerializeField] private string level4SceneName;
    [SerializeField] private string level5SceneName;

    [Header("World UI Visual")]
    [SerializeField] private Image worldUIImage;
    [SerializeField] private Color unhealthyColor = Color.red;
    [SerializeField] private Color healthyColor = Color.white;

    private HashSet<string> collectedTrashIDs = new HashSet<string>();

    public int TotalTrashCount => totalTrashCount;
    public int CollectedTrashCount => collectedTrashCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterTotalTrash(int total)
    {
        totalTrashCount = total;
        UpdateWorldColor();
    }

    public void CollectTrash(string trashID)
    {
        if (string.IsNullOrEmpty(trashID)) return;
        if (collectedTrashIDs.Contains(trashID)) return;

        collectedTrashIDs.Add(trashID);
        collectedTrashCount++;
        UpdateWorldColor();
    }

    public bool HasCollectedTrash(string trashID)
    {
        return collectedTrashIDs.Contains(trashID);
    }

    public float GetCleanupRatio()
    {
        if (totalTrashCount <= 0) return 0f;
        return (float)collectedTrashCount / totalTrashCount;
    }

    public int GetOverworldLevel()
    {
        float ratio = GetCleanupRatio();

        if (ratio <= 0.10f) return 1;
        if (ratio <= 0.30f) return 2;
        if (ratio <= 0.50f) return 3;
        if (ratio <= 0.80f) return 4;
        return 5;
    }

    public string GetOverworldSceneName()
    {
        int level = GetOverworldLevel();

        switch (level)
        {
            case 1: return level1SceneName;
            case 2: return level2SceneName;
            case 3: return level3SceneName;
            case 4: return level4SceneName;
            case 5: return level5SceneName;
            default: return level1SceneName;
        }
    }

    public void SetWorldUIImage(Image image)
    {
        worldUIImage = image;
        UpdateWorldColor();
    }

    public void UpdateWorldColor()
    {
        if (worldUIImage == null) return;

        float ratio = GetCleanupRatio();
        worldUIImage.color = Color.Lerp(unhealthyColor, healthyColor, ratio);
    }
}