using UnityEngine;

public class Apple : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }

    public void SetGridPosition(Vector2Int gridPosition)
    {
        GridPosition = gridPosition;

        transform.position =
            GridManager.Instance.GridToWorldPosition(gridPosition);
    }
}