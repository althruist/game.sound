public class SentenceCell : Cell
{
    private LevelData.SentenceData data;

    public void SetData(LevelData.SentenceData sentenceData)
    {
        data = sentenceData;
        text.SetText(data.text);
        text.faceColor = level.sentenceColor;
    }

    public override void Activate()
    {
        AudioManager.Instance.Play(data.clip, SoundType.Sentence);
    }
}