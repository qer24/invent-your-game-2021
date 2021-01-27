using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagable : MonoBehaviour, IDamagable
{
    //[SerializeField] ParticleSystem onHitParticles = null;
    //[SerializeField] float onHitFreezeTime = 0.25f;

    public Health hp;
    public static Action OnTakeDamage;

    public virtual void TakeDamage(float amount)
    {
        if (hp.isDead) return;
        if (!enabled) return;

        CinemachineShake.Instance.ShakeCamera(amount, 0.6f);
        //TimeStopManager.Instance.FreezeTime(onHitFreezeTime);
        //onHitParticles.Play();

        OnTakeDamage?.Invoke();
        hp.RemoveHealth(amount);
    }
}
