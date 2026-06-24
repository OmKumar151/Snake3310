using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }

    [Header("State")]
    public GameState state;

    [Header("References")]
    public SnakeController snake;
    public Apple applePrefab;

    private Apple currentApple;

    [Header("UI - Panels")]
    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
    public GameObject hudPanel;

    [Header("UI - Text")]
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;
    public TMP_Text menuHighScoreText;

    private int score;
    private int highScore;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        ShowMainMenu();
    }

    private void Update()
    {
        if (state == GameState.Playing)
        {
            CheckAppleCollision();
        }
    }

    // ---------------- STATE CONTROL ----------------

    public void StartGame()
    {
        state = GameState.Playing;

        score = 0;
        UpdateScoreUI();

        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        hudPanel.SetActive(true);

        snake.ResetSnake();

        SpawnApple();
    }

    public void ShowMainMenu()
    {
        state = GameState.MainMenu;

        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        hudPanel.SetActive(false);

        menuHighScoreText.text = "High Score: " + highScore;
    }

    public void GameOver()
    {
        state = GameState.GameOver;

        AudioManager.Instance.PlayGameOver();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        gameOverPanel.SetActive(true);
        hudPanel.SetActive(false);

        finalScoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + highScore;
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void GoToMainMenu()
    {
        ShowMainMenu();
    }

    // ---------------- GAMEPLAY ----------------

    private void CheckAppleCollision()
    {
        if (currentApple == null)
            return;

        if (snake.GetHeadPosition() == currentApple.GridPosition)
        {
            score++;

            AudioManager.Instance.PlayEat();

            snake.Grow();

            Destroy(currentApple.gameObject);
            currentApple = null;

            SpawnApple();
            UpdateScoreUI();
        }
    }

    private void SpawnApple()
    {
        List<Vector2Int> freeCells = new List<Vector2Int>();

        for (int x = 1; x < Board.Instance.width - 1; x++)
        {
            for (int y = 1; y < Board.Instance.height - 1; y++)
            {
                freeCells.Add(new Vector2Int(x, y));
            }
        }

        foreach (Vector2Int pos in snake.GetSnakePositions())
        {
            freeCells.Remove(pos);
        }

        Vector2Int spawn = freeCells[Random.Range(0, freeCells.Count)];

        currentApple = Instantiate(
            applePrefab,
            Board.Instance.GridToWorld(spawn),
            Quaternion.identity
        );

        currentApple.SetGridPosition(spawn);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore() => score;
}