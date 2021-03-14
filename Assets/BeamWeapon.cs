using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeapon : Weapon
{
    PlayerShooter shooter;

    private void Start()
    {
        OnEquip += () => shooter = GetComponentInParent<PlayerShooter>();
    }

    public override void Attack(string enemyTag)
    {
        GameObject go = Lean.Pool.LeanPool.Spawn(aoePrefab, shooter.shootPoint.position + Vector3.up * 3f, shooter.shootPoint.rotation);
        go.GetComponent<Aoe>().Init(FinalDamage, aoeLifeTime, FinalSize, enemyTag);
    }
}
