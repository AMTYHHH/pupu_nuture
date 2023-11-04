using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleSaveSystem{
    public abstract class SaveableBehavior : MonoBehaviour, ISaveable
    {
        //TO DO: We can replace this guid with a better GUID System, so that the duplicate object will not have same guid
        //In this guid, we'll need to manually track and assign guid for each saveable object
        public System.Guid guid{get{return new System.Guid(byteGuid);}}
        [SerializeField, ShowOnly]
        private string byteGuid = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
        [ContextMenu("GetGuid")]
        public void GetGuid(){
            if(UnityEditor.EditorApplication.isPlaying) return;

            UnityEditor.Undo.RecordObject(gameObject, "Generate new guid");
            byteGuid = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif
        public abstract object CaptureState();
        public abstract void RestoreState(object state);
    }
}
