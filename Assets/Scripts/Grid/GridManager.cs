using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Size")]
    public int width = 18;
    public int height = 18;

    [Header("Cell Size")]
    public float cellSize = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * cellSize,
            gridPosition.y * cellSize,
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

    public List<Vector2Int> GetAllCells()
    {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells.Add(new Vector2Int(x, y));
            }
        }

        return cells;
    }
}