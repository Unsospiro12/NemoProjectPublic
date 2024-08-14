using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWaveUI : MonoBehaviour
{
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text waveText;
    private int adWave = 1;

    private void Start()
    {
        SetWaveUI();

        GameManager.Instance.OnGameStart += ResetWaveText;
    }

    private void SetWaveUI()
    {
        icon.sprite = iconSprite;
    }

    public void SetWaveText(int value)
    {
        waveText.text = $"{value + adWave}";
    }

    public void ResetWaveText()
    {
        waveText.text = adWave.ToString();
    }
}
