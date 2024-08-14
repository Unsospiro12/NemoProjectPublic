using System.Linq;
using UnityEngine;

public class UserUnitStateMachine
{
    #region Private Field
    private UserUnitBaseState currentState;
    private bool isDebugging = false;
    #endregion
    #region Public Properties
    public UserUnitBaseState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }
    //public UserUnitBaseState PreviousState
    //{
    //    get; private set;
    //}
    #endregion
    #region Public Methods
    /// <summary>
    /// 평범한 State바꾸는 함수
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(UserUnitBaseState state)
    {
        if (isDebugging)
        {
            Debug.Log("Just Change to State : " + currentState + " -> " + state);
        }

        currentState?.Exit(); // 원래 있던 상태를 꺼냄
        currentState = state; // 받아온 상태를 현재 상태에 반환
        currentState.Enter(); // 새로운 것이 들어왔으니 초기화
    }
    /// <summary>
    /// 새로운 SubState로 바꾸는 함수
    /// 현재 State를 SuperState로 저장해줌
    /// </summary>
    /// <param name="state"></param>
    public void ChangeToSubState(UserUnitBaseState state)
    {
        if (currentState.SubStates.Contains(state))
        {
            if (isDebugging)
            {
                Debug.Log("ChangeToSubState" + currentState + " -> " + state);
            }

            currentState.Exit();
            currentState.SetCurrentSubState(state);
            state.SetSuperState(currentState);
            currentState = state;
            currentState.Enter();
        }
        else
        {
            ChangeState(state);
        }
    }
    /// <summary>
    /// 현재 State가 가진 SuperState로 바꿔주는 함수
    /// </summary>
    public void ChangeToSuperState()
    {

        if (currentState.SuperState.IsOkeyToChange())
        {
            if (isDebugging)
            {
                Debug.Log("Change To SuperState : " + currentState + " -> " + currentState.SuperState);
            }

            currentState.Exit();
            currentState = currentState.SuperState;
            currentState.BackFromSubState();
        }
        else
        {
            if (isDebugging)
            {
                Debug.Log("Change Back : " + currentState + " -> " + currentState);
            }

            currentState.CameBackToThisState();
        }
    }

    /// <summary>
    /// 매 프레임 업데이트할 State내용
    /// </summary>
    public void Update()
    {
        currentState?.Update(); // 현재 상태 업데이트
    }

    /// <summary>
    /// 적 유닛이 공격 범위에 들어오면 호출
    /// </summary>
    public void EnemyInRange()
    {
        currentState?.EnemyInRange();
    }
    /// <summary>
    /// 적유닛이 공격 범위를 벗어나면 호출
    /// </summary>
    public void EnemyExitRange()
    {
        currentState?.EnemyExitRange();
    }
    #endregion
}
