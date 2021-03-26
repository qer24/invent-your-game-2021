using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aoe : MonoBehaviour, IDamager
{
    public LeanTweenType inType;
    public LeanTweenType outType;

    Vector3 startScale;

    string enemyTag = "Enemy";
    float damage = 0;

    public Action OnDespawn;

    public bool constantDamage = false;
    public int ticksPerDamage = 30;
    int currentTick = 0;

    public Action<float, Transform> OnDealDamage { get; set; }

    public void Init(float _damage, float _lifetime, Vector3 _size, string _enemyTag)
    {
        damage = _damage;
        enemyTag = _enemyTag;

        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, _size, _lifetime).setEase(inType).setOnComplete(
            () => LeanTween.scale(gameObject, Vector3.zero, _lifetime * 0.25f).setEase(outType).setOnComplete(Despawn)
            );
    }

    public void Init(float _damage, float _lifetime, Vector3 _size, string _enemyTag, Action ScaleFunction)
    {
        damage = _damage;
        enemyTag = _enemyTag;

        ScaleFunction();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (constantDamage) return;

        if (col.CompareTag(enemyTag))
        {
            if (col.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(damage);
                OnDealDamage?.Invoke(damage, col.transform);
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (!constantDamage) return;

        if (col.CompareTag(enemyTag))
        {
            currentTick++;

            if (currentTick >= ticksPerDamage)
            {
                currentTick = 0;
                if (col.TryGetComponent<IDamagable>(out var damagable))
                {
                    damagable.TakeDamage(damage);
                    OnDealDamage?.Invoke(damage, col.transform);
                }
            }
        }
    }

    public void Despawn()
    {
        OnDespawn?.Invoke();

        foreach (var behaviour in GetComponentsInChildren<OnDamageBehaviour>())
        {
            Destroy(behaviour);
        }

        LeanPool.Despawn(gameObject);
    }
}
