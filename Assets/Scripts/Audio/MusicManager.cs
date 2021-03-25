using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;
using System;
using Debug = UnityEngine.Debug;

public static class MusicManager
{
    static EventInstance CurrentMusicInstance = new EventInstance();
    public static bool isStopped = false;

    /// <summary>Creates a 2D Oneshot Sound Instance, use 'trimBeginning = true' if you are using [FMODUnity.EventRef] attribute</summary>
    public static void Play(string sound)
    {
        try
        {;
            CurrentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CurrentMusicInstance = RuntimeManager.CreateInstance(sound);
            CurrentMusicInstance.start();
            isStopped = false;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            return;
        }
    }

    public static void Stop()
    {
        CurrentMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        isStopped = true;
    }
}
