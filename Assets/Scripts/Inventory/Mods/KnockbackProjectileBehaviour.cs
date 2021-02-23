using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackProjectileBehaviour : ProjectileBehaviour
{
    static float knockbackForce = 700f;

    public override void Start()
    {
        base.Start();

        projectile.OnCollision += KnockbackEnemy;
    }

    private void OnDestroy()
    {
        projectile.OnCollision -= KnockbackEnemy;
    }

    void KnockbackEnemy(Collider col)
    {
        if(col.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.AddForce(projectile.transform.forward * knockbackForce);
        }
    }
}
