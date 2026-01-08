using UnityEngine;

public class Intro : MonoBehaviour
{
    private AudioSource audioSource;
    private CinematicLetterbox letterbox;
    [SerializeField] AudioClip althruistClip;
    [SerializeField] AudioClip titleClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        letterbox = FindFirstObjectByType<CinematicLetterbox>();
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
        letterbox.duration = 0f;
        letterbox.distance = 10f;
        letterbox.active = true;
    }
}
