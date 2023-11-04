using UnityEditorInternal;
using UnityEditor;
using UnityEngine;
public class FindGameObjectThroughLayer : EditorWindow
{
    public LayerMask layer;
    [MenuItem("Tools/Finders/Find Gameobject Through Layer")]
    public static void ShowWindow(){
        GetWindow<FindGameObjectThroughLayer>("Find Gameobject Through Layer");
    }

    void OnGUI(){
        LayerMask tempMask = EditorGUILayout.MaskField( InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layer), InternalEditorUtility.layers);
        layer = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        if(GUILayout.Button("Clear Layer")){
            layer = 0;
        }

        if(GUILayout.Button("Find GameObjects")){
            bool IfSelectObject = false;
            if(Selection.gameObjects.Length!=0 && Selection.activeGameObject){
                IfSelectObject = true;
            }
            var goArray = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            var goList = new System.Collections.Generic.List<GameObject>();

            if(!IfSelectObject){
                for (int i = 0; i < goArray.Length; i++){
                    if ((layer.value & (int)Mathf.Pow(2,goArray[i].layer)) == (int)Mathf.Pow(2,goArray[i].layer)){
                        goList.Add(goArray[i]);
                    }
                }
            }
            else{
                for (int i = 0; i < goArray.Length; i++){
                    if ((layer.value & (int)Mathf.Pow(2,goArray[i].layer)) == (int)Mathf.Pow(2,goArray[i].layer)){
                        if(goArray[i].transform.IsChildOf(Selection.activeGameObject.transform)){
                            goList.Add(goArray[i]);
                        }
                    }
                }               
            }

            Selection.objects = goList.ToArray();
        }

        GUILayout.Label("Find " + Selection.objects.Length.ToString() + " Objects");
    }
}
