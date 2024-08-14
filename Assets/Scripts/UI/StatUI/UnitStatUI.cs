using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatUI : BaseStatUI
{ 
    [SerializeField] private TMPro.TMP_Text attackUpgradeText;
    [SerializeField] private Image attackSprite;
    [SerializeField] List<Sprite> attackIcons;
    [SerializeField] private TMPro.TMP_Text attackStatText;

    public void SetAttackUpgradeText(UnitType type,float stat)
    {
        if(type == UnitType.Magic)
        {
            attackSprite.sprite = attackIcons[1];
        }
        else
        {
            attackSprite.sprite = attackIcons[0];
        }
        attackUpgradeText.text = $"+{stat}";
    }

    public void SetAttackStatText(UnitType type, float baseStat, float addStat)
    {
        if (type == UnitType.Magic)
        {
            attackStatText.text = $"주문력 : {baseStat} [<color=#00C800>+{addStat}</color>]";
        }
        else
        {
            attackStatText.text = $"공격력 : {baseStat} [<color=#00C800>+{addStat}</color>]";
        }
    }

    public override void DeleteStatData()
    {
        base.DeleteStatData();
        attackUpgradeText.text = "";
    }
}
