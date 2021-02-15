using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.VFX;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    //public float shootCooldown = 0.2f;
    public Transform shootPoint;

    public InventoryManager inventory;
    public Image chargeUI;

    [Header("Bullet stats")]
    //TODO: Make some player stats script that holds all player stats including these
    public float bulletRotationSpeed = 1f;
    public float bulletSeekDistance = 4f;

    [SerializeField] VisualEffect muzzleFlash = null;

    float shootTimer;
    float chargeTimer;
    Material bulletMaterial;

    private void Start()
    {
        bulletMaterial = GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        if (MapPanel.IsOpen) return;
        if (inventory.CurrentWeapon.isCharged)
        {
            chargeUI.fillAmount = chargeTimer / inventory.CurrentWeapon.timeToCharge;

            if (Input.GetMouseButton(1) && shootTimer <= 0) //left mouse button
            {
                chargeTimer += Time.deltaTime;
            }
            else
            {
                shootTimer -= Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(1) && chargeTimer >= inventory.CurrentWeapon.timeToCharge)
            {
                chargeTimer = 0;
                shootTimer = inventory.CurrentWeapon.baseAttackRate;
                Shoot();
            }else if (Input.GetMouseButtonUp(1))
            {
                chargeTimer = 0;
            }
        }
        else
        {
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
