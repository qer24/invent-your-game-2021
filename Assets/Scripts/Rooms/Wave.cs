using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
public class Wave : ScriptableObject
{
    public EnemySpawn[] enemiesToSpawn;
}
