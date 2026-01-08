using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [Header("Grid")]
    public int width = 6;
    public int height = 6;

    [Tooltip("Distance between cell centers in world units.")]
    public float spacing = 1.1f;

    public Cell cellPrefab;

    // Optional: quick lookup if you want it later
    public Dictionary<Vector2Int, Cell> cells = new();

    void Start()
    {
        SpawnCentered();
    }

    void SpawnCentered()
    {
        cells.Clear();

        // Center the grid around (0,0)
        float offsetX = (width - 1) * spacing * 0.5f;
        float offsetY = (height - 1) * spacing * 0.5f;

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            Vector2Int coord = new(x, y);

            Vector3 pos = new Vector3(
                x * spacing - offsetX,
                y * spacing - offsetY,
                0f
            );

            Cell cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
            cell.coord = coord;

            cells[coord] = cell;
        }
    }
}
