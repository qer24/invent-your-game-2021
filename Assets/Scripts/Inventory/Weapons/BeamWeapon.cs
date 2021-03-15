using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeapon : Weapon
{
    [Header("Beam weapon")]
    public float knockbackForce = 5f;
    PlayerShooter shooter;
    Rigidbody playerRb;

    private void Start()
    {
        OnEquip += () => 
        {
            shooter = GetComponentInParent<PlayerShooter>();
            playerRb = GetComponentInParent<Rigidbody>();
        };
    }

    public override void Attack(string enemyTag)
    {
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(-playerRb.transform.forward * knockbackForce, ForceMode.Impulse);

        GameObject go = Lean.Pool.LeanPool.Spawn(aoePrefab, shooter.shootPoint.position, shooter.shootPoint.rotation);
        go.GetComponent<Aoe>().Init(FinalDamage, aoeLifeTime, FinalSize, enemyTag);
    }
}
