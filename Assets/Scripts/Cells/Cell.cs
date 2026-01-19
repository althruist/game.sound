using System.Collections;
using TMPro;
using UnityEngine;

public enum CellType
{
    Normal,
    Sentence,
    Note,
    Head,
    Body
}

public enum SoundType
{
    Bass,
    Melody,
    Chord,
    Sentence,
    SFX
}

public class Cell : MonoBehaviour
{
    public Vector2Int coord { get; private set; }
    public bool IsDrawn { get; private set; }
    public CellType CellType { get; private set; }
    public SoundType SoundType { get; private set; }
    public CellType originalCellType { get; private set; }
    public bool IsHead { get; private set; }
    public LevelData level { get; private set; }
    private SpriteRenderer sr;
    public GameObject vfx;
    public TextMeshPro text;
    public AudioClip sound;
    public int index { get; private set; }

    IEnumerator playAnimation(Animator anim, float randomTime)
    {
        float duration = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);
        anim.Play("IdleCell", 0, randomTime);
        anim.Update(0f);
    }

    void Start()
    {
        originalCellType = CellType;
        Animator anim = GetComponent<Animator>();
        float randomTime = Random.Range(0f, 1f);
        StartCoroutine(playAnimation(anim, randomTime));
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    public void SetLevel(LevelData lvl)
    {
        level = lvl;
    }

    public void SetDrawn(bool value)
    {
        IsDrawn = value;
    }

    public void SetCoord(Vector2Int value)
    {
        coord = value;
    }

    public void SetIndex(int value)
    {
        index = value;
    }

    public virtual bool TryGetNoteData(out LevelData.NoteData note)
    {
        note = default;
        return false;
    }

    public void SetCellType(CellType value)
    {
        CellType = value;

        if (CellType == CellType.Sentence)
        {
            sr.color = level.sentenceColor;
            vfx.SetActive(true);
        }
        else if (CellType == CellType.Note)
        {
            sr.color = level.noteColor;
            vfx.SetActive(true);
        }
        else if (CellType == CellType.Head)
        {
            sr.color = level.headColor;
        }
        else if (CellType == CellType.Body)
        {
            sr.color = level.bodyColor;
        }
        else if (CellType == CellType.Normal)
        {
            sr.color = level.tileColor;
        }
    }

    public virtual void Activate()
    {
        AudioSource src = AudioManager.Instance.Play(sound, SoundType.SFX);
        src.pitch = Random.Range(0.9f, 1.3f);
    }
}