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
    public Vector2Int startPosition =
        new Vector2Int(9, 9);


    [Header("References")]
    public Transform headTransform;

    public GameObject bodyPrefab;

    public GameObject tailPrefab;


    [Header("Sprites")]
    public SnakeSprites snakeSprites;


    private List<Vector2Int> snakePositions =
        new List<Vector2Int>();


    private List<GameObject> snakeObjects =
        new List<GameObject>();


    private void Start()
    {
        CreateSnake();

        UpdateVisuals();
    }


    private void Update()
    {
        HandleInput();


        moveTimer += Time.deltaTime;


        if (moveTimer >= moveDelay)
        {
            moveTimer = 0;

            Move();
        }
    }



    private void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.W)
        && direction != Vector2Int.down)
        {
            nextDirection = Vector2Int.up;
        }


        if (Input.GetKeyDown(KeyCode.S)
        && direction != Vector2Int.up)
        {
            nextDirection = Vector2Int.down;
        }


        if (Input.GetKeyDown(KeyCode.A)
        && direction != Vector2Int.right)
        {
            nextDirection = Vector2Int.left;
        }


        if (Input.GetKeyDown(KeyCode.D)
        && direction != Vector2Int.left)
        {
            nextDirection = Vector2Int.right;
        }

    }



    private void Move()
    {

        direction = nextDirection;


        Vector2Int newHead =
            snakePositions[0] + direction;



        if (!GridManager.Instance.IsInsideGrid(newHead))
        {
            GameOver();
            return;
        }


        snakePositions.Insert(0, newHead);



        snakePositions.RemoveAt(
            snakePositions.Count - 1
        );


        UpdateVisuals();

    }



    private void CreateSnake()
    {

        snakePositions.Clear();


        snakePositions.Add(startPosition);

        snakePositions.Add(
            startPosition + Vector2Int.left
        );


        snakePositions.Add(
            startPosition +
            Vector2Int.left * 2
        );



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
                    GridManager.Instance
                    .GridToWorldPosition(
                    snakePositions[i]),
                    Quaternion.identity
                );


            snakeObjects.Add(segment);

        }


        headTransform.position =
            GridManager.Instance
            .GridToWorldPosition(
            snakePositions[0]);

    }



    public void Grow()
    {

        snakePositions.Add(
            snakePositions[
            snakePositions.Count - 1]
        );


        GameObject segment =
            Instantiate(
            bodyPrefab,
            GridManager.Instance
            .GridToWorldPosition(
            snakePositions[
            snakePositions.Count - 1]),
            Quaternion.identity);


        snakeObjects.Add(segment);


    }



    private void UpdateVisuals()
    {


        headTransform.position =
        GridManager.Instance
        .GridToWorldPosition(
        snakePositions[0]);



        UpdateHeadSprite();



        for (int i = 1; i < snakePositions.Count; i++)
        {

            snakeObjects[i - 1].transform.position =
            GridManager.Instance
            .GridToWorldPosition(
            snakePositions[i]);


            SpriteRenderer sr =
            snakeObjects[i - 1]
            .GetComponent<SpriteRenderer>();


            sr.sprite =
            snakeSprites.bodyHorizontal;

        }

    }



    private void UpdateHeadSprite()
    {

        SpriteRenderer sr =
        headTransform
        .GetComponent<SpriteRenderer>();


        if (direction == Vector2Int.up)
            sr.sprite = snakeSprites.headUp;


        if (direction == Vector2Int.down)
            sr.sprite = snakeSprites.headDown;


        if (direction == Vector2Int.left)
            sr.sprite = snakeSprites.headLeft;


        if (direction == Vector2Int.right)
            sr.sprite = snakeSprites.headRight;

    }



    public Vector2Int GetHeadPosition()
    {
        return snakePositions[0];
    }



    public List<Vector2Int> GetSnakePositions()
    {
        return snakePositions;
    }



    private void GameOver()
    {
        Debug.Log("GAME OVER");
    }

}