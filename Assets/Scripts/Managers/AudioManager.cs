using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundChannel
    {
        public SoundType type;
        public int maxSimultaneous = 1;
        public bool interruptOldest = true;
        public float volume = 1f;
        [HideInInspector] public List<AudioSource> sources;
    }
    public AudioClip travelLevelSFX;
    public AudioClip travelLevelFinalSFX;
    public static AudioManager Instance;
    public List<SoundChannel> channels;

    public List<LevelData.NoteData> bass = new();
    public List<LevelData.NoteData> melody = new();
    public List<LevelData.NoteData> chord = new();

    public List<LevelData.SentenceData> endingDialogue;

    public void AddNote(LevelData.NoteData note)
    {
        switch (note.soundType)
        {
            case SoundType.Bass:
                bass.Add(note);
                break;
            case SoundType.Melody:
                melody.Add(note);
                break;
            case SoundType.Chord:
                chord.Add(note);
                break;
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var channel in channels)
        {
            channel.sources = new List<AudioSource>();
            for (int i = 0; i < channel.maxSimultaneous; i++)
            {
                AudioSource src = gameObject.AddComponent<AudioSource>();
                src.volume = channel.volume;
                channel.sources.Add(src);
            }
        }
    }

    public AudioSource Play(AudioClip clip, SoundType type)
    {
        if (!clip) return null;

        SoundChannel channel = channels.Find(c => c.type == type);
        if (channel == null) return null;

        AudioSource src = channel.sources.Find(s => !s.isPlaying);

        if (src == null)
        {
            if (channel.interruptOldest)
            {
                src = channel.sources[0];
                src.Stop();
            }
            else
            {
                return null;
            }
        }
        src.clip = clip;
        src.volume = channel.volume;
        src.Play();

        return src;
    }

    private Dictionary<AudioSource, Coroutine> fadeCoroutines = new();

    public void StartFade(AudioSource src, float startVolume, float targetVolume, float duration)
    {
        if (fadeCoroutines.ContainsKey(src) && fadeCoroutines[src] != null)
        {
            return;
        }

        fadeCoroutines[src] = StartCoroutine(FadeIn(src, startVolume, targetVolume, duration));
    }

    public IEnumerator FadeIn(AudioSource src, float startVolume, float targetVolume, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            src.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }

        src.volume = targetVolume;
    }
}