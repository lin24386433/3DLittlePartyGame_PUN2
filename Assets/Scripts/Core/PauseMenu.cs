using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    public bool IsOn = false;

    [SerializeField]
    private CanvasGroup canvasGroup = null;

    public void OpenMenu()
    {
        IsOn = true;
        canvasGroup.alpha = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        IsOn = false;
        canvasGroup.alpha = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Resume()
    {
        if (!IsOn) return;

        CloseMenu();
    }

    public void Settings()
    {
        if (!IsOn) return;
    }

    public void LeaveRoom()
    {
        if (!IsOn) return;

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}
