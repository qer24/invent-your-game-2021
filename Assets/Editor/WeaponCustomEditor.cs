using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon), true)]
public class WeaponCustomEditor : Editor
{
    string[] propertiesInBaseClass;
    private Object go;

    SerializedProperty audio;

    private void OnEnable()
    {
        FieldInfo[] fields = typeof(Weapon).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        propertiesInBaseClass = new string[fields.Length];
        for (int i = 0; i < fields.Length; i++)
        {
            propertiesInBaseClass[i] = fields[i].Name;
        }

        audio = serializedObject.FindProperty("onAttackAudio");
    }

    public override void OnInspectorGUI()
    {
        Weapon weapon = (Weapon)target;

        EditorGUI.BeginChangeCheck();

        //EditorGUILayout.LabelField("Tooltip", EditorStyles.boldLabel);

        //weapon.name = EditorGUILayout.TextField("Name", weapon.name);

        //EditorGUILayout.LabelField("Description");
        //weapon.description = EditorGUILayout.TextArea(weapon.description, GUILayout.MinHeight(40));

        //EditorGUILayout.Space();

        EditorGUILayout.LabelField("Generic variables", EditorStyles.boldLabel);
        weapon.baseDamage = EditorGUILayout.FloatField("Base Damage", weapon.baseDamage);
        weapon.baseAttackRate = EditorGUILayout.FloatField("Base Attack Rate", weapon.baseAttackRate);
        //weapon.onAttackAudio = EditorGUILayout.TextField("On attack audio", weapon.onAttackAudio);
        EditorGUILayout.PropertyField(audio, new GUIContent("Weapon Audio"));
        weapon.rodzajnik = (Rodzajniki)EditorGUILayout.EnumPopup("Rodzajnik", weapon.rodzajnik);
        EditorGUILayout.Space();

        weapon.isCharged = EditorGUILayout.Toggle("Is Charged", weapon.isCharged);
        if (weapon.isCharged)
        {
            weapon.timeToCharge = EditorGUILayout.FloatField("Time to charge", weapon.timeToCharge);
            EditorGUILayout.Space();
        }

        weapon.isProjectile = EditorGUILayout.Toggle("Is Projectile", weapon.isProjectile);

        if (weapon.isProjectile)
        {
            //EditorGUILayout.LabelField("Projectile weapon variables", EditorStyles.boldLabel);
            weapon.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", weapon.projectilePrefab, typeof(GameObject), true);
            weapon.projectileSpeed = EditorGUILayout.FloatField("Projectile Speed", weapon.projectileSpeed);
            weapon.projectileLifetime = EditorGUILayout.FloatField("Projectile Lifetime", weapon.projectileLifetime);
            EditorGUILayout.Space();
        }

        weapon.isAoe = EditorGUILayout.Toggle("Is Area Of Effect", weapon.isAoe);

        if (weapon.isAoe)
        {
            weapon.aoePrefab = (GameObject)EditorGUILayout.ObjectField("Aoe Prefab", weapon.aoePrefab, typeof(GameObject), true);
            weapon.size = EditorGUILayout.Vector2Field("Size", weapon.size);
            weapon.aoeLifeTime = EditorGUILayout.FloatField("Aoe Lifetime", weapon.aoeLifeTime);
            EditorGUILayout.Space();
        }

        if (!weapon.isProjectile && !weapon.isCharged)
        {
            EditorGUILayout.Space();
        }

        DrawPropertiesExcluding(serializedObject, propertiesInBaseClass);

        if (EditorGUI.EndChangeCheck())
        {
            if (PrefabUtility.GetPrefabAssetType(weapon.gameObject) != PrefabAssetType.NotAPrefab)
                PrefabUtility.SavePrefabAsset(weapon.gameObject);
            Undo.RegisterCompleteObjectUndo(weapon, "Undo change weapon");
        }
    }
}
