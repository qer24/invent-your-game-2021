using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    string selectedPropertyPath;
    protected SerializedProperty selectedProperty;

    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach (SerializedProperty p in prop)
        {
            if(p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }else
            {
                if(!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; }

                lastPropPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }

    protected void DrawSidebar(SerializedProperty prop)
    {
        foreach (SerializedProperty p in prop)
        {
            var path = p.propertyPath;
            int pos = int.Parse(path.Split('[').LastOrDefault().TrimEnd(']'));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(ObjectNames.NicifyVariableName($"Item {pos + 1}")))
            {
                selectedPropertyPath = p.propertyPath;
            }
            if (GUILayout.Button("X", GUILayout.MaxWidth(25)))
            {
                prop.DeleteArrayElementAtIndex(pos);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("New item"))
        {
            prop.arraySize++;
        }

        if (!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
        }
    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
}
