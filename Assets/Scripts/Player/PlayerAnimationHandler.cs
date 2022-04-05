using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerModel playerModel = null;

    [SerializeField]
    private PlayerInputHandler inputHandler = null;

    float idleTime = 0f;

    private void Update()
    {
        if (!photonView.IsMine) return;

        playerModel.Animator.SetFloat("XInput", inputHandler.PlayerMovementInput.x);
        playerModel.Animator.SetFloat("YInput", inputHandler.PlayerMovementInput.z);

        playerModel.Animator.SetBool("isGround", playerModel.IsGround);

        float groundVelocity = new Vector2(playerModel.Rigidbody.velocity.x, playerModel.Rigidbody.velocity.z).magnitude;

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
