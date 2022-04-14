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
    public float lastTookAttackTime = 0f;

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

    public Vector3 Position
    {
        get => transform.position;
        set
        {
            photonView.RPC(nameof(SetPositionRPC), RpcTarget.All, value);
        }
    }

    [PunRPC]
    void SetPositionRPC(Vector3 value)
    {
        transform.position = value;
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
        hash.Add("Point", totalPoints);
        photonView.Owner.SetCustomProperties(hash);
    }

    void Die()
    {
        if(lastTookAttackPlayer != null && (Time.time - lastTookAttackTime) <= 10f)
        {
            UIManager.Instance.KillMessenger.ShowKillMessages($"{lastTookAttackPlayer.NickName}  Killed  {photonView.Owner.NickName}");

            Hashtable killHash = lastTookAttackPlayer.CustomProperties;
            int kill = (int)killHash["Kill"];
            killHash["Kill"] = kill + 1;
            lastTookAttackPlayer.SetCustomProperties(killHash);
        }
        else
        {
            UIManager.Instance.KillMessenger.ShowKillMessages($"{photonView.Owner.NickName} is Dead");
        }
        lastTookAttackPlayer = null;

        Hashtable deathHash = photonView.Owner.CustomProperties;
        int death = (int)deathHash["Death"];
        deathHash["Death"] = death + 1;
        photonView.Owner.SetCustomProperties(deathHash);

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
