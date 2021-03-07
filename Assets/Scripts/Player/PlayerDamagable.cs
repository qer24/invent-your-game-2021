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
    public float immunityTime = 0.05f;
    public Collider col;

    float immunityTimer = 0;

    bool immune = false;

    private void Update()
    {
        if (!immune) return;

        immunityTimer += Time.deltaTime;

        if (immunityTimer > immunityTime)
        {
            col.enabled = true;
            immune = false;
            immunityTimer = 0;
        }
    }

    public virtual void TakeDamage(float amount)
    {
        if (hp.isDead) return;
        if (!enabled) return;
        if (immune) return;

        CinemachineShake.Instance.ShakeCamera(amount * 2f, 0.6f);
        //TimeStopManager.Instance.FreezeTime(onHitFreezeTime);
        //onHitParticles.Play();

        OnTakeDamage?.Invoke();
        hp.RemoveHealth(amount);

        col.enabled = false;
        immune = true;
    }
}
