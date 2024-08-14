using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : InGameSingleton<GameManager>
{
    public event Action OnGameStart;
    public event Action OnGameOver;

    [SerializeField] private int startUnitCount = 0;

    public Currency currency;

    // 게임 종료 패널 용
    [SerializeField] public GameObject gameOverUI;
    public GameOverUI gameOverUISC;
    public EnemyRespawn EnemyRespawn;
    public WaveManager waveManager;
    public UnityAction<int, int, int, int> GameOverUIAction;
    public float time;

    [SerializeField] private UserHealthUI healthUI;

    // 게임 초기화 용 더미 오브젝트
    [SerializeField] private GameObject dummyObject;
    public GameObject DummyObject
    {
        get
        {
            if (dummyObject == null)
            {
                dummyObject = new GameObject();
                dummyObject.name = "DummyObject";
            }
            return dummyObject;
        }
    }


    // 체력 기록용
    public int baseHealth;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        OnGameStart += ResumeGame;
        OnGameStart += InitilalizeUnit;
        OnGameStart += SetGameOverEvent;
        OnGameStart += RecordBaseHealth;

        OnGameOver += StopGame;

        GameStart();
    }

    private void GameManager_OnGameOver()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        SetTime();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void GameStart()
    {
        OnGameStart?.Invoke();
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public void LoadGame()
    {

    }

    public void RestartGame()
    {
        UserData.Instance.ProjectilePool.DisableAllObjects();
        UserData.Instance.SkillObjectPool.DisableAllObjects();
        EliminateUnits();
        UnitUpgradeManager.Instance.ResetUpgrade();
        CurrencyEventHandler.Instance.ResetCurrency();
        UserData.Instance.UserHealth = baseHealth;
        EnemyManager.Instance.ResetStage();
        StatUI.Instance.DeleteAllStatUIData();
        gameOverUI.SetActive(false);

        time = 0;

        OnGameStart?.Invoke();
    }

    private void EliminateUnits()
    {
        Destroy(DummyObject);
        dummyObject = null;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void CheckGameOver(int userHealth)
    {
        if (userHealth <= 0)
        {
            SetGameOverUI();
            gameOverUI.SetActive(true);
            GameOver();
        }
        else
        {
            return;
        }
    }

    private void InitilalizeUnit()
    {
        UnitSpawnManager.Instance.SpawnUnitRed((int)UnitName.Archer, startUnitCount);
    }

    // GameOverUI에서 호출
    public void SetGameOverEvent()
    {
        GameOverUIAction = gameOverUISC.SetGameOverUI;
    }

    public void SetGameOverUI()
    {
        int maxHealth = baseHealth;
        GameOverUIAction?.Invoke(waveManager.WaveLevel, (int)time, maxHealth, UserData.Instance.UserHealth);
    }

    public void SetTime()
    {
        time += Time.deltaTime;
    }

    public void RecordBaseHealth()
    {
        baseHealth = UserData.Instance.UserHealth;
        SetBaseHealthUI();
    }
    private void SetBaseHealthUI()
    {
        healthUI.SetBaseHealthText(baseHealth);
    }

    public void ResetTime()
    {
        time = 0;
    }
}
