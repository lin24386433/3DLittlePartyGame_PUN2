using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Skill/SkillSO")]
public class SkillSO : ScriptableObject
{
    public string SkillName = "";
    public string SkillDescription = "";
    public Texture2D SkillIcon = null;

    public GameObject SkillObj = null;
}
