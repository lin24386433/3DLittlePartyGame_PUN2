using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenTime : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float hurtTime = 3f;

    [SerializeField]
    private float scaleSpeed = 10f;

    private void Start()
    {
        if (!photonView.IsMine) return;

        Invoke(nameof(DestorySelf), 3f);
    }

    private void DestorySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    private void Update()
    {
        transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * scaleSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHurtHandler>(out PlayerHurtHandler hurtHandler))
        {
            if (photonView.Owner == hurtHandler.photonView.Owner) return;

            hurtHandler.Hurt(photonView.Owner, transform.position, 0, hurtTime);
        }
    }
}
