using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource source;

    public AudioClip eat;
    public AudioClip gameOver;
    public AudioClip button;

    private bool muted;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayEat()
    {
        if (!muted) source.PlayOneShot(eat);
    }

    public void PlayGameOver()
    {
        if (!muted) source.PlayOneShot(gameOver);
    }

    public void PlayButton()
    {
        if (!muted) source.PlayOneShot(button);
    }

    public void ToggleMute()
    {
        muted = !muted;
        source.mute = muted;
    }

    public bool IsMuted() => muted;
}