using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserHealthUI : MonoBehaviour
{
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text baseHealthText;

    private void Start()
    {
        SetHealthUI();
        UserData.Instance.SetHealthEvent(SetHealthText);
    }

    private void SetHealthUI()
    {
        icon.sprite = iconSprite;
    }

    public void SetHealthText(int value)
    {
        healthText.text = $"{value}";
    }

    public void SetBaseHealthText(int value)
    {
        baseHealthText.text = $"/ {value}";
    }
}
