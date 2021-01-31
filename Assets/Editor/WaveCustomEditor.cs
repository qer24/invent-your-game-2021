using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        Wave obj = EditorUtility.InstanceIDToObject(instanceId) as Wave;
        if(obj != null)
        {
            WaveCustomEditorWindow.Open(obj);
            return true;
        }

        return false;
    }
}

[CustomEditor(typeof(Wave))]
public class WaveCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Edit wave"))
        {
            WaveCustomEditorWindow.Open((Wave)target);
        }
    }
}
