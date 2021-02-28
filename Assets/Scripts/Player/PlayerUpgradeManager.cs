using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProcGen;

public class PlayerUpgradeManager : MonoBehaviour
{
    [SerializeField] CanvasGroup buttonCanvasGroup = null;
    ScaleTween buttonScaleTween;
    Button button;

    float alphaTime = 1;

    void Start()
    {
        buttonScaleTween = buttonCanvasGroup.GetComponent<ScaleTween>();
        button = buttonCanvasGroup.GetComponent<Button>();

        RoomManager.OnRoomChanged += DisableButton;
        RoomManager.OnRoomComplete += EnableButton;
    }

    private void Update()
    {
        if(buttonCanvasGroup.gameObject.activeSelf)
        {
            alphaTime += Time.deltaTime;
            buttonCanvasGroup.alpha = Mathf.Clamp((Mathf.Sin(alphaTime) + 1f) * 0.5f, 0.1f, 1f);
        }
    }

    public void EnableButton()
    {
        button.interactable = true;

        alphaTime = 1;
        buttonCanvasGroup.gameObject.SetActive(true);
    }

    public void DisableButton()
    {
        button.interactable = false;

        alphaTime = 1;
        buttonScaleTween.Close();
    }
}
