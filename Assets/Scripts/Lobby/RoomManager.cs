using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    public Dictionary<string, RoomInfo> CachedRoomList = new Dictionary<string, RoomInfo>();

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { IsVisible = true });
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.SetMenu(MenuManager.MenuType.Load);
        CachedRoomList.Clear();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.SetMenu(MenuManager.MenuType.Load);
    }

    #region - In Room -
    [SerializeField] TMP_Text roomNameTxt = null;
    [SerializeField] Transform playerListContent = null;
    [SerializeField] GameObject PlayerListCellPrefab = null;
    [SerializeField] GameObject startGameButton = null;

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.SetMenu(MenuManager.MenuType.Room);
        roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(PlayerListCellPrefab, playerListContent).GetComponent<PlayerListCell>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room Creation Failed: " + message);
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.SetMenu(MenuManager.MenuType.RoomList);
        CachedRoomList.Clear();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListCellPrefab, playerListContent).GetComponent<PlayerListCell>().SetUp(newPlayer);
    }
    #endregion


    #region - Room List -
    [SerializeField] Transform roomListContent = null;
    [SerializeField] GameObject roomListCellPrefab = null;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);

        foreach (Transform trans in roomListContent)
        {
            if (trans != null)
                if (trans.gameObject != null)
                    Destroy(trans.gameObject);
        }

        foreach (var info in CachedRoomList)
        {
            Instantiate(roomListCellPrefab, roomListContent).GetComponent<RoomListCell>().SetUp(info.Value);
        }
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                CachedRoomList.Remove(info.Name);
            }
            else
            {
                CachedRoomList[info.Name] = info;
            }
        }
    }
    #endregion
}
