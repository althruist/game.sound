using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip retrySound;
    [SerializeField] List<Button> menuButtons;

    public static UIManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void FeedbackSound(string soundName)
    {
        if (soundName == "hover")
        {
            AudioManager.Instance.Play(hoverSound, SoundType.SFX);
        }
        else if (soundName == "click")
        {
            AudioManager.Instance.Play(clickSound, SoundType.SFX);
        }
        else if (soundName == "retry")
        {
            AudioManager.Instance.Play(clickSound, SoundType.SFX);
        }
    }

    public void ClickAnimationAll()
    {
        foreach (Button button in menuButtons)
        {
            button.GetComponent<Animator>().Play("ButtonClick");
            button.interactable = false;
            FeedbackSound("click");
        }
    }

    public void ClickAnimationSolo(Button button)
    {
        button.GetComponent<Animator>().Play("ButtonClick");
        FeedbackSound("click");
    }
}
