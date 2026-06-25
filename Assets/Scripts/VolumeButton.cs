using UnityEngine;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
    public Sprite mutedSprite;
    public Sprite unmutedSprite;

    public Image image;


    private void Start()
    {
        UpdateIcon();
    }


    public void Toggle()
    {
        AudioManager.Instance.ToggleMute();

        UpdateIcon();
    }


    public void UpdateIcon()
    {
        image.sprite =
            AudioManager.Instance.IsMuted()
            ? mutedSprite
            : unmutedSprite;
    }
}