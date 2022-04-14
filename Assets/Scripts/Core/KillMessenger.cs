using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class KillMessenger : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<string> killMessages = new List<string>();

    [SerializeField]
    private TMP_Text killMessagesTxt = null;

    public void ShowKillMessages(string messages)
    {
        photonView.RPC(nameof(ShowKillMessagesRPC), RpcTarget.All, messages);
    }

    [PunRPC]
    void ShowKillMessagesRPC(string messages)
    {
        killMessages.Add(messages);
        Invoke(nameof(ClearTopMessages), 5f);
    }

    private void LateUpdate()
    {
        string messagesToShow = "";

        for(int i = 0; i < killMessages.Count; i++)
        {
            messagesToShow += killMessages[i] + '\n';
        }

        killMessagesTxt.text = messagesToShow;
    }

    void ClearTopMessages()
    {
        if (killMessages.Count == 0) return;

        killMessages.RemoveAt(0);
    }
}
