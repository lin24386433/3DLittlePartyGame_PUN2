using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListPanel : MonoBehaviourPunCallbacks
{
    public void OnClickCreateRoom()
    {
        MenuManager.Instance.SetMenu(MenuManager.MenuType.CreateRoom);
    }
}
