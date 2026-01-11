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

    public LevelType levelType;
    public Difficulty difficulty;
    public int gridWidth, gridHeight;
    public List<CellType> cellTypes;

    public Color headColor;
    public Color bodyColor;
    public Color tileColor;
    public Color noteColor;
    public Color sentenceColor;

    void OnValidate()
    {
        if (gridWidth < 1) gridWidth = 1;
        if (gridHeight < 1) gridHeight = 1;

        int required = gridWidth * gridHeight;

        if (cellTypes == null) cellTypes = new List<CellType>(required);

        while (cellTypes.Count < required)
            cellTypes.Add(CellType.Normal);

        if (cellTypes.Count > required)
            cellTypes.RemoveRange(required, cellTypes.Count - required);
    }
}
