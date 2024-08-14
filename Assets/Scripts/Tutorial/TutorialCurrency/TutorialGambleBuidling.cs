using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGambleBuidling : Building
{
    public Currency currency;

    [Header("Tutorial Currency")]
    [SerializeField] TutorialCurrency tutorialCurrency;

    public void OnClickGamble()
    {
        CloseAllUI();
        currency.PurchaseCurB();
        tutorialCurrency.ClickGamble();
    }
}
