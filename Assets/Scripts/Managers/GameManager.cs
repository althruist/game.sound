using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public LevelData currentLevel;
    public List<LevelData> easyLevels;
    public List<LevelData> mediumLevels;
    public List<LevelData> hardLevels;

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

    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadGameWithDifficulty(LevelData.Difficulty difficulty)
    {
        if (difficulty == LevelData.Difficulty.Easy)
        {
            currentLevel = easyLevels[0];
            LevelManager.Instance.levels = easyLevels;
        }
        else if (difficulty == LevelData.Difficulty.Medium)
        {
            currentLevel = mediumLevels[0];
            LevelManager.Instance.levels = mediumLevels;
        }
        else if (difficulty == LevelData.Difficulty.Hard)
        {
            currentLevel = hardLevels[0];
            LevelManager.Instance.levels = hardLevels;
        }
        SceneManager.LoadScene("GameScene");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel(LevelData nextLevel)
    {
        currentLevel = nextLevel;
        ReloadLevel();
    }
}
