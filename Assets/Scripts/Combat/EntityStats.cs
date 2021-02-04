using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Entity Stats")]
public class EntityStats : ScriptableObject
{
    public int maxHealth;

    public float projectileDamage = 5f;
    public float projectileSpeed = 60;
    public float projectileLifetime = 2f;
    public float projectileRotationSpeed = 0f;
    public float projectileSeekDistance = 0f;
}
