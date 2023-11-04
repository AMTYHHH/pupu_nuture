using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 public class FindGameObjectThroughMaterial : EditorWindow {
    Material material;
    string stArea = "Empty List";

    [MenuItem("Tools/Finders/Finder Gameobject through Material")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(FindGameObjectThroughMaterial));
    }
  
     public void OnGUI() {
        GUILayout.Label("Enter shader to find:");
        material = EditorGUILayout.ObjectField(material, typeof(Material), false) as Material;
        if(material==null) return;
        if (GUILayout.Button("Find Objects")) {
            FindObjects(material);
        }
        GUILayout.Label(stArea);
     }
     
     private void FindObjects(Material material) {
        bool IfSelectObject = false;
        if(Selection.gameObjects.Length!=0 && Selection.activeGameObject){
            IfSelectObject = true;
        }

        int count = 0;
        stArea = "Objects using material: " + material.name+":\n\n";
        var goArray = (Renderer[])Resources.FindObjectsOfTypeAll(typeof(Renderer));
        var goList = new System.Collections.Generic.List<GameObject>();
         
        foreach (Renderer rend in goArray) {
            if(IfSelectObject && !rend.transform.IsChildOf(Selection.activeGameObject.transform))
                continue;
            foreach (Material mat in rend.sharedMaterials) {
                if(mat == material){
                    count++;
                    goList.Add(rend.gameObject);
                }
            }
        }

        Selection.objects = goList.ToArray();
         
        stArea += "\n"+count + " objects using material " + material.name + " found.";
     }
 }
