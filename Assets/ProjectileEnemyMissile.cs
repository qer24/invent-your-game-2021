using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyMissile : ProjectileMissile
{
    public override void Explode(Collider col)
    {
        if (explosionTimer > 0) return;
        if (lifeTimeLeft < disableDamageThreshold) return;

        explosionTimer = explosionCooldown;

        AudioManager.Play(explosionAudio, true);

        GameObject go = LeanPool.Spawn(aoePrefab, transform.position, transform.rotation);
        go.GetComponent<Aoe>().Init(missileDamage * (transform.localScale.z * 0.5f), aoeLifetime, aoeSize, enemyTag);
        go.GetComponentInChildren<Renderer>().material = GetComponentInChildren<Renderer>().material;
    }
}
