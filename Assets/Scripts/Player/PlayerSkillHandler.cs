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

    [SerializeField]
    private ParticleSystem explosionParticle = null;

    int selectedSkillIndex = 0;

    private void Start()
    {
        if (!photonView.IsMine) return;

        OnSKillFieldsChanged?.Invoke(skillFields);

        GamePlayManager.OnCountDown += ResetSkills;
    }

    private void OnDestroy()
    {
        GamePlayManager.OnCountDown -= ResetSkills;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if(inputHandler.SelectedSkillIndex != selectedSkillIndex)
        {
            SetSelectedSkillIndex(inputHandler.SelectedSkillIndex);
        }
    }

    void ResetSkills()
    {
        skillFields[0].skillSO = null;
        skillFields[0].Amount = 0;
        skillFields[1].skillSO = null;
        skillFields[1].Amount = 0;
        skillFields[2].skillSO = null;
        skillFields[2].Amount = 0;
        OnSKillFieldsChanged?.Invoke(skillFields);
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
                if (skillFields[selectedSkillIndex].skillSO.SkillName == "Explosion")
                {
                    photonView.RPC(nameof(DisplayExplosionRPC), RpcTarget.All);
                }

                PlayerModel hitPlayerModel = null;

                for(int i = 0; i < 5; i++)
                {
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f + .5f * i, 0.5f + .5f * i));
                    ray.origin = Camera.main.transform.position;
                    if (Physics.Raycast(ray, out RaycastHit hit)) hitPlayerModel = hit.collider.GetComponent<PlayerModel>();
                }

                if (hitPlayerModel != null)
                {
                    GameObject skillObj = null;

                    if (skillFields[selectedSkillIndex].skillSO.shootable)
                        skillObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillFields[selectedSkillIndex].skillSO.SkillName), skillTriggerTransform.position, skillTriggerTransform.rotation, 0);
                    else
                        skillObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", skillFields[selectedSkillIndex].skillSO.SkillName), hitPlayerModel.transform.position, hitPlayerModel.transform.rotation, 0);

                    skillObj.GetComponent<INeedTarget>().Init(this.playerModel, hitPlayerModel);
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

    [PunRPC]
    void DisplayExplosionRPC()
    {
        this.playerModel.Animator.SetTrigger("Explosion");
        explosionParticle.Play();
    }
}
