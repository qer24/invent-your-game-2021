using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class TestChargedWeapon : Weapon
{
    public override void Shoot(Vector3 position, Quaternion rotation, string allyTag, string enemyTag, Material projectileMaterial)
    {
        base.Shoot(position, rotation, allyTag, enemyTag, projectileMaterial);

        ShootProjectile(position, rotation, allyTag, enemyTag, projectileMaterial);
    }
}
