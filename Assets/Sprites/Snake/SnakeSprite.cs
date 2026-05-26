using UnityEngine;

[CreateAssetMenu(menuName = "Snake/Snake Sprites")]
public class SnakeSprites : ScriptableObject
{
    [Header("Head")]
    public Sprite headUp;
    public Sprite headDown;
    public Sprite headLeft;
    public Sprite headRight;

    [Header("Body")]
    public Sprite bodyHorizontal;
    public Sprite bodyVertical;

    [Header("Corners")]
    public Sprite cornerTopLeft;
    public Sprite cornerTopRight;
    public Sprite cornerBottomLeft;
    public Sprite cornerBottomRight;

    [Header("Tail")]
    public Sprite tailUp;
    public Sprite tailDown;
    public Sprite tailLeft;
    public Sprite tailRight;
}