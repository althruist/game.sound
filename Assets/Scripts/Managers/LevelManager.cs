using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private LevelData level;
    [SerializeField] public List<LevelData> levels;
    public static LevelManager Instance;
    [SerializeField] private Camera cam;

    [Header("Grid")]
    private int width;
    private int height;
    private float spacing;
    public Cell cellPrefab;
    public Cell musicCellPrefab;
    public Cell sentenceCellPrefab;
    public Dictionary<Vector2Int, Cell> cells = new();
    public GameObject background;
    public TextMeshPro subtitles;

    [Header("Cell Prefabs")]
    [SerializeField] private List<CellType> prefabTypes;
    [SerializeField] private List<Cell> prefabList;
    private Dictionary<CellType, Cell> prefabMap;

    private void Awake()
    {
        if (!cam) cam = Camera.main;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        prefabMap = new Dictionary<CellType, Cell>();
        for (int i = 0; i < Mathf.Min(prefabTypes.Count, prefabList.Count); i++)
        {
            prefabMap[prefabTypes[i]] = prefabList[i];
        }
    }

    void Start()
    {
        subtitles.rectTransform.localPosition = GameManager.Instance.currentLevel.SentenceTextSettings.position;
        subtitles.rectTransform.sizeDelta = GameManager.Instance.currentLevel.SentenceTextSettings.size;
        subtitles.fontSize = GameManager.Instance.currentLevel.SentenceTextSettings.fontSize;

        if (GameManager.Instance != null && GameManager.Instance.currentLevel != null)
        {
            level = GameManager.Instance.currentLevel;
        }

        if (level.GameDifficulty == LevelData.Difficulty.Easy)
        {
            Instance.levels = GameManager.Instance.easyLevels;
        }
        else if (level.GameDifficulty == LevelData.Difficulty.Medium)
        {
            Instance.levels = GameManager.Instance.mediumLevels;
        }
        else if (level.GameDifficulty == LevelData.Difficulty.Hard)
        {
            Instance.levels = GameManager.Instance.hardLevels;
        }

        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            return;
        }
        
        cam.GetComponent<PixelPerfectCamera>().assetsPPU = level.CameraSize;
        width = level.GridSize;
        height = level.GridSize;
        spacing = level.GridSpacing;

        background.GetComponent<SpriteRenderer>().color = level.BackgroundColor;

        SpawnCentered();
    }

    void SpawnCentered()
    {
        cells.Clear();

        float offsetX = (width - 1) * spacing * 0.5f;
        float offsetY = (height - 1) * spacing * 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int coord = new(x, y);
                int flippedY = height - 1 - y;
                int index = flippedY * width + x;

                Vector3 pos = new Vector3(
                    x * spacing - offsetX,
                    y * spacing - offsetY,
                    0f
                );

                LevelData.CellData cellData = level.CellTypes[index];
                CellType type = cellData.type;

                Cell prefabToSpawn;
                if (!prefabMap.TryGetValue(type, out prefabToSpawn))
                {
                    prefabToSpawn = cellPrefab;
                }

                Cell cell = Instantiate(prefabToSpawn, pos, Quaternion.identity, transform);
                cell.SetCoord(coord);
                cell.SetLevel(level);
                cell.SetCellType(type);
                cell.SetIndex(cellData.dataIndex);

                if (cell is SentenceCell sentenceCell && type == CellType.Sentence)
                {
                    sentenceCell.SetData(cellData.sentenceData);
                }
                else if (cell is MusicCell musicCell && type == CellType.Note)
                {
                    cellData.noteData.soundType = level.SoundType;
                    musicCell.SetData(cellData.noteData);
                }

                cells[coord] = cell;
            }
        }
    }
}