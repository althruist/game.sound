using System.Collections;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] AudioClip althruistClip;
    [SerializeField] AudioClip titleClip;

    void Start()
    {
        CinematicLetterbox.Instance.duration = 0f;
        CinematicLetterbox.Instance.active = false;
    }

    public void PlayAlthruist()
    {

        AudioManager.Instance.Play(althruistClip, SoundType.SFX);
    }

    public void PlayTitle()
    {
        AudioManager.Instance.Play(titleClip, SoundType.SFX);
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.LoadLevel("MenuScene");
    }

    public void BlackScreen()
    {
        CinematicLetterbox.Instance.TopBorder.GetComponent<SpriteRenderer>().color = Color.black;
        CinematicLetterbox.Instance.BottomBorder.GetComponent<SpriteRenderer>().color = Color.black;
        CinematicLetterbox.Instance.duration = 0f;
        CinematicLetterbox.Instance.distance = 10f;
        CinematicLetterbox.Instance.active = true;
        StartCoroutine(LoadScene());
    }
}
