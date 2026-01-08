using UnityEngine;

public enum CellType
{
    Normal,
    Sentence,
    Note
}
public class Cell : MonoBehaviour
{
    public Vector2Int coord { get; private set; }
    public bool IsDrawn { get; private set; }
    public CellType CellType { get; private set; }

    public void Init(Vector2Int c) => coord = c;

    public void SetDrawn(bool value)
    {
        IsDrawn = value;
    }

    public void SetCoord(Vector2Int value)
    {
        coord = value;
    }

    public void SetCellType(CellType value)
    {
        CellType = value;

        if (CellType == CellType.Sentence)
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;
        } else if (CellType == CellType.Note)
        {
            this.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }
}