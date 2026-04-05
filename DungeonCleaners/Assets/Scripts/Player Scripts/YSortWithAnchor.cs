using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortWithAnchor : MonoBehaviour
{
    [SerializeField]
    private Transform sortPoint;

    [SerializeField]
    private int offset = 0;

    [SerializeField]
    private float precision = 100f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sortPoint == null)
            sortPoint = transform;
    }

    private void LateUpdate()
    {
        sr.sortingOrder = Mathf.RoundToInt(-sortPoint.position.y * precision) + offset;
    }
}