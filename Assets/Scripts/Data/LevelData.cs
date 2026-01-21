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
        [HideInInspector] public SoundType soundType;
        [HideInInspector] public AudioClip clip;
    }

    [System.Serializable]
    public struct CellData
    {
        public CellType type;
        public int dataIndex;
        public SentenceData sentenceData;
        public NoteData noteData;
    }

    [System.Serializable]
    [SerializeField]
    public struct SentenceText
    {
        public Vector3 position;
        public Vector2 size;
        public float fontSize;
    }

    [SerializeField] private LevelType levelType;
    [SerializeField] private SoundType soundType;
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private int levelIndex;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float gridSpacing = 1.1f;
    [SerializeField] private SoundData soundData;
    [SerializeField] private List<CellData> cellTypes;

    [SerializeField] private Color headColor;
    [SerializeField] private Color bodyColor;
    [SerializeField] private Color tileColor;
    [SerializeField] private Color noteColor;
    [SerializeField] private Color sentenceColor;
    [SerializeField] private Color backgroundColor;
    [SerializeField] private int cameraSize;
    [SerializeField] private SentenceText sentenceTextSettings;

    public LevelType GameLevelType { get; private set; }
    public SoundType SoundType { get; private set; }
    public Difficulty GameDifficulty { get; private set; }
    public int LevelIndex { get; private set; }
    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }
    public float GridSpacing { get; private set; }
    public SoundData SoundData { get; private set; }
    public List<CellData> CellTypes { get; private set; }
    public Color HeadColor { get; private set; }
    public Color BodyColor { get; private set; }
    public Color TileColor { get; private set; }
    public Color NoteColor { get; private set; }
    public Color SentenceColor { get; private set; }
    public Color BackgroundColor { get; private set; }
    public int CameraSize { get; private set; }
    public SentenceText SentenceTextSettings { get; private set; }

    private void OnEnable()
    {
        GameLevelType = levelType;
        SoundType = soundType;
        GameDifficulty = difficulty;
        LevelIndex = levelIndex;
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        GridSpacing = gridSpacing;
        SoundData = soundData;
        CellTypes = cellTypes;

        HeadColor = headColor;
        BodyColor = bodyColor;
        TileColor = tileColor;
        NoteColor = noteColor;
        SentenceColor = sentenceColor;
        BackgroundColor = backgroundColor;
        CameraSize = cameraSize;
        SentenceTextSettings = sentenceTextSettings;
    }

    private void OnValidate()
    {
        if (gridWidth < 1) gridWidth = 1;
        if (gridHeight < 1) gridHeight = 1;

        int required = gridWidth * gridHeight;

        if (cellTypes == null) cellTypes = new List<CellData>(required);

        while (cellTypes.Count < required)
            cellTypes.Add(new CellData { type = CellType.Normal, dataIndex = -1 });

        if (cellTypes.Count > required)
            cellTypes.RemoveRange(required, cellTypes.Count - required);
    }
}