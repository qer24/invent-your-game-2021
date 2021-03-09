using FMODUnity;
using System;
using UnityEngine;

public static class AudioManager
{

    /// <summary>Creates a 2D Oneshot Sound Instance, use 'trimBeginning = true' to 
    /// cut "event:/" from a string</summary>
    public static void Play(string sound, bool trimBeginning = false)
    {
        if (trimBeginning)
            sound = sound.Substring(7);

        try
        {
            RuntimeManager.PlayOneShot($"event:/{sound}");
        }
        catch (Exception)
        {
            Debug.LogWarning($"Sound: {sound} + not found!");
            return;
        }
    }

    /// <summary>Creates a 3D Oneshot Sound Instance, use 'trimBeginning = true' to 
    /// cut "event:/" from a string</summary>
    public static void Play(string sound, Vector3 position, bool trimBeginning = false)
    {
        if (trimBeginning)
            sound = sound.Split('/')[1];

        try
        {
            RuntimeManager.PlayOneShot($"event:/{sound}", position);
        }
        catch (Exception)
        {
            Debug.LogWarning($"Sound: {sound} + not found!");
            return;
        }
    }
}
