using UnityEngine;

public class SnakeSegment : MonoBehaviour
{
    public Vector2Int gridPosition;

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();
    }
}