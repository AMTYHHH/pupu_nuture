using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class RoomManager
    {
        private static List<RoomInfoData> roomList;

        private static int roomIndex = 1;
        public static int RoomIndex => roomIndex;

        public static void OpenRommDialog()
        {
            UIManager.OpenDialog(DialogType.RoomCommonDialog);
            UIManager.OpenDialog(GetCurRoomInfoData().dialogType);
        }

        public static bool ChangeRoom(int changeIndex)
        {
            var nextRoomIndex = roomIndex + changeIndex;

            if (nextRoomIndex < 0 || roomList != null && nextRoomIndex >= roomList.Count)
                return false;

            UIManager.CloseDialog(GetRoomInfoData(roomIndex).dialogType);
            roomIndex = nextRoomIndex;
            UIManager.OpenDialog(GetRoomInfoData(roomIndex).dialogType);
            return true;
        }

        /// <summary>
        /// 改变房间编辑状态
        /// </summary>
        /// <param name="isEdit">true:编辑中</param>
        public static void ChangeRoomEditState(bool isEdit)
        {
            var roomCommonDialog = UIManager.GetDialog("RoomCommonDialog") as RoomCommonDialog;
            if (isEdit)
            {
                roomCommonDialog?.EnterEditMode();
                UIManager.CloseDialog(GetCurRoomInfoData().dialogType);
            }
            else
            {
                roomCommonDialog?.ExitEditMode();
                UIManager.OpenDialog(GetCurRoomInfoData().dialogType);
            }
        }

        public static RoomInfoData GetCurRoomInfoData()
        {
            return GetRoomInfoData(roomIndex);
        }

        public static RoomInfoData GetRoomInfoData(int targetRoomIndex)
        {
            if (roomList == null)
                InitRoomData();

            var targetRoomData = roomList[targetRoomIndex];
            return roomList[targetRoomIndex];
        }

        public static void InitRoomData()
        {
            roomList = new List<RoomInfoData>()
            {
                new RoomInfoData(){roomName = "衣帽间", dialogType = DialogType.RoomRoleDialog},
                new RoomInfoData(){roomName = "大厅", dialogType = DialogType.RoomLobbyDialog},
                new RoomInfoData(){roomName = "炼金室", dialogType = DialogType.RoomAlchemistDialog},
                new RoomInfoData(){roomName = "传送门", dialogType = DialogType.None},
            };

        }
    }

    public enum RoomType
    {
        Cloakroom,      // 衣帽间
        Lobby,          // 大厅
        Alchemy,        // 炼金室
        Portals,        // 传送门
    }

    public class RoomInfoData
    {
        public string roomName;
        public DialogType dialogType;
    }
}
