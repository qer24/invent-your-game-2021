using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using System;

public interface IDamager
{
    Action<float, Transform> OnDealDamage { get; set; }
}

public class Projectile : MonoBehaviour, IDamager
{
    public Rigidbody rb = null;
    [SerializeField] AnimationCurve scalingCurve = null;
    [SerializeField] GameObject impactPrefab = null;

    [HideInInspector] public float damage = 0;

    [HideInInspector] public float velocity = 500;
    [HideInInspector] public float totalLifeTime = 5f;
    float lifeTimeLeft = 5f;

    float scaleThreshold = 2f;
    float disableDamageThreshold = 1f;

    [HideInInspector] public string enemyTag = "Enemy";

    Vector3 startScale;

    [HideInInspector] public bool despawnOnCollision = true;

    public Action OnDespawn;
    public Action<Collider> OnCollision;
    public Action OnUpdate;

    public Action<float, Transform> OnDealDamage { get; set; }

    public void Init(float _damage, float _velocity, float _lifetime, string _enemyTag)
    {
        damage = _damage;
        velocity = _velocity;
        totalLifeTime = _lifetime;
        enemyTag = _enemyTag;

        lifeTimeLeft = totalLifeTime;

        startScale = transform.localScale;
        scaleThreshold = totalLifeTime * 0.6f;
        disableDamageThreshold = totalLifeTime * 0.2f;

        despawnOnCollision = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        rb.velocity = transform.forward.normalized * velocity;

        lifeTimeLeft -= Time.deltaTime;
        if (lifeTimeLeft < 0)
        {
            Despawn();
            return;
        }

        if (lifeTimeLeft < scaleThreshold)
        {
            Vector3 scale = Vector3.Lerp(startScale, Vector3.zero, 1 - scalingCurve.Evaluate((scaleThreshold - lifeTimeLeft) / scaleThreshold));
            transform.localScale = scale;
        }

        OnUpdate?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lifeTimeLeft < disableDamageThreshold) return;
        if (other.CompareTag(enemyTag))
        {
            OnCollision?.Invoke(other);

            if (impactPrefab != null)
            {
                GameObject go = LeanPool.Spawn(impactPrefab, other.transform.position + Vector3.up * 3f, Quaternion.identity);
                LeanPool.Despawn(go, 2f);
            }

            if(despawnOnCollision)
                Despawn();

            if (other.TryGetComponent<IDamagable>(out var damagable) && damage > 0)
            {
                damagable.TakeDamage(damage);
                OnDealDamage?.Invoke(damage, other.transform);   
            }
        }
    }

    protected void Despawn()
    {
        OnDespawn?.Invoke();

        foreach (var item in GetComponentsInChildren<OnDamageBehaviour>())
        {
            Destroy(item);
        }

        LeanPool.Despawn(gameObject);
    }
}
