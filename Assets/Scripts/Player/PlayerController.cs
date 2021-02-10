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

    [SerializeField] KeepOnScreen screenConfiner = null;

    public float knockbackForce = 100f;

    [Header("Do not change")]
    public bool moveToPoint = false;
    public Vector3 movePoint;

    Camera mainCam;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();

        screenConfiner.OnTeleportStart += DisableParticles;
        screenConfiner.OnTeleportEnd += EnableParticles;
    }

    private void OnDisable()
    {
        screenConfiner.OnTeleportStart -= DisableParticles;
        screenConfiner.OnTeleportEnd -= EnableParticles;
    }

    private void FixedUpdate()
    {
        if (MapPanel.IsOpen) return;
        if (moveToPoint)
        {
            screenConfiner.enabled = false;
            MoveToPoint();
            return;
        }else
        {
            screenConfiner.enabled = true;
        }

        RotateToPoint(mainCam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0))
        {
            rb.AddForce(transform.forward * moveForce * 0.01f);
        }
    }

    private void MoveToPoint()
    {
        RotateToPoint(movePoint);
        rb.AddForce(transform.forward * moveForce * 0.03f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        Vector3 direction = (collision.transform.position - transform.position).normalized;
        direction.y = 0;
        rb.AddForce(-direction * knockbackForce, ForceMode.Impulse);
    }

    private void RotateToPoint(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        float gfxAngle = -(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y);
        //gfxAngle = Mathf.Clamp(gfxAngle, -70, 70);
        Quaternion gfxRotation = Quaternion.Euler(0, 0, gfxAngle);

        gfx.localRotation = Quaternion.Slerp(gfx.localRotation, gfxRotation, gfxRotationSpeed * Time.deltaTime);
    }

    void DisableParticles()
    {
        movementParticles.Pause();
    }

    void EnableParticles()
    {
        movementParticles.Play();

        foreach (var trail in movementTrails)
        {
            trail.Clear();
        }
    }
}
