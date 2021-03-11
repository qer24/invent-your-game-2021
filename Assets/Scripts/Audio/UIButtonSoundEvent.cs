using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSoundEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [FMODUnity.EventRef] public string mouseOverAudio;
    [FMODUnity.EventRef] public string mouseDownAudio;
    [FMODUnity.EventRef] public string mouseUpAudio;

    public void OnPointerEnter(PointerEventData ped)
    {
        if (string.IsNullOrEmpty(mouseOverAudio)) return;

        AudioManager.Play(mouseOverAudio, true);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        if (string.IsNullOrEmpty(mouseDownAudio)) return;

        AudioManager.Play(mouseDownAudio, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(mouseUpAudio)) return;

        AudioManager.Play(mouseUpAudio, true);
    }
}