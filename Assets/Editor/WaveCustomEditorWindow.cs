using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProcGen
{
    public class WaveCustomEditorWindow : ExtendedEditorWindow
    {
        static string newName = null;

        public static void Open(Wave waveObject)
        {
            WaveCustomEditorWindow window = GetWindow<WaveCustomEditorWindow>("Wave editor");
            window.serializedObject = new SerializedObject(waveObject);

            newName = window.serializedObject.targetObject.name;
        }

        private void OnGUI()
        {
            newName = EditorGUILayout.TextField("Name: ", newName);

            currentProperty = serializedObject.FindProperty("enemiesToSpawn");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

            DrawSidebar(currentProperty);

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

            if (selectedProperty != null)
            {
                DrawProperties(selectedProperty, true);
            }
            else
            {
                EditorGUILayout.LabelField("Select an item from the list");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            Apply();
        }

        void OnDestroy()
        {
            var scriptableObject = serializedObject.targetObject;

            if (newName == scriptableObject.name) return;

            string assetPath = AssetDatabase.GetAssetPath(scriptableObject.GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, newName);
            AssetDatabase.SaveAssets();
        }
    }
}
