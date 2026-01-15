using System.Data.SqlTypes;
using UnityEngine;

public class MusicCell : Cell
{
    public enum Note { C, D, E, F, G, A, B };
    private SoundType soundType;

    private LevelData.NoteData data;

    public void SetData(LevelData.NoteData data)
    {
    }

    public override void Activate()
    {
        Debug.Log(soundType);
    }
}
