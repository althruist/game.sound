using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public LevelData level;

    [Header("Grid")]
    public int width;
    public int height;
    public float spacing = 1.1f;
    public Cell cellPrefab;
    public Dictionary<Vector2Int, Cell> cells = new();

    void Start()
    {
        width = level.gridWidth;
        height = level.gridHeight;

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

                Cell cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                cell.SetCoord(coord);
                cell.SetLevel(level);
                cell.SetCellType(level.cellTypes[index]);

                cells[coord] = cell;
            }
    }
}