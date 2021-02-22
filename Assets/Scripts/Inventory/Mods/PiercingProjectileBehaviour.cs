using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingProjectileBehaviour : ProjectileBehaviour
{
    public override void Start()
    {
        base.Start();

        projectile.OnCollision += DisableDespawnOnCollision;
    }

    private void OnDestroy()
    {
        projectile.OnCollision -= DisableDespawnOnCollision;
    }

    void DisableDespawnOnCollision(Collider col)
    {
        projectile.despawnOnCollision = false;
    }
}
