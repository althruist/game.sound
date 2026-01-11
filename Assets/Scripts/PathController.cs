using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public LevelData level;

    private void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            DetectCell("DOWN");

        if (Input.GetMouseButton(0))
            DetectCell("HOLD");

        if (Input.GetMouseButtonUp(0))
            DetectCell("UP");
    }

    bool IsOrthogonalNeighbor(Cell a, Cell b)
    {
        Vector2Int delta = a.coord - b.coord;
        return Mathf.Abs(delta.x) + Mathf.Abs(delta.y) == 1;
    }


    private Cell oldCell = null;
    private Cell currentCell = null;
    private bool isHeld;
    List<Cell> path = new List<Cell>();
    void DetectCell(string phase)
    {
        if (phase == "UP")
        {
            isHeld = false;
            return;
        }

        Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 world2 = new Vector2(world.x, world.y);

        RaycastHit2D hit = Physics2D.Raycast(world2, Vector2.zero);
        if (hit.collider == null) return;

        Cell cell = hit.collider.GetComponent<Cell>();
        if (cell == null) return;

        if (phase == "DOWN")
        {
            if (cell.CellType != CellType.Head) return;

            isHeld = true;
            currentCell = cell;

            if (!path.Contains(cell))
            {
                path.Add(cell);
            }
            return;
        }

        Animator anim = cell.GetComponent<Animator>();

        if (!isHeld) return;

        if (cell == currentCell) { return; }

        if (!IsOrthogonalNeighbor(cell, currentCell)) { return; }

        oldCell = currentCell;
        currentCell = cell;
        oldCell.SetCellType(CellType.Body);

        if (cell.vfx.activeInHierarchy && !cell.IsDrawn)
        {
            anim.Play("ThroughCell");
            cell.vfx.GetComponent<SpriteRenderer>().color = level.headColor;
        } else if (!cell.IsDrawn)
        {
            anim.Play("SlowThroughCell");
        } else
        {
            anim.Play("SlowThroughCell1");
        }

        cell.SetDrawn(true);

        if (!path.Contains(cell))
        {
            path.Add(cell);
            cell.SetCellType(CellType.Head);
        }
        else
        {
            if (path.Count >= 2 && cell == path[path.Count - 2])
            {
                if (oldCell.originalCellType == CellType.Sentence)
                {
                    oldCell.GetComponent<Animator>().Play("ThroughCell_Reverse");
                    oldCell.vfx.GetComponent<SpriteRenderer>().color = level.sentenceColor;
                }
                else if (oldCell.originalCellType == CellType.Note)
                {
                    oldCell.GetComponent<Animator>().Play("ThroughCell_Reverse");
                    oldCell.vfx.GetComponent<SpriteRenderer>().color = level.noteColor;
                }
                path[path.Count - 1].SetCellType(path[path.Count - 1].originalCellType);
                path[path.Count - 1].SetDrawn(false);
                cell.SetCellType(CellType.Head);
                path.RemoveAt(path.Count - 1);
            }
            else
            {
                currentCell = oldCell;
                currentCell.SetCellType(CellType.Head);
                return;
            }
        }
    }
}