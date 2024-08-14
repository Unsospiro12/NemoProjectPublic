using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUnitJustMoveState : UserUnitBaseState
{
    public UserUnitJustMoveState(UserUnit userUnit) : base(userUnit)
    {
    }
    public override void Enter()
    {
        base.Enter();
        userUnit.Action.IsAlert = false;
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
}
