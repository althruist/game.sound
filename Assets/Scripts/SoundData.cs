using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    [Header("Bass")]
    public AudioClip bassC;
    public AudioClip bassD;
    public AudioClip bassE;
    public AudioClip bassF;
    public AudioClip bassG;
    public AudioClip bassA;
    public AudioClip bassB;
    [Header("Melody")]
    public AudioClip melodyC;
    public AudioClip melodyD;
    public AudioClip melodyE;
    public AudioClip melodyF;
    public AudioClip melodyG;
    public AudioClip melodyA;
    public AudioClip melodyB;
    [Header("Chord")]
    public AudioClip chordC;
    public AudioClip chordD;
    public AudioClip chordE;
    public AudioClip chordF;
    public AudioClip chordG;
    public AudioClip chordA;
    public AudioClip chordB;
}
