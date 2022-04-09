using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    public SkillFieldsDisplay SkillFieldsDisplay = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
