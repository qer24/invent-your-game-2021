using UnityEngine;

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
    [Tooltip("Per second")] public float baseAttackRate = 0.5f;
    public bool isCharged = false;
    public bool isProjectile = true;

    [Header("Charged weapon variables")]
    public float timeToCharge = 0.5f;

    [Header("Projectile weapon variables")]
    public GameObject projectilePrefab = null;
    public float projectileSpeed = 80f;
    public float projectileLifetime = 2f;

    //non projectile weapons
    public virtual void Attack()
    {
        Debug.LogWarning("Attack not implemented");
    }

    //projectile weapons
    public virtual void Shoot(Vector3 position, Quaternion rotation, string allyTag, string enemyTag, float projectileRotationSpeed, float projectileSeekDistance, Material projectileMaterial)
    {
        Debug.LogWarning("Attack not implemented");
    }
}
