using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    public SkillFieldsDisplay SkillFieldsDisplay = null;

    public PauseMenu PauseMenu = null;

    public ScoreBoard ScoreBoard = null;

    public KillMessenger KillMessenger = null;

    public TMP_Text PlayerNameTxt = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
