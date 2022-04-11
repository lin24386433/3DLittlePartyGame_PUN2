using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerInputHandler inputHandler = null;

    [SerializeField]
    private PlayerAnimationHandler animationHandler = null;

    [SerializeField]
    private PlayerSkillHandler skillHandler = null;

    [SerializeField]
    private Transform shootPoint = null;

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (inputHandler.IsNormalAttack)
        {
            NormalAttack();
            animationHandler.TriggerAttack();
        }
        else if (inputHandler.IsSkillAttack)
        {
            skillHandler.UseSkill(shootPoint);
            animationHandler.TriggerSkill();
        }
    }

    private void NormalAttack()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = Camera.main.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.TryGetComponent<DestroyableFloor>(out DestroyableFloor hittedFloor))
            {
                hittedFloor.DestroySelf();
            }
        }
    }
}
