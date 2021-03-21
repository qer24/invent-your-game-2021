using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FolowMouse : MonoBehaviour
{
    Camera mainCam;

    private void Start()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) => mainCam = Camera.main;
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = 0;
        transform.position = mousePos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
