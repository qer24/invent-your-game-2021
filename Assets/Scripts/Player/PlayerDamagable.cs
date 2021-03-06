using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDamagable : MonoBehaviour, IDamagable
{
    //[SerializeField] ParticleSystem onHitParticles = null;
    //[SerializeField] float onHitFreezeTime = 0.25f;

    public Health hp;
    public static Action OnTakeDamage;
    public float immunityTime = 0.05f;
    public Collider col;

    public Volume hitPostProcess;
    public float hitPostProcessTime = 1f;

    float immunityTimer = 0;

    bool immune = false;

    [HideInInspector] public float damageToTake = 0;
    public Action<float> OnPlayerTakeDamage;

    private void Update()
    {
        if (!immune) return;

        immunityTimer += Time.deltaTime;

        if (immunityTimer > immunityTime)
        {
            immune = false;
            immunityTimer = 0;
        }
    }

    public virtual void TakeDamage(float amount)
    {
        EndScreen.DamageTaken += (int)amount;
        if (hp.isDead) return;
        if (!enabled) return;
        if (immune) return;

        damageToTake = amount;
        OnPlayerTakeDamage?.Invoke(damageToTake);

        if (damageToTake <= 0) return;

        AudioManager.Play("event:/SFX/Player/PlayerHit", true);
        CinemachineShake.Instance.ShakeCamera(damageToTake * 2f, 0.6f);

        LeanTween.cancel(hitPostProcess.gameObject);
        hitPostProcess.weight = 1;
        LeanTween.value(hitPostProcess.gameObject, 1, 0, hitPostProcessTime).setOnUpdate((float value) => hitPostProcess.weight = value).setIgnoreTimeScale(true);
        //TimeStopManager.Instance.FreezeTime(onHitFreezeTime);
        //onHitParticles.Play();

        OnTakeDamage?.Invoke();
        hp.RemoveHealth(damageToTake);

        immune = true;
    }
}
