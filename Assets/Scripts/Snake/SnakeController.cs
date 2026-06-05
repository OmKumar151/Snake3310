using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("Movement")]
    public float moveDelay = 0.2f;

    private float moveTimer;

    private Vector2Int direction = Vector2Int.right;
    private Vector2Int currentGridPosition;

    private bool isDead;

    [Header("References")]
    public Transform headTransform;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;

    [Header("Body Settings")]
    public int startingBodyParts = 1;

    private readonly List<SnakeSegment> bodySegments =
        new List<SnakeSegment>();

    private SnakeSegment tailSegment;

    private readonly List<Vector2Int> previousPositions =
        new List<Vector2Int>();

    private void Start()
    {
        currentGridPosition =
    new Vector2Int(0, 0);

        transform.position =
            GridManager.Instance.GridToWorldPosition(
                currentGridPosition
            );

        CreateStartingSnake();
    }

    private void Update()
    {
        if (isDead)
            return;

        HandleInput();

        moveTimer += Time.deltaTime;

        if (moveTimer >= moveDelay)
        {
            moveTimer = 0f;
            Move();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) &&
            direction != Vector2Int.down)
        {
            direction = Vector2Int.up;
        }

        if (Input.GetKeyDown(KeyCode.S) &&
            direction != Vector2Int.up)
        {
            direction = Vector2Int.down;
        }

        if (Input.GetKeyDown(KeyCode.A) &&
            direction != Vector2Int.right)
        {
            direction = Vector2Int.left;
        }

        if (Input.GetKeyDown(KeyCode.D) &&
            direction != Vector2Int.left)
        {
            direction = Vector2Int.right;
        }
    }

    private void Move()
    {
        previousPositions.Clear();

        previousPositions.Add(currentGridPosition);

        foreach (SnakeSegment segment in bodySegments)
        {
            previousPositions.Add(segment.gridPosition);
        }

        if (tailSegment != null)
        {
            previousPositions.Add(tailSegment.gridPosition);
        }

        currentGridPosition += direction;

        if (!GridManager.Instance.IsInsideGrid(currentGridPosition))
        {
            Die();
            return;
        }

        transform.position =
            GridManager.Instance.GridToWorldPosition(
                currentGridPosition
            );

        for (int i = 0; i < bodySegments.Count; i++)
        {
            bodySegments[i].gridPosition =
                previousPositions[i];

            bodySegments[i].transform.position =
                GridManager.Instance.GridToWorldPosition(
                    bodySegments[i].gridPosition
                );
        }

        if (tailSegment != null)
        {
            Vector2Int newTailPosition;

            if (bodySegments.Count > 0)
            {
                newTailPosition =
                    previousPositions[
                        previousPositions.Count - 2
                    ];
            }
            else
            {
                newTailPosition =
                    previousPositions[0];
            }

            tailSegment.gridPosition =
                newTailPosition;

            tailSegment.transform.position =
                GridManager.Instance.GridToWorldPosition(
                    tailSegment.gridPosition
                );
        }
    }

    private bool CheckSelfCollision()
    {
        foreach (SnakeSegment segment in bodySegments)
        {
            if (segment.gridPosition ==
                currentGridPosition)
            {
                return true;
            }
        }

        return false;
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("GAME OVER");
    }

    private void CreateStartingSnake()
    {
        Vector2Int spawnPosition =
            currentGridPosition;

        for (int i = 0; i < startingBodyParts; i++)
        {
            spawnPosition += Vector2Int.left;

            GameObject segmentObject =
                Instantiate(
                    bodyPrefab,
                    GridManager.Instance.GridToWorldPosition(
                        spawnPosition
                    ),
                    Quaternion.identity
                );

            SnakeSegment segment =
                segmentObject.GetComponent<SnakeSegment>();

            segment.gridPosition =
                spawnPosition;

            bodySegments.Add(segment);
        }

        spawnPosition += Vector2Int.left;

        GameObject tailObject =
            Instantiate(
                tailPrefab,
                GridManager.Instance.GridToWorldPosition(
                    spawnPosition
                ),
                Quaternion.identity
            );

        tailSegment =
            tailObject.GetComponent<SnakeSegment>();

        tailSegment.gridPosition =
            spawnPosition;
    }

    public void Grow()
    {
        Vector2Int spawnPosition;

        if (tailSegment != null)
        {
            spawnPosition =
                tailSegment.gridPosition;
        }
        else
        {
            spawnPosition =
                currentGridPosition;
        }

        GameObject segmentObject =
            Instantiate(
                bodyPrefab,
                GridManager.Instance.GridToWorldPosition(
                    spawnPosition
                ),
                Quaternion.identity
            );

        SnakeSegment segment =
            segmentObject.GetComponent<SnakeSegment>();

        segment.gridPosition =
            spawnPosition;

        bodySegments.Add(segment);
    }

    public Vector2Int GetHeadPosition()
    {
        return currentGridPosition;
    }

    public List<SnakeSegment> GetBodySegments()
    {
        return bodySegments;
    }
}