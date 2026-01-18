using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    public List<SoundClips> soundClips;
}

[System.Serializable]
public struct SoundClips
{
    public SoundType type;
    public AudioClip clip;
}