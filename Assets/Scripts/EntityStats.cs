using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Entity Stats")]
public class EntityStats : ScriptableObject
{
    public int maxHealth;
    public int damage;
}
