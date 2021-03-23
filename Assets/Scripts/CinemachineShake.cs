using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour {

    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Awake()
    {
        Instance = this;

        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        if (!GameplaySettings.DoScreenShake) return;

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                Mathf.Lerp(startingIntensity, 0f, 1 - shakeTimer / shakeTimerTotal);
        }
    }

}
