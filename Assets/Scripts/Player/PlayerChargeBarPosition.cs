using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChargeBarPosition : MonoBehaviour
{
    [SerializeField] Transform chargeBar = null;
    Camera mainCam;

    private void Start()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) => mainCam = Camera.main;
        mainCam = Camera.main;
    }

    void FixedUpdate()
    {
        chargeBar.position = mainCam.WorldToScreenPoint(transform.position);
    }
}
