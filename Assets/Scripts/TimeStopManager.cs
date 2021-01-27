using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopManager : MonoBehaviour
{
    public static TimeStopManager Instance;

    float remainingDuration = 0;
    bool isFrozen = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if(remainingDuration > 0 && !isFrozen)
        {
            StartCoroutine(Freeze());
        }
    }

    IEnumerator Freeze()
    {
        isFrozen = true;
        float original = Time.timeScale;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(remainingDuration);
        Time.timeScale = original;
        remainingDuration = 0;
        isFrozen = false;
    }

    public void FreezeTime(float duration)
    {
        remainingDuration = duration;
    }
}
