using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnitDictionaryManager : InGameSingleton<UnitDictionaryManager>
{
    [System.Serializable]
    public class Dict<T>
    {
        public int Key;
        public T Value;
    }

    // 플레이어 유닛과 적 유닛 sprite array
    [SerializeField] private Sprite[] userUnitSprites;
    [SerializeField] private List<Dict<Sprite>> enemySprites;

    // 적 SO 데이터
    [SerializeField] private List<Dict<EnemyStatData>> EnemySOs;

    // 딕셔너리에 보여줄 목록 prefab
    [SerializeField] private DictionaryUnit dictionaryContent;
    [SerializeField] private Transform parentContent;
    private ObjectPool dictionaryContentPool;
    private List<DictionaryUnit> dictionaryContents = new List<DictionaryUnit>();

    // 열쇠 갯수 텍스트
    [SerializeField] private TextMeshProUGUI keyText;

    // 유닛 상세 정보 창
    [SerializeField] private GameObject unitInfoScreen;
    [SerializeField] private Image unitIconSprite;
    [SerializeField] private TextMeshProUGUI unitLvNName;
    [SerializeField] private TextMeshProUGUI unitCost;
    [SerializeField] private TextMeshProUGUI unitStats;
    [SerializeField] private GameObject coverPanel;

    // 유닛 정보 선택 버튼
    [SerializeField] private Button ShowUnitButton;
    [SerializeField] private Button ShowEnemyButton;

    // 유닛 해금 경고 화면
    [SerializeField] private GameObject unitUnLockCheckBox;
    [SerializeField] private TextMeshProUGUI unitUnLockText;
    private int unitTryingToUnLock;

    private int _keyCount;
    private bool isUserUnit;

    [Header("BtnSound")]
    [SerializeField] private BtnSound btnSound;

    private int keyCount
    {
        get
        {
            return _keyCount;
        }
        set
        {
            _keyCount = value;
            DataManager.Instance.KeyCount = value;
            keyText.SetText(value.ToString());
        }
    }

    private void Start()
    {
        unitInfoScreen.SetActive(false);
        coverPanel.SetActive(false);

        // 오브젝트 풀 초기화
        dictionaryContentPool = new ObjectPool(dictionaryContent.gameObject, 0);

        keyCount = DataManager.Instance.KeyCount;

        ShowUnitButton.Select();
        ShowUserUnits();
    }

    /// <summary>
    /// 유닛 리스트를 생성하는 공통 메서드
    /// </summary>
    private void CreateUnitList<T>(List<Dict<T>> units, System.Func<int, string> getName, System.Func<int, string> getDescription)
    {
        foreach (var unit in units)
        {
            GameObject gameObject = dictionaryContentPool.SpawnFromPool();

            // RectTransform 초기화
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parentContent, false);  // SetParent 후 false로 설정
            rectTransform.localScale = Vector3.one;  // 크기 초기화
            rectTransform.anchoredPosition = Vector2.zero;  // 위치 초기화
            rectTransform.sizeDelta = dictionaryContent.GetComponent<RectTransform>().sizeDelta;  // 기본 크기 설정

            // 버튼 사운드 설정
            Button btn = gameObject.GetComponent<Button>();
            btn.onClick.AddListener(btnSound.PlayBtnClickSound);

            dictionaryContents.Add(gameObject.GetComponent<DictionaryUnit>());
            DictionaryUnit dict = gameObject.GetComponent<DictionaryUnit>();

            dict.Icon.sprite = unit.Value as Sprite;
            dict.UnitName.SetText(getName(unit.Key));
            dict.UnitDescription.SetText(getDescription(unit.Key));
            dict.UnitID = unit.Key;

            // 유저 유닛일 경우 잠금 상태 처리
            if (isUserUnit && !DataManager.Instance.UserUnitIDs.Contains(unit.Key))
            {
                dict.Lock();
            }
            else
            {
                dict.UnLock();
            }

            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 사용 가능한 유닛 목록의 유닛들을 딕셔너리에 보여주는 함수
    /// </summary>
    private void ShowUserUnits()
    {
        ShowEnemyButton.image.color = Color.white;
        ShowUnitButton.image.color = Color.yellow;

        isUserUnit = true;
        List<Dict<Sprite>> userUnits = new List<Dict<Sprite>>();

        for (int i = 0; i < userUnitSprites.Length; i++)
        {
            userUnits.Add(new Dict<Sprite> { Key = i, Value = userUnitSprites[i] });
        }

        CreateUnitList(userUnits,
            id => DataManager.Instance.UnitSO[id].CharacterName,
            id => DataManager.Instance.UnitSO[id].CharacterDescription);
    }

    /// <summary>
    /// 적 유닛의 목록을 보여주는 함수
    /// </summary>
    private void ShowEnemyUnits()
    {
        ShowUnitButton.image.color = Color.white;
        ShowEnemyButton.image.color = Color.yellow;
        isUserUnit = false;

        CreateUnitList(enemySprites,
            id => EnemySOs.Find(e => e.Key == id).Value.CharacterName,
            id => EnemySOs.Find(e => e.Key == id).Value.CharacterDescription);
    }

    // 메인씬 로드
    public void OnClickClose()
    {
        SceneManager.LoadScene(Constants.Scenes.MainScene);
    }

    // 유닛 상세 창 끄기
    public void OnClickCloseUnitDescription()
    {
        unitInfoScreen.SetActive(false);
        coverPanel.SetActive(false);
    }

    // 유닛 잠금 해제 확인
    public void UnLockUnit(int unitID)
    {
        if (keyCount > 0)
        {
            unitTryingToUnLock = unitID;
            unitUnLockText.SetText(DataManager.Instance.UnitSO[unitID].CharacterName + "을/를 해금 하시겠습니까?");
            unitUnLockCheckBox.SetActive(true);
        }
    }

    // 유닛 잠금 해제
    public void OnClickUnLock()
    {
        if(unitTryingToUnLock >= 0)
        {
            dictionaryContents.Find(k => k.UnitID == unitTryingToUnLock).UnLock();
            DataManager.Instance.UserUnitIDs.Add(unitTryingToUnLock);
            if (keyCount > 0)
            {
                keyCount--;
            }
        }
        unitTryingToUnLock = -1;
        unitUnLockCheckBox.SetActive(false);
    }

    public void OnClickUnLockCancel()
    {
        unitTryingToUnLock = -1;
        unitUnLockCheckBox.SetActive(false);
    }

    // 유닛 상세 창 켜기
    public void ShowUnitDetail(int unitID)
    {
        coverPanel.SetActive(true);
        unitInfoScreen.SetActive(true);

        if (isUserUnit)
        {
            UnitStatData unitData = DataManager.Instance.UnitSO[unitID];

            unitIconSprite.sprite = userUnitSprites[unitID];
            unitLvNName.SetText($"{unitData.CharacterName}");

            string cost = $"비용 : {unitData.UnitCost} 골드";
            unitCost.SetText($"{cost}");

            string specialEffect = "없음";
            string skillDesc = "없음";

            if (unitID == 3)
            {
                specialEffect = "공격시 상대를 느리게 만듭니다.";
            }
            if (unitID == 5)
            {
                skillDesc = "10초마다 5초동안 공격 속도가 2배가 됩니다.";
            }
            else if (unitID == 6)
            {
                skillDesc = "3초마다 2.5배의 데미지로 적을 공격합니다";
            }
            else if (unitID == 7)
            {
                skillDesc = "5초마다 공격이 땅을 불태워 5초동안 해당 영역 위의 적에게 0.2초마다 40의 마법 데미지를 줍니다";
            }

            unitStats.SetText($"공격력 : {unitData.Atk}   공격 속도 : {(1 / unitData.AttackSpeed).ToString("F1")}회/초\n" +
                $"사거리 : {unitData.AtkRange}m   공격 범위 : {unitData.AtkArea}\n" +
                $"특수 능력 : {specialEffect}\n" +
                $"스킬 : {skillDesc}");
        }
        else
        {
            EnemyStatData enemyData = EnemySOs.Find(e => e.Key == unitID).Value;

            unitIconSprite.sprite = enemySprites.Find(e => e.Key == unitID).Value;
            unitLvNName.SetText($"{enemyData.CharacterName}");

            string enemyDefType = "";
            switch (enemyData.DefType)
            {
                case UnitDefType.LightArmor:
                    enemyDefType = "경장갑 :\n" +
                        "받는 근거리 데미지 +30%\n" +
                        "받는 원거리 데미지 +10%\n" +
                        "받는 마법 데미지 +15%";
                    break;
                case UnitDefType.HeavyArmor:
                    enemyDefType = "중장갑 :\n" +
                        "받는 근거리 데미지 +15%\n" +
                        "받는 원거리 데미지 -30%\n" +
                        "받는 마법 데미지 -20%";
                    break;
            }
            unitCost.SetText($"{enemyData.CharacterDescription}");
            unitStats.SetText($"{enemyDefType}\n\n이동속도 : {enemyData.MovementSpeed}");
        }
    }

    public void OnClickShowEnemyUnitBtn()
    {
        ClearContent();
        ShowEnemyUnits();
    }

    public void OnClickShowUserUnitBtn()
    {
        ClearContent();
        ShowUserUnits();
    }

    // 컨텐츠 클리어
    private void ClearContent()
    {
        foreach(DictionaryUnit oj in dictionaryContents)
        {
            dictionaryContentPool.ReturnObject(oj.gameObject);
        }
        dictionaryContents.Clear();
    }
}
