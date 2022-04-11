using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviourPunCallbacks, INeedTarget
{
    [SerializeField]
    private float attackerAnimateTime = 2f;

    [SerializeField]
    private float explodeTime = 1f;

    [SerializeField]
    private float range = 10f;

    [SerializeField]
    private ParticleSystem explodeParticle = null;

    private PlayerModel ownerPlayerModel = null;
    private PlayerModel targetPlayerModel = null;

    public void Init(PlayerModel ownerPlayerModel, PlayerModel targetPlayerModel)
    {
        this.ownerPlayerModel = ownerPlayerModel;
        this.targetPlayerModel = targetPlayerModel;

        Invoke(nameof(WaitForAttackAnimate), attackerAnimateTime);
    }

    void WaitForAttackAnimate()
    {
        photonView.RPC(nameof(ExplodeParticleRPC), RpcTarget.All);
        Invoke(nameof(Explode), explodeTime);
    }

    [PunRPC]
    void ExplodeParticleRPC()
    {
        explodeParticle.Play();
    }

    void Explode()
    {
        Collider[] floorColliders = Physics.OverlapSphere(transform.position, range);

        List<int> floorsToDestoryIndex = new List<int>();

        for(int i = 0; i < floorColliders.Length; i++)
        {
            if(floorColliders[i].TryGetComponent<DestroyableFloor>(out DestroyableFloor destroyableFloor))
            {
                floorsToDestoryIndex.Add(destroyableFloor.Index);
            }
        }

        FloorManager.Instace.DestroyFloors(floorsToDestoryIndex.ToArray());
    }
}
