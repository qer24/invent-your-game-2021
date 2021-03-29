using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public RectTransform mainPanel;
    CanvasGroup mainGroup;
    public RectTransform optionsPanel;
    CanvasGroup optionsGroup;

    public float transitionTime = 0.5f;
    public LeanTweenType transitionType;

    bool optionsToggled = false;

    void Start()
    {
        mainGroup = mainPanel.GetComponent<CanvasGroup>();
        optionsGroup = optionsPanel.GetComponent<CanvasGroup>();

        MusicManager.Play("event:/Music/Menu");
    }

    public void ToggleOptions()
    {
        LeanTween.cancel(gameObject);
        optionsToggled = !optionsToggled;

        if (optionsToggled)
        {
            mainGroup.blocksRaycasts = false;
            optionsGroup.blocksRaycasts = true;
            LeanTween.moveX(mainPanel, -mainPanel.sizeDelta.x, transitionTime).setEase(transitionType).setIgnoreTimeScale(true);
            LeanTween.moveX(optionsPanel, 0, transitionTime).setEase(transitionType).setIgnoreTimeScale(true);
        }
        else
        {
            mainGroup.blocksRaycasts = true;
            optionsGroup.blocksRaycasts = false;
            LeanTween.moveX(mainPanel, 0, transitionTime).setEase(transitionType).setIgnoreTimeScale(true);
            LeanTween.moveX(optionsPanel, optionsPanel.sizeDelta.x, transitionTime).setEase(transitionType).setIgnoreTimeScale(true);
        }
    }

    public void LoadLevel(int level)
    {
        GameManager.Instance.LoadGame(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
