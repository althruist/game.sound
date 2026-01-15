using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public enum LevelType
    {
        Sentence,
        Note
    };

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    };

    [System.Serializable]
    public struct SentenceData
    {
        public string text;
        public AudioClip clip;
    }

    [System.Serializable]
    public struct NoteData
    {
        public SoundType soundType;
        public AudioClip clip;
    }

    [System.Serializable]
    public struct CellData
    {
        public CellType type;
        public int dataIndex;
        public SentenceData sentenceData;
        public NoteData noteData;
    }

    public LevelType levelType;
    public Difficulty difficulty;
    public int gridSize;
    public float gridSpacing = 1.1f;
    public List<CellData> cellTypes;

    public Color headColor;
    public Color bodyColor;
    public Color tileColor;
    public Color noteColor;
    public Color sentenceColor;
    public Color backgroundColor;

    private void OnValidate()
    {
        if (gridSize < 1) gridSize = 1;

        int required = gridSize * gridSize;

        if (cellTypes == null) cellTypes = new List<CellData>(required);

        while (cellTypes.Count < required)
            cellTypes.Add(new CellData { type = CellType.Normal, dataIndex = -1 });

        if (cellTypes.Count > required)
            cellTypes.RemoveRange(required, cellTypes.Count - required);
    }
}