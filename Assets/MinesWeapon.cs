using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcGen;

public class MinesWeapon : Weapon
{
    public float stopTime = 2f;

    public override void Shoot(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial)
    {
        base.Shoot(position, rotation, enemyTag, projectileMaterial);

        ShootProjectile(position, rotation, enemyTag, projectileMaterial, out var go);
        var proj = go.GetComponent<ProjectileMissile>();
        proj.InitMissile(FinalDamage, projectileSpeed, projectileLifetime, enemyTag, aoePrefab, FinalSize, aoeLifeTime, this);

        LeanTween.value(proj.gameObject, proj.velocity, 0, stopTime).setOnUpdate((float val) => proj.velocity = val);

        RoomManager.OnRoomChanged += () =>
        {
            if(proj != null)
                proj.lifeTimeLeft = 0.1f;
        };
    }
}
