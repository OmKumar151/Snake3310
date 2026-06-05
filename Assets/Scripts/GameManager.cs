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
        List<Vector2Int> freeCells =
            GridManager.Instance.GetAllCells();

        freeCells.Remove(
            snake.GetHeadPosition()
        );

        foreach (SnakeSegment segment in snake.GetBodySegments())
        {
            freeCells.Remove(
                segment.gridPosition
            );
        }

        if (freeCells.Count == 0)
        {
            Debug.Log("No free cells remaining.");
            return;
        }

        Vector2Int spawnCell =
            freeCells[
                Random.Range(
                    0,
                    freeCells.Count
                )
            ];

        currentApple = Instantiate(
            applePrefab,
            GridManager.Instance.GridToWorldPosition(
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