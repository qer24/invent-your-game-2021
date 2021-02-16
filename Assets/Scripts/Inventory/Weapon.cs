using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

public enum WeaponRarities
{
    Common, //grey
    Rare, //blue
    Epic, //purple
    Legendary, //gold
    Mythic //red
}

public abstract class Weapon : MonoBehaviour
{
    [Header("Tooltip")]
    public new string name;
    [TextArea] public string description;

    [Header("Generic variables")]
    public float baseDamage = 10;
    public float baseAttackRate = 0.5f;
    public float AttackRatePerSecond { get => 1/baseAttackRate; }
    public bool isCharged = false;
    public bool isProjectile = true;
    public WeaponRarities rarity = WeaponRarities.Common;
    public string rarityString = "Common";
    public int modSlots = 0;

    [Header("Charged weapon variables")]
    public float timeToCharge = 0.5f;

    [Header("Projectile weapon variables")]
    public GameObject projectilePrefab = null;
    public float projectileSpeed = 80f;
    public float projectileLifetime = 2f;

    public Action OnTooltipUpdate;

    public virtual void Start()
    {
        UpdateRarityString();
    }

    public void UpdateName(string newName)
    {
        name = newName;

        UpdateRarityString();
    }

    public void UpdateDescription(string newDesc)
    {
        description = newDesc;
    }

    void UpdateRarityString()
    {
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Rarity Text", rarity.ToString());
        if (op.IsDone)
        {
            rarityString = op.Result;
            OnTooltipUpdate?.Invoke();
        }
        else
        {
            op.Completed += (o) => 
            {
                rarityString = op.Result;
                OnTooltipUpdate?.Invoke();
            };
        }
    }

    //non projectile weapons
    public virtual void Attack()
    {
        Debug.LogWarning("Attack not implemented");
    }

    //projectile weapons
    public virtual void Shoot(Vector3 position, Quaternion rotation, string allyTag, string enemyTag, float projectileRotationSpeed, float projectileSeekDistance, Material projectileMaterial)
    {
        Debug.LogWarning("Projectile attack not implemented");
    }
}
