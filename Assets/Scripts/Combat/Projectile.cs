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
    [HideInInspector] public float lifeTimeLeft = 5f;

    [HideInInspector] public float ingoreCollisionTime = 0f;
    [HideInInspector] public float scaleThreshold = 2f;
    [HideInInspector] public float disableDamageThreshold = 1f;

    [HideInInspector] public string enemyTag = "Enemy";

    [HideInInspector] public Vector3 startScale;

    [HideInInspector] public bool despawnOnCollision = true;

    public Action OnDespawn;
    public Action<Collider> OnCollision;
    public Action OnUpdate;

    public Action<float, Transform> OnDealDamage { get; set; }

    float damageCooldown = 0f;

    public void Init(float _damage, float _velocity, float _lifetime, string _enemyTag, float _ignoreCollisionTime = 0f)
    {
        damage = _damage;
        velocity = _velocity;
        totalLifeTime = _lifetime;
        enemyTag = _enemyTag;

        lifeTimeLeft = totalLifeTime;

        startScale = transform.localScale;
        scaleThreshold = totalLifeTime * 0.6f;
        disableDamageThreshold = totalLifeTime * 0.2f;

        ingoreCollisionTime = _ignoreCollisionTime;

        despawnOnCollision = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(ingoreCollisionTime > 0)
        {
            ingoreCollisionTime -= Time.deltaTime;
        }

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
        damageCooldown -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (lifeTimeLeft < disableDamageThreshold || ingoreCollisionTime > 0 || damageCooldown > 0) return;
        damageCooldown = 0.1f;
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
