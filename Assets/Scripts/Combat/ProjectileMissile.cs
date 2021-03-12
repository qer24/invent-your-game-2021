using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMissile : Projectile
{
    GameObject aoePrefab;
    float aoeLifetime;
    Vector3 aoeSize;
    float missileDamage = 0;

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
        OnCollision += Explode;
    }

    private void OnDestroy()
    {
        OnCollision -= Explode;
    }

    public void Explode(Collider col)
    {
        Aoe aoe = Lean.Pool.LeanPool.Spawn(aoePrefab, transform.position, Quaternion.identity).GetComponent<Aoe>();
        aoe.Init(missileDamage, aoeLifetime, aoeSize, enemyTag);
    }
}
