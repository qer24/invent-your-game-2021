using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aoe : MonoBehaviour
{
    public LeanTweenType inType;
    public LeanTweenType outType;

    Vector3 startScale;

    string enemyTag = "Enemy";
    float damage = 0;
    float radius = 1;

    public Action OnDespawn;

    public void Init(float _damage, float _lifetime, float _radius, string _enemyTag)
    {
        damage = _damage;
        radius = _radius;
        enemyTag = _enemyTag;

        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, new Vector3(radius, radius, radius), _lifetime).setEase(inType).setOnComplete(
            () => LeanTween.scale(gameObject, Vector3.zero, _lifetime * 0.25f).setEase(outType).setOnComplete(Despawn)
            );
        AoeDamage();
    }

    public void AoeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in colliders)
        {
            if(col.CompareTag(enemyTag))
            {
                if (col.TryGetComponent<IDamagable>(out var damagable))
                {
                    damagable.TakeDamage(damage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Despawn()
    {
        OnDespawn?.Invoke();

        LeanPool.Despawn(gameObject);
    }
}
