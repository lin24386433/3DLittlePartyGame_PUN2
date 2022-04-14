using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private UIManager uiManager = null;

    [SerializeField]
    private PlayerSkillHandler skillHandler = null;

    [SerializeField]
    private PlayerInputHandler inputHandler = null;

    [SerializeField]
    private TMP_Text playerNameTxt = null;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            playerNameTxt.text = photonView.Owner.NickName;

            return;
        }

        playerNameTxt.text = "";

        uiManager = UIManager.Instance;

        uiManager.PlayerNameTxt.text = photonView.Owner.NickName;

        skillHandler.OnSKillFieldsChanged += HandleSkillFieldsChanged;
    }

    private void OnDestroy()
    {
        if (!photonView.IsMine) return;

        skillHandler.OnSKillFieldsChanged -= HandleSkillFieldsChanged;
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine) return;

        uiManager.SkillFieldsDisplay.SetSelectedSkill(inputHandler.SelectedSkillIndex);

        if (inputHandler.IsESC)
        {
            if(uiManager.PauseMenu.IsOn)
                uiManager.PauseMenu.CloseMenu();
            else
                uiManager.PauseMenu.OpenMenu();
        }

        if(inputHandler.IsTab)
            uiManager.ScoreBoard.OpenMenu();
        else
            uiManager.ScoreBoard.CloseMenu();
    }

    void HandleSkillFieldsChanged(SkillField[] skillFields)
    {
        uiManager.SkillFieldsDisplay.SetSkillFields(skillFields);
    }
}
