using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingDisplayer : MonoBehaviour
{
    [SerializeField] Text pingDisplay = null; 

    float lastUpdateTime = 0f;

    private void Update()
    {
        if (Time.time - lastUpdateTime >= .1f)
        {
            pingDisplay.text = "Ping : " + PhotonNetwork.GetPing().ToString("0.0");
            lastUpdateTime = Time.time;
        }
    }
}
