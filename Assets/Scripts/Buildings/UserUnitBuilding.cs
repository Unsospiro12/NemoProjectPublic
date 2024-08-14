using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUnitBuilding : Building, ISelectable
{
    public void DeSelectThis()
    {

    }

    public void SelectThis()
    {
        CloseAllUI();
        UIManager.Instance.Show<SpawnUI>();
    }

    public SelectableType SelectType()
    {
        throw new System.NotImplementedException();
    }

}
