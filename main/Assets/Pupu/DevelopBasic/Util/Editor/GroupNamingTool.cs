using UnityEngine;
using UnityEditor;

public class GroupNamingTool : EditorWindow
{
    protected string Naming = "Message";
    protected int StartIndex = 0;
    [MenuItem("Tools/Others/Group Naming Tool")]
    public static void ShowWindow(){
        GetWindow<GroupNamingTool>("Group Naming Tool");
    }
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Group Name");
            Naming = GUILayout.TextField(Naming);
            StartIndex = EditorGUILayout.IntField("StartIndex",StartIndex);
        }
        GUILayout.EndHorizontal();
        if(GUILayout.Button("Renaming")){
            GameObject[] objects = Selection.gameObjects;
            if(objects==null || objects.Length==0){
                Debug.Log("No Objects Selected");
                return;
            }
            int minIndex = objects[0].transform.GetSiblingIndex();
            for(int i=0; i<objects.Length; i++){
                if(minIndex > objects[i].transform.GetSiblingIndex()){
                    minIndex = objects[i].transform.GetSiblingIndex();
                }
            }
            for(int i=0; i<objects.Length; i++){
                objects[i].name = Naming + "_" + (objects[i].transform.GetSiblingIndex()-minIndex+StartIndex).ToString();
            }
        }
    }
}
