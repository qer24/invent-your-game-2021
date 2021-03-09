using UnityEngine;

public class Stats : ScriptableObject
{
    [Header("Health")]
    public int maxHealth;
}

[CreateAssetMenu(fileName = "Enemy Card", menuName = "ScriptableObjects/Enemy Card")]
public class EnemyCard : Stats
{
    [Header("Basic variables")]
    public Enemy prefab;
    public int weight = 1;
    public int cost = 1;
    [FMODUnity.EventRef]
    public string onTakeDamageAudio;
    [FMODUnity.EventRef]
    public string onDeathAudio;

    [Header("Conditions //ignore for now")]
    public bool canSpawn = true;

    [Header("Projectile stats")]
    public GameObject projectile;
    public float projectileDamage = 5f;
    public float projectileSpeed = 60;
    public float projectileLifetime = 2f;

    [Header("Other stats")]
    public float nonProjectileDamage = 0f;
}
