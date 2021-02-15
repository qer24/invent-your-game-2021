using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class RoomCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Room room = (Room)target;

        GUILayout.Label("Waves to spawn: ");

        if (room.waves.Count > 0)
        {
            for (int i = 0; i < room.waves.Count; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                room.waves[i] = (Wave)EditorGUILayout.ObjectField(room.waves[i], typeof(Wave), true);
                if (EditorGUI.EndChangeCheck())
                {
                    if (PrefabUtility.GetPrefabAssetType(room.gameObject) != PrefabAssetType.NotAPrefab)
                        PrefabUtility.SavePrefabAsset(room.gameObject);
                }

                if (room.waves[i] != null)
                {
                    if (GUILayout.Button("Edit", GUILayout.MaxWidth(40)))
                    {
                        WaveCustomEditorWindow.Open(room.waves[i]);
                        if (PrefabUtility.GetPrefabAssetType(room.gameObject) != PrefabAssetType.NotAPrefab)
                            PrefabUtility.SavePrefabAsset(room.gameObject);
                    }
                }
                else
                {
                    if (GUILayout.Button("New", GUILayout.MaxWidth(40)))
                    {
                        room.waves[i] = CreateNewWaveObject();
                        if (PrefabUtility.GetPrefabAssetType(room.gameObject) != PrefabAssetType.NotAPrefab)
                            PrefabUtility.SavePrefabAsset(room.gameObject);
                    }
                }

                if (GUILayout.Button("X", GUILayout.MaxWidth(25)))
                {
                    room.waves.RemoveAt(i);
                    if (PrefabUtility.GetPrefabAssetType(room.gameObject) != PrefabAssetType.NotAPrefab)
                        PrefabUtility.SavePrefabAsset(room.gameObject);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            }
        }

        if (GUILayout.Button("Add wave"))
        {
            room.waves.Add(null);
            if (PrefabUtility.GetPrefabAssetType(room.gameObject) != PrefabAssetType.NotAPrefab)
                PrefabUtility.SavePrefabAsset(room.gameObject);
        }

        GUILayout.Space(25);

        room.maxTimeBetweenWaves = EditorGUILayout.FloatField("Max time between waves: ", room.maxTimeBetweenWaves);
    }

    Wave CreateNewWaveObject()
    {
        Wave asset = CreateInstance<Wave>();

        AssetDatabase.CreateAsset(asset, "Assets/Scripts/_ScriptableObjects/Waves/NewWave.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        return asset;
    }
}
