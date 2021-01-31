using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOnScreen : MonoBehaviour
{
    Camera mainCam;

    public float screenEdge = 0.03f;
    public Action OnTeleportStart, OnTeleportEnd;

    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckScreenEdges();
    }

    private void CheckScreenEdges()
    {
        Vector3 pos = mainCam.WorldToViewportPoint(transform.position);

        bool posChanged = false;
        if (pos.x < -screenEdge)
        {
            pos = new Vector3(1.0f, pos.y, pos.z);
            posChanged = true;
        }
        else if (pos.x >= 1 + screenEdge)
        {
            pos = new Vector3(0.0f, pos.y, pos.z);
            posChanged = true;
        }

        if (pos.y < -screenEdge)
        {
            pos = new Vector3(pos.x, 1.0f, pos.z);
            posChanged = true;
        }
        else if (pos.y >= 1 + screenEdge)
        {
            pos = new Vector3(pos.x, 0.0f, pos.z);
            posChanged = true;
        }

        if (posChanged)
        {
            OnTeleportStart?.Invoke();

            transform.position = mainCam.ViewportToWorldPoint(pos);

            OnTeleportEnd?.Invoke();
        }
    }
}
