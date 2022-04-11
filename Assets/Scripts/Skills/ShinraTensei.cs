using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinraTensei : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float force = 15f;

    [SerializeField]
    private float range = 10f;

    [SerializeField]
    private float hurtTime = 1f;

    [SerializeField]
    private LayerMask affectLayer = default;

    private void Start()
    {
        if (!photonView.IsMine) return;

        Trigger();
        Invoke(nameof(DestorySelf), .2f);
    }

    private void Update()
    {
        transform.localScale += new Vector3(2, 2, 2) * Time.deltaTime * force;

        if (transform.localScale.x <= 0)
        {
            transform.localScale = Vector3.zero;
        }
    }

    private void DestorySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    public void Trigger()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, affectLayer);

        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].TryGetComponent<PlayerHurtHandler>(out PlayerHurtHandler hurtHandler))
            {
                if (photonView.Owner == hurtHandler.photonView.Owner) continue;

                hurtHandler.Hurt(photonView.Owner, transform.position, force, hurtTime);
            }
        }
    }
}
