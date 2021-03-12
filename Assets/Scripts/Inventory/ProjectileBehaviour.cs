using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    protected Projectile projectile;

    public bool disableOnDespawn = true;

    public virtual void Start()
    {
        projectile = GetComponent<Projectile>();

        if(disableOnDespawn)
            projectile.OnDespawn += DestroyBehaviour;
    }

    public virtual void DestroyBehaviour()
    {
        projectile.OnDespawn -= DestroyBehaviour;

        Destroy(this);
    }
}
