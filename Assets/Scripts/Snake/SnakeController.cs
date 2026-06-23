using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("Movement")]
    public float moveDelay = 0.2f;

    private float moveTimer;

    private Vector2Int direction = Vector2Int.right;
    private Vector2Int nextDirection;

    [Header("Starting Position")]
    public Vector2Int startPosition = new Vector2Int(9, 9);

    [Header("References")]
    public Transform headTransform;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;

    [Header("Sprites")]
    public SnakeSprites snakeSprites;

    private List<Vector2Int> snakePositions = new List<Vector2Int>();
    private List<GameObject> snakeObjects = new List<GameObject>();

    private bool isDead = false;

    private void Start()
    {
        if (Board.Instance == null)
        {
            Debug.LogError("Board.Instance is NULL.");
            return;
        }

        nextDirection = direction;

        CreateSnake();
        UpdateVisuals();
    }

    private void Update()
    {
        if (isDead) return;

        HandleInput();

        moveTimer += Time.deltaTime;

        if (moveTimer >= moveDelay)
        {
            moveTimer = 0f;
            Move();
            UpdateVisuals();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2Int.down)
            nextDirection = Vector2Int.up;

        if (Input.GetKeyDown(KeyCode.S) && direction != Vector2Int.up)
            nextDirection = Vector2Int.down;

        if (Input.GetKeyDown(KeyCode.A) && direction != Vector2Int.right)
            nextDirection = Vector2Int.left;

        if (Input.GetKeyDown(KeyCode.D) && direction != Vector2Int.left)
            nextDirection = Vector2Int.right;
    }

    private void Move()
    {
        direction = nextDirection;

        Vector2Int newHead = snakePositions[0] + direction;

        // ❌ WALL COLLISION
        if (!Board.Instance.IsInside(newHead))
        {
            GameOver();
            return;
        }

        // ❌ SELF COLLISION
        if (IsCollidingWithSelf(newHead))
        {
            GameOver();
            return;
        }

        snakePositions.Insert(0, newHead);
        snakePositions.RemoveAt(snakePositions.Count - 1);
    }

    private bool IsCollidingWithSelf(Vector2Int position)
    {
        for (int i = 0; i < snakePositions.Count; i++)
        {
            if (snakePositions[i] == position)
                return true;
        }

        return false;
    }

    private void GameOver()
    {
        isDead = true;
        Debug.Log("GAME OVER");

        // OPTIONAL CLEANUP HOOK (you can extend later)
        // GameManager.Instance.OnGameOver();
    }

    private void CreateSnake()
    {
        snakePositions.Clear();

        snakePositions.Add(startPosition);
        snakePositions.Add(startPosition + Vector2Int.left);
        snakePositions.Add(startPosition + Vector2Int.left * 2);

        foreach (GameObject obj in snakeObjects)
        {
            Destroy(obj);
        }

        snakeObjects.Clear();

        for (int i = 1; i < snakePositions.Count; i++)
        {
            GameObject segment =
                Instantiate(
                    bodyPrefab,
                    Board.Instance.GridToWorld(snakePositions[i]),
                    Quaternion.identity
                );

            snakeObjects.Add(segment);
        }

        headTransform.position =
            Board.Instance.GridToWorld(snakePositions[0]);
    }

    public void Grow()
    {
        Vector2Int tail = snakePositions[snakePositions.Count - 1];

        snakePositions.Add(tail);

        GameObject segment =
            Instantiate(
                bodyPrefab,
                Board.Instance.GridToWorld(tail),
                Quaternion.identity
            );

        snakeObjects.Add(segment);
    }

    private void UpdateVisuals()
    {
        headTransform.position =
            Board.Instance.GridToWorld(snakePositions[0]);

        UpdateHeadSprite();

        for (int i = 1; i < snakePositions.Count; i++)
        {
            Vector2Int curr = snakePositions[i];

            GameObject obj = snakeObjects[i - 1];
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

            obj.transform.position =
                Board.Instance.GridToWorld(curr);

            if (i == snakePositions.Count - 1)
            {
                Vector2Int dir = snakePositions[i - 1] - curr;
                UpdateTailSprite(sr, dir);
                continue;
            }

            Vector2Int prev = snakePositions[i - 1];
            Vector2Int next = snakePositions[i + 1];

            UpdateBodySprite(sr, curr - prev, curr - next);
        }
    }

    private void UpdateHeadSprite()
    {
        SpriteRenderer sr = headTransform.GetComponent<SpriteRenderer>();

        if (direction == Vector2Int.up)
            sr.sprite = snakeSprites.headUp;
        else if (direction == Vector2Int.down)
            sr.sprite = snakeSprites.headDown;
        else if (direction == Vector2Int.left)
            sr.sprite = snakeSprites.headLeft;
        else if (direction == Vector2Int.right)
            sr.sprite = snakeSprites.headRight;
    }

    private void UpdateBodySprite(SpriteRenderer sr, Vector2Int fromPrev, Vector2Int toNext)
    {
        if (fromPrev.x == toNext.x)
        {
            sr.sprite = snakeSprites.bodyVertical;
            return;
        }

        if (fromPrev.y == toNext.y)
        {
            sr.sprite = snakeSprites.bodyHorizontal;
            return;
        }

        if (fromPrev == Vector2Int.up && toNext == Vector2Int.right ||
            fromPrev == Vector2Int.right && toNext == Vector2Int.up)
        {
            sr.sprite = snakeSprites.cornerTopRight;
            return;
        }

        if (fromPrev == Vector2Int.up && toNext == Vector2Int.left ||
            fromPrev == Vector2Int.left && toNext == Vector2Int.up)
        {
            sr.sprite = snakeSprites.cornerTopLeft;
            return;
        }

        if (fromPrev == Vector2Int.down && toNext == Vector2Int.right ||
            fromPrev == Vector2Int.right && toNext == Vector2Int.down)
        {
            sr.sprite = snakeSprites.cornerBottomRight;
            return;
        }

        if (fromPrev == Vector2Int.down && toNext == Vector2Int.left ||
            fromPrev == Vector2Int.left && toNext == Vector2Int.down)
        {
            sr.sprite = snakeSprites.cornerBottomLeft;
            return;
        }
    }

    private void UpdateTailSprite(SpriteRenderer sr, Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            sr.sprite = snakeSprites.tailDown;
        else if (dir == Vector2Int.down)
            sr.sprite = snakeSprites.tailUp;
        else if (dir == Vector2Int.left)
            sr.sprite = snakeSprites.tailRight;
        else if (dir == Vector2Int.right)
            sr.sprite = snakeSprites.tailLeft;
    }

    public Vector2Int GetHeadPosition()
    {
        return snakePositions[0];
    }

    public List<Vector2Int> GetSnakePositions()
    {
        return snakePositions;
    }
}