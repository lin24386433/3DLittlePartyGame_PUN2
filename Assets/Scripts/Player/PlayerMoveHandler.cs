using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerModel playerModel = null;

    [SerializeField]
    private PlayerJumpHandler jumpHandler = null;

    [SerializeField]
    private PlayerInputHandler inputHandler = null;

    [SerializeField]
    private Transform cameraPos = null;

    [SerializeField]
    private Transform stepRayUpper = null;
    [SerializeField]
    private Transform stepRayLower = null;

    [SerializeField]
    private float stepSmooth = 2f;

    private float moveSpeed = 3f;

    [SerializeField]
    private float walkSpeed = 7f;
    [SerializeField]
    private float runSpeed = 10f;

    [SerializeField]
    private float groundDrag = 10f;

    [SerializeField]
    private float airMultiplier = -5;

    Transform cameraFollowPoint = null;

    private void Start()
    {
        if (!photonView.IsMine) return;

        cameraFollowPoint = GameObject.Find("Camera Follow").transform;

        cameraFollowPoint.transform.position = cameraPos.position;
        cameraFollowPoint.transform.rotation = cameraPos.rotation;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        moveSpeed = inputHandler.IsRun ? runSpeed : walkSpeed;
        StepClimb();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        Rotate();
        Move();
        SpeedControl();
        DragControl();
        StepClimb();
    }

    private void LateUpdate()
    {
        if (cameraFollowPoint == null) return;

        cameraFollowPoint.transform.position = cameraPos.position;
        cameraFollowPoint.transform.rotation = cameraPos.rotation;
    }

    void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2f : moveSpeed;

        Vector3 moveVector = transform.TransformDirection(inputHandler.PlayerMovementInput) * speed;

        playerModel.Rigidbody.velocity = new Vector3(moveVector.x, playerModel.Rigidbody.velocity.y, moveVector.z);

        if (!playerModel.IsGround)
            playerModel.Rigidbody.AddForce(playerModel.Rigidbody.velocity * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerModel.Rigidbody.velocity.x, 0f, playerModel.Rigidbody.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            playerModel.Rigidbody.velocity = new Vector3(limitedVel.x, playerModel.Rigidbody.velocity.y, limitedVel.z);
        }
    }

    private void DragControl()
    {
        if (playerModel.IsGround)
            playerModel.Rigidbody.drag = groundDrag;
        else
            playerModel.Rigidbody.drag = 0;
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Euler(Vector3.up * inputHandler.MouseXInput);

        Quaternion cameraPosQuaternion = Quaternion.Euler(inputHandler.MouseYInput, 0f, 0f);
        cameraPos.localRotation = cameraPosQuaternion;
    }

    void StepClimb()
    {
        if (inputHandler.PlayerMovementInput == Vector3.zero) return;
        if (playerModel.Rigidbody.velocity.y <= -.5f) return;

        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                playerModel.Rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
            {
                playerModel.Rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
            {
                playerModel.Rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }
}
