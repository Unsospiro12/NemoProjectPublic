using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
public interface IDamagable
{
    /// <summary>
    /// 해당 유닛이 죽었는지 판단하는 값
    /// </summary>
    public bool Dead { get; set; }
    /// <summary>
    /// 해당 유닛이 공격 받을 경우 호출되는 함수
    /// </summary>
    /// <param name="damage">적이 계산한 최종 데미지</param>
    public void TakeDamage(int damage,UnitType type);
    /// <summary>
    /// 해당 유닛이 죽을 경우 호출되는 함수
    /// </summary>
    public void Die();
}
public interface IAttackable
{
    /// <summary>
    /// 공격을 실행하는 함수
    /// </summary>
    public void SetTarget(Enemy target);
    /// <summary>
    /// 공격하면서 이동하는 함수
    /// UserUnit에 있는 함수를 확인
    /// </summary>
    /// <param name="targetPosition"></param>
    public void AttackWhileMoving(Vector2 targetPosition);
}
public interface IMovable
{
    /// <summary>
    /// 이동 함수
    /// </summary>
    /// <param name="target">이동하는 위치</param>
    /// <param name="offset">위치에서 떨어진 목표지점 거리</param>
    public void Move(Vector2 target);
    /// <summary>
    /// 홀드 상태 함수. 자동으로 적을 추적하지 않음
    /// </summary>
    public void Hold();
    /// <summary>
    /// 잠깐 이동을 멈추는 함수. 적이 보이면 자동으로 추적함
    /// </summary>
    public void StopMoving();
}
public interface ISelectable
{
    /// <summary>
    /// 이 유닛이 선택 되었을 때 불리는 함수
    /// </summary>
    public void SelectThis();
    /// <summary>
    /// 이 유닛이 선택 해제되었을 때 불리는 함수
    /// </summary>
    public void DeSelectThis();
    /// <summary>
    /// Selectable이 어떤 타입인지 반환
    /// </summary>
    /// <returns>SelectableType Enum</returns>
    public SelectableType SelectType();
}
public interface IState
{
    public void Enter();

    public void Update();

    public void Exit();

}
public class Unit : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    public Animator Anim { get; protected set; }
    public virtual StateMachine StateMachine { get; protected set; }

    #endregion
    #region Monobehaviour Callbacks
    protected virtual void Awake()
    {
        StateMachine = new StateMachine();
    }
    protected virtual void Update()
    {
        StateMachine.Update();
    }
    #endregion
}
