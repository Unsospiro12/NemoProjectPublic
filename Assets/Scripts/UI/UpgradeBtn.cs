using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeBtn : MonoBehaviour
{
    public List<GameObject> UpgradeButtons;
    [SerializeField] private List<TextMeshProUGUI> upgradeTexts;
    [SerializeField] private List<TextMeshProUGUI> upgradeCostTexts;
    [SerializeField] private List<TextMeshProUGUI> levelTexts;

    [Header("Upgrade Manager")]
    public UnitUpgradeManager unitManager;

    private void Start()
    {
        //foreach(var btn in UpgradeButtons)
        //{
        //    CollectButton(btn);
        //}

        StartCoroutine(CheckUnitManager());
    }

    // 버튼 오브젝트 내 필요한 컴포넌트 수집 
    private void CollectButton(GameObject btn)
    {
        upgradeTexts.Add(btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>());
        upgradeCostTexts.Add(btn.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>());
        levelTexts.Add(btn.transform.GetChild(3).GetComponent<TextMeshProUGUI>());
    }

    private void SetButton(UnitType unitType)
    {
        string upgradeText = "";
        switch (unitType)
        {

            case UnitType.Melee:
                upgradeText = "근거리";
                break;
            case UnitType.Range:
                upgradeText = "원거리";
                break;
            case UnitType.Magic:
                upgradeText = "마법";
                break;
        }

        upgradeTexts[(int)unitType].text = $"{upgradeText}\n업그레이드";

        upgradeCostTexts[(int)unitType].text = $"{unitManager.Stats[(int)unitType].upCost}";
        levelTexts[(int)unitType].text = $"Lv. {unitManager.Stats[(int)unitType].upLv}";
    }

    // 텍스트 업데이트
    public void UpdateLevelText(UnitType unitType, int value)
    {
        levelTexts[(int)unitType].text = $"Lv. {value}";
    }

    // unitManager가 null이 아닐 때까지 대기
    // 유닛 타입으로 버튼 세팅
    public IEnumerator CheckUnitManager()
    {
        while (unitManager == null)
        {
            yield return null;
        }

        for (int i = 0; i < UpgradeButtons.Count; i++)
        {
            SetButton((UnitType)i);
        }

        yield return null;
    }
}