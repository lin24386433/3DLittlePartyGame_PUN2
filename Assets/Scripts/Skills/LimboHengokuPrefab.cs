using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboHengokuPrefab : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Rigidbody rigi = null;

    [SerializeField]
    private SkinnedMeshRenderer meshRenderer = null;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float force = 15f;

    [SerializeField]
    private float hurtTime = 1f;

    private void Start()
    {
        if (photonView.IsMine)
        {
            meshRenderer.enabled = true;
            Invoke(nameof(DestorySelf), 15f);
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void DestorySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    private void Update()
    {
        rigi.velocity = transform.TransformDirection(Vector3.forward * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHurtHandler>(out PlayerHurtHandler hurtHandler))
        {
            if (photonView.Owner == hurtHandler.photonView.Owner) return;

            hurtHandler.Hurt(photonView.Owner, transform.position, force, hurtTime);
        }
    }
}
