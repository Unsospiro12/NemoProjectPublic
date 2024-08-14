using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UserData;

public class CurrencyEventHandler : InGameSingleton<CurrencyEventHandler>
{
    public delegate void SetCurGold(int value);
    private event SetCurGold setCurGold;
    public delegate void SetCurDiamond(int value);
    private event SetCurDiamond setCurDiamond;

    [SerializeField] private int curGold = 1500;
    [SerializeField] private int curDiamond = 15;

    public int CurGold
    {
        get
        {
            return curGold;
        }
        set
        {
            curGold = value;
            setCurGold?.Invoke(curGold);
        }
    }
    public int CurDiamond
    {
        get
        {
            return curDiamond;
        }
        set
        {
            curDiamond = value;
            setCurDiamond?.Invoke(curDiamond);
        }
    }

    private void OnDestroy()
    {
        setCurGold = null;
        setCurDiamond = null;
    }
    public void SetCurGoldEvent(SetCurGold action)
    {
        setCurGold += action;
        setCurGold.Invoke(curGold);
    }
    public void RemoveCurGoldEvent(SetCurGold action)
    {
        setCurGold -= action;
    }
    public void SetCurDiamondEvent(SetCurDiamond action)
    {
        setCurDiamond += action;
        setCurDiamond.Invoke(curDiamond);
    }
    public void RemoveCurDiamondEvent(SetCurDiamond action)
    {
        setCurDiamond -= action;
    }
    public void ResetCurrency()
    {
        CurGold = 1500;
        CurDiamond = 15;
    }
}
