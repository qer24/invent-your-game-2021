using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnDamageBehaviour : MonoBehaviour
{
    protected IDamager damager;

    public virtual void OnEnable()
    {
        damager = GetComponentInParent<IDamager>();
    }

    public virtual void OnDisable()
    {
        Destroy(gameObject);
    }
}
