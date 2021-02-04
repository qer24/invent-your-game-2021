using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int maxHealth;
    public int damage;
}
