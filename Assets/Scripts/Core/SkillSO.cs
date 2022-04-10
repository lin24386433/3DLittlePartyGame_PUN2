using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Skill/SkillSO")]
public class SkillSO : ScriptableObject
{
    public string SkillName = "";
    [TextArea(10, 10)]
    public string SkillDescription = "";
    public Texture2D SkillIcon = null;

    public GameObject SkillObj = null;

    public bool needTartgetToTrigger = false;
    public bool shootable = false;
    public int GiveAmount = 1;
}
