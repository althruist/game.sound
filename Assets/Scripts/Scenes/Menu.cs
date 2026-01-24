using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Canvas UI;
    [SerializeField] List<Button> buttons;
    [SerializeField] GameObject text;
    [SerializeField] GameObject videoPlayer;
    public Button PlayButton, DifficultyButton, QuitButton, EasyButton, MediumButton, HardButton;
    public TextMeshProUGUI completionText;
    private LevelData.Difficulty chosenDifficulty = LevelData.Difficulty.Easy;

    void Start()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
        CinematicLetterbox.Instance.active = false;
        PlayButton.onClick.AddListener(Play);
        DifficultyButton.onClick.AddListener(delegate { ToggleWindow(UI.transform.Find("DifficultyWindow").gameObject); });
        QuitButton.onClick.AddListener(Quit);
        EasyButton.onClick.AddListener(delegate { SetDifficulty(LevelData.Difficulty.Easy); });
        MediumButton.onClick.AddListener(delegate { SetDifficulty(LevelData.Difficulty.Medium); });
        HardButton.onClick.AddListener(delegate { SetDifficulty(LevelData.Difficulty.Hard); });
        SaveLoadManager.Instance.LoadData();
        if (GameData.songsCompleted == 1)
            completionText.SetText($"you finished {GameData.songsCompleted} song");
        else
            completionText.SetText($"you finished {GameData.songsCompleted} songs");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        StartCoroutine(PlayGame());
    }

    public void ToggleWindow(GameObject panel)
    {
        StartCoroutine(SwitchMenu(panel));
    }

    public void SetDifficulty(LevelData.Difficulty difficulty)
    {
        chosenDifficulty = difficulty;
        UI.transform.Find("DifficultyWindow").gameObject.SetActive(false);
        ToggleWindow(UI.transform.Find("Menu").gameObject);
    }

    IEnumerator PlayGame()
    {
        videoPlayer.GetComponent<Animator>().Play("SlideAnim");
        text.SetActive(false);
        UIManager.Instance.ClickAnimationAll();
        yield return new WaitForSeconds(1f);
        foreach (Button button in buttons)
        {
            button.interactable = false;
            button.GetComponent<EventTrigger>().enabled = false;
        }
        yield return new WaitForSeconds(48f);
        GameManager.Instance.LoadGameWithDifficulty(chosenDifficulty);
    }

    IEnumerator SwitchMenu(GameObject panel)
    {
        yield return new WaitForSeconds(0.5f);
        UI.transform.Find("Menu").gameObject.SetActive(false);
        UI.transform.Find("DifficultyWindow").gameObject.SetActive(false);
        panel.SetActive(true);
        foreach (Button button in panel.GetComponent<UIPanel>().buttons)
        {
            button.interactable = true;
            button.GetComponent<Animator>().Play("ButtonSpawn");
            button.GetComponent<Animator>().SetTrigger("Normal");
        }
    }
}
