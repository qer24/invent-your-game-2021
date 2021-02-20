using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeBarPosition : MonoBehaviour
{
    [SerializeField] Transform chargeBar = null;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    void FixedUpdate()
    {
        chargeBar.position = mainCam.WorldToScreenPoint(transform.position);
    }
}
