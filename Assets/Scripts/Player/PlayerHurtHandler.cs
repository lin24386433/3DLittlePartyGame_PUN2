using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerModel playerModel = null;

    public void Hurt(Player attackPlayer, Vector3 attckPos, float force, float durationTime)
    {
        Vector3 attackDirection = (attckPos - transform.position).normalized;

        playerModel.AddForce(-attackDirection * force, ForceMode.Impulse);

        photonView.RPC(nameof(HurtRPC), RpcTarget.All, attackPlayer, attckPos, force, durationTime);
    }

    [PunRPC]
    public void HurtRPC(Player attackPlayer, Vector3 attckPos, float force, float durationTime, PhotonMessageInfo info)
    {
        if (!photonView.IsMine) return;

        playerModel.IsHurt = true;
        playerModel.lastTookAttackPlayer = attackPlayer;

        Vector3 attackDirection = (attckPos - transform.position).normalized;

        playerModel.AddForce(-attackDirection * force, ForceMode.Impulse);

        Invoke(nameof(ResetHurt), durationTime);
    }

    void ResetHurt() => playerModel.IsHurt = false;
}
