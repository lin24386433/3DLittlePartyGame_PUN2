using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerModel playerModel = null;

    public void Hurt(Vector3 attckPos, float force, float durationTime)
    {
        Vector3 attackDirection = (attckPos - transform.position).normalized;

        playerModel.AddForce(-attackDirection * force, ForceMode.Impulse);

        photonView.RPC(nameof(HurtRPC), RpcTarget.All, attckPos, force, durationTime);
    }

    [PunRPC]
    public void HurtRPC(Vector3 attckPos, float force, float durationTime, PhotonMessageInfo info)
    {
        if (!photonView.IsMine) return;

        playerModel.IsHurt = true;

        Vector3 attackDirection = (attckPos - transform.position).normalized;

        playerModel.AddForce(-attackDirection * force, ForceMode.Impulse);

        Invoke(nameof(ResetHurt), durationTime);
    }

    void ResetHurt() => playerModel.IsHurt = false;
}
