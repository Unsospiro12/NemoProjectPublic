using UnityEngine;
using UnityEngine.UI;

public class EnemyStat : UnitStat
{
    [SerializeField] protected BaseStatSO enemyStatData;
    public GameObject healthBarBackGround;
    public ResourceStat HP = new ResourceStat();
    public AttributeStat ADDef = new AttributeStat();
    public AttributeStat APDef = new AttributeStat();
    public UnitDefType EnemyDefType;
    public Transform healthBarTransform;
    private Canvas healthBarCanvas;
    private GameObject hpInstance;


    protected override void Awake()
    {
        base.Awake();

        SetText(HP.Text, null);
        SetText(ADDef.Text, null);
        SetText(APDef.Text, null);

        EnemyBaseStats(); // 초기 스탯 적용

        // EnemyManager에 등록
        ResisterEnemyList();

        HealthBar();

    }
    public void EnemyBaseStats()
    {
        if (enemyStatData is EnemyStatData enemyStat)
        {
            lv.BaseValue = enemyStat.Lv;
            HP.MaxValue = enemyStat.Hp;
            HP.CurrentValue = enemyStat.Hp;
            ADDef.BaseValue = enemyStat.ADDef;
            APDef.BaseValue = enemyStat.APDef;
            MovementSpeed.BaseValue = enemyStat.MovementSpeed;
            EnemyDefType = enemyStat.DefType;
        }

    }

    public void HealthBar()
    {
        GameObject healthBarPrefab = Resources.Load<GameObject>("HealthBarPrefab");

        if (healthBarPrefab != null)
        {
            GameObject healthBarInstance = Instantiate(healthBarPrefab, transform);
            healthBarCanvas = healthBarInstance.GetComponent<Canvas>();
            hpInstance = healthBarInstance;

            healthBarCanvas.worldCamera = Camera.main;

            Transform backGroundTransform = hpInstance.transform.Find("BackGround");
            healthBarBackGround = backGroundTransform.gameObject;

            Transform hpBarTransform = backGroundTransform.Find("HpBar");
            HP.Image = hpBarTransform.GetComponent<Image>();
            HP.Image.fillAmount = 1f;

            healthBarTransform = hpBarTransform.transform;
        }
    }

    public void InitiallizeStats(int stageLevel) // 스테이지마다 Enemy Stat 변동
    {
        int stageRate = stageLevel - 1;
        if (enemyStatData is EnemyStatData enemyStat)
        {
            HP.MaxValue = (int)(enemyStat.Hp + (Constants.EnemyStatRate.HPRate * stageRate)); // 스테이지 마다 체력 증가량 설정
            HP.CurrentValue = HP.MaxValue;
            ADDef.BaseValue = enemyStat.ADDef + (Constants.EnemyStatRate.ADDefRate * stageRate);
            APDef.BaseValue = enemyStat.APDef + (Constants.EnemyStatRate.APDefRate * stageRate);
            MovementSpeed.BaseValue = enemyStatData.MovementSpeed;
        }
    }

    // EnemyManager 등록 용
    public void ResisterEnemyList()
    {
        EnemyManager.Instance.RegisterEnemy(this);
    }

    public void SetImage(Image bar, float value)
    {
        if (bar != null)
        {
            bar.fillAmount = value;
        }
    }
}
