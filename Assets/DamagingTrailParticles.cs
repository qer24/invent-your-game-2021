using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTrailParticles : MonoBehaviour
{
    ParticleSystem part;

    public float damage = 5;
    public float damageCooldown = 1;

    float hitCooldown;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        hitCooldown = 0;
    }

    private void Update()
    {
        if(hitCooldown >= 0)
            hitCooldown -= Time.deltaTime;
    }

    void OnParticleCollision(GameObject other)
    {
        if (hitCooldown > 0) return;
        if (other.tag != "Player") return;

        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.TakeDamage(damage);
            hitCooldown = damageCooldown;
        }
    }
}
