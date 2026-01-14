using System;
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
        public float index;
    }

    public LevelType levelType;
    public Difficulty difficulty;
    public int gridSize;
    public float gridSpacing = 1.1f;
    public List<CellType> cellTypes;
    public List<SentenceData> sentence;
    public List<SoundType> soundType;

    public Color headColor;
    public Color bodyColor;
    public Color tileColor;
    public Color noteColor;
    public Color sentenceColor;

    void OnValidate()
    {
        if (gridSize < 1) gridSize = 1;

        int required = gridSize * gridSize;

        if (cellTypes == null) cellTypes = new List<CellType>(required);

        while (cellTypes.Count < required)
            cellTypes.Add(CellType.Normal);

        if (cellTypes.Count > required)
            cellTypes.RemoveRange(required, cellTypes.Count - required);

        int sentenceCellsRequired = 0;
        int musicCellsRequired = 0;

        foreach (var cell in cellTypes)
        {
            if (cell == CellType.Sentence) sentenceCellsRequired++;
            if (cell == CellType.Note) musicCellsRequired++;
        }

        if (sentence == null) sentence = new List<SentenceData>();
        if (soundType == null) soundType = new List<SoundType>();

        while (sentence.Count < sentenceCellsRequired)
            sentence.Add(new SentenceData());
        while (sentence.Count > sentenceCellsRequired)
            sentence.RemoveAt(sentence.Count - 1);
        while (soundType.Count < musicCellsRequired)
            soundType.Add(SoundType.Melody);
        while (soundType.Count > musicCellsRequired)
            soundType.RemoveAt(soundType.Count - 1);
    }

}