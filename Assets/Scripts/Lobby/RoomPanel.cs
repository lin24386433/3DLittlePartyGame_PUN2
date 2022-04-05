using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomPanel : MonoBehaviourPunCallbacks
{
    public void OnClickStartGame()
    {
        PhotonNetwork.LoadLevel("GamePlay");
        MenuManager.Instance.SetMenu(MenuManager.MenuType.Load);
    }

    public void OnClickExitRoom()
    {
        RoomManager.Instance.LeaveRoom();
    }

    
}
