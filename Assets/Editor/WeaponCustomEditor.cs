using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon), true)]
public class WeaponCustomEditor : Editor
{
    string[] propertiesInBaseClass;

    private void OnEnable()
    {
        FieldInfo[] fields = typeof(Weapon).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        propertiesInBaseClass = new string[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            propertiesInBaseClass[i] = fields[i].Name;
        }
    }

    public override void OnInspectorGUI()
    {
        Weapon weapon = (Weapon)target;

        EditorGUILayout.LabelField("Tooltip", EditorStyles.boldLabel);

        weapon.name = EditorGUILayout.TextField("Name", weapon.name);

        EditorGUILayout.LabelField("Description");
        weapon.description = EditorGUILayout.TextArea(weapon.description, GUILayout.MinHeight(40));

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Generic variables", EditorStyles.boldLabel);
        weapon.baseDamage = EditorGUILayout.FloatField("Base Damage", weapon.baseDamage);
        weapon.baseAttackRate = EditorGUILayout.FloatField("Base Attack Rate", weapon.baseAttackRate);
        weapon.isCharged = EditorGUILayout.Toggle("IsCharged", weapon.isCharged);
        weapon.isProjectile = EditorGUILayout.Toggle("Is Projectile", weapon.isProjectile);

        EditorGUILayout.Space();

        if (weapon.isProjectile)
        {
            //EditorGUILayout.LabelField("Projectile weapon variables", EditorStyles.boldLabel);
            weapon.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("ProjectilePrefab", weapon.projectilePrefab, typeof(GameObject), true);
            weapon.projectileSpeed = EditorGUILayout.FloatField("Projectile Speed", weapon.projectileSpeed);
            weapon.projectileLifetime = EditorGUILayout.FloatField("Projectile Lifetime", weapon.projectileLifetime);
            EditorGUILayout.Space();
        }

        DrawPropertiesExcluding(serializedObject, propertiesInBaseClass);
    }
}
