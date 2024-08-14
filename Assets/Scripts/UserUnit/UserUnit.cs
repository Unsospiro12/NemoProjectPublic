using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UserUnit : MonoBehaviour, ISelectable, IAttackable, IMovable
{
    #region Private Field
    private SelectableType selectableType = SelectableType.UserUnit;
    #endregion
    #region Public Field
    public int AnimIsMoving = Animator.StringToHash("IsMoving");
    public int AnimOnAttack = Animator.StringToHash("OnAttack");
    public GameObject HoldFlag;
    #endregion
    #region Serialize Field
    [SerializeField] private UserUnitAction action;
    [SerializeField] private UserUnitStat stat;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private GameObject SelectionMarker;
    [SerializeField] private CircleCollider2D enemyDetector;
    #endregion
    #region Public Properties
    public UserUnitAction Action
    {
        get { return action; }
        private set { action = value; }
    }
    public UserUnitStat Stat
    {
        get { return stat; }
        private set { stat = value; }
    }
    public NavMeshAgent Agent
    {
        get
        {
            return agent;
        }
    }
    public SelectableType SelectableType
    {
        get { return selectableType; }
        private set { selectableType = value; }
    }
    public UserUnitIdleState IdleState { get; private set; }
    public UserUnitMoveState MoveState {  get; private set; }
    public UserUnitMovingAttackState MovingAttackState {  get; private set; }
    public UserUnitTrackingState TrackingState {  get; private set; }
    public UserUnitAttackState AttackState { get; private set; }
    public UserUnitJustMoveState JustMoveState { get; private set; }

    [field: SerializeField]
    public Animator Anim { get; protected set; }
    public virtual UserUnitStateMachine StateMachine { get; protected set; }
    #endregion
    #region MonoBehaviour Callbacks
    private void Awake()
    {
        // NavMesh Plus 용 오류 수정
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // 스테이트 생성과 세팅
        StateMachine = new UserUnitStateMachine();

        // 루트
        IdleState = new UserUnitIdleState(this); 

        // 1층 브랜치
        MovingAttackState = new UserUnitMovingAttackState(this);
        TrackingState = new UserUnitTrackingState(this);
        JustMoveState = new UserUnitJustMoveState(this);

        // 2층 브렌치
        MoveState = new UserUnitMoveState(this);

        // 리프 브랜치
        AttackState = new UserUnitAttackState(this);

        // 각 스테이트 SuperState 세트 해주기 (leaf 스테이트는 super가 바뀔 수 있기에 안줌
        MovingAttackState.SetSuperState(IdleState);
        TrackingState.SetSuperState(IdleState);
        JustMoveState.SetSuperState(IdleState);

        // 각 스테이트 SubState 세트
        IdleState.SetSubStates(new UserUnitBaseState[] { MovingAttackState, TrackingState, JustMoveState, AttackState });
        MovingAttackState.SetSubStates(new UserUnitBaseState[] { MoveState, AttackState });
        TrackingState.SetSubStates(new UserUnitBaseState[] { MoveState, AttackState });
        JustMoveState.SetSubStates(new UserUnitBaseState[] { MoveState });
        MoveState.SetSubStates(new UserUnitBaseState[] { AttackState } );
    }
    private void Start()
    {
        // 적 감지 trigger 콜라이더의 반지름을 유닛 사거리로 설정한다.
        enemyDetector.radius = stat.AttackRange.TotalValule;
        StateMachine.ChangeState(IdleState);
        
        agent.speed = stat.MovementSpeed.TotalValule;
        agent.acceleration = 100f;
    }
    private void Update()
    {
        StateMachine.Update();
    }
    private void OnDestroy()
    {
        StateMachine = null;

        // 루트
        IdleState = null;

        // 1층 브랜치
        MovingAttackState = null;
        TrackingState = null;
        JustMoveState = null;

        // 2층 브렌치
        MoveState = null;

        // 리프 브랜치
        AttackState = null;
    }
    #endregion
    #region Public Methods
    public void DeSelectThis()
    {
        if (SelectionMarker != null)
        {
            SelectionMarker.SetActive(false);
        }
    }
    public void Move(Vector2 target)
    {
        action.TargetPosition = target;
        StateMachine.ChangeToSubState(JustMoveState);
    }
    public void SelectThis()
    {
        if (SelectionMarker != null)
        {
            SelectionMarker?.SetActive(true);
        }
    }
    public void StopMoving()
    {
        
    }
    public void Hold()
    {
        action.IsHold = true;
        StateMachine.ChangeState(IdleState);
    }
    public SelectableType SelectType()
    {
        return selectableType;
    }
    public void Attack()
    {
        throw new System.NotImplementedException();
    }
    public void SetTarget(Enemy target)
    {
        action.SetTarget(target);
        StateMachine.ChangeToSubState(TrackingState);
    }
    /// <summary>
    /// 적이 있는지 확인하는 IsAlert를 켜고, 가는 길에 적을 추적하느라 따른 길로 세지 않도록 IsTracking은 끄고, 가다가 적을 공격해도 다시 이동하도록 IsOnTheMove는 켜준다.
    /// 그 다음 해당 지점으로 이동을 시킨다.
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="offset"></param>
    public void AttackWhileMoving(Vector2 targetPosition)
    {
        action.TargetPosition = targetPosition;
        StateMachine.ChangeToSubState(MovingAttackState);
    }
    /// <summary>
    /// 유닛 뒤집는 함수
    /// true를 주면 오른쪽으로 뒤집히고 false를 주면 왼쪽으로 뒤집힘
    /// </summary>
    /// <param name="isFlip"></param>
    public void FlipToRight(bool isFlip)
    {
        transform.rotation = isFlip ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
    }
    #endregion
}
