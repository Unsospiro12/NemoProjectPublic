using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatUI :BaseStatUI
{
    [SerializeField] private TMPro.TMP_Text maxHpText;
    [SerializeField] private TMPro.TMP_Text currentHpText;
    [SerializeField] private TMPro.TMP_Text defTypeText;

    public void SetHPStat(float maxHP,float hp)
    {        
        UpdataHPUI(maxHP, hp);
    } 
    public void SetDefType(UnitDefType type)
    {
        switch (type)
        {
            case UnitDefType.HeavyArmor:
                defTypeText.text = "중장갑";
                break;
            case UnitDefType.LightArmor:
                defTypeText.text = "경장갑";
                break;
        }
    }
    public override void DeleteStatData()
    {
        base.DeleteStatData();
        maxHpText.text = "";
        currentHpText.text = "";
        defTypeText.text = "";
    }
    void UpdataHPUI(float MaxHP, float HP)
    {
        maxHpText.text = $"{MaxHP}";
        currentHpText.text = $"{HP}";
    }
}
