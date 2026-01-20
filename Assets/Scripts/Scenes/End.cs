using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class End : MonoBehaviour
{
    public TextMeshPro subtitles;
    public Button retryButton;
    public AudioClip retrySound;
    public VideoPlayer videoPlayer;

    public Color transitionColor;

    private bool breakSong;

    void Start()
    {
        CinematicLetterbox.Instance.duration = 1f;
        CinematicLetterbox.Instance.distance = 2f;
        CinematicLetterbox.Instance.active = true;
        StartCoroutine(RepeatLoop(SoundType.Bass, AudioManager.Instance.bass));
        StartCoroutine(RepeatLoop(SoundType.Melody, AudioManager.Instance.melody));
        StartCoroutine(RepeatLoop(SoundType.Chord, AudioManager.Instance.chord));
        StartCoroutine(PlayVideo());
        StartCoroutine(PlayEndingDialogue());
        retryButton.onClick.AddListener(OnRetryClicked);
        GameData.songsCompleted++;
        SaveLoadManager.Instance.SaveData();
    }
    IEnumerator PlayEndingDialogue()
    {
        yield return new WaitForSeconds(2f);
        foreach (LevelData.SentenceData data in AudioManager.Instance.endingDialogue)
        {
            AudioSource src = AudioManager.Instance.Play(data.clip, SoundType.SFX);
            subtitles.SetText(data.text);
            src.volume = 1;
            src.pitch = 1;
            yield return new WaitForSeconds(data.clip.length * 1.5f);
        }
        yield return new WaitForSeconds(2f);
        subtitles.SetText("");
        retryButton.gameObject.SetActive(true);
        retryButton.GetComponent<Animator>().Play("ButtonSpawn");
    }

    IEnumerator PlayVideo()
    {
        yield return new WaitForSeconds(1.5f);
        videoPlayer.Play();
    }

    public void OnRetryClicked()
    {
        UIManager.Instance.ClickAnimationSolo(retryButton);
        StartCoroutine(Retry());
    }

    IEnumerator Retry()
    {
        CinematicLetterbox.Instance.active = false;
        yield return new WaitForSeconds(0.1f);
        CinematicLetterbox.Instance.distance = 20f;
        CinematicLetterbox.Instance.easeType = DG.Tweening.Ease.Linear;
        CinematicLetterbox.Instance.active = true;
        retryButton.gameObject.SetActive(false);
        AudioManager.Instance.Play(retrySound, SoundType.SFX);
        breakSong = true;

        float timer = 0f;

        while (timer < 2f)
        {
            timer += Time.deltaTime;
            CinematicLetterbox.Instance.TopBorder.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, transitionColor, timer / 2f);
            CinematicLetterbox.Instance.BottomBorder.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, transitionColor, timer / 2f);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        GameManager.Instance.LoadLevel("IntroScene");
    }

    IEnumerator RepeatLoop(SoundType soundType, List<LevelData.NoteData> audioClips)
    {
        while (!breakSong)
        {
            if (audioClips.Count == 0)
            {
                break;
            }
            foreach (var data in audioClips)
            {
                AudioSource src = AudioManager.Instance.Play(data.clip, soundType);
                if (src != null)
                {
                    AudioManager.Instance.StartFade(src, 0f, 1f, 30f);

                }

                yield return new WaitForSeconds(60f / (GameData.songTempo * 2));
            }
        }
    }

}
