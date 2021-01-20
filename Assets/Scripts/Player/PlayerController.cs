using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveForce = 25;
    [SerializeField] float rotationSpeed = 25;

    [SerializeField] Transform gfx = null;
    [SerializeField] float gfxRotationSpeed = 5;

    [SerializeField] TrailRenderer[] movementTrails = null;
    [SerializeField] ParticleSystem movementParticles = null;

    public float knockbackForce = 100f;

    Camera mainCam;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    private void CheckScreenEdges()
    {
        Vector3 pos = mainCam.WorldToViewportPoint(transform.position);

        bool posChanged = false;
        if (pos.x < -0.03f)
        {
            pos = new Vector3(1.0f, pos.y, pos.z);
            posChanged = true;
        }
        else if (pos.x >= 1.03f)
        {
            pos = new Vector3(0.0f, pos.y, pos.z);
            posChanged = true;
        }

        if (pos.y < -0.03f)
        {
            pos = new Vector3(pos.x, 1.0f, pos.z);
            posChanged = true;
        }
        else if (pos.y >= 1.03f)
        {
            pos = new Vector3(pos.x, 0.0f, pos.z);
            posChanged = true;
        }

        if(posChanged)
        {
            movementParticles.Pause();

            transform.position = mainCam.ViewportToWorldPoint(pos);

            movementParticles.Play();

            foreach (var trail in movementTrails)
            {
                trail.Clear();
            }
        }
    }

    private void FixedUpdate()
    {
        RotateToMouse();

        CheckScreenEdges();

        if (Input.GetMouseButton(0))
        {
            rb.AddForce(transform.forward * moveForce * 0.01f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.forward * knockbackForce, ForceMode.Impulse);
    }

    private void RotateToMouse()
    {
        Vector3 dir = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        float gfxAngle = -(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y);
        //gfxAngle = Mathf.Clamp(gfxAngle, -70, 70);
        Quaternion gfxRotation = Quaternion.Euler(0, 0, gfxAngle);

        gfx.localRotation = Quaternion.Slerp(gfx.localRotation, gfxRotation, gfxRotationSpeed * Time.deltaTime);
    }
}
