using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviourPunCallbacks
{
    public event Action<SkillField[]> OnSKillFieldsChanged = null;

    [SerializeField]
    private PlayerModel playerModel = null;

    [SerializeField]
    private PlayerInputHandler inputHandler = null;

    [SerializeField]
    private SkillField[] skillFields = new SkillField[3];

    int selectedSkillIndex = 0;

    private void Start()
    {
        if (!photonView.IsMine) return;

        OnSKillFieldsChanged?.Invoke(skillFields);
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
    }

    public void UseSkill(Transform skillTriggerTransform)
    {
        if (!photonView.IsMine) return;

        if (skillFields[selectedSkillIndex].skillSO != null && skillFields[selectedSkillIndex].Amount > 0)
        {
            if (skillFields[selectedSkillIndex].skillSO.needTartgetToTrigger)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                ray.origin = Camera.main.transform.position;
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.TryGetComponent<PlayerModel>(out PlayerModel playerModel))
                    {
                        GameObject skillObj = null;

                        if (skillFields[selectedSkillIndex].skillSO.shootable)
                            skillObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillFields[selectedSkillIndex].skillSO.SkillName), skillTriggerTransform.position, skillTriggerTransform.rotation, 0);
                        else
                            skillObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillFields[selectedSkillIndex].skillSO.SkillName), transform.position, transform.rotation, 0);

                        skillObj.GetComponent<INeedTarget>().Init(this.playerModel, playerModel);
                    }
                }
            }
            else
            {
                if (skillFields[selectedSkillIndex].skillSO.shootable)
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillFields[selectedSkillIndex].skillSO.SkillName), skillTriggerTransform.position, skillTriggerTransform.rotation, 0);
                else
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillFields[selectedSkillIndex].skillSO.SkillName), transform.position, transform.rotation, 0);
            }

            skillFields[selectedSkillIndex].Amount--;

            if (skillFields[selectedSkillIndex].Amount == 0)
            {
                skillFields[selectedSkillIndex].skillSO = null;
            }
            OnSKillFieldsChanged?.Invoke(skillFields);
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
                OnSKillFieldsChanged?.Invoke(skillFields);
                return true;
            }
        }
        for (int i = 0; i < skillFields.Length; i++)
        {
            if (skillFields[i].skillSO == null)
            {
                skillFields[i].skillSO = skillSO;
                skillFields[i].Amount = amount;
                OnSKillFieldsChanged?.Invoke(skillFields);
                return true;
            }
        }

        return false;
    }
}
