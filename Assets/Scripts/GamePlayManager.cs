using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviourPunCallbacks
{
    Player[] playersInRoom;

    private void Start()
    {
        playersInRoom = PhotonNetwork.PlayerList;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playersInRoom = PhotonNetwork.PlayerList;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playersInRoom = PhotonNetwork.PlayerList;
    }
}
