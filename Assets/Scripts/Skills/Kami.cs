using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kami : MonoBehaviourPunCallbacks, INeedTarget
{
    private PlayerModel ownerPlayerModel = null;
    private PlayerModel targetPlayerModel = null;

    public void Init(PlayerModel ownerPlayerModel, PlayerModel targetPlayerModel)
    {
        this.ownerPlayerModel = ownerPlayerModel;
        this.targetPlayerModel = targetPlayerModel;

        ChangePosition();

        if (!photonView.IsMine) return;

        Invoke(nameof(DestorySelf), 1f);
    }

    void ChangePosition()
    {
        Vector3 pos1 = ownerPlayerModel.Position;
        Vector3 pos2 = targetPlayerModel.Position;

        ownerPlayerModel.Position = pos2;
        targetPlayerModel.Position = pos1;
    }

    void DestorySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

}
