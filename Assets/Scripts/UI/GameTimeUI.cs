using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeUI : MonoBehaviour
{
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text timeText;
    private int min;
    private int sec;

    private void Start()
    {
        SetTimeUI();
        //UserData.Instance.SetHealthEvent(SetTimeText);
    }

    private void Update()
    {
        SetTimeText((int)GameManager.Instance.time);
    }


    private void SetTimeUI()
    {
        icon.sprite = iconSprite;
    }

    public void SetTimeText(int value)
    {
        DivideTime(value);
        timeText.text = $"{min:D2}:{sec:D2}";
    }

    private void DivideTime(int value)
    {
        min = value / 60;
        sec = value % 60;
    }
}
