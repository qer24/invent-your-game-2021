using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMissile : Projectile
{
    [FMODUnity.EventRef] public string explosionAudio;

    GameObject aoePrefab;
    float aoeLifetime;
    Vector3 aoeSize;
    float missileDamage = 0;

    public float explosionCooldown = 0.25f;
    float explosionTimer = 0;

    public void InitMissile(float _damage, float _velocity, float _lifetime, string _enemyTag, GameObject _aoePrefab, Vector3 _aoeSize, float _aoeLifetime)
    {
        Init(_damage, _velocity, _lifetime, _enemyTag);
        missileDamage = _damage;
        damage = 0;
        aoePrefab = _aoePrefab;
        aoeSize = _aoeSize;
        aoeLifetime = _aoeLifetime;
    }

    private void Start()
    {
        OnDespawn += DespawnExplosion;
        OnCollision += Explode;
    }

    protected override void Update()
    {
        explosionTimer -= Time.deltaTime;

        base.Update();
    }

    private void OnDestroy()
    {
        OnDespawn -= DespawnExplosion;
        OnCollision -= Explode;
    }

    void DespawnExplosion()
    {
        aoeSize *= 0.5f;
        Explode(null);
    }

    public void Explode(Collider col)
    {
        if (explosionTimer > 0) return;

        explosionTimer = explosionCooldown;

        AudioManager.Play(explosionAudio, true);

        Aoe aoe = Lean.Pool.LeanPool.Spawn(aoePrefab, transform.position, Quaternion.identity).GetComponent<Aoe>();
        aoe.Init(missileDamage, aoeLifetime, aoeSize, enemyTag);
    }
}
