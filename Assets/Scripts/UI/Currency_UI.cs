using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Currency_UI : MonoBehaviour
{
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private List<Sprite> iconSprites;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text curText;

    private void Start()
    {
        SetUI(currencyType);
    }

    //게임내 자원을 표시하는 UI에 아이콘을 자동 지정하는 함수
    public void SetUI(CurrencyType type)
    {
        this.currencyType = type;
        icon.sprite = iconSprites[(int)currencyType];
    }

    //게임내 자원을 표시하는 UI에 텍스트를 설정하는 함수
    public void SetText(int value)
    {
        curText.text = $"{value}";
    }
}
