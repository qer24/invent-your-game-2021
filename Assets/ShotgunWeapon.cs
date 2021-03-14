using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : Weapon
{
    [Header("Shotgun")]
    public int bullets = 5;
    public float randomAngleMax = 40;

    public string ShotgunDamage { get => $"{FinalDamage * 0.6f}-{FinalDamage}"; }
    public string ShotgunBulletSpeed { get => $"{projectileSpeed * 0.6f}-{projectileSpeed}"; }

    public override void Shoot(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial)
    {
        base.Shoot(position, rotation, enemyTag, projectileMaterial);

        for (int i = 0; i < bullets; i++)
        {
            float randomAngle = Random.Range(-randomAngleMax, randomAngleMax);
            float randomMultiplier = Random.Range(0.6f, 1f);

            ShootProjectile(position, rotation * Quaternion.AngleAxis(randomAngle, Vector3.up), enemyTag, projectileMaterial, randomMultiplier, randomMultiplier, randomMultiplier);
        }
    }
}
