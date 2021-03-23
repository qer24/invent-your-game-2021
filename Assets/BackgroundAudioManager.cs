using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudioManager : MonoBehaviour
{
    public static bool MuteInBackground = true;
    bool muted = false;

    FMOD.Studio.Bus Master;

    private void Awake()
    {
        Master = RuntimeManager.GetBus("bus:/Master");
        MuteInBackground = PlayerPrefs.GetInt("MuteInBackground", 1) == 1;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && MuteInBackground && !muted)
        {
            muted = true;
        }
        else if (focus && muted)
        {
            muted = false;
        }

        Master.setMute(muted);
    }
}
