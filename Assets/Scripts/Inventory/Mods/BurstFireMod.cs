using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFireMod : Mod
{
    public float burstDelay = 0.1f;
    public float damageMultiplier = 0.3f;

    DamageModifier damageModifier;
    PlayerShooter playerShooter;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        playerShooter = GetComponentInParent<PlayerShooter>();

        damageModifier = new DamageModifier(0, damageMultiplier);
        attachedWeapon.damageModifiers.Add(damageModifier);
        playerShooter.OnAttack += Burst;
    }

    private void OnDestroy()
    {
        if (attachedWeapon != null)
            playerShooter.OnAttack -= Burst;
    }

    public override void DetachWeapon()
    {
        if (attachedWeapon.isProjectile)
        {
            playerShooter.OnAttack -= Burst;
        }
        attachedWeapon.damageModifiers.Remove(damageModifier);

        base.DetachWeapon();
    }

    void Burst()
    {
        playerShooter.OnAttack -= Burst;

        Invoke(nameof(DelayedShot), burstDelay);
        Invoke(nameof(DelayedShot), burstDelay * 2f);
        Invoke(nameof(EnableBackBurst), burstDelay * 3f);
    }

    void DelayedShot()
    {
        playerShooter.Shoot(playerShooter.transform.rotation * Quaternion.AngleAxis(Random.Range(-5, 5), Vector3.up));
        playerShooter.shootTimer = attachedWeapon.FinalFireRate;
    }

    void EnableBackBurst()
    {
        playerShooter.OnAttack += Burst;
        playerShooter.shootTimer = attachedWeapon.FinalFireRate;
    }
}
