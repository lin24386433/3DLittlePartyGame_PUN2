using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerModel : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody Rigidbody = null;

    public Animator Animator = null;

    public bool IsGround = false;

    public bool IsHurt = false;

    public Player lastTookAttackPlayer = null;

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

    public void AddPoints(int points)
    {
        if (!photonView.IsMine) return;

        int totalPoints = points;

        if (photonView.Owner.CustomProperties.ContainsKey("Points"))
        {
            totalPoints += (int)photonView.Owner.CustomProperties["Points"];
        }

        Hashtable hash = new Hashtable();
        hash.Add("Points", totalPoints);
        photonView.Owner.SetCustomProperties(hash);
    }

    void Die()
    {
        Hashtable hash = new Hashtable();
        hash.Add("Points", 0);
        photonView.Owner.SetCustomProperties(hash);
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
