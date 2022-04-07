using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody Rigidbody = null;

    public Animator Animator = null;

    public bool IsGround = false;

    public bool IsHurt = false;

    private PlayerManager playerManager = null;

    private void Start()
    {
        playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (transform.position.y < 15f) 
        {
            Die();
        }
    }

    public Vector3 Velocity
    {
        get => Rigidbody.velocity;
        set => Rigidbody.velocity = value;
    }

    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        Rigidbody.AddForce(force, forceMode);
    }

    void Die()
    {
        playerManager.Die();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine)
        {
            stream.SendNext(IsGround);
            stream.SendNext(Velocity);
        }
        else
        {
            IsGround = (bool)stream.ReceiveNext();
            Velocity = (Vector3)stream.ReceiveNext();
        }
    }
}
