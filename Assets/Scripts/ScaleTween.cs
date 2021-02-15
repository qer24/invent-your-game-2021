using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    public LeanTweenType inType;
    public LeanTweenType outType;
    public float duration;
    public float delay;

    [HideInInspector] public bool isClosing = false;
    [HideInInspector] public bool isOpening = false;

    public enum ScaleAxis
    {
        All,
        X,
        Y,
        Z
    }
    public ScaleAxis scaleAxis;

    // Start is called before the first frame update
    void OnEnable()
    {
        isOpening = true;

        switch (scaleAxis)
        {
            case ScaleAxis.X:
                transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
                LeanTween.scaleX(gameObject, 1, duration).setDelay(delay).setEase(inType).setIgnoreTimeScale(true).setOnComplete(FinishOpening);
                break;
            case ScaleAxis.Y:
                transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
                LeanTween.scaleY(gameObject, 1, duration).setDelay(delay).setEase(inType).setIgnoreTimeScale(true).setOnComplete(FinishOpening);
                break;
            case ScaleAxis.Z:
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0);
                LeanTween.scaleZ(gameObject, 1, duration).setDelay(delay).setEase(inType).setIgnoreTimeScale(true).setOnComplete(FinishOpening);
                break;
            case ScaleAxis.All:
                transform.localScale = Vector3.zero;
                LeanTween.scale(gameObject, Vector3.one, duration).setDelay(delay).setEase(inType).setIgnoreTimeScale(true).setOnComplete(FinishOpening);
                break;
        }
    }

    void FinishOpening()
    {
        isOpening = false;
    }

    public void Close()
    {
        isClosing = true;

        switch (scaleAxis)
        {
            case ScaleAxis.X:
                LeanTween.scaleX(gameObject, 0, duration).setEase(outType).setIgnoreTimeScale(true).setOnComplete(DisableSelf);
                break;
            case ScaleAxis.Y:
                LeanTween.scaleY(gameObject, 0, duration).setEase(outType).setIgnoreTimeScale(true).setOnComplete(DisableSelf);
                break;
            case ScaleAxis.Z:
                LeanTween.scaleZ(gameObject, 0, duration).setEase(outType).setIgnoreTimeScale(true).setOnComplete(DisableSelf);
                break;
            case ScaleAxis.All:
                LeanTween.scale(gameObject, Vector3.zero, duration).setEase(outType).setIgnoreTimeScale(true).setOnComplete(DisableSelf);
                break;
        }
    }

    void DisableSelf()
    {
        isClosing = false;

        gameObject.SetActive(false);
    }
}
