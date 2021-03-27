using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinousProjectileBehaviour : ProjectileBehaviour
{
    Camera mainCam;

    public float screenEdge = 0.03f;

    public override void Start()
    {
        base.Start();

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) => mainCam = CameraManager.Instance.mainCam;
        mainCam = CameraManager.Instance.mainCam;
    }

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
            transform.position = mainCam.ViewportToWorldPoint(pos);
        }
    }
}
