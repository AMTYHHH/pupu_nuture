using Spine.Unity;
using UnityEngine;

namespace GameLogic
{
    public class MainGame
    {
        public static Player player;
        public static void StartGame()
        {
            //player = new Player("player", "Spine/spineboy-pro/spineboy-pro_prefab");
        }

        public static void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit))
                //{
                //    GameObject go = hit.collider.gameObject;    //获得选中物体
                //    string goName = go.name;    //获得选中物体的名字，使用hit.transform.name也可以
                //    if (goName == "player")
                //        player?.ChangeAni();

                //    Debug.Log("goName" + goName);
                //}

                //player?.ChangeAni();
            }
        }
    }
}
