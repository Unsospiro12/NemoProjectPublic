using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserData : InGameSingleton<UserData>
{
    #region Private Field
    private int[] currency;
    // 라이프, 가진 유닛 종류/수 ,
    #endregion
    #region Serialize Field
    // 자원 UI
    // TODO : UI로 이동
    [SerializeField] private TextMeshProUGUI[] currencyText;
    [SerializeField] private TagObjectPair<ProjectileNames>[] projectilePair;
    [SerializeField] private TagObjectPair<TextVariety>[] textPair;
    [SerializeField] private TagObjectPair<SkillEffect>[] skillPair;
    #endregion
    #region Public Properties
    // 게임 기본 화폐 2종
    // TODO : PlyaerStatus로 이동
    public int[] Currency
    {
        get
        {
            return currency;
        }
        private set
        {
            currency = value;
        }
    }
    public ObjectPool<ProjectileNames> ProjectilePool { get; private set; }
    public ObjectPool<TextVariety> TextObjectPool { get; private set; }
    public ObjectPool<SkillEffect> SkillObjectPool { get; private set; }
    #endregion
    #region MonoBehaviour Callbacks
    protected override void Awake()
    {
        base.Awake();
        ProjectilePool = new ObjectPool<ProjectileNames>();
        ProjectilePool.InitializePool(projectilePair);
        TextObjectPool = new ObjectPool<TextVariety>();
        TextObjectPool.InitializePool(textPair);
        SkillObjectPool = new ObjectPool<SkillEffect>();
        SkillObjectPool.InitializePool(skillPair);
    }
    private void Start()
    {
        SetCurrency();
    }
    #endregion
    #region Public Methods
    // 어떤 화폐를 얼마나 얻는 지
    public void AddCurrency(CurrencyType curType, int income)
    {
        Currency[(int)curType] += income;
        currencyText[(int)curType].text = Currency[(int)curType].ToString();
    }
    // 화폐 소모
    public bool SpendCurrency(CurrencyType curType, int cost)
    {
        if (Currency[(int)curType] < cost)
        {
            return false;
        }
        else
        {
            Currency[(int)curType] -= cost;
            currencyText[(int)curType].text = Currency[(int)curType].ToString();
            return true;
        }
    }
    #endregion
    #region Private Methods
    // 화폐 초기화
    private void SetCurrency()
    {
        Currency = new int[2];
        Currency[(int)CurrencyType.Gold] = 0;
        Currency[(int)CurrencyType.Diamond] = 0;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        setHealth = null;
    }
    #endregion

    public delegate void SetHealth(int value);
    private event SetHealth setHealth;

    [SerializeField] private int userHealth = 10;
    public int UserHealth
    {
        get => userHealth;
        set
        {
            userHealth = value;
            setHealth?.Invoke(userHealth);
            GameManager.instance.CheckGameOver(userHealth);
        }
    }

    public void SetHealthEvent(SetHealth action)
    {
        setHealth += action;
        setHealth.Invoke(userHealth);
    }
}
