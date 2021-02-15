using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class TestWeapon : Weapon
{
    public override void Shoot(Vector3 position, Quaternion rotation, string allyTag, string enemyTag, float projectileRotationSpeed, float projectileSeekDistance, Material projectileMaterial)
    {
        GameObject go = LeanPool.Spawn(projectilePrefab, position, rotation);
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * projectileSpeed);
        go.GetComponent<Projectile>().Init(baseDamage, projectileSpeed, projectileLifetime, allyTag, enemyTag, projectileRotationSpeed, projectileSeekDistance);
        go.GetComponent<Renderer>().material = projectileMaterial;
    }
}
