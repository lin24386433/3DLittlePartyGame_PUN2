using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private SkillSO[] skillSOs = default;

    int selectedSkillIndex = 0;

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSelectedSkillIndex(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSelectedSkillIndex(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSelectedSkillIndex(2);
        }
    }

    public void SetSelectedSkillIndex(int value)
    {
        selectedSkillIndex = value;
    }

    public void UseSkill(Transform skillTriggerTransform)
    {
        if(skillSOs[selectedSkillIndex] != null)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillSOs[selectedSkillIndex].SkillName), skillTriggerTransform.position, skillTriggerTransform.rotation, 0);
        }
    }
}
