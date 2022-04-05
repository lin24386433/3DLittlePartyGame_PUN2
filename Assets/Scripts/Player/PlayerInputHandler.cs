using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerModel playerModel = null;

    public Vector3 PlayerMovementInput = default;

    public bool IsRun = false;
    public bool IsJump = false;

    public float MouseXInput = 1f;
    public float MouseYInput = 1f;

    [SerializeField]
    private float mouseXSensitivity = 1f;
    [SerializeField]
    private float mouseYSensitivity = 1f;

    float firstTimePressWKey = 0f;

    private void Update()
    {
        if (!photonView.IsMine) return;

        GetInput();
    }

    void GetInput()
    {
        MouseXInput += Input.GetAxis("Mouse X") * mouseXSensitivity;
        MouseYInput += Input.GetAxis("Mouse Y") * mouseYSensitivity * -1;

        MouseYInput = Mathf.Clamp(MouseYInput, -89f, 89f);

        if (Input.GetKeyDown(KeyCode.W))
        {
            if(Time.time - firstTimePressWKey > .5f)
            {
                firstTimePressWKey = Time.time;
            }
            else
            {
                IsRun = true;
            }
        }
        
        if(Input.GetKeyUp(KeyCode.W)) IsRun = false;

        IsJump = Input.GetKeyDown(KeyCode.Space);

        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }
}
