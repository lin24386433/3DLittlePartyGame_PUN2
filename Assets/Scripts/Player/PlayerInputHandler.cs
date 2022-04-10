using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviourPunCallbacks
{
    private void Update()
    {
        if (!photonView.IsMine) return;

        GetAttackInput();
        GetMouseInput();
        GetMovementInput();
        GetOtherInput();
    }

    [SerializeField]
    private float mouseXSensitivity = 1f;
    [SerializeField]
    private float mouseYSensitivity = 1f;
    public float MouseXInput = 1f;
    public float MouseYInput = 1f;

    void GetMouseInput()
    {
        MouseXInput += Input.GetAxis("Mouse X") * mouseXSensitivity;
        MouseYInput += Input.GetAxis("Mouse Y") * mouseYSensitivity * -1;

        MouseYInput = Mathf.Clamp(MouseYInput, -89f, 89f);
    }

    public bool IsNormalAttack = false;
    public bool IsSkillAttack = false;
    public int SelectedSkillIndex = 0;

    void GetAttackInput()
    {
        IsNormalAttack = Input.GetMouseButtonDown(0);
        IsSkillAttack = Input.GetMouseButtonDown(1);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedSkillIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedSkillIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectedSkillIndex = 2;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SelectedSkillIndex++;
            if (SelectedSkillIndex >= 3)
            {
                SelectedSkillIndex = 0;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SelectedSkillIndex--;
            if (SelectedSkillIndex <= -1)
            {
                SelectedSkillIndex = 2;
            }
        }
    }

    public Vector3 PlayerMovementInput = default;

    public bool IsRun = false;
    public bool IsJump = false;

    float firstTimePressWKey = 0f;

    void GetMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - firstTimePressWKey > .5f)
            {
                firstTimePressWKey = Time.time;
            }
            else
            {
                IsRun = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.W)) IsRun = false;

        IsJump = Input.GetKeyDown(KeyCode.Space);

        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    public bool IsESC = false;
    public bool IsTab = false;

    void GetOtherInput()
    {
        IsESC = Input.GetKeyDown(KeyCode.Escape);
        IsTab = Input.GetKey(KeyCode.Tab);
    }
}
