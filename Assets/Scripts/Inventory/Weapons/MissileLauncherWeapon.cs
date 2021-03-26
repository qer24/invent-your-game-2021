using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncherWeapon : Weapon
{
    public override void Shoot(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial)
    {
        base.Shoot(position, rotation, enemyTag, projectileMaterial);

        ShootProjectile(position, rotation, enemyTag, projectileMaterial, out var missile);
        missile.GetComponent<ProjectileMissile>().InitMissile(FinalDamage, projectileSpeed, projectileLifetime, enemyTag, aoePrefab, FinalSize, aoeLifeTime, this);
    }
}
