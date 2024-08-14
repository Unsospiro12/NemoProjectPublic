using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class CurrencyTextUI : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    public Animator Animator;
    private int hash;
    private ObjectPool<TextVariety> objectPool;

    private void Awake()
    {
        hash = Animator.StringToHash("ActiveText");
    }

    private void Start()
    {
        objectPool = UserData.Instance.TextObjectPool;
    }

    public void SetCurrencyText(int currency, RewardType retype, CurrencyType curType)
    {
        string currencyType = "";

        switch (curType)
        {
            case CurrencyType.Gold:
                currencyType = "골드";
                break;
            case CurrencyType.Diamond:
                currencyType = "다이아몬드";
                break;
        }

        switch(retype)
        {
            case RewardType.Round:
                currencyText.text = $"+ {currency} {currencyType} : 웨이브 보상!";
                break;
            case RewardType.Kill:
                currencyText.text = $"+ {currency} {currencyType} : 처치 보상!";
                break;
            case RewardType.Gamble:
                currencyText.text = $"+ {currency} {currencyType}!";
                break;
        }

        // 애니메이션 적용
        ActiveCurTextAnimation();
    }

    public void ActiveCurTextAnimation()
    {
        Animator.SetTrigger(hash);
    }

    public void DisableObject()
    {
        objectPool.ReturnObject(TextVariety.CurrencyText, this.gameObject);
    }
}
