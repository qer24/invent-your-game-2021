using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaveCustomEditorWindow : ExtendedEditorWindow
{
    public static void Open(Wave waveObject)
    {
        WaveCustomEditorWindow window = GetWindow<WaveCustomEditorWindow>("Wave editor");
        window.serializedObject = new SerializedObject(waveObject);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("enemiesToSpawn");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSidebar(currentProperty);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        if(selectedProperty != null)
        {
            DrawProperties(selectedProperty, true);
        }else
        {
            EditorGUILayout.LabelField("Select an item from the list");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
    }
}
