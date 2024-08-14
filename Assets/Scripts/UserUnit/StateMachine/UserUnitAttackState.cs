using UnityEngine;

public class UserUnitAttackState : UserUnitBaseState
{
    #region Private Field
    private float lastAttackTime;
    private float attackSpeed;
    private Enemy targetEnemy;
    private bool isUpdating;
    private float attackRange;
    #endregion

    #region Public Methods
    public UserUnitAttackState(UserUnit userUnit) : base(userUnit)
    {
        lastAttackTime = 0;
    }
    public override void Enter()
    {
        base.Enter();
        attackSpeed = userUnit.Stat.AttackSpeed.TotalValule;
        targetEnemy = userUnit.Action.TargetEnemy;
        attackRange = userUnit.Stat.AttackRange.TotalValule;

        agent.isStopped = true;
        isUpdating = true;
        if (targetEnemy == null || targetEnemy.Dead)
        {
            userUnit.Action.TargetEnemy = null;
            userUnit.StateMachine.ChangeToSuperState();
            return;
        }

        if (Time.time - lastAttackTime > attackSpeed && InAttackRange())
        {
            lastAttackTime = Time.time;
            userUnit.Action.Attack();
        }
    }

    public override void Exit()
    {
        isUpdating = false;
        agent.isStopped = false;
    }

    public override void Update()
    {
        if (isUpdating)
        {
            if (targetEnemy == null || targetEnemy.Dead)
            {
                userUnit.Action.TargetEnemy = null;
                userUnit.StateMachine.ChangeToSuperState();
                return;
            }
            // 공격 속도에 따른 시간 마다 Alert 상태면 공격 시도
            if (Time.time - lastAttackTime > attackSpeed && InAttackRange())
            {
                lastAttackTime = Time.time;
                userUnit.Action.Attack();
            }
        }
    }
    public override void EnemyExitRange()
    {
        if (isUpdating)
        {
            if (userUnit.Action.IsOnTheMove || userUnit.Action.IsHold)
            {
                targetEnemy = null;
                userUnit.Action.TargetEnemy = null;
            }
            userUnit.StateMachine.ChangeToSuperState();
        }
    }
    #endregion
    #region Private Methods
    private bool InAttackRange()
    {
        return Vector2.Distance((Vector2)targetEnemy.transform.position, (Vector2)userUnit.transform.position) < (attackRange + 0.2f);
    }
    #endregion
}