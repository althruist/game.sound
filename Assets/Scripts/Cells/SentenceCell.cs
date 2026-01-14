using UnityEngine;

public class SentenceCell : Cell
{

    [SerializeField] string syllable;

    private LevelData.SentenceData data;

    public void SetData(LevelData.SentenceData sentenceData)
    {
        data = sentenceData;
    }

    public override void Activate()
    {
        Debug.Log(data.text);
        AudioManager.Instance.Play(data.clip, SoundType.Sentence);
    }
}