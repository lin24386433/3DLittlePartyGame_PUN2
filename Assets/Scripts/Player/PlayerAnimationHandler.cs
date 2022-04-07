using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerModel playerModel = null;

    float idleTime = 0f;

    float groundVelocity = 0f;

    private void Update()
    {
        playerModel.Animator.SetFloat("XInput", transform.InverseTransformDirection(playerModel.Velocity).x);
        playerModel.Animator.SetFloat("YInput", transform.InverseTransformDirection(playerModel.Velocity).z);

        playerModel.Animator.SetBool("isGround", playerModel.IsGround);

        groundVelocity = new Vector2(playerModel.Velocity.x, playerModel.Velocity.z).magnitude;

        if (playerModel.IsGround)
        {
            if (groundVelocity <= 0.5f)
            {
                idleTime += Time.deltaTime;

                if (idleTime > .05f)
                {
                    playerModel.Animator.SetBool("isWalk", false);
                    playerModel.Animator.SetBool("isRun", false);
                }
            }
            else if (groundVelocity > 5f)
            {
                idleTime = 0f;
                playerModel.Animator.SetBool("isRun", true);
                playerModel.Animator.SetBool("isWalk", false);
            }
            else if (groundVelocity > 0.5f)
            {
                idleTime = 0f;
                playerModel.Animator.SetBool("isWalk", true);
                playerModel.Animator.SetBool("isRun", false);
            }
        }
    }
}
