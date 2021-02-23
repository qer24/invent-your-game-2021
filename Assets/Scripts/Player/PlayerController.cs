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

    [HideInInspector] public bool moveToPoint = false;
    [HideInInspector] public Vector3 movePoint;
    float moveToPointVelocityScale = 1;

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
        if (ProcGen.MapPanel.IsOpen || ModDrop.draggingMod) return;
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

    public void StartMovingToPoint(Vector3 newPoint, float newVelocityScale = 1)
    {
        movePoint = newPoint;
        moveToPointVelocityScale = newVelocityScale;

        moveToPoint = true;
    }

    public void StopMovingToPoint()
    {
        moveToPoint = false;
    }

    private void MoveToPoint()
    {
        RotateToPoint(movePoint, 2f);
        rb.AddForce(transform.forward * moveForce * 0.03f * moveToPointVelocityScale);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 direction = (collision.transform.position - transform.position).normalized;
        direction.y = 0;
        float impactForce = collision.rigidbody.velocity.magnitude * 0.5f * rb.velocity.magnitude * 0.5f;
        rb.AddForce(-direction * knockbackForce * impactForce * 0.0005f, ForceMode.Impulse);
        collision.rigidbody.AddForce(direction * knockbackForce * impactForce * 0.0001f, ForceMode.Impulse); 
    }

    private void RotateToPoint(Vector3 target, float multiplier = 1f)
    {
        Vector3 dir = (target - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        float gfxAngle = -(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y);
        //gfxAngle = Mathf.Clamp(gfxAngle, -70, 70);
        Quaternion gfxRotation = Quaternion.Euler(0, 0, gfxAngle);

        gfx.localRotation = Quaternion.Slerp(gfx.localRotation, gfxRotation, gfxRotationSpeed * Time.deltaTime * multiplier);
    }

    public void DisableParticles()
    {
        movementParticles.Pause();
    }

    public void EnableParticles()
    {
        movementParticles.Play();

        foreach (var trail in movementTrails)
        {
            trail.Clear();
        }
    }
}
