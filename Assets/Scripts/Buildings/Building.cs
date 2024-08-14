using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField] protected GameObject selectionMarker;
    [SerializeField] protected GameObject spawnUI;
    [SerializeField] protected GameObject upgradeUI;

    // 다른 UI 모두 꺼주기
    public void CloseAllUI()
    {
        spawnUI.SetActive(false);
        upgradeUI.SetActive(false);
    }
}
