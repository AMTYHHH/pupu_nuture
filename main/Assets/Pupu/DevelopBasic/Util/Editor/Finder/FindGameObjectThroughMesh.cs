using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 public class FindGameObjectThroughMesh : EditorWindow {
    Mesh mesh;
    string stArea = "Empty List";

    [MenuItem("Tools/Finders/Find GameObject Through Mesh")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(FindGameObjectThroughMesh));
    }
  
     public void OnGUI() {
        GUILayout.Label("Enter shader to find:");
        mesh = EditorGUILayout.ObjectField(mesh, typeof(Mesh), false) as Mesh;
        if(mesh==null) return;
        if (GUILayout.Button("Find Objects")) {
            FindObjects(mesh);
        }
        GUILayout.Label(stArea);
     }
     
     private void FindObjects(Mesh m_mesh) {
        bool IfSelectObject = false;
        if(Selection.gameObjects.Length!=0 && Selection.activeGameObject){
            IfSelectObject = true;
        }

        int count = 0;
        stArea = "Objects using mesh: " + mesh.name+":\n\n";
        var goArray = (MeshFilter[])Resources.FindObjectsOfTypeAll(typeof(MeshFilter));
        var goList = new System.Collections.Generic.List<GameObject>();
         
        foreach (MeshFilter meshFilter in goArray) {
            if(IfSelectObject && !meshFilter.transform.IsChildOf(Selection.activeGameObject.transform))
                continue;
            if(meshFilter.sharedMesh == m_mesh){
                count ++;
                goList.Add(meshFilter.gameObject);
            }
        }

        Selection.objects = goList.ToArray();
         
        stArea += "\n"+count + " objects using mesh: " + mesh.name + " found.";
     }
 }
