using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T> : MonoBehaviour where T:Singleton<T>
{
    public static T Instance{get{return instance;}}
    public static bool IsInitialized{get {return instance != null;}}
    private static T instance;
    protected virtual void Awake(){
        if(instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = (T) this;
        }
    }
    protected virtual void OnDestroy(){
        if(instance == this){
            instance = null;
        }
    }
}
