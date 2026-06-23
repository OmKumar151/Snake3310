using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public SnakeController snake;
    public Apple applePrefab;

    private Apple currentApple;

    [Header("UI")]
    public TMP_Text scoreText;

    private int score;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Board.Instance == null)
        {
            Debug.LogError(
                "Board.Instance is NULL. Attach Board.cs to the Board object."
            );
            return;
        }

        SpawnApple();

        UpdateScoreUI();
    }

    private void Update()
    {
        CheckAppleCollision();
    }

    private void CheckAppleCollision()
    {
        if (currentApple == null)
            return;

        if (snake.GetHeadPosition() ==
            currentApple.GridPosition)
        {
            score++;

            snake.Grow();

            Destroy(currentApple.gameObject);

            currentApple = null;

            SpawnApple();

            UpdateScoreUI();
        }
    }

    private void SpawnApple()
    {
        if (Board.Instance == null)
            return;

        List<Vector2Int> freeCells =
            new List<Vector2Int>();

        for (int x = 1; x < Board.Instance.width - 1; x++)
        {
            for (int y = 1; y < Board.Instance.height - 1; y++)
            {
                freeCells.Add(new Vector2Int(x, y));
            }
        }

        foreach (Vector2Int position in
                 snake.GetSnakePositions())
        {
            freeCells.Remove(position);
        }

        if (freeCells.Count == 0)
        {
            Debug.Log(
                "No free cells remaining."
            );
            return;
        }

        Vector2Int spawnCell =
            freeCells[
                Random.Range(
                    0,
                    freeCells.Count
                )
            ];

        currentApple =
            Instantiate(
                applePrefab,
                Board.Instance.GridToWorld(
                    spawnCell
                ),
                Quaternion.identity
            );

        currentApple.SetGridPosition(
            spawnCell
        );

        Debug.Log(
            "Apple spawned at: " +
            spawnCell
        );
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text =
                "Score: " + score;
        }
    }

    public int GetScore()
    {
        return score;
    }
}