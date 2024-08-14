using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatToolTipUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private TMPro.TMP_Text attackDefenceText;
    [SerializeField] private TMPro.TMP_Text magicDefenceText;
    [SerializeField] private TMPro.TMP_Text moveSpeedText;
    [SerializeField] private TMPro.TMP_Text lightArmorText;
    [SerializeField] private TMPro.TMP_Text heavyArmorText;
    [SerializeField] private TMPro.TMP_Text magicArmorText;
    [SerializeField] private UnitDataDic unitDataDic;

    private void Start()
    {
        unitDataDic = UnitDataDic.Instance;
    }
    public void SetNameText(int id)
    {
        if(id < 100)
        {
            nameText.text = unitDataDic.GetUnitWeponName(id);
        }
        else if(id > 100)
        {
            nameText.text = unitDataDic.GetEnemyWeponName(id);
        }
        
    }
    public void SetADStatText(int id,float baseStat,float addStat, UnitType type = UnitType.Melee)
    {
        if(id < 100)
        {
            if(type == UnitType.Magic)
            {
                attackDefenceText.text = $"주문력 : {baseStat} [{FormatStat(addStat)}]";
            }
            else
            {
                attackDefenceText.text = $"공격력 : {baseStat} [{FormatStat(addStat)}]";
            }            
        }
        else if(id > 100)
        {
            attackDefenceText.text = $"물리 방어력 : {baseStat} [{FormatStat(addStat)}]";
        }
    }
    public void SetMDStatText(int id,float baseStat, float addStat)
    {
        if (id < 100)
        {
            magicDefenceText.text = $"공격속도 : {baseStat} [{FormatStat(addStat)}]";
        }
        else if (id > 100)
        {
            magicDefenceText.text = $"마법 방어력 : {baseStat} [{FormatStat(addStat)}]";
        }
        
    }
    public void SetMoveSpeedStatText(float stat)
    {
        moveSpeedText.text = $"이동속도 : {stat}";
    }
    public void SetArmorAttackStat(UnitType type)
    {
        int lightArmorStat = 0;
        int heavyArmorStat = 0;
        switch (type)
        {
            case UnitType.Melee:
                lightArmorStat = 30;
                heavyArmorStat = 15;
                break;
            case UnitType.Range:
                lightArmorStat = 10;
                heavyArmorStat = -30;
                break;
            case UnitType.Magic:
                lightArmorStat = 15;
                heavyArmorStat = -30;
                break;
        }
        
        lightArmorText.text = $"경장갑 : {FormatStat(lightArmorStat)}%";
        heavyArmorText.text = $"중장갑 : {FormatStat(heavyArmorStat)}%";
    }
    public void SetArmorAttackStat(UnitDefType type)
    {
        int meleestat = 0;
        int rangestat = 0;
        int magicstat = 0;
        switch (type)
        {
            case UnitDefType.LightArmor:
                meleestat = 30;
                rangestat = 10;
                magicstat = 15;
                break;
            case UnitDefType.HeavyArmor:
                meleestat = 15;
                rangestat = -30;
                magicstat = -30;
                break;
        }
        lightArmorText.text = $"근접공격 : {EnemyFormatStat(meleestat)}%";
        heavyArmorText.text = $"원거리공격 : {EnemyFormatStat(rangestat)}%";
        magicArmorText.text = $"마법공격 : {EnemyFormatStat(magicstat)}%";
    }
    private string FormatStat(float stat)
    {
        if(stat == 0)
        {
            return $"+{stat}";
        }
        else if(stat > 0)
        {
            return $"<color=#00C800>+{stat}</color>";
        }
        else
        {
            return $"<color=red>{stat}</color>";
        }
    }
    private string EnemyFormatStat(int stat)
    {
        if (stat < 0)
        {
            return $"<color=#00C800>{stat}</color>";
        }
        else
        {
            return $"<color=red>+{stat}</color>";
        }
    }

    public void DeleteStatToolTip()
    {
        nameText.text = "";
        attackDefenceText.text = "";
        magicDefenceText.text = "";
        moveSpeedText.text = "";
        lightArmorText.text = "";
        heavyArmorText.text = "";
        magicArmorText.text = "";
    }
}
