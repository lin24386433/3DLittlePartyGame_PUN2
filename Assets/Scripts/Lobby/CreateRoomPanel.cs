using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateRoomPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInputField = null;

    public void OnClickCreate()
    {
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { IsVisible = true });
        MenuManager.Instance.SetMenu(MenuManager.MenuType.Load);
    }

    public void OnClickBack()
    {
        MenuManager.Instance.SetMenu(MenuManager.MenuType.RoomList);
    }
}
