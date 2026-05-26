using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Size")]
    public int width = 18;
    public int height = 18;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x,
            gridPosition.y,
            0f
        );
    }

    public bool IsInsideGrid(Vector2Int position)
    {
        return position.x >= 0 &&
               position.x < width &&
               position.y >= 0 &&
               position.y < height;
    }
}