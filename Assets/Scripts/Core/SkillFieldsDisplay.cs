using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SkillFieldsDisplay : MonoBehaviour
{
    public int SelectedSkillIndex { get; private set; }
    public int SkillAmount = 3;

    [SerializeField]
    private SkillField[] skillFields = default;

    [SerializeField]
    private Image[] fieldImages = null;
    [SerializeField]
    private Image[] skillIconImages = null;
    [SerializeField]
    private TMP_Text[] skillAmountTxts = null;
    [SerializeField]
    private CanvasGroup skillInfoCanvasGroup = null;
    [SerializeField]
    private TMP_Text skillNameTxt = null;
    [SerializeField]
    private TMP_Text skillDescriptionTxt = null;

    [SerializeField]
    private Color selectedColor = default;
    [SerializeField]
    private Color unSelectedColor = default;

    private void Start()
    {
        skillInfoCanvasGroup.alpha = 0;
    }

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
        if (SelectedSkillIndex == selectedIndex) return;

        SelectedSkillIndex = selectedIndex;

        if (skillFields[SelectedSkillIndex].skillSO != null)
        {
            skillInfoCanvasGroup.alpha = 1;
            DOTween.To(() => skillInfoCanvasGroup.alpha, x => skillInfoCanvasGroup.alpha = x, 0, 1.5f).SetEase(Ease.InQuart);
            skillNameTxt.text = skillFields[SelectedSkillIndex].skillSO.SkillName;
            skillDescriptionTxt.text = skillFields[SelectedSkillIndex].skillSO.SkillDescription;
        }
        else
        {
            skillInfoCanvasGroup.alpha = 0;
        }

        for (int i = 0; i < SkillAmount; i++)
        {
            if(selectedIndex == i)
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
