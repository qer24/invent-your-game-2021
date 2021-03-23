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

    [SerializeField] VisualEffect muzzleFlash = null;

    float shootTimer;
    float chargeTimer;
    Material bulletMaterial;

    private void Start()
    {
        bulletMaterial = GetComponentInChildren<Renderer>().material;
        inventory.OnWeaponSwap += ResetWeaponState;
    }

    private void OnDisable()
    {
        inventory.OnWeaponSwap -= ResetWeaponState;
    }

    private void Update()
    {
        if (PauseManager.paused)
        {
            chargeTimer = 0;
            return;
        }
        if (ProcGen.MapPanel.IsOpen || ModDrop.DraggingMod || PlayerUpgradeManager.IsPanelOpen) return;
        if (inventory.CurrentWeapon.isCharged)
        {
            chargeUI.fillAmount = chargeTimer / inventory.CurrentWeapon.FinalChargeTime;

            if (Input.GetMouseButton(1) && shootTimer <= 0) //left mouse button
            {
                chargeTimer += Time.deltaTime;
            }
            else
            {
                shootTimer -= Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(1) && chargeTimer >= inventory.CurrentWeapon.FinalChargeTime)
            {
                chargeTimer = 0;
                shootTimer = inventory.CurrentWeapon.FinalFireRate;
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
                shootTimer = inventory.CurrentWeapon.FinalFireRate;
                Shoot();
            }
            else
            {
                shootTimer -= Time.deltaTime;
            }
        }
    }

    void ResetWeaponState()
    {
        chargeUI.fillAmount = 0;
        chargeTimer = 0;
        shootTimer = 0.1f;
    }

    private void Shoot()
    {
        muzzleFlash.SendEvent("Play");

        if(!string.IsNullOrEmpty(inventory.CurrentWeapon.onAttackAudio))
            AudioManager.Play(inventory.CurrentWeapon.onAttackAudio, true);

        if(inventory.CurrentWeapon.isProjectile)
        {
            inventory.CurrentWeapon.Shoot(shootPoint.position, transform.rotation, "Enemy", bulletMaterial);
        }else
        {
            inventory.CurrentWeapon.Attack("Enemy");
        }
    }
}
