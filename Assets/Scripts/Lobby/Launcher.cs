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

    [SerializeField]
    private ResolutionSetter resolutionSetter = null;

    private void Awake()
    {
        resolutionSetter.SetFullScreen(PlayerPrefs.GetInt("isFullScreen", 0) == 1 ? true : false);
        resolutionSetter.SetResolution(PlayerPrefs.GetInt("Resolution", 2));

        Application.targetFrameRate = 300;

        PhotonNetwork.SendRate = 60;
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
