using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserUnitStat : UnitStat
{
    #region Private Field
    private UserUnit userUnit;
    private ResourceStat mp = new ResourceStat();
    private int id;

    private AttributeStat attack = new AttributeStat();
    private AttributeStat attackSpeed = new AttributeStat();
    private AttributeStat attackRange = new AttributeStat();
    private AttributeStat cost = new AttributeStat();
    private AttributeStat attackArea = new AttributeStat();
    private bool isEffectOnEnemy;
    private UnitType unitAttackType;
    #endregion
    #region Public Properties
    public int ID
    {
        get { return id; }
    }
    public ResourceStat MP
    {
        get
        {
            return mp;
        }
    }
    public AttributeStat Atk
    {
        get
        {
            return attack;
        }
    }
    public AttributeStat AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
    }
    public AttributeStat AttackRange
    {
        get
        {
            return attackRange;
        }
    }
    public AttributeStat Cost
    {
        get
        {
            return cost;
        }
    }
    public AttributeStat AttackArea
    {
        get
        {
            return attackArea;
        }
    }
    public bool IsEffectOnEnemy
    {
        get
        {
            return isEffectOnEnemy;
        }
        private set
        {
            isEffectOnEnemy = value;
        }
    }
    public float AttackIncrement
    {
        get;
        private set;
    }
    public UnitType UnitAttackType
    {
        get
        {
            return unitAttackType;
        }

    }
    #endregion
    #region MonoBehaviour Callbacks
    protected override void Awake()
    {
        base.Awake();
        unitAttackType = new UnitType();

        if (InitialStat is UnitStatData unitstat)
        {
            attack.BaseValue = unitstat.Atk;
            mp.MaxValue = unitstat.Mp;
            attackSpeed.BaseValue = unitstat.AttackSpeed;
            attackRange.BaseValue = unitstat.AtkRange;
            cost.BaseValue = unitstat.UnitCost;
            attackArea.BaseValue = unitstat.AtkArea;
            movementSpeed.BaseValue = unitstat.MovementSpeed;
            id = unitstat.ID;

            mp.CurrentValue = mp.MaxValue;
            isEffectOnEnemy = unitstat.EffectOnEnemy1;
            AttackIncrement = unitstat.AttackIncrement;
        }
        unitAttackType = InitialStat.unitType;
        userUnit = GetComponent<UserUnit>();
    }
    private void Start()
    {
        SetUpgradeAttack();
    }

    #endregion

    #region Private Methods

    // 유닛 업그레이드 이벤트 등록, 유닛 타입으로 이벤트 구분 등록
    private void SetUpgradeAttack()
    {
        UnitUpgradeManager.Instance.SetUpgradeUnitEvent(UpgradeAttack, InitialStat.unitType);
    }

    private void UpgradeAttack(int value)
    {
        // 유닛의 공격력 보너스 수치는 유닛 개인의 공격력 증가량(AttackIncrement) * 업그레이드 레벨
        attack.BonusValue = AttackIncrement * value;
    }
    #endregion
}
