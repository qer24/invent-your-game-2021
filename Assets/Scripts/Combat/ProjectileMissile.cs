using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMissile : Projectile
{
    [FMODUnity.EventRef] public string explosionAudio;

    protected GameObject aoePrefab;
    protected float aoeLifetime;
    protected Vector3 aoeSize;
    protected float missileDamage = 0;

    public float explosionCooldown = 0.25f;
    protected float explosionTimer = 0;

    Weapon weapon;

    public void InitMissile(float _damage, float _velocity, float _lifetime, string _enemyTag, GameObject _aoePrefab, Vector3 _aoeSize, float _aoeLifetime, Weapon _weapon = null)
    {
        Init(_damage, _velocity, _lifetime, _enemyTag);
        missileDamage = _damage;
        damage = 0;
        aoePrefab = _aoePrefab;
        aoeSize = _aoeSize;
        aoeLifetime = _aoeLifetime;

        weapon = _weapon;
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

    public virtual void Explode(Collider col)
    {
        if (explosionTimer > 0) return;

        explosionTimer = explosionCooldown;

        AudioManager.Play(explosionAudio, true);

        weapon.CreateAoe(enemyTag, aoePrefab, transform.position, Quaternion.identity, missileDamage, aoeLifetime, aoeSize, GetComponentInChildren<Renderer>().material);
    }
}
