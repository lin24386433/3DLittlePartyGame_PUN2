using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerInputHandler inputHandler = null;

    [SerializeField]
    private SkillField[] skillFields = default;

    int selectedSkillIndex = 0;

    private void Start()
    {
        if (!photonView.IsMine) return;

        UIManager.Instance.SkillFieldsDisplay.SetSelectedSkill(selectedSkillIndex);
        UpdateUI();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if(inputHandler.SelectedSkillIndex != selectedSkillIndex)
        {
            SetSelectedSkillIndex(inputHandler.SelectedSkillIndex);
        }
    }

    public void SetSelectedSkillIndex(int value)
    {
        if (!photonView.IsMine) return;

        selectedSkillIndex = value;
        UIManager.Instance.SkillFieldsDisplay.SetSelectedSkill(selectedSkillIndex);
        UpdateUI();
    }

    public void UseSkill(Transform skillTriggerTransform)
    {
        if (!photonView.IsMine) return;

        if (skillFields[selectedSkillIndex].skillSO != null && skillFields[selectedSkillIndex].Amount > 0)
        {
            if(skillFields[selectedSkillIndex].skillSO.shootable)
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillFields[selectedSkillIndex].skillSO.SkillName), skillTriggerTransform.position, skillTriggerTransform.rotation, 0);
            else
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillFields[selectedSkillIndex].skillSO.SkillName), transform.position, transform.rotation, 0);

            skillFields[selectedSkillIndex].Amount--;

            if(skillFields[selectedSkillIndex].Amount == 0)
            {
                skillFields[selectedSkillIndex].skillSO = null;
            }
            UpdateUI();
        }
    }

    public bool AddSkill(SkillSO skillSO, int amount)
    {
        if (!photonView.IsMine) return false;

        for (int i = 0; i < skillFields.Length; i++)
        {
            if(skillFields[i].skillSO == skillSO)
            {
                skillFields[i].Amount += amount;
                UpdateUI();
                return true;
            }
        }
        for (int i = 0; i < skillFields.Length; i++)
        {
            if (skillFields[i].skillSO == null)
            {
                skillFields[i].skillSO = skillSO;
                skillFields[i].Amount = amount;
                UpdateUI();
                return true;
            }
        }

        UpdateUI();
        return false;
    }

    void UpdateUI()
    {
        if (!photonView.IsMine) return;

        UIManager.Instance.SkillFieldsDisplay.SetSkillFields(skillFields);
    }
}
