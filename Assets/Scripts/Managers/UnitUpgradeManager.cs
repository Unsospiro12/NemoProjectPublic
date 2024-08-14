using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CurrencyEventHandler;
using static UserData;

public class UnitUpgradeManager : InGameSingleton<UnitUpgradeManager>
{
    [Header("Unit List")]
    public List<UnitUpgradeStats> Stats;
    public UnitUpgradeStats meleeUpgradeStats;
    public UnitUpgradeStats rangeUpgradeStats;
    public UnitUpgradeStats magicUpgradeStats;

    public delegate void OnUpgradeUnit(int value);
    private event OnUpgradeUnit onUpgradeMeleeUnit;
    private event OnUpgradeUnit onUpgradeRangeUnit;
    private event OnUpgradeUnit onUpgradeMagicUnit;

    [SerializeField] private UpgradeBtn upgradeBtn;
    [SerializeField] private ObjectPool<TextVariety> TextOP;
    [SerializeField] private GameObject alarmUI;


    protected override void Awake()
    {
        base.Awake();
        upgradeBtn = GetComponent<UpgradeBtn>();
    }

    private void Start()
    {
        upgradeBtn.unitManager = this;
        Stats.Add(meleeUpgradeStats);
        Stats.Add(rangeUpgradeStats);
        Stats.Add(magicUpgradeStats);

        TextOP = UserData.Instance.TextObjectPool;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onUpgradeMeleeUnit = null;
        onUpgradeRangeUnit = null;
        onUpgradeMagicUnit = null;
    }

    // 버튼 할당
    // 업그레이드 타입에 따라 레벨업 및 공격력 증가
    // 이벤트 호출
    public void UpgradeMelee()
    {
        if (!CheckCurrency(CurrencyType.Diamond, meleeUpgradeStats.upCost))
        {
            ShowCurCancelNotification();
            return;
        }

        meleeUpgradeStats.upLv += 1;                         // 레벨업
        meleeUpgradeStats.Atk += meleeUpgradeStats.upAtk;    // 공격력 증가
        onUpgradeMeleeUnit?.Invoke(meleeUpgradeStats.Atk);   // 이벤트 호출

        // 텍스트 업데이트
        upgradeBtn.UpdateLevelText(UnitType.Melee, meleeUpgradeStats.upLv);
    }

    public void UpgradeRange()
    {
        if (!CheckCurrency(CurrencyType.Diamond, rangeUpgradeStats.upCost))
        {
            ShowCurCancelNotification();
            return;
        }

        rangeUpgradeStats.upLv += 1;
        rangeUpgradeStats.Atk += rangeUpgradeStats.upAtk;
        onUpgradeRangeUnit?.Invoke(rangeUpgradeStats.Atk);

        // 텍스트 업데이트
        upgradeBtn.UpdateLevelText(UnitType.Range, rangeUpgradeStats.upLv);
    }

    public void UpgradeMagic()
    {
        if (!CheckCurrency(CurrencyType.Diamond, magicUpgradeStats.upCost))
        {
            ShowCurCancelNotification();
            return;
        }

        magicUpgradeStats.upLv += 1;
        magicUpgradeStats.Atk += magicUpgradeStats.upAtk;
        onUpgradeMagicUnit?.Invoke(magicUpgradeStats.Atk);

        // 텍스트 업데이트
        upgradeBtn.UpdateLevelText(UnitType.Magic, magicUpgradeStats.upLv);
    }

    private void ShowCurCancelNotification()
    {
        GameObject obj = TextOP.SpawnFromPool(TextVariety.UnitText);

        obj.transform.SetParent(alarmUI.transform, false);
        UnitTextUI TextUI = obj.GetComponent<UnitTextUI>();
        TextUI.SetText(Notification.Diamond);
    }

    // 유닛 타입에 따라 이벤트 할당
    // TODO : 스위치 문 안 쓰고 다른 방법으로 변경?
    public void SetUpgradeUnitEvent(OnUpgradeUnit action, UnitType type)
    {
        switch(type)
        {
            case UnitType.Melee:
                onUpgradeMeleeUnit += action;
                onUpgradeMeleeUnit?.Invoke(meleeUpgradeStats.Atk);
                break;
            case UnitType.Range:
                onUpgradeRangeUnit += action;
                onUpgradeRangeUnit?.Invoke(rangeUpgradeStats.Atk);
                break;
            case UnitType.Magic:
                onUpgradeMagicUnit += action;
                onUpgradeMagicUnit?.Invoke(magicUpgradeStats.Atk);
                break;
        }
    }

    // 화폐 체크용
    // curType에 맞는 화폐가 충분한지 체크
    private bool CheckCurrency(CurrencyType curType, int cost)
    {
        if (curType == CurrencyType.Gold)
        {
            int CurA = CurrencyEventHandler.instance.CurGold;
            if (CurA < cost) return false;
            CurrencyEventHandler.instance.CurGold -= cost;
            return true;
        }
        else if (curType == CurrencyType.Diamond)
        {
            int CurB = CurrencyEventHandler.instance.CurDiamond;//
            if (CurB < cost) return false;
            CurrencyEventHandler.instance.CurDiamond -= cost;
            return true;
        }
        Debug.Log("화폐 체크 버그!");
        return false;
    }

    public void ResetUpgrade()
    {
        // 업그레이드 초기화
        meleeUpgradeStats.upLv = 0;
        meleeUpgradeStats.Atk = 0;
        rangeUpgradeStats.upLv = 0;
        rangeUpgradeStats.Atk = 0;
        magicUpgradeStats.upLv = 0;
        magicUpgradeStats.Atk = 0;
        
        // 이벤트 호출
        onUpgradeMeleeUnit?.Invoke(meleeUpgradeStats.Atk);
        onUpgradeRangeUnit?.Invoke(rangeUpgradeStats.Atk);
        onUpgradeMagicUnit?.Invoke(magicUpgradeStats.Atk);

        // 텍스트 업데이트
        upgradeBtn.UpdateLevelText(UnitType.Melee, meleeUpgradeStats.upLv);
        upgradeBtn.UpdateLevelText(UnitType.Range, rangeUpgradeStats.upLv);
        upgradeBtn.UpdateLevelText(UnitType.Magic, magicUpgradeStats.upLv);
    }
}

[System.Serializable]
public struct UnitUpgradeStats
{
    public int upLv;
    public int upCost;
    public int upAtk;
    public int Atk;
}
