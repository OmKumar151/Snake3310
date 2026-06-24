using UnityEngine;

public class SnakeWallCollision : MonoBehaviour
{
    private SnakeController snake;

    private void Start()
    {
        snake = GetComponentInParent<SnakeController>();

        if (snake == null)
        {
            Debug.LogError("SnakeController not found in parent.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only act if you actually tag walls
        if (other.CompareTag("Wall"))
        {
            snake.SendMessage("ForceGameOver", SendMessageOptions.DontRequireReceiver);
        }
    }
}