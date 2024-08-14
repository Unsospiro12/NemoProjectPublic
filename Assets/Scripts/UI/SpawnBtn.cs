using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBtn : MonoBehaviour
{
    public int btnIndex;
    public Image unitImage;
    public Image costImage;
    public Sprite CurA;
    public Sprite CurB;
    public TextMeshProUGUI unitText;
    public TextMeshProUGUI costText;

    public void SetSpawnBtn(string unit, int cost)
    {
        unitText.text = unit;
        costText.text = cost.ToString();
    }

    public void SetSpawnBtn(string unit, int cost, CurrencyType curType)
    {
        unitText.text = unit;
        costText.text = cost.ToString();

        switch (curType)
        {
            case CurrencyType.Gold:
                costImage.sprite = CurA;
                break;
            case CurrencyType.Diamond:
                costImage.sprite = CurB;
                break;
        }
    }
}
