using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoFakeOutline))]
public class AutoFakeOutlineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("重新生成"))
        {
            AutoFakeOutline myScript = (AutoFakeOutline)target;
            myScript.StartGenearte();
        }
    }
}