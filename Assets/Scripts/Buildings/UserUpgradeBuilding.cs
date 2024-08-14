using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUpgradeBuilding : Building, ISelectable
{
    public void DeSelectThis()
    {

    }

    public void SelectThis()
    {
        CloseAllUI();
        UIManager.Instance.Show<UpgradeUI>();
    }

    public SelectableType SelectType()
    {
        throw new System.NotImplementedException();
    }
}
