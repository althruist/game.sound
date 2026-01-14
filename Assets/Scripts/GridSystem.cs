using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public LevelData level;

    [Header("Grid")]
    private int width;
    private int height;
    private float spacing;
    public Cell cellPrefab;
    public Cell musicCellPrefab;
    public Cell sentenceCellPrefab;
    public Dictionary<Vector2Int, Cell> cells = new();
    int sentenceIndex = 0;
    int musicIndex = 0;


    [Header("Cell Prefabs")]
    [SerializeField] private List<CellType> prefabTypes;
    [SerializeField] private List<Cell> prefabList;
    private Dictionary<CellType, Cell> prefabMap;

    private void Awake()
    {
        prefabMap = new Dictionary<CellType, Cell>();
        for (int i = 0; i < Mathf.Min(prefabTypes.Count, prefabList.Count); i++)
        {
            prefabMap[prefabTypes[i]] = prefabList[i];
        }
    }

    void Start()
    {
        width = level.gridSize;
        height = level.gridSize;
        spacing = level.gridSpacing;

        SpawnCentered();
    }

    void SpawnCentered()
    {
        cells.Clear();

        float offsetX = (width - 1) * spacing * 0.5f;
        float offsetY = (height - 1) * spacing * 0.5f;

        for (int y = 0; y < height; y++)
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

                CellType type = level.cellTypes[index];
                Cell prefabToSpawn;

                if (!prefabMap.TryGetValue(type, out prefabToSpawn))
                {
                    prefabToSpawn = cellPrefab;
                }

                Cell cell = Instantiate(prefabToSpawn, pos, Quaternion.identity, transform);

                cell.SetCoord(coord);
                cell.SetLevel(level);
                cell.SetCellType(level.cellTypes[index]);

                if (cell is SentenceCell sentenceCell)
                {
                    sentenceCell.SetData(level.sentence[sentenceIndex++]);
                }
                else if (cell is MusicCell musicCell)
                {
                    musicCell.SetSoundType(level.soundType[musicIndex++]);
                }


                cells[coord] = cell;
            }
    }
}