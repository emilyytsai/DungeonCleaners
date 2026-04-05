using UnityEngine;
using UnityEngine.UI;

public class WorldUIImageLink : MonoBehaviour
{
    [SerializeField] private Image worldUIImage;

    private void Start()
    {
        if (TrashProgressManager.instance != null)
        {
            TrashProgressManager.instance.SetWorldUIImage(worldUIImage);
        }
    }
}