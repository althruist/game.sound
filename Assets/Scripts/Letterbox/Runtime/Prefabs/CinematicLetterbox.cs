using UnityEngine;
using DG.Tweening;
using System;

public class CinematicLetterbox : MonoBehaviour
{
    public static CinematicLetterbox Instance { get; private set; }

    [Header("Borders")]
    [SerializeField] private Transform TopBorder;
    [SerializeField] private Transform BottomBorder;

    [Header("Settings")]
    [SerializeField] private float _duration;
    [SerializeField] private bool _active;
    [SerializeField] private float _distance;
    [SerializeField] private Ease _easeType = Ease.OutCirc;

    public bool active
    {
        get => _active;
        set
        {
            if (_active != value)
            {
                _active = value;
                onActiveChanged();
            }
        }
    }

    public float duration
    {
        get => _duration;
        set
        {
            if (_duration != value)
            {
                _duration = value;
            }
        }
    }

    public float distance
    {
        get => _distance;
        set
        {
            if (_distance != value)
            {
                _distance = value;
            }
        }
    }

    public Ease easeType
    {
        get => _easeType;
        set
        {
            if (_easeType != value)
            {
                _easeType = value;
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void onActiveChanged()
    {
        if (active)
        {
            TopBorder.DOScale(new Vector3(20, -_distance, 1), _duration).SetEase(_easeType);
            BottomBorder.DOScale(new Vector3(20, -_distance, 1), _duration).SetEase(_easeType);
        }
        else
        {
            TopBorder.DOScale(new Vector3(20, 0, 1), _duration).SetEase(_easeType);
            BottomBorder.DOScale(new Vector3(20, 0, 1), _duration).SetEase(_easeType);
        }
    }
}