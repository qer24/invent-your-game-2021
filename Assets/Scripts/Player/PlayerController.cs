using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveForce = 25;
    public float rotationSpeed = 25;

    [SerializeField] Transform gfx = null;
    [SerializeField] float gfxRotationSpeed = 5;

    [SerializeField] TrailRenderer[] movementTrails = null;
    [SerializeField] ParticleSystem movementParticles = null;

    [SerializeField] KeepOnScreen screenConfiner = null;
    public float knockbackForce = 100f;

    [SerializeField, EventRef] string shipHummingSound = null;
    FMOD.Studio.EventInstance humInstance;

    [HideInInspector] public bool moveToPoint = false;
    [HideInInspector] public Vector3 movePoint;
    float moveToPointVelocityScale = 1;

    Camera mainCam;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = CameraManager.Instance.mainCam;
        rb = GetComponent<Rigidbody>();

        screenConfiner.OnTeleportStart += DisableParticles;
        screenConfiner.OnTeleportEnd += EnableParticles;

        humInstance = RuntimeManager.CreateInstance(shipHummingSound);
        humInstance.start();
        humInstance.setPaused(true);

        SceneManager.sceneLoaded += ResetPlayer;

        PauseManager.OnPause += () => humInstance.setPaused(true);

        MusicManager.Play("event:/Music/Level");
    }

    void ResetPlayer(Scene scene, LoadSceneMode loadSceneMod)
    {
        mainCam = CameraManager.Instance.mainCam;
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private void OnDisable()
    {
        screenConfiner.OnTeleportStart -= DisableParticles;
        screenConfiner.OnTeleportEnd -= EnableParticles;

        humInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.sceneLoaded -= ResetPlayer;
    }

    private void FixedUpdate()
    {
        if (ProcGen.MapPanel.IsOpen || ModDrop.DraggingMod || PlayerUpgradeManager.IsPanelOpen)
        {
            humInstance.getPaused(out var pause);
            if (!pause)
            {
                humInstance.setPaused(true);
            }
            return;
        }

        if (moveToPoint)
        {
            screenConfiner.enabled = false;
            MoveToPoint();
            return;
        }else
        {
            screenConfiner.enabled = true;
        }

        if(mainCam != null)
            RotateToPoint(mainCam.ScreenToWorldPoint(Input.mousePosition));

        humInstance.getPaused(out var paused);
        if (Input.GetMouseButton(0))
        {
            if(paused)
                humInstance.setPaused(false);
            rb.AddForce(transform.forward * moveForce * 0.01f);
        }else if(!paused)
        {
            humInstance.setPaused(true);
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
        if(!collision.collider.isTrigger)
            KnockBack(collision.transform);
    }

    public void KnockBack(Transform collidee)
    {
        AudioManager.Play("event:/SFX/Player/PlayerKnockback", true);

        Vector3 direction = (collidee.position - transform.position).normalized;
        direction.y = 0;
        var colRb = collidee.GetComponent<Rigidbody>();
        float impactForce = colRb.velocity.magnitude * 0.5f * rb.velocity.magnitude * 0.5f;
        rb.velocity = Vector3.zero;
        rb.AddForce(-direction * knockbackForce * impactForce * 0.00035f, ForceMode.Impulse);
        colRb.velocity = Vector3.zero;
        colRb.AddForce(direction * knockbackForce * impactForce * 0.0001f, ForceMode.Impulse);
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
