using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PathManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private LevelData level;
    public LevelManager levelManager;
    public TextMeshPro subtitles;

    [SerializeField] private InputController inputController;
    [SerializeField] private Path path;

    private void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    private void Start()
    {
        level = GameManager.Instance.currentLevel;
    }

    private void OnEnable()
    {
        inputController.OnInput += DetectCell;
    }

    private void OnDisable()
    {
        inputController.OnInput -= DetectCell;
    }

    bool IsOrthogonalNeighbor(Cell a, Cell b)
    {
        Vector2Int delta = a.coord - b.coord;
        return Mathf.Abs(delta.x) + Mathf.Abs(delta.y) == 1;
    }

    IEnumerator LerpBackgroundColor(Color from, Color to, float duration)
    {
        float timer = 0f;
        SpriteRenderer bg = levelManager.background.GetComponent<SpriteRenderer>();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            bg.color = Color.Lerp(from, to, t);
            yield return null;
        }

        bg.color = to;
    }


    IEnumerator ProgressToLevelAnimation()
    {
        foreach (var cell in levelManager.cells)
        {
            cell.Value.GetComponent<Animator>().Play("SuccessCell");
            if (cell.Value.originalCellType == CellType.Sentence || cell.Value.originalCellType == CellType.Note)
            {
                cell.Value.vfx.SetActive(false);
            }
        }
        yield return new WaitForSeconds(1.6f);
        DOTween.To(
            () => cam.GetComponent<PixelPerfectCamera>().assetsPPU,
            x => cam.GetComponent<PixelPerfectCamera>().assetsPPU = Mathf.RoundToInt(x),
            1972,
            0.7f
        ).SetEase(Ease.InCirc);

        LevelData newLevel = null;

        foreach (LevelData level in levelManager.levels)
        {
            if (GameManager.Instance.currentLevel.GameDifficulty == level.GameDifficulty && level.LevelIndex - GameManager.Instance.currentLevel.LevelIndex == 1)
            {
                newLevel = level;
            } else
            {
            }
        }

        if (newLevel)
        {
            AudioSource src = AudioManager.Instance.Play(AudioManager.Instance.travelLevelSFX, SoundType.SFX);
            src.pitch = 1;
            StartCoroutine(LerpBackgroundColor(GameManager.Instance.currentLevel.BackgroundColor, newLevel.BackgroundColor, 2f));
            yield return new WaitForSeconds(2f);
            GameManager.Instance.LoadNextLevel(newLevel);
        }
        else
        {
            AudioSource src = AudioManager.Instance.Play(AudioManager.Instance.travelLevelFinalSFX, SoundType.SFX);
            src.volume = 0.5f;
            src.pitch = 1;
            StartCoroutine(LerpBackgroundColor(GameManager.Instance.currentLevel.BackgroundColor, Color.white, 2f));
            yield return new WaitForSeconds(4f);
            GameManager.Instance.LoadLevel("EndScene");
        }
    }

    void DetectCell(Collider2D collider, InputController.InputPhase phase)
    {
        Cell cell = collider.GetComponent<Cell>();

        if (path.completed || cell == null) return;

        if (phase == InputController.InputPhase.Down)
        {
            if (cell.CellType != CellType.Head) return;

            path.SetVariable(Path.PathVariable.Held, true);
            path.SetVariable(Path.PathVariable.CurrentCell, cell);

            if (!path.cells.Contains(cell))
            {
                path.cells.Add(cell);
            }
            return;
        }

        Animator anim = cell.GetComponent<Animator>();

        if (!path.held) return;

        if (cell == path.currentCell) { return; }

        if (!IsOrthogonalNeighbor(cell, path.currentCell)) { return; }

        path.SetVariable(Path.PathVariable.OldCell, path.currentCell);
        path.SetVariable(Path.PathVariable.CurrentCell, cell);
        path.oldCell.SetCellType(CellType.Body);

        if (cell.vfx.activeInHierarchy && !cell.IsDrawn)
        {
            if (cell.CellType == CellType.Sentence)
            {
                anim.Play("SentenceHover");
                subtitles.SetText($"{subtitles.text} {cell.text.text}");
            }
            else
            {
                anim.Play("ThroughCell");
            }
            cell.vfx.GetComponent<SpriteRenderer>().color = level.HeadColor;
        }
        else if (!cell.IsDrawn)
        {
            anim.Play("SlowThroughCell");
        }
        else
        {
            anim.Play("SlowThroughCell1");
        }

        cell.SetDrawn(true);

        if (!path.cells.Contains(cell))
        {
            path.cells.Add(cell);
            cell.SetCellType(CellType.Head);
            cell.Activate();
        }
        else
        {
            if (path.cells.Count >= 2 && cell == path.cells[path.cells.Count - 2])
            {
                if (path.oldCell.originalCellType == CellType.Sentence)
                {
                    path.oldCell.GetComponent<Animator>().Play("ThroughCell_Reverse");
                    path.oldCell.vfx.GetComponent<SpriteRenderer>().color = level.SentenceColor;
                    subtitles.SetText(
                        Regex.Replace(
                            subtitles.text,
                            $@"\b{Regex.Escape(path.oldCell.text.text)}\b",
                            ""
                        ).Trim()
                    );
                }
                else if (path.oldCell.originalCellType == CellType.Note)
                {
                    path.oldCell.GetComponent<Animator>().Play("ThroughCell_Reverse");
                    path.oldCell.vfx.GetComponent<SpriteRenderer>().color = level.NoteColor;
                }
                path.oldCell.SetCellType(path.oldCell.originalCellType);
                path.oldCell.SetDrawn(false);
                cell.SetCellType(CellType.Head);
                path.cells.RemoveAt(path.cells.Count - 1);
            }
            else
            {

                path.SetVariable(Path.PathVariable.CurrentCell, path.oldCell);
                path.currentCell.SetCellType(CellType.Head);
                return;
            }
        }

        if ((level.GridWidth * level.GridHeight) == path.cells.Count)
        {
            List<int> tempCells = new List<int>();
            foreach (Cell el in path.cells)
            {
                if (el.originalCellType == CellType.Sentence)
                {
                    tempCells.Add(el.index);
                }
            }

            bool inOrder = true;

            for (int i = 1; i < tempCells.Count; i++)
            {
                if (tempCells[i] <= tempCells[i - 1])
                {
                    inOrder = false;
                    break;
                }
            }

            if (inOrder)
            {
                path.SetVariable(Path.PathVariable.Completed, true);
                path.SetVariable(Path.PathVariable.Held, false);
                StartCoroutine(ProgressToLevelAnimation());

                if (level.GameLevelType == LevelData.LevelType.Note)
                {
                    foreach (Cell nCell in path.cells)
                    {
                        if (nCell.TryGetNoteData(out LevelData.NoteData note))
                        {
                            AudioManager.Instance.AddNote(note);
                        }
                        else
                        {
                            LevelData.NoteData data = new LevelData.NoteData();
                            data.soundType = level.SoundType;
                            AudioManager.Instance.AddNote(data);
                        }
                    }
                }
            }
            else
            {
                levelManager.GetComponent<Animator>().Play("IncorrectPath");
                subtitles.GetComponent<Animator>().Play("IncorrectSentence");
            }
        }
    }
}