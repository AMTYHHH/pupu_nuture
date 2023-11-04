using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupuDetect : MonoBehaviour
{
    private const string CharacterTag = "Character";
    void OnTriggerExit2D(Collider2D other){
        if(other.tag == CharacterTag){
            EventHandler.Call_OnPlayerInViewChange(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == CharacterTag){
            EventHandler.Call_OnPlayerInViewChange(true);
        }
    }
}
