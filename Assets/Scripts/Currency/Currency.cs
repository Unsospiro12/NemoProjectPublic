using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Currency : MonoBehaviour
{
    private UserData userCurrency;
    private int killCount;
    public int killCountThreshold; // 2킬당 보상주기

    // 프로퍼티
    public UserData UserCurrency
    {
        get => userCurrency;
        set => userCurrency = value;
    }

    [SerializeField] private int roundReward = 100;
    [SerializeField] private int killReward = 100;
    [SerializeField] private int CurBCost = 100;
    [SerializeField][Range(1, 5)] private int CurBRange = 5;

    // 텍스트 오브젝트를 오브젝트풀에 카운트만큼 생성하고 큐에 저장
    [Header("Currency Text")]
    public GameObject alarmUI;
    public int curTextObjectCount = 10;
    private ObjectPool<TextVariety> objectPool;

    private void Start()
    {
        userCurrency = GetComponent<UserData>();

        // 오브젝트 풀 캐싱
        objectPool = UserData.Instance.TextObjectPool;
    }

    // 라운드 마다 호출 될 함수
    public void GiveRoundRewardToPlayer()
    {
        CurrencyEventHandler.Instance.CurGold += roundReward;

        // 텍스트 오브젝트 활성화   // 
        UpdateCurText(roundReward, RewardType.Round, CurrencyType.Gold);
    }

    public void PurchaseCurB()
    {
        if (CurrencyEventHandler.Instance.CurGold >= CurBCost)
        {
            CurrencyEventHandler.Instance.CurGold -= CurBCost;
            int GambleResult = Random.Range(1, CurBRange + 1);
            CurrencyEventHandler.Instance.CurDiamond += GambleResult;

            // 텍스트 오브젝트 활성화
            UpdateCurText(GambleResult, RewardType.Gamble, CurrencyType.Diamond);
        }
        else
        {
            GameObject obj = objectPool.SpawnFromPool(TextVariety.UnitText);

            obj.transform.SetParent(alarmUI.transform, false);
            UnitTextUI TextUI = obj.GetComponent<UnitTextUI>();
            TextUI.SetText(Notification.Gold);
        }
    }

    // 죽인 적의 수를 카운트 해서 호출 될 함수
    public void GiveKillCountRewardToPlayer(int enemyCount)
    {
        killCount += enemyCount;
        if(killCount >= killCountThreshold)
        {
            CurrencyEventHandler.Instance.CurGold += killReward;
            killCount = 0;

            // 텍스트 오브젝트 활성화 
            UpdateCurText(killReward, RewardType.Kill, CurrencyType.Gold);
        }
    }

    // 보상을 얻게 된 경로와 보상 타입을 받아서 텍스트를 생성하는 함수
    private void UpdateCurText(int reward, RewardType rewardType, CurrencyType curType)
    {
        GameObject obj = objectPool.SpawnFromPool(TextVariety.CurrencyText);

        // RecTransform의 부모를 설정할 땐 SetParent를 사용하는 것

        obj.transform.SetParent(alarmUI.transform, false);
        CurrencyTextUI currencyTextUI = obj.GetComponent<CurrencyTextUI>();
        currencyTextUI.SetCurrencyText(reward, rewardType, curType); // 텍스트 적용
    }
}
