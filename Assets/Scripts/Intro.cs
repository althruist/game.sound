using UnityEngine;

public class Intro : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip althruistClip;
    [SerializeField] AudioClip titleClip;

    void Start()
    {
        CinematicLetterbox.Instance.duration = 0f;
        CinematicLetterbox.Instance.active = false;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAlthruist()
    {
        audioSource.PlayOneShot(althruistClip);
    }

    public void PlayTitle()
    {
        audioSource.PlayOneShot(titleClip);
    }

    public void BlackScreen()
    {
        CinematicLetterbox.Instance.TopBorder.GetComponent<SpriteRenderer>().color = Color.black;
        CinematicLetterbox.Instance.BottomBorder.GetComponent<SpriteRenderer>().color = Color.black;
        CinematicLetterbox.Instance.duration = 0f;
        CinematicLetterbox.Instance.distance = 10f;
        CinematicLetterbox.Instance.active = true;
    }
}
