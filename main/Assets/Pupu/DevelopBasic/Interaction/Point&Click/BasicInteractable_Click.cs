using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInteractable_Click : MonoBehaviour
{
    public bool CanBeInteracted = true;
    protected bool IsInteracting = false;
    [SerializeField]
    protected Collider hitbox;
    private void OnMouseDown() {
        if(CanBeInteracted && !IsInteracting){
            IsInteracting = true;
            OnInteracting();
        }    
    }
    private void OnMouseUp() {
        if(IsInteracting){
            IsInteracting = false;
            OnNotInteracting();
        }    
    }
    public void DisableCollider(){hitbox.enabled = false;}
    public void EnableCollider(){hitbox.enabled = true;}
    protected virtual void OnInteracting(){}
    protected virtual void OnNotInteracting(){}
}
