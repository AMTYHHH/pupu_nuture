using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimpleSaveSystem;

public class FindISaveableThroughGuid : EditorWindow
{
    private string guidString;
    [MenuItem("Tools/Finders/Find ISaveable through guid")]
    public static void ShowWindow(){
        GetWindow<FindISaveableThroughGuid>("Find ISaveable through guid");
    }
    void OnGUI(){
        guidString = EditorGUILayout.TextField(guidString);
        if(GUILayout.Button("Find")){
            var goArray = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            var goList = new List<GameObject>();

            for(int i=0; i<goArray.Length; i++){
                var saveable = goArray[i].GetComponent<ISaveable>();
                if(saveable!=null && saveable.guid.ToString() == guidString){
                    goList.Add(goArray[i]);
                }
            }
            Selection.objects = goList.ToArray();
        }
    }
}
