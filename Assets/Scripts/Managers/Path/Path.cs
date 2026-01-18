using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public enum PathVariable
    {
        OldCell,
        CurrentCell,
        Completed,
        Held,
        Cells
    }

    public Cell oldCell { get; private set; }
    public Cell currentCell { get; private set; }
    public bool completed { get; private set; }
    public bool held { get; private set; }
    public List<Cell> cells { get; private set; }

    void Awake()
    {
        cells = new List<Cell>();
    }

    public void SetVariable(PathVariable variable, object value)
    {
        switch (variable)
        {
            case PathVariable.Completed:
                completed = (bool)value;
                break;
            case PathVariable.Held:
                held = (bool)value;
                break;
            case PathVariable.OldCell:
                oldCell = (Cell)value;
                break;
            case PathVariable.CurrentCell:
                currentCell = (Cell)value;
                break;
        }
    }
}
