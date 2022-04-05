using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField playerNameInputField = null;

    private void Start()
    {
        Screen.SetResolution(1920 , 1080, false);
    }

    public void StartLaunching()
    {
        PhotonNetwork.ConnectUsingSettings();

        MenuManager.Instance.SetMenu(MenuManager.MenuType.Load);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined To Lobby");

        PhotonNetwork.NickName = playerNameInputField.text;

        MenuManager.Instance.SetMenu(MenuManager.MenuType.RoomList);
    }
}
