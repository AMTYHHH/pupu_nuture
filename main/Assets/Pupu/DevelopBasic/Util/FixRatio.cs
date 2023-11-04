using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Camera/FixRatio")]
public class FixRatio : MonoBehaviour {
	public Vector2Int ReferenceRatio = new Vector2Int(16,9);
	Camera AspectCamera;
	Vector2Int ScreenSize = Vector2Int.zero;
	// Use this for initialization
	void Start () {
		AspectCamera = GetComponent<Camera>();
		RescaleCamera();
	}
	void RescaleCamera(){
        if (!AspectCamera.isActiveAndEnabled)
        {
            return;
        }

        if (Screen.width == ScreenSize.x && Screen.height == ScreenSize.y) return;
        float scaleheight = (float)(Screen.width*ReferenceRatio.y) / (float)(Screen.height*ReferenceRatio.x);
 
        if (scaleheight < 1.0f)
        {
            Rect rect = AspectCamera.rect;
 
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
 
             AspectCamera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;
 
            Rect rect = AspectCamera.rect;
 
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
 
             AspectCamera.rect = rect;
        }
 
        ScreenSize.x = Screen.width;
        ScreenSize.y = Screen.height;		
        StartCoroutine(coroutineClearBuffer());
	}
    IEnumerator coroutineClearBuffer(){
        yield return null;
        GL.Clear(true, true, Color.black);
    }
	// void OnPreCull(){
    //     if (Application.isEditor) return;
    //     Rect wp = Camera.main.rect;
    //     Rect nr = new Rect(0, 0, 1, 1);
 
    //     Camera.main.rect = nr;
    //     GL.Clear(true, true, Color.black);
       
    //     Camera.main.rect = wp;
	// }
	void Update(){
		RescaleCamera();
	}
}
