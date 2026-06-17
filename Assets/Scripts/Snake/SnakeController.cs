using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("Movement")]
    public float moveDelay = 0.2f;

    private float moveTimer;

<<<<<<< HEAD
    private Vector2Int direction = Vector2Int.right;
=======
    private Vector2Int direction =
        Vector2Int.right;

>>>>>>> 8444d17 (Snake movement improved)
    private Vector2Int currentGridPosition;

    private bool isDead;

    [Header("References")]
    public Transform headTransform;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;

    [Header("Sprites")]
    public SnakeSprites snakeSprites;

    private SpriteRenderer headSpriteRenderer;

    [Header("Body Settings")]
    public int startingBodyParts = 1;

    private readonly List<SnakeSegment> bodySegments =
        new List<SnakeSegment>();

    private SnakeSegment tailSegment;

    private readonly List<Vector2Int> previousPositions =
        new List<Vector2Int>();

    private void Start()
    {
<<<<<<< HEAD
        currentGridPosition =
    new Vector2Int(0, 0);
=======
        headSpriteRenderer =
            headTransform.GetComponent<SpriteRenderer>();

        currentGridPosition =
            new Vector2Int(
                GridManager.Instance.width / 2,
                GridManager.Instance.height / 2
            );
>>>>>>> 8444d17 (Snake movement improved)

        transform.position =
            GridManager.Instance.GridToWorldPosition(
                currentGridPosition
            );

        CreateStartingSnake();

        UpdateHeadSprite();
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

            UpdateHeadSprite();
        }

        if (Input.GetKeyDown(KeyCode.S) &&
            direction != Vector2Int.up)
        {
            direction = Vector2Int.down;

            UpdateHeadSprite();
        }

        if (Input.GetKeyDown(KeyCode.A) &&
            direction != Vector2Int.right)
        {
            direction = Vector2Int.left;

            UpdateHeadSprite();
        }

        if (Input.GetKeyDown(KeyCode.D) &&
            direction != Vector2Int.left)
        {
            direction = Vector2Int.right;

            UpdateHeadSprite();
        }
    }

    private void Move()
    {
        previousPositions.Clear();

<<<<<<< HEAD
        previousPositions.Add(currentGridPosition);
=======
        previousPositions.Add(
            currentGridPosition
        );
>>>>>>> 8444d17 (Snake movement improved)

        foreach (SnakeSegment segment in bodySegments)
        {
            previousPositions.Add(
                segment.gridPosition
            );
        }

        if (tailSegment != null)
        {
            previousPositions.Add(
                tailSegment.gridPosition
            );
        }

        currentGridPosition += direction;

<<<<<<< HEAD
        if (!GridManager.Instance.IsInsideGrid(currentGridPosition))
=======
        if (!GridManager.Instance.IsInsideGrid(
            currentGridPosition))
>>>>>>> 8444d17 (Snake movement improved)
        {
            Die();
            return;
        }

        transform.position =
            GridManager.Instance.GridToWorldPosition(
                currentGridPosition
            );

<<<<<<< HEAD
        for (int i = 0; i < bodySegments.Count; i++)
=======
        for (int i = 0;
             i < bodySegments.Count;
             i++)
>>>>>>> 8444d17 (Snake movement improved)
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

<<<<<<< HEAD
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
=======
    private void UpdateHeadSprite()
    {
        if (snakeSprites == null)
            return;

        if (headSpriteRenderer == null)
            return;

        if (direction == Vector2Int.up)
        {
            headSpriteRenderer.sprite =
                snakeSprites.headUp;
        }
        else if (direction == Vector2Int.down)
        {
            headSpriteRenderer.sprite =
                snakeSprites.headDown;
        }
        else if (direction == Vector2Int.left)
        {
            headSpriteRenderer.sprite =
                snakeSprites.headLeft;
        }
        else if (direction == Vector2Int.right)
        {
            headSpriteRenderer.sprite =
                snakeSprites.headRight;
        }
>>>>>>> 8444d17 (Snake movement improved)
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

<<<<<<< HEAD
        for (int i = 0; i < startingBodyParts; i++)
=======
        for (int i = 0;
             i < startingBodyParts;
             i++)
>>>>>>> 8444d17 (Snake movement improved)
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