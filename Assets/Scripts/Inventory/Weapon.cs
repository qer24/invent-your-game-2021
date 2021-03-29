using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
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

    public List<float> chargeTimeModifiers;
    public List<float> sizeModifiers;
    public List<float> fireRateModifiers;

    public List<GameObject> OnDamageBehaviours;

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
    public float FinalFireRate
    {
        get
        {
            if(fireRateModifiers.Count == 0)
                return Mathf.Round((baseAttackRate / RarityMultiplier) * 100f) / 100f;

            float totalModifier = 1;
            foreach (var modifier in fireRateModifiers)
            {
                totalModifier += modifier - 1;
            }

            return Mathf.Round(((baseAttackRate / RarityMultiplier) / totalModifier) * 100f) / 100f;
        }
    }
    public float FinalChargeTime
    {
        get
        {
            if (chargeTimeModifiers.Count == 0) return Mathf.Round((timeToCharge / (RarityMultiplier * RarityMultiplier)) * 100f) / 100f; ;

            float totalChargeTimeModifier = 1;
            foreach (var modifier in chargeTimeModifiers)
            {
                totalChargeTimeModifier += modifier - 1;
            }

            return Mathf.Round((timeToCharge / (RarityMultiplier * RarityMultiplier) * totalChargeTimeModifier) * 100f) / 100f;
        }
    }
    public Vector3 FinalSize
    {
        get
        {
            if(sizeModifiers.Count == 0) return new Vector3(size.x, 1, size.y);

            float totalSizeModifier = 1;
            foreach (var modifier in sizeModifiers)
            {
                totalSizeModifier += modifier - 1;
            }

            return new Vector3(size.x * totalSizeModifier, 1, size.y * totalSizeModifier);
        }
    }

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

    protected Camera mainCam;
    protected PlayerShooter playerShooter;

    private void Awake()
    {
        damageModifiers = new List<DamageModifier>();
        damageModifiers.Add(new DamageModifier(0, RarityMultiplier));

        UpdateRarityString();

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) => mainCam = CameraManager.Instance.mainCam;
        mainCam = CameraManager.Instance.mainCam;
        playerShooter = PlayerPersistencyMenager.Instance.GetComponent<PlayerShooter>();
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
        damageModifiers.Add(new DamageModifier(0, 1 + 0.1f * (DifficultyManager.Instance.currentDifficulty - 1)));

        StartCoroutine(nameof(ForceReloadTooltip));
        UpdateRarityString();
    }

    public System.Collections.IEnumerator ForceReloadTooltip()
    {
        yield return null;

        foreach (var str in GetComponents<LocalizeStringEvent>())
        {
            str.RefreshString();
        }

        yield return null;

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

        foreach (var behaviour in OnDamageBehaviours)
        {
            Instantiate(behaviour, go.transform).SetActive(true);
        }

        OnProjectileCreated?.Invoke(go);
    }

    public void ShootProjectile(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial, out GameObject projectileGameObject,float damageMultiplier = 1, float speedMultiplier = 1, float sizeMultiplier = 1)
    {
        GameObject go = LeanPool.Spawn(projectilePrefab, position, rotation);

        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * projectileSpeed * speedMultiplier);
        go.GetComponent<Projectile>().Init(FinalDamage * damageMultiplier, projectileSpeed * speedMultiplier, projectileLifetime, enemyTag);
        go.GetComponentInChildren<Renderer>().material = projectileMaterial;

        go.transform.localScale *= sizeMultiplier;

        foreach (var behaviour in OnDamageBehaviours)
        {
            Instantiate(behaviour, go.transform).SetActive(true);
        }

        projectileGameObject = go;

        OnProjectileCreated?.Invoke(go);
    }

    public void CreateAoe(string enemyTag, GameObject prefab, Vector3 pos, Quaternion rot, float damage, float lifeTime, Vector3 size)
    {
        GameObject go = LeanPool.Spawn(prefab, pos, rot);
        go.GetComponent<Aoe>().Init(damage, lifeTime, size, enemyTag);

        foreach (var behaviour in OnDamageBehaviours)
        {
            Instantiate(behaviour, go.transform).SetActive(true);
        }
    }
}
