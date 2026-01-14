using System.Data.SqlTypes;
using UnityEngine;

public class MusicCell : Cell
{
    public enum Note { C, D, E, F, G, A, B };
    private SoundType soundType;

    public void SetSoundType(SoundType sType)
    {
        soundType = sType;
    }

    public override void Activate()
    {
        Debug.Log(soundType);
    }
}
