public class StateMachine
{
    protected IState currentState; // 현재 상태를 받아오는 변수

    public IState CurrentState { get { return currentState; } }

    /// <summary>
    /// 상태를 전환하는 함수
    /// 현재 State에 Exit함수를 부르고
    /// 다음 State에 Enter함수를 부른다
    /// </summary>
    /// <param name="state"></param>
    public virtual void ChangeState(IState state)
    {
        currentState?.Exit(); // 원래 있던 상태를 꺼냄
        currentState = state; // 받아온 상태를 현재 상태에 반환
        currentState?.Enter(); // 새로운 것이 들어왔으니 초기화
    }

    /// <summary>
    /// 매 프레임 업데이트할 State내용
    /// </summary>
    public void Update()
    {
        currentState?.Update(); // 현재 상태 업데이트
    }
}