using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;

    [Header("Board")]
    public SpriteRenderer boardRenderer;

    [Header("Grid")]
    public float cellSize = 0.5f;

    public int width { get; private set; }
    public int height { get; private set; }

    private Vector2 origin;

    private void Awake()
    {
        Instance = this;

        CalculateGrid();
    }

    private void CalculateGrid()
    {
        Vector2 size =
            boardRenderer.bounds.size;

        width =
            Mathf.RoundToInt(
                size.x / cellSize);

        height =
            Mathf.RoundToInt(
                size.y / cellSize);

        origin =
            (Vector2)boardRenderer.bounds.min;

        Debug.Log(
            $"Board Grid: {width} x {height}");
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        Vector2 origin =
            (Vector2)boardRenderer.bounds.center
            - new Vector2(width, height) * cellSize * 0.5f;

        return origin +
               new Vector2(
                   gridPosition.x * cellSize,
                   gridPosition.y * cellSize
               );
    }

    public bool IsInside(Vector2Int position)
    {
        return position.x >= 0 &&
               position.x < width &&
               position.y >= 0 &&
               position.y < height;
    }
}