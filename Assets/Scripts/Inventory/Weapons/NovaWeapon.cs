using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaWeapon : Weapon
{
    PlayerShooter shooter;

    private void Start() => OnEquip += () => shooter = GetComponentInParent<PlayerShooter>();

    public override void Attack(string enemyTag)
    {
        GameObject go = Lean.Pool.LeanPool.Spawn(aoePrefab, shooter.transform.position, shooter.transform.rotation);
        go.GetComponent<Aoe>().Init(FinalDamage, aoeLifeTime, FinalSize, enemyTag);
    }
}
