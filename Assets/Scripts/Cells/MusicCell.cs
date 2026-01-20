using System.Collections.Generic;
using UnityEngine;

public class MusicCell : Cell
{
    public enum Note { C, D, E, F, G, A, B };
    private SoundType soundType;
    public LevelData.NoteData data;

    private List<AudioClip> selectedClips = new List<AudioClip>();

    public void SetData(LevelData.NoteData lData)
    {
        selectedClips.Clear();

        data = lData;
        soundType = data.soundType;

        foreach (SoundClips audio in level.SoundData.soundClips)
        {
            if (audio.type == soundType)
            {
                selectedClips.Add(audio.clip);
            }
        }
        data.clip = selectedClips[Random.Range(0, selectedClips.Count - 1)];
    }

    public override bool TryGetNoteData(out LevelData.NoteData sentence)
    {
        sentence = data;
        return true;
    }

    public override void Activate()
    {
        AudioManager.Instance.Play(data.clip, soundType);
    }
}
