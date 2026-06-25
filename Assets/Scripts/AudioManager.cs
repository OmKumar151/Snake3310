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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        muted =
            PlayerPrefs.GetInt("Muted", 0) == 1;


        source.mute = muted;
    }


    public void PlayEat()
    {
        if (!muted)
            source.PlayOneShot(eat);
    }


    public void PlayGameOver()
    {
        if (!muted)
            source.PlayOneShot(gameOver);
    }


    public void PlayButton()
    {
        if (!muted)
            source.PlayOneShot(button);
    }


    public void ToggleMute()
    {
        muted = !muted;

        source.mute = muted;


        PlayerPrefs.SetInt(
            "Muted",
            muted ? 1 : 0
        );

        PlayerPrefs.Save();
    }


    public bool IsMuted()
    {
        return muted;
    }
}