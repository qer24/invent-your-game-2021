using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Card", menuName = "ScriptableObjects/Enemy Card")]
public class EnemyCard : ScriptableObject
{
    [Header("Basic variables")]
    public GameObject prefab;
    public int weight = 1;
    public int cost = 1;

    [Header("Conditions //ignore for now")]
    public bool canSpawn = true;

    [Header("Defensive stats")]
    public int maxHealth;

    [Header("Projectile stats")]
    public float projectileDamage = 5f;
    public float projectileSpeed = 60;
    public float projectileLifetime = 2f;
}
