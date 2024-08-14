using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITopBar : MonoBehaviour
{
    // 재화 두가지를 변수로 받음
    [SerializeField] private Currency_UI curA;
    [SerializeField] private Currency_UI curB;
    
    //게임이 시작될때 상단 UI에있는 재화 UI관리 이벤트에 구독
    private void Start()
    {
        CurrencyEventHandler.Instance.SetCurGoldEvent(curA.SetText);
        CurrencyEventHandler.Instance.SetCurDiamondEvent(curB.SetText);
    }
    //신이 변경되거나 상단 UI가 파괴된다면 재화 UI관리 이벤트에 구독해제
    //private void OnDestroy()
    //{
    //    UITest.Instance.RemoveCurAEvent(curA.SetText);
    //    UITest.Instance.RemoveCurBEvent(curA.SetText);
    //}

}
