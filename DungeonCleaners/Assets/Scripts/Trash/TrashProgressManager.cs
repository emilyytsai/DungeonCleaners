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

    [Header("Progress Thresholds")]
    [SerializeField] [Range(0f, 1f)] private float level2Threshold = 0.20f;
    [SerializeField] [Range(0f, 1f)] private float level3Threshold = 0.45f;
    [SerializeField] [Range(0f, 1f)] private float level4Threshold = 0.70f;
    [SerializeField] [Range(0f, 1f)] private float level5Threshold = 0.95f;

    [Header("Color Progress")]
    [SerializeField] private float colorCurvePower = 2f;

    private HashSet<string> registeredTrashIDs = new HashSet<string>();
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

    public void RegisterTrash(string trashID)
    {
        if (string.IsNullOrEmpty(trashID)) return;
        if (registeredTrashIDs.Contains(trashID)) return;

        registeredTrashIDs.Add(trashID);
        totalTrashCount = registeredTrashIDs.Count;
        UpdateWorldColor();
    }

    public void CollectTrash(string trashID)
    {
        if (string.IsNullOrEmpty(trashID)) return;
        if (collectedTrashIDs.Contains(trashID)) return;

        collectedTrashIDs.Add(trashID);
        collectedTrashCount = collectedTrashIDs.Count;
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

        if (ratio < level2Threshold) return 1;
        if (ratio < level3Threshold) return 2;
        if (ratio < level4Threshold) return 3;
        if (ratio < level5Threshold) return 4;
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
        float adjustedRatio = Mathf.Pow(ratio, colorCurvePower);
        worldUIImage.color = Color.Lerp(unhealthyColor, healthyColor, adjustedRatio);
    }
}