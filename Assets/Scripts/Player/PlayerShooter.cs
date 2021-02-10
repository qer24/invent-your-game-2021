using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.VFX;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootCooldown = 0.2f;
    public Transform shootPoint;

    [Header("Bullet stats")]
    //TODO: Make some player stats script that holds all player stats including these
    public float bulletDamage = 5f;
    public float bulletSpeed;
    public float bulletLifetime = 2f;
    public float bulletRotationSpeed = 1f;
    public float bulletSeekDistance = 4f;

    [SerializeField] VisualEffect muzzleFlash = null;

    float shootTimer;
    Material bulletMaterial;

    private void Start()
    {
        bulletMaterial = GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        if (MapPanel.IsOpen) return;

        if (Input.GetMouseButton(1) && shootTimer <= 0) //left mouse button
        {
            shootTimer = shootCooldown;
            Shoot();
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        muzzleFlash.SendEvent("Play");

        GameObject go = LeanPool.Spawn(bulletPrefab, shootPoint.position, transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * bulletSpeed);
        go.GetComponent<Projectile>().Init(bulletDamage, bulletSpeed, bulletLifetime, gameObject.tag, "Enemy", bulletRotationSpeed, bulletSeekDistance);
        go.GetComponent<Renderer>().material = bulletMaterial;
    }
}
