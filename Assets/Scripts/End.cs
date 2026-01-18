using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class End : MonoBehaviour
{
    public CinematicLetterbox letterbox;
    public TextMeshPro subtitles;

    void Start()
    {
        letterbox.duration = 1f;
        letterbox.distance = 2f;
        letterbox.active = true;
        StartCoroutine(RepeatLoop(SoundType.Bass, AudioManager.Instance.bass));
        StartCoroutine(RepeatLoop(SoundType.Melody, AudioManager.Instance.melody));
        StartCoroutine(RepeatLoop(SoundType.Chord, AudioManager.Instance.chord));
    }


    IEnumerator RepeatLoop(SoundType soundType, List<LevelData.NoteData> audioClips)
    {
        while (true)
        {
            foreach (var data in audioClips)
            {
                AudioSource src = AudioManager.Instance.Play(data.clip, soundType);

                if (src != null)
                {
                    AudioManager.Instance.StartFade(src, 0f, 1f, 10f);

                }

                yield return new WaitForSeconds(60f / (GameManager.Instance.gameData.songTempo * 2));
            }
        }
    }

}
