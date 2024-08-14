using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnManager : InGameSingleton<UnitSpawnManager>
{
    public List<Transform> userUnitSpawnPoint;

    public Transform SpawnUI;
    public GameObject SpawnBtn;
    public LayerMask userUnit;

    [SerializeField] private GameManager gm;
    [SerializeField] private ObjectPool<TextVariety> TextOP;
    [SerializeField] private GameObject alarmUI;

    // 유닛 데이터 SO
    UnitStatData[] unitDataSO;
    // 유닛 프리팹 모음
    public List<GameObject> unitPrefabs;

    // 유닛 생산버튼 스프라이트 용
    public UnitDataDic unitSpriteDic;

    // 유닛 출현 검사할 반경 
    private float checkRadius = 0.5f;

    // 최대 탐색 거리, 맵밖에 나가지 않도록 설정
    private float maxDistance = 4.0f;

    // 탐색 시 증가시킬 거리
    private float stepSize = 0.5f;

    [Header("Button")]
    [SerializeField] private BtnSound btnSound;

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        unitSpriteDic = UnitDataDic.Instance;
        unitDataSO = DataManager.Instance.UnitSO;
        List<int> unitIds = DataManager.Instance.UserUnitIDs;

        for (int i = 0; i < unitIds.Count; i++)
        {
            CreateSpawnBtn(unitIds[i]);
        }

        // 레이어 마스크 설정
        userUnit = LayerMask.GetMask("UserUnit");

        // 텍스트 오브젝트풀 캐싱
        TextOP = UserData.Instance.TextObjectPool;
    }

    private void CreateSpawnBtn(int unitId)
    {
        GameObject btn = Instantiate(SpawnBtn, SpawnUI);
        btn.GetComponent<SpawnBtn>().btnIndex = unitId;

        SetSpawnBtnText(btn, unitId);

        // 람다 표현식 내에서 사용될 별도의 지역 변수 생성
        int indexCopy = unitId;
        Button buttonInstance = btn.GetComponent<Button>();
        buttonInstance.onClick.AddListener(() => SpawnUnit(indexCopy));
        buttonInstance.onClick.AddListener(() => btnSound.PlayBtnClickSound());
    }

    // 버튼 생성하며 텍스트 설정
    private void SetSpawnBtnText(GameObject btn, int unitId)
    {
        
        string unitName = unitDataSO[unitId].CharacterName;
        int unitCost = unitDataSO[unitId].UnitCost;

        SpawnBtn spawnBtn = btn.GetComponent<SpawnBtn>();
        spawnBtn.SetSpawnBtn(unitName, unitCost, CurrencyType.Gold);

        if (unitSpriteDic == null)
        {
            return;
        }

        // 버튼 스프라이트 설정
        int adNum = 0;
        int id = unitId + adNum;

        if (unitSpriteDic.GetUnitSprite(id) != null)
        {
            spawnBtn.unitImage.sprite = unitSpriteDic.GetUnitSprite(id);
        }
    }

    private IEnumerator CheckSpriteData(SpawnBtn spawnBtn, int id)
    {
        // 스프라이트 데이터가 로드될 때까지 대기
        while (unitSpriteDic.UnitSpriteDic.TryGetValue(id, out Sprite value) == false)
        {
            yield return null;
        }

        if (unitSpriteDic.GetUnitSprite(id) != null)
        {
            spawnBtn.unitImage.sprite = unitSpriteDic.GetUnitSprite(id);
        }
    }

    public void SpawnUnit(int index)
    {
        if (!CheckCurrency(CurrencyType.Gold, unitDataSO[index].UnitCost))
        {
            // 화폐가 부족하면 리턴
            ShowCurCancelNotification();
            return;
        }

        Vector3 spawnPosition = userUnitSpawnPoint[(int)UserColor.Red].position;
        Vector3 originPosition = spawnPosition;

        if (IsPositionOccupied(spawnPosition))
        {
            // 위치가 이미 차지되어 있는지 체크
            spawnPosition = FindNewPosition(spawnPosition);

            if (spawnPosition == originPosition)
            {
                // 새로운 위치가 없다면 리턴
                RefundCost(CurrencyType.Gold, unitDataSO[index].UnitCost);

                // 유닛 출현 공간 없음 알림
                ShowAreaCancelNotification();
                return;
            }
        }
        GameObject obj = Instantiate(unitPrefabs[index], spawnPosition, Quaternion.identity, gm.DummyObject.transform);
    }

    private void ShowCurCancelNotification()
    {
        GameObject obj = TextOP.SpawnFromPool(TextVariety.UnitText);

        obj.transform.SetParent(alarmUI.transform, false);
        UnitTextUI TextUI = obj.GetComponent<UnitTextUI>();
        TextUI.SetText(Notification.Gold);
    }

    private void ShowAreaCancelNotification()
    {
        GameObject obj = TextOP.SpawnFromPool(TextVariety.UnitText);

        obj.transform.SetParent(alarmUI.transform, false);
        UnitTextUI TextUI = obj.GetComponent<UnitTextUI>();
        TextUI.SetText(Notification.Area);
    }

    // 유닛 로드용
    public void LoadUnit(int index, Vector3 position)
    {
        GameObject obj = Instantiate(unitPrefabs[index], position, Quaternion.identity);
    }

    // 해당 위치에 유닛이 있는지 체크
    private bool IsPositionOccupied(Vector3 position)
    {
        // 레이어 추가 매개변수를 통해 정확도 높일 수 있음
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, checkRadius, userUnit);
        if (hitColliders.Length > 0)
        {
            // 하나 이상의 콜라이더가 검출됬다면
            return true;
        }
        return false;
    }

    // 새로운 위치를 찾아 반환
    private Vector3 FindNewPosition(Vector3 originalPosition)
    {
        Vector3 newPosition = originalPosition;

        for (float distance = stepSize; distance <= maxDistance; distance += stepSize)
        {
            // 오른쪽 방향으로 탐색
            Vector3 rightDirection = originalPosition + new Vector3(distance, 0, 0);

            if (!IsPositionOccupied(rightDirection))
            {
                newPosition = rightDirection;
                break;
            }
        }
        return newPosition;
    }

    // Physics.OverlapSphere()를 사용하여 해당 위치에 다른 유닛이 있는지 체크
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float checkRadius = 0.5f;
        Vector3 position = userUnitSpawnPoint[(int)UserColor.Red].position;
        Gizmos.DrawWireSphere(position, checkRadius);
    }

    // 화폐 체크용
    // curType에 맞는 화폐가 충분한지 체크
    private bool CheckCurrency(CurrencyType curType, int cost)
    {
        if (curType == CurrencyType.Gold)
        {
            int CurA = CurrencyEventHandler.instance.CurGold;
            if (CurA < cost) return false;
            CurrencyEventHandler.instance.CurGold -= cost;
            return true;
        }
        else if (curType == CurrencyType.Diamond)
        {
            int CurB = CurrencyEventHandler.instance.CurDiamond;
            if (CurB < cost) return false;
            CurrencyEventHandler.instance.CurDiamond -= cost;
            return true;
        }
        return false;
    }

    // 화폐 환불용
    private void RefundCost(CurrencyType curType, int cost)
    {
        if (curType == CurrencyType.Gold)
        {
            CurrencyEventHandler.instance.CurGold += cost;
        }
        else if (curType == CurrencyType.Diamond)
        {
            CurrencyEventHandler.instance.CurDiamond += cost;
        }
    }

    // 게임 스타트 용
    // 스폰 포지션을 기점으로 stepSize간격으로 스폰
    public void SpawnUnitRed(int index, int count)
    {
        Vector3 spawnPosition = userUnitSpawnPoint[(int)UserColor.Red].position;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(unitPrefabs[index], spawnPosition, Quaternion.identity, gm.DummyObject.transform);
            spawnPosition = spawnPosition + new Vector3(stepSize, 0, 0);
        }
    }

    // 오브젝트 풀 초기화
    // 자식 오브젝트들 수만큼 반복하며 비활성화
    public void DisableObjectPool()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }
}
