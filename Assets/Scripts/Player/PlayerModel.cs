using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviourPunCallbacks
{
    public PhotonView PhotonView = null;

    public Rigidbody Rigidbody = null;

    public Animator Animator = null;

    public bool IsGround = false;

    private PlayerManager playerManager = null;

    private void Start()
    {
        playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (transform.position.y < -10f) // Die if you fall out of the world
        {
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();
    }
}
