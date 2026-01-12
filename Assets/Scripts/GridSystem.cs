using System.Collections.Generic;
using UnityEditor.UI;
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
    Cell selectedPrefab = null;
    public Dictionary<Vector2Int, Cell> cells = new();

    void Start()
    {
        width = level.gridWidth;
        height = level.gridHeight;
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

                if (level.cellTypes[index] == CellType.Normal)
                {
                    selectedPrefab = cellPrefab;
                }
                else if (level.cellTypes[index] == CellType.Sentence)
                {
                    selectedPrefab = sentenceCellPrefab;
                }
                else if (level.cellTypes[index] == CellType.Note)
                {
                    selectedPrefab = musicCellPrefab;
                }

                Debug.Log(selectedPrefab);
                Cell cell = Instantiate(selectedPrefab, pos, Quaternion.identity, transform);
                cell.SetCoord(coord);
                cell.SetLevel(level);
                cell.SetCellType(level.cellTypes[index]);

                cells[coord] = cell;
            }
    }
}