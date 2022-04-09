using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class GamePlayManager : MonoBehaviourPunCallbacks
{
    public static GamePlayManager Instance = null;

    Player[] playersInRoom;

    public GameState State = GameState.CountDown;

    [SerializeField]
    private SystemMessage systemMessage = null;
    [SerializeField]
    private TMP_Text timerTxt = null;

    private double countDownTimer = 0f;
    private double roundTimer = 0f;

    [SerializeField]
    private double countDownTime = 5f;
    [SerializeField]
    private double roundTime = 120f;

    double stateStartTime = 0;
    Hashtable CustomeValue;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        playersInRoom = PhotonNetwork.PlayerList;

        SetState(GameState.CountDown);

        int minutes = (int)(roundTime / 60f);
        int seconds = (int)(roundTime % 60);

        timerTxt.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }

    void SetState(GameState state)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CustomeValue = new Hashtable();

            State = state;
            stateStartTime = PhotonNetwork.Time;

            CustomeValue.Add("GameState", State);
            CustomeValue.Add("StateStartTime", stateStartTime);

            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
    }

    private void LateUpdate()
    {
        switch (State)
        {
            case GameState.CountDown:
                CountDown();
                break;
            case GameState.Gaming:
                Gaming();
                break;
            case GameState.Ended:
                Ended();
                break;
        }
    }

    private void CountDown()
    {
        countDownTimer = (PhotonNetwork.Time - stateStartTime);

        if (PhotonNetwork.IsMasterClient)
        {
            if(countDownTimer >= countDownTime)
            {
                SetState(GameState.Gaming);
            }
        }

        systemMessage.ShowMessages($"Game will start in {(int)countDownTime - (int)countDownTimer} seconds");
    }

    private void Gaming()
    {
        roundTimer = (PhotonNetwork.Time - stateStartTime);

        if (PhotonNetwork.IsMasterClient)
        {
            if (roundTimer >= roundTime)
            {
                SetState(GameState.Ended);
            }
        }

        int minutes = (int)((roundTime - roundTimer) / 60f);
        int seconds = (int)((roundTime - roundTimer) % 60);

        timerTxt.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }

    private void Ended()
    {

    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.IsMasterClient) return;

        State = (GameState)int.Parse(propertiesThatChanged["GameState"].ToString());
        stateStartTime = double.Parse(propertiesThatChanged["StateStartTime"].ToString());
    }

    public enum GameState
    {
        CountDown,
        Gaming,
        Ended
    }
}
