using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillFieldsDisplay : MonoBehaviour
{
    public int selectedSkillIndex = 0;
    public int skillAmount = 0;

    [SerializeField]
    private SkillField[] skillFields = default;

    [SerializeField]
    private Image[] fieldImages = null;
    [SerializeField]
    private Image[] skillIconImages = null;
    [SerializeField]
    private TMP_Text[] skillAmountTxts = null;

    [SerializeField]
    private Color selectedColor = default;
    [SerializeField]
    private Color unSelectedColor = default;

    public void SetSkillFields(SkillField[] skillFields)
    {
        this.skillFields = skillFields;

        for(int i = 0; i < skillFields.Length; i++)
        {
            if(skillFields[i].skillSO == null)
            {
                skillAmountTxts[i].text = "";
                skillIconImages[i].sprite = null;
                skillIconImages[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                skillIconImages[i].sprite = UtilityFuntion.Texture2DToSprite(skillFields[i].skillSO.SkillIcon);
                skillAmountTxts[i].text = skillFields[i].Amount.ToString();
                skillIconImages[i].color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void SetSelectedSkill(int selectedIndex)
    {
        selectedSkillIndex = selectedIndex;

        for(int i = 0; i < skillAmount; i++)
        {
            if(selectedSkillIndex == i)
            {
                fieldImages[i].color = selectedColor;
            }
            else
            {
                fieldImages[i].color = unSelectedColor;
            }
        }
    }
}
