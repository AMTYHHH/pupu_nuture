using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animation))]
public class AnimationPlayer : MonoBehaviour
{
    public void PlayAnimation(){
        GetComponent<Animation>().Play();
    }
    public void PlayAnimation(string clipName){
        GetComponent<Animation>().Play(clipName);
    }
}
