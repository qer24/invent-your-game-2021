using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class TestChargedWeapon : Weapon
{
    public override void Shoot(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial)
    {
        base.Shoot(position, rotation, enemyTag, projectileMaterial);

        ShootProjectile(position, rotation, enemyTag, projectileMaterial);
    }
}
