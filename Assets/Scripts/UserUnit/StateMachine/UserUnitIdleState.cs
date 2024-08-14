using System.Diagnostics;

public class UserUnitIdleState : UserUnitBaseState
{
    #region Private Field
    private bool isUpdating;
    #endregion
    #region Public Methods
    public UserUnitIdleState(UserUnit userUnit) : base(userUnit)
    {

    }
    public override void Enter()
    {
        base.Enter();
        userUnit.Action.IsAlert = true;
        userUnit.Action.IsOnTheMove = false;
        userUnit.Action.IsTracking = !userUnit.Action.IsHold;
        userUnit.HoldFlag.SetActive(userUnit.Action.IsHold);

        agent.SetDestination(userUnit.transform.position);
        isUpdating = true;
        animator.SetBool(userUnit.AnimIsMoving, false);
    }

    public override void Exit()
    {
        isUpdating = false;
    }

    public override void EnemyInRange()
    {
        if (isUpdating)
        {
            if (userUnit.Action.IsHold)
            {
                userUnit.StateMachine.ChangeToSubState(userUnit.AttackState);
            }
            else
            {
                userUnit.StateMachine.ChangeToSubState(userUnit.TrackingState);
            }
        }
    }
    #endregion
}
