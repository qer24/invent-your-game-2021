using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class TestWeapon : Weapon
{
    public override void Shoot(Vector3 position, Quaternion rotation, string allyTag, string enemyTag, float projectileRotationSpeed, float projectileSeekDistance, Material projectileMaterial)
    {
        base.Shoot(position, rotation, allyTag, enemyTag, projectileRotationSpeed, projectileSeekDistance, projectileMaterial);

        ShootProjectile(position, rotation, allyTag, enemyTag, projectileRotationSpeed, projectileSeekDistance, projectileMaterial);
    }
}
