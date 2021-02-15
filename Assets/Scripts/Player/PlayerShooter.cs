using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.VFX;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    //public float shootCooldown = 0.2f;
    public Transform shootPoint;

    public InventoryManager inventory;

    [Header("Bullet stats")]
    //TODO: Make some player stats script that holds all player stats including these
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
            shootTimer = inventory.CurrentWeapon.baseAttackRate;
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

        if(inventory.CurrentWeapon.isProjectile)
        {
            inventory.CurrentWeapon.Shoot(shootPoint.position, transform.rotation, gameObject.tag, "Enemy", bulletRotationSpeed, bulletSeekDistance, bulletMaterial);
        }else
        {
            inventory.CurrentWeapon.Attack();
        }
    }
}
