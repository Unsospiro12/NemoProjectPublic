using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGambleBuilding : Building
{
    public Currency currency;

    public void OnClickGamble()
    {
        CloseAllUI();
        currency.PurchaseCurB();
    }
}
