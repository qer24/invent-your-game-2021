using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Random = UnityEngine.Random;

public enum WeaponRarities
{
    Common, //grey
    Rare, //blue
    Epic, //purple
    Legendary, //gold
    Mythic //red
}

public enum Rodzajniki
{
    Meski,
    Zenski,
    Nijaki
}


public struct DamageModifier
{
    public float addedDamage;
    public float damageMultiplier;

    public DamageModifier(float addedDamage = 0f, float damageMultiplier = 1.0f)
    {
        this.addedDamage = addedDamage;
        this.damageMultiplier = damageMultiplier;
    }
}

public abstract class Weapon : MonoBehaviour
{
    [Header("Tooltip")]
    public new string name;
    [TextArea] public string description;

    [Header("Generic variables")]
    public float baseDamage = 10;
    public float baseAttackRate = 0.5f;
    public float AttackRatePerSecond => Mathf.Round(1 / FinalFireRate * 100f) / 100f;
    public bool isCharged = false;
    public bool isProjectile = true;
    public bool isAoe = false;
    public WeaponRarities rarity = WeaponRarities.Common;
    public Rodzajniki rodzajnik = Rodzajniki.Meski;
    public string rodzajnikString = string.Empty;
    public string rarityString = "Common";
    public int modSlots = 2;
    [FMODUnity.EventRef]
    public string onAttackAudio;

    [Header("Charged weapon variables")]
    public float timeToCharge = 0.5f;

    [Header("Projectile weapon variables")]
    public GameObject projectilePrefab = null;
    public float projectileSpeed = 80f;
    public float projectileLifetime = 2f;

    [Header("Aoe weapon variables")]
    public GameObject aoePrefab = null;
    public Vector2 size = Vector2.one;
    public float aoeLifeTime = 0.25f;

    public Action OnTooltipUpdate;

    public Action OnAttack;
    public Action<Vector3, Quaternion, string, Material> OnProjectileAttack;

    public Action OnEquip;
    public List<DamageModifier> damageModifiers;
    public Action<GameObject> OnProjectileCreated;

    public float FinalDamage
    {
        get
        {
            if (damageModifiers.Count == 0) return baseDamage;

            float value = baseDamage;
            foreach (var modifier in damageModifiers)
            {
                value += modifier.addedDamage;
            }
            foreach (var modifier in damageModifiers)
            {
                value *= modifier.damageMultiplier;
            }

            return Mathf.Round(value * 100f) / 100f;
        }
    }
    public float FinalFireRate => Mathf.Round((baseAttackRate / RarityMultiplier) * 100f) / 100f;
    public float FinalChargeTime => Mathf.Round((timeToCharge / (RarityMultiplier * RarityMultiplier)) * 100f) / 100f;
    public Vector3 FinalSize => new Vector3(size.x, 1, size.y);

    float RarityMultiplier
    {
        get
        {
            switch (rarity)
            {
                case WeaponRarities.Common:
                    return 1f;
                case WeaponRarities.Rare:
                    return 1.1f;
                case WeaponRarities.Epic:
                    return 1.2f;
                case WeaponRarities.Legendary:
                    return 1.4f;
                case WeaponRarities.Mythic:
                    return 1.6f;
            }
            return 1f;
        }
    }

    private void Awake()
    {
        damageModifiers = new List<DamageModifier>();
        damageModifiers.Add(new DamageModifier(0, RarityMultiplier));

        UpdateRarityString();
    }

    public void GenerateWeapon()
    {
        float randomValue = Random.value;
        int modAmount = 1;
        if (randomValue <= 0.4)
        {
            rarity = WeaponRarities.Common;
            modAmount = Random.Range(1, 3);
        }
        else if (randomValue > 0.4 && randomValue <= 0.7f)
        {
            rarity = WeaponRarities.Rare;
            modAmount = Random.Range(2, 5);
        }
        else if (randomValue > 0.7 && randomValue <= 0.88f)
        {
            rarity = WeaponRarities.Epic;
            modAmount = Random.Range(3, 7);
        }
        else if (randomValue > 0.88f && randomValue <= 0.97f)
        {
            rarity = WeaponRarities.Legendary;
            modAmount = Random.Range(5, 8);
        }
        else if (randomValue > 0.97)
        {
            rarity = WeaponRarities.Mythic;
            modAmount = Random.Range(7, 9);
        }

        modSlots = modAmount;

        damageModifiers = new List<DamageModifier>();
        damageModifiers.Add(new DamageModifier(0, RarityMultiplier));

        StartCoroutine(nameof(ForceReloadTooltip));
        UpdateRarityString();
    }

    System.Collections.IEnumerator ForceReloadTooltip()
    {
        yield return new WaitForSeconds(0.1f);

        var currentLocale = LocalizationSettings.SelectedLocale;
        //hack to refresh the localised string database
        if (currentLocale.Identifier == "en")
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        else
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];

        yield return null;
        LocalizationSettings.SelectedLocale = currentLocale;

        OnTooltipUpdate?.Invoke();
    }

    public void UpdateName(string newName)
    {
        name = newName;

        UpdateRarityString();
    }

    public void UpdateDescription(string newDesc)
    {
        description = newDesc;

        UpdateRarityString();
    }

    public void UpdateRarityString()
    {
        var operation = LocalizationSettings.SelectedLocaleAsync;
        if (operation.IsDone)
        {
            if (operation.Result.Identifier == "en")
            {
                rodzajnikString = string.Empty;
            }
            else
            {
                rodzajnikString = GetFinishedRodzajnik();
            }
            OnTooltipUpdate?.Invoke();
        }
        else
        {
            operation.Completed += (o) =>
            {
                if (operation.Result.Identifier == "en")
                {
                    rodzajnikString = string.Empty;
                }
                else
                {
                    rodzajnikString = "e";
                }
                OnTooltipUpdate?.Invoke();
            };
        }

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
            };
        }
    }

    string GetFinishedRodzajnik()
    {
        switch (rodzajnik)
        {
            case Rodzajniki.Meski:
                if (rarity == WeaponRarities.Common || rarity == WeaponRarities.Legendary || rarity == WeaponRarities.Mythic)
                {
                    return "y";
                }else
                {
                    return "i";
                }
            case Rodzajniki.Zenski:
                return "a";
            case Rodzajniki.Nijaki:
                if (rarity == WeaponRarities.Common || rarity == WeaponRarities.Legendary || rarity == WeaponRarities.Mythic)
                {
                    return "e";
                }
                else
                {
                    return "ie";
                }
        }
        return string.Empty;
    }

    //non projectile weapons
    public virtual void Attack(string enemyTag)
    {
        Debug.LogWarning("Attack not implemented");
    }

    //projectile weapons
    public virtual void Shoot(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial)
    {
        OnProjectileAttack?.Invoke(position, rotation, enemyTag, projectileMaterial);
    }

    public void ShootProjectile(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial, float damageMultiplier = 1, float speedMultiplier = 1, float sizeMultiplier = 1)
    {
        GameObject go = LeanPool.Spawn(projectilePrefab, position, rotation);

        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * projectileSpeed * speedMultiplier);
        go.GetComponent<Projectile>().Init(FinalDamage * damageMultiplier, projectileSpeed * speedMultiplier, projectileLifetime, enemyTag);
        go.GetComponent<Renderer>().material = projectileMaterial;

        go.transform.localScale *= sizeMultiplier;

        OnProjectileCreated?.Invoke(go);
    }

    public void ShootProjectile(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial, out GameObject projectileGameObject,float damageMultiplier = 1, float speedMultiplier = 1, float sizeMultiplier = 1)
    {
        GameObject go = LeanPool.Spawn(projectilePrefab, position, rotation);

        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * projectileSpeed * speedMultiplier);
        go.GetComponent<Projectile>().Init(FinalDamage * damageMultiplier, projectileSpeed * speedMultiplier, projectileLifetime, enemyTag);
        go.GetComponentInChildren<Renderer>().material = projectileMaterial;

        go.transform.localScale *= sizeMultiplier;

        projectileGameObject = go;

        OnProjectileCreated?.Invoke(go);
    }
}
