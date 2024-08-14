using UnityEngine;
using UnityEngine.AI;

public class UserUnitMovingAttackState : UserUnitBaseState
{
    #region Private Fields
    #endregion

    #region Public Methods
    public UserUnitMovingAttackState(UserUnit userUnit) : base(userUnit)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        userUnit.Action.IsAlert = true;
        userUnit.Action.IsOnTheMove = true;
        userUnit.Action.IsTracking = false;
        userUnit.Action.IsHold = false;
        userUnit.Action.TargetEnemy = null;

        agent.SetDestination(userUnit.Action.TargetPosition);

        userUnit.StateMachine.ChangeToSubState(userUnit.MoveState);
    }

    public override void BackFromSubState()
    {
        userUnit.StateMachine.ChangeToSuperState();
    }
    #endregion
}
