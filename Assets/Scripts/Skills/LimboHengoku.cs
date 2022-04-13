using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LimboHengoku : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string visibleLimboHengokuPrefabName = null;

    private void Start()
    {
        if (photonView.IsMine)
        {
            SpawnVisibleLimboHengoku();
            Invoke(nameof(DestorySelf), .2f);
        }
    }
    private void DestorySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    void SpawnVisibleLimboHengoku()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", visibleLimboHengokuPrefabName), transform.position + transform.TransformDirection(new Vector3(0, 0, 3)), transform.rotation, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", visibleLimboHengokuPrefabName), transform.position + transform.TransformDirection(new Vector3(3, 0, 0)), transform.rotation * Quaternion.Euler(0, 90, 0), 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", visibleLimboHengokuPrefabName), transform.position + transform.TransformDirection(new Vector3(0, 0, -3)), transform.rotation * Quaternion.Euler(0, 180, 0), 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", visibleLimboHengokuPrefabName), transform.position + transform.TransformDirection(new Vector3(-3, 0, 0)), transform.rotation * Quaternion.Euler(0, 270, 0), 0);
    }
}
