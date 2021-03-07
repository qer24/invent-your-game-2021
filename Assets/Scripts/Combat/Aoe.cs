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

    public Action OnDespawn;

    public void Init(float _damage, float _lifetime, Vector3 _size, string _enemyTag)
    {
        damage = _damage;
        enemyTag = _enemyTag;

        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, _size, _lifetime).setEase(inType).setOnComplete(
            () => LeanTween.scale(gameObject, Vector3.zero, _lifetime * 0.25f).setEase(outType).setOnComplete(Despawn)
            );
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(enemyTag))
        {
            if (col.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
    }

    void Despawn()
    {
        OnDespawn?.Invoke();

        LeanPool.Despawn(gameObject);
    }
}
