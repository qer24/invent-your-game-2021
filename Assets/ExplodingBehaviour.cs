using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBehaviour : OnDamageBehaviour
{
    public GameObject explosionPrefab;
    public float explosionLifeTime;
    public Vector2 explosionSize = Vector2.one;

    Vector3 convertedSize { get => new Vector3(explosionSize.x, 1, explosionSize.y); }

    private void Start()
    {
        damager.OnDealDamage += Explode;
    }

    private void OnDestroy()
    {
        if (damager != null)
        {
            damager.OnDealDamage -= Explode;
        }
    }

    void Explode(float damage, Transform damagable)
    {
        if (damagable.TryGetComponent(out Enemy enemy))
        {
            if (enemy.health.currentHealth <= 0)
            {
                GameObject go = LeanPool.Spawn(explosionPrefab, transform.position, transform.rotation);
                go.GetComponent<Aoe>().Init(damage, explosionLifeTime, convertedSize, "Enemy");
            }
        }
    }
}
