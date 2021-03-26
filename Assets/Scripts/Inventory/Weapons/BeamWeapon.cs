using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeapon : Weapon
{
    [Header("Beam weapon")]
    public float knockbackForce = 5f;
    Rigidbody playerRb;

    private void Start()
    {
        OnEquip += () => 
        {
            playerRb = GetComponentInParent<Rigidbody>();
        };
    }

    public override void Attack(string enemyTag)
    {
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(-playerRb.transform.forward * knockbackForce, ForceMode.Impulse);

        CreateAoe(enemyTag, aoePrefab, playerShooter.shootPoint.position, playerShooter.shootPoint.rotation, FinalDamage, aoeLifeTime, FinalSize);
    }
}
