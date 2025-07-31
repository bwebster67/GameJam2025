using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortByPosition : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float previousY;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        previousY = transform.position.y;
    }

    void LateUpdate()
    {
        float currentY = transform.position.y;
        if (Mathf.Abs(currentY - previousY) > 0.01f) // update only if moved enough
        {
            spriteRenderer.sortingOrder = -(int)(currentY * 100);
            previousY = currentY;
        }
    }
}
