using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserUnitTrackingState : UserUnitBaseState
{
    #region Private Fields
    private Enemy targetEnemy;
    private float attackRange;
    private bool isUpdating;
    #endregion

    #region Public Methods
    public UserUnitTrackingState(UserUnit userUnit) : base(userUnit)
    {
        
    }
    /// <summary>
    /// 타겟 적을 가져오고
    /// 공격 범위를 가져옴 (사거리를 고려해서 적이 멀어지면 추격해서 도착할 목표 위치 설정)
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        userUnit.Action.IsAlert = false; // 주변에 적이 들어오면 공격
        userUnit.Action.IsOnTheMove = false; // 이동중이 아님
        userUnit.Action.IsTracking = true; // 적을 추격하는 중
        userUnit.Action.IsHold = false;
        agent.isStopped = false; // 에이전트 이동

        targetEnemy = userUnit.Action.TargetEnemy;
        attackRange = userUnit.Stat.AttackRange.TotalValule;
        isUpdating = true;

        if (IsTargetEnemyInvalid())
        {
            userUnit.StateMachine.ChangeToSuperState();
            return;
        }

        if (IsTargetEnemyOutOfRange())
        {
            SetNewDestination();
        }
    }

    public override void Exit()
    {
        isUpdating = false;
    }
    public override bool IsOkeyToChange()
    {
        if(targetEnemy == null)
        {
            return true;
        }
        if (currentSubState == userUnit.MoveState)
        {
            if (IsTargetEnemyOutOfRange())
            {
                userUnit.Action.TargetPosition = CalculateNewTargetPosition();
                return false;
            }
        }
        return true;
    }
    public override void EnemyInRange()
    {
        if (isUpdating)
        {
            userUnit.StateMachine.ChangeToSubState(userUnit.AttackState);
        }
    }
    #endregion

    #region Private Methods
    private bool IsTargetEnemyInvalid()
    {
        return targetEnemy == null || targetEnemy.Dead;
    }

    private bool IsTargetEnemyOutOfRange()
    {
        return Vector3.Distance(targetEnemy.transform.position, userUnit.transform.position) > attackRange;
    }

    private void SetNewDestination()
    {
        userUnit.Action.TargetPosition = CalculateNewTargetPosition();
        userUnit.StateMachine.ChangeToSubState(userUnit.MoveState);
    }

    private Vector3 CalculateNewTargetPosition()
    {
        Vector3 directionToEnemy = (targetEnemy.transform.position - userUnit.transform.position).normalized;
        Vector3 newPosition = GridGenerator.Instance.Grid.WorldPositionInBound(targetEnemy.transform.position - directionToEnemy * (attackRange - 1.0f));
        NavMeshHit hit;
        NavMesh.SamplePosition(newPosition, out hit, 10f, 1);
        return hit.position;
    }
    #endregion
}
