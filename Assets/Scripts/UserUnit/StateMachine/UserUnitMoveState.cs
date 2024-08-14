using System.Threading;
using UnityEngine;

/// <summary>
/// 경계와 추적 상태를 해제하고 목표지점을 설정하고 이동한다
/// Update에서 이동을 하고 이동이 끝나면 Idle 상태로 전환한다.
/// </summary>
public class UserUnitMoveState : UserUnitBaseState
{
    #region Private Fields
    private bool isUpdating;
    private float timeLimit; // 일정 시간 이상 이동만 할 경우 강제로 상위 스테이트로 전환
    private float startTime;

    #endregion
    #region Public Methods
    public UserUnitMoveState(UserUnit userUnit) : base(userUnit)
    {

    }
    public override void Enter()
    {
        base.Enter();
        agent.updatePosition = true;
        agent.isStopped = false;
        animator.SetBool(userUnit.AnimIsMoving, true);
        animator.Play("1_Run");
        userUnit.HoldFlag.SetActive(false);

        agent.speed = userUnit.Stat.MovementSpeed.TotalValule;
        userUnit.FlipToRight(userUnit.Action.TargetPosition.x > userUnit.transform.position.x);
        agent.SetDestination(userUnit.Action.TargetPosition);
        startTime = Time.time;
        timeLimit = Vector2.Distance(userUnit.Action.TargetPosition, userUnit.transform.position) * 1.5f / agent.speed;
        isUpdating = true;
    }

    public override void Exit()
    {
        animator.SetBool(userUnit.AnimIsMoving, false);
        isUpdating = false;
    }

    public override void Update()
    {
        //목적지에 도달했나 확인
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            userUnit.StateMachine.ChangeToSuperState();
            return;
        }

        // 시간 제한에 도달했나 확인
        if (Time.time - startTime > timeLimit)
        {
            userUnit.StateMachine.ChangeToSuperState();
        }
    }
    public override void EnemyInRange()
    {
        if (userUnit.Action.IsAlert && isUpdating)
        {
            userUnit.StateMachine.ChangeToSubState(userUnit.AttackState);
        }
    }
    public override void CameBackToThisState()
    {
        agent.speed = userUnit.Stat.MovementSpeed.TotalValule;
        userUnit.FlipToRight(userUnit.Action.TargetPosition.x > userUnit.transform.position.x);
        agent.SetDestination(userUnit.Action.TargetPosition); // 혹시 여기서 멈칫거리는 문제가?
    }
    #endregion
}
