using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Susanoo : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string bulletName = "";

    [SerializeField]
    private float timeToShoot = 1f;

    private void Start()
    {
        if (!photonView.IsMine) return;

        Invoke(nameof(Shoot), timeToShoot);
        Invoke(nameof(DestorySelf), 1.5f);
    }

    private void DestorySelf()
    {
        if (!photonView.IsMine) return;

        PhotonNetwork.Destroy(this.gameObject);
    }

    void Shoot()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", bulletName), transform.position + transform.TransformDirection(Vector3.forward * 7f), transform.rotation, 0);
    }
}
