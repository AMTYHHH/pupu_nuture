using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Player
{
    public GameObject gameObj;
    SkeletonAnimation skeletonAnimation;
    public List<string> aniNameList;
    public Player(string gameObjName, string prefabName)
    {
        aniNameList = new List<string>();
        aniNameList.Add("jump");
        aniNameList.Add("aim");
        aniNameList.Add("walk");

        GameObject prefabGameObj = (GameObject)Resources.Load(prefabName);
        gameObj = Object.Instantiate(prefabGameObj, new Vector3(0, -2.5f, 0), Quaternion.identity);
        gameObj.name = gameObjName;
        skeletonAnimation = gameObj.GetComponent<SkeletonAnimation>();
        //skeletonAnimation.AnimationState.Complete += AniCompleteEvent;

        //gameObj.transform.position = new  Vector3(0, -2.5f, 0);


        InitAni();

    }

    private void InitAni()
    {
        SetRoleAni("idle");
    }

    public void ChangeAni()
    {
        int index = Random.Range(0, aniNameList.Count);
        SetRoleAni(aniNameList[index]);
    }

    private void AniCompleteEvent(Spine.TrackEntry trackEntry)
    {
        Debug.Log("AniCompleteEvent-start");
        skeletonAnimation.AnimationState.Complete -= AniCompleteEvent;
        if (skeletonAnimation.AnimationName == "idle")
            return;
        SetRoleAni("idle");
        Debug.Log("AniCompleteEvent-end");
    }

    public void SetRoleAni(string aniName)
    {

        Debug.Log("SetRoleAni-start:" + aniName);
        if (aniName == skeletonAnimation.AnimationName)
            return;

        var loop = false;
        if (aniName == "idle")
            loop = true;

        skeletonAnimation.AnimationState.Complete += AniCompleteEvent;
        skeletonAnimation.AnimationState.SetAnimation(0, aniName, loop);

        Debug.Log("SetRoleAni-end:" + skeletonAnimation.AnimationName);
    }
}
