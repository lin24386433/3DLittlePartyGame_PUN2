using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerModel playerModel = null;

    [SerializeField]
    private PlayerInputHandler inputHandler = null;

    [SerializeField]
    private Transform feetTrans = null;

    [SerializeField]
    private LayerMask floorLayer = default;

    [SerializeField]
    private LayerMask stepLayer = default;

    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private float gravity = -9.8f;

    [SerializeField]
    private float detectFloorRange = .1f;
    [SerializeField]
    private float detectStepRange = .1f;

    private void Update()
    {
        if (!photonView.IsMine) return;

        playerModel.IsGround = isGround() || IsStepClimb();

        if (inputHandler.IsJump && playerModel.IsGround)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (isGround())
        {
            //playerModel.Rigidbody.useGravity = false;
            playerModel.Rigidbody.AddForce(Vector3.up * gravity);
        }
        else
        {
            //playerModel.Rigidbody.useGravity = true;
            playerModel.Rigidbody.AddForce(Vector3.up * gravity);
        }
    }

    void Jump()
    {
        playerModel.Velocity = new Vector3(playerModel.Velocity.x, 0f, playerModel.Velocity.z);

        playerModel.Rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    bool isGround() => Physics.CheckSphere(feetTrans.transform.position, detectFloorRange, floorLayer);

    bool IsStepClimb() => Physics.CheckSphere(feetTrans.transform.position, detectStepRange, stepLayer);
}
