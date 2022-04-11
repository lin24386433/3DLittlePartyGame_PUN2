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

    public string CoinOwner = null;

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
        State = state;

        if (PhotonNetwork.IsMasterClient)
        {
            CustomeValue = new Hashtable();

            stateStartTime = PhotonNetwork.Time;

            CustomeValue.Add("GameState", State);
            CustomeValue.Add("StateStartTime", stateStartTime);

            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }

        switch (state)
        {
            case GameState.CountDown:
                CountDownInit();
                break;
            case GameState.Gaming:
                GamingInit();
                break;
            case GameState.Ended:
                EndedInit();
                break;
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

    private void CountDownInit()
    {

    }

    private void GamingInit()
    {
        systemMessage.ShowMessages($"Start!!");
    }

    private void EndedInit()
    {
        systemMessage.ShowMessages($"Game Over!!");

        Invoke(nameof(ShowWinner), 1.5f);
    }

    void ShowWinner()
    {
        if(CoinOwner == null)
        {
            systemMessage.ShowMessages($"There is no Winner!");
        }
        else
        {
            systemMessage.ShowMessages($"Winner is {CoinOwner}!!");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            Invoke(nameof(Restart), 5f);
        }
    }

    void Restart()
    {
        Hashtable hash = new Hashtable();
        hash.Add("Points", 0);
        for (int i = 0; i < playersInRoom.Length; i++)
        {
            playersInRoom[i].SetCustomProperties(hash);
        }

        SetState(GameState.CountDown);
    }

    private void CountDown()
    {
        countDownTimer = (PhotonNetwork.Time - stateStartTime);

        if (PhotonNetwork.IsMasterClient)
        {
            if(countDownTimer >= countDownTime)
            {
                SetState(GameState.Gaming);
                return;
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
                return;
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

        SetState((GameState)int.Parse(propertiesThatChanged["GameState"].ToString()));
        stateStartTime = double.Parse(propertiesThatChanged["StateStartTime"].ToString());
    }

    public enum GameState
    {
        CountDown,
        Gaming,
        Ended
    }
}