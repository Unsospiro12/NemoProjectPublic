using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : InGameSingleton<StatUI>
{
    [SerializeField] private UnitStatUI unitStatUI;
    [SerializeField] private UnitIcon unitIcon;
    [SerializeField] private EnemyStatUI enemyStatUI;
    [SerializeField] private UnitDataDic unitDataDic;    
    [SerializeField] private StatToolTipUI tooltip;

    private void Start()
    {
        unitDataDic = UnitDataDic.Instance;
    }
    public void UserUnitStatSetting(UserUnitStat stat,int count)
    {
        string name = stat.CharacterName;
        int id = stat.CharaterID;
        UnitType attackType = stat.UnitAttackType;
        int magicUpgradeLV = UnitUpgradeManager.Instance.magicUpgradeStats.upLv;
        int meleeUpgradeLV = UnitUpgradeManager.Instance.meleeUpgradeStats.upLv;
        int rangeUpgradeLV = UnitUpgradeManager.Instance.rangeUpgradeStats.upLv;

        SetUnitName(id, name,count);
        SetUnitIcon(id);
        if(attackType == UnitType.Melee)
        {
            SetUnitAttackStat(attackType, meleeUpgradeLV);
            unitStatUI.SetAttackStatText(attackType, stat.Atk.BaseValue, stat.Atk.BonusValue);
        }
        else if(attackType == UnitType.Range)
        {
            SetUnitAttackStat(attackType, rangeUpgradeLV);
            unitStatUI.SetAttackStatText(attackType, stat.Atk.BaseValue, stat.Atk.BonusValue);
        }
        else if(attackType == UnitType.Magic)
        {
            SetUnitAttackStat(attackType, magicUpgradeLV);
            unitStatUI.SetAttackStatText(attackType, stat.Atk.BaseValue, stat.Atk.BonusValue);
        }
        SetUserToolTip(stat,id);        
    }
    public void EnemyStatSetting(EnemyStat stat,int count)
    {       
        int id = stat.CharaterID;
        int maxHp = stat.HP.MaxValue;
        int currentHp = stat.HP.CurrentValue;
        string enemyName = stat.CharacterName;
        UnitDefType defType = stat.EnemyDefType;
        SetUnitName(id, enemyName, count);
        SetEnemyHPStat(maxHp, currentHp);
        SetEnemyDefType(defType);
        SetEnemyUnitIcon(id);
        if(count == 0)
        {
            SetEnemyToolTip(stat,id);
        }
  
    }
    public void SetUnitName(int id,string name,int count)
    {
        //일반 유닛
        if(id < 100)
        {
            unitStatUI.gameObject.SetActive(true);
            unitIcon.gameObject.SetActive(false);
            enemyStatUI.gameObject.SetActive(false);
            unitStatUI.SetName(name);
        }
        //적 유닛
        else if(id > 100 && count == 0)
        {
            unitStatUI.gameObject.SetActive(false);
            unitIcon.gameObject.SetActive(false);
            enemyStatUI.gameObject.SetActive(true);
            enemyStatUI.SetName(name);
        }
        
    }
    public void SetUnitAttackStat(UnitType type, float stat)
    {
        unitStatUI.SetAttackUpgradeText(type, stat);
    }
    public void SetEnemyHPStat(float maxHP, float hp)
    {
        enemyStatUI.SetHPStat(maxHP, hp);
    }
    public void SetEnemyDefType(UnitDefType type)
    {
        enemyStatUI.SetDefType(type);
    }
    void SetUserToolTip(UserUnitStat stat,int id)
    {        
        float moveSpeed = stat.MovementSpeed.TotalValule;
        float attackBaseStat = stat.Atk.BaseValue;
        float attackAddStat = stat.Atk.BonusValue;
        float attackspeedbase = stat.AttackSpeed.BaseValue;
        float attackSpeedAdd = stat.AttackSpeed.BonusValue;
        UnitType attackType = stat.UnitAttackType;
        tooltip.SetNameText(id);
        tooltip.SetADStatText(id, attackBaseStat, attackAddStat, attackType);
        //공격속도
        tooltip.SetMDStatText(id, attackspeedbase, attackSpeedAdd);
        tooltip.SetMoveSpeedStatText(moveSpeed);
        tooltip.SetArmorAttackStat(attackType);

    }
    void SetEnemyToolTip(EnemyStat stat,int id)
    {
        float adDefenceBaseStat = stat.ADDef.BaseValue;
        float adDefenceAddStat = stat.ADDef.BonusValue;
        float moveSpeed = stat.MovementSpeed.TotalValule;
        float apDefenceBaseStat = stat.APDef.BaseValue;
        float apDefenceAddStat = stat.APDef.BonusValue;
        UnitDefType defType = stat.EnemyDefType;
        tooltip.SetNameText(id);
        tooltip.SetADStatText(id,adDefenceBaseStat, adDefenceAddStat);
        tooltip.SetMDStatText(id,apDefenceBaseStat, apDefenceAddStat);
        tooltip.SetMoveSpeedStatText(moveSpeed);
        tooltip.SetArmorAttackStat(defType);
    }
    public void DeleteUnitName()
    {
        unitStatUI.DeleteName();
        enemyStatUI.DeleteName();
    }

    public void DeleteUnitStat()
    {
        unitStatUI.DeleteStatData();
        enemyStatUI.DeleteStatData();
        unitStatUI.gameObject.SetActive(false);
        enemyStatUI.gameObject.SetActive(false);
    }
    public void SetUnitIcon(int id)
    {
        Sprite icon = UnitDataDic.Instance.GetUnitSprite(id);
        unitStatUI.SetIcon(icon);
    }
    public void SetEnemyUnitIcon(int id)
    {
        Sprite icon = UnitDataDic.Instance.GetEnemySprite(id);
        enemyStatUI.SetIcon(icon);
    }
    public void SetUnitIcon(List<UserUnit> selectedUnits)
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            int id = selectedUnits[i].Stat.CharaterID;           
            Sprite icon = UnitDataDic.Instance.GetUnitSprite(id);
            unitIcon.SetIcon(i, icon);
        }        
    }
    //유닛 표시 페이지에 따라 토글의 개수설정
    //만약 선택한 유닛개수가 14개라면 토글은 하나만 활성화
    public void SetToggles(int count)
    {
        unitStatUI.gameObject.SetActive(false);
        unitIcon.gameObject.SetActive(true);
        enemyStatUI.gameObject.SetActive(false);
        //모든 토글 초기화
        unitIcon.OffToggles();

        // 14로 나눈 후 올림 처리
        int toggleCount = Mathf.CeilToInt(count / 14f); 

        for (int i = 0; i < toggleCount && i < 4; i++)
        {
            unitIcon.OnToggles(i);
        }

    }
    public void DeleteUnitIcon()
    {
        unitStatUI.DeleteIcon();
        enemyStatUI.DeleteIcon();
    }
    public void DeleteUnitIcon(int idx)
    {
        unitIcon.DeleteIcon(idx);
        unitIcon.OffToggles();
    }
    public void DeActiveUnitIcon()
    {
        unitIcon.gameObject.SetActive(false);
    }

    public void DeleteAllStatUIData()
    {
        unitStatUI.gameObject.SetActive(true);
        enemyStatUI.gameObject.SetActive(true);
        DeleteUnitName();
        DeleteUnitStat();
        unitIcon.gameObject.SetActive(true);
        DeleteUnitIcon();
    }
}
