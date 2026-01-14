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
    }

    public static AudioManager Instance;
    public List<SoundChannel> channels;
    public AudioSource audioSourcePrefab;
    private Dictionary<SoundType, List<AudioSource>> activeSources = new();

    void Awake()
    {
        Instance = this;

        foreach (var channel in channels)
            activeSources[channel.type] = new List<AudioSource>();
    }

    public void Play(AudioClip clip, SoundType type)
    {
        if (!clip) return;

        SoundChannel channel = channels.Find(c => c.type == type);
        var sources = activeSources[type];

        sources.RemoveAll(s => !s.isPlaying);

        if (sources.Count >= channel.maxSimultaneous)
        {
            if (channel.interruptOldest)
            {
                sources[0].Stop();
                sources.RemoveAt(0);
            }
            else
            {
                return;
            }
        }

        var src = GetComponent<AudioSource>();
        src.clip = clip;
        src.volume = channel.volume;
        src.Play();

        sources.Add(src);
    }
}