using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool useOldBillboard = true;

    public Vector3 lookRot = Vector3.forward;
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = CameraManager.Instance.mainCam.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (useOldBillboard)
        {
            transform.LookAt(cam);
        }
        else
        {
            var newRot = Quaternion.FromToRotation(-Vector3.forward, cam.position - transform.position);
            Vector3 eulerAngles = newRot.eulerAngles;
            transform.rotation = Quaternion.Euler(eulerAngles);
        }
    }
}
