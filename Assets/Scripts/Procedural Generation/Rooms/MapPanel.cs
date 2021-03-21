using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
    public class MapPanel : MonoBehaviour
    {
        public LeanTweenType inType;
        public LeanTweenType outType;
        public float duration;
        public float delay;

        public static bool IsOpen = false;

        private void Start()
        {
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        }

        public void Open()
        {
            IsOpen = true;

            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
            LeanTween.scaleX(gameObject, 1, duration).setDelay(delay).setEase(inType).setIgnoreTimeScale(true);
        }

        public void Close(Action callback = null)
        {
            IsOpen = false;

            LeanTween.scaleX(gameObject, 0, duration).setEase(outType)
                .setIgnoreTimeScale(true)
                .setOnComplete(() => 
                {
                    transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
                    callback();
                });
        }

        public void Close()
        {
            IsOpen = false;

            LeanTween.scaleX(gameObject, 0, duration).setEase(outType)
                .setIgnoreTimeScale(true)
                .setOnComplete(() => transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z));
        }
    }
}
