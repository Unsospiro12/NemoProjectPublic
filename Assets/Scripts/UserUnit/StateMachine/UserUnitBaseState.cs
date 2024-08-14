using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserUnitBaseState : IState
{
    protected NavMeshAgent agent;
    protected Animator animator;
    protected UserUnit userUnit;

    public UserUnitBaseState SuperState;
    public UserUnitBaseState[] SubStates;
    protected UserUnitBaseState currentSubState;
    public UserUnitBaseState(UserUnit userUnit)
    {
        this.userUnit = userUnit;
        agent = userUnit.Agent;
        animator = userUnit.Anim;
        SubStates = new UserUnitBaseState[0];
    }
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        
    }
    /// <summary>
    /// 적이 공격 사거리에 들어왔을 때 들어오는 함수
    /// </summary>
    public virtual void EnemyInRange()
    {

    }
    /// <summary>
    /// 공격중인 적이 사거리에서 벗어나면 호출되는 함수
    /// </summary>
    public virtual void EnemyExitRange()
    {

    }
    /// <summary>
    /// SuperState로 바뀔 때 바뀌어도 좋은지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    public virtual bool IsOkeyToChange()
    {
        return true;
    }
    public virtual void CameBackToThisState()
    {

    }

    public virtual void BackFromSubState()
    {
        Enter();
    }

    /// <summary>
    /// 상위 상태를 
    /// </summary>
    /// <param name="SuperState"></param>
    public void SetSuperState(UserUnitBaseState SuperState)
    {
        this.SuperState = SuperState;
    }
    public void SetSubStates(UserUnitBaseState[] SubStates)
    {
        this.SubStates = SubStates;
    }
    public void SetCurrentSubState(UserUnitBaseState CurrentSubState)
    {
        currentSubState = CurrentSubState;
    }
}
