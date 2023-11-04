 using UnityEngine;
 using UnityEditor;
 using System.Collections.Generic;
 
 public class FindMaterialInSceneThroughShader : EditorWindow {
     Shader shader;
     string stArea = "Empty List";
  
     [MenuItem("Tools/Finders/Find Material through shader")]
     public static void ShowWindow() {
         EditorWindow.GetWindow(typeof(FindMaterialInSceneThroughShader));
     }
  
     public void OnGUI() {
         GUILayout.Label("Enter shader to find:");
         shader = EditorGUILayout.ObjectField(shader, typeof(Shader), false) as Shader;
         if(shader==null) return;
         if (GUILayout.Button("Find Materials")) {
             FindShader(shader.name);
         }
         GUILayout.Label(stArea);
     }
     
     private void FindShader(string shaderName) {
         int count = 0;
         stArea = "Materials using shader " + shaderName+":\n\n";
         
         List<Material> armat = new List<Material>();
         
            Renderer[] arrend = (Renderer[])Resources.FindObjectsOfTypeAll(typeof(Renderer));
         foreach (Renderer rend in arrend) {
             foreach (Material mat in rend.sharedMaterials) {
                 if (!armat.Contains (mat)) {
                     armat.Add (mat);
                 }
             }
         }
         
         foreach (Material mat in armat) {
             if (mat != null && mat.shader != null && mat.shader.name != null && mat.shader.name == shaderName) {
                 stArea += ">"+mat.name + "\n";
                 count++;
             }
         }
         
         stArea += "\n"+count + " materials using shader " + shaderName + " found.";
     }
 }
