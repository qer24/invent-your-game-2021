using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    protected Projectile projectile;

    public virtual void Start()
    {
        projectile = GetComponent<Projectile>();

        projectile.OnDespawn += DestroyBehaviour;
    }

    void DestroyBehaviour()
    {
        projectile.OnDespawn -= DestroyBehaviour;

        Destroy(this);
    }
}
