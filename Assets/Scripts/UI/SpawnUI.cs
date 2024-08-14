using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUI : BaseUI
{
    [SerializeField] private Currency_UI curA;
    [SerializeField] private Currency_UI curB;
    //UI비활성화 함수
    public override void HideDirect()
    {
        UIManager.Instance.Hide<SpawnUI>();
    }

    //UI 여는 함수
    //UI를 열면 게임내 자원을 관리하는 이벤트에 구독
    public override void OnOpened(object[] param)
    {
        SetCurACurrency();
        SetCurBCurrency();
    }

    //UI 닫는 함수
    //UI를 닫으면 게임내 자원을 관리하는 이벤트에 구독해제
    public override void OnClosed(object[] param)
    {
        RemoveCurACurrency();
        RemoveCurBCurrency();
    }

    //자원A를 관리하는 이벤트 구독
    public void SetCurACurrency()
    {
        CurrencyEventHandler.Instance.SetCurGoldEvent(curA.SetText);
    }
    //자원B를 관리하는 이벤트 구독
    public void SetCurBCurrency()
    {
        CurrencyEventHandler.Instance.SetCurDiamondEvent(curB.SetText);
    }
    //자원A를 관리하는 이벤트 구독해제
    public void RemoveCurACurrency()
    {
        CurrencyEventHandler.Instance.RemoveCurGoldEvent(curA.SetText);
    }
    //자원B를 관리하는 이벤트 구독해제
    public void RemoveCurBCurrency()
    {
        CurrencyEventHandler.Instance.RemoveCurDiamondEvent(curB.SetText);
    }
}
