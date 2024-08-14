using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class UserUnitAction : MonoBehaviour
{
    private UserUnit userUnit;
    private UserUnitStat stat;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 effectOriginPosition;

    private Enemy targetEnemy;
    private LinkedList<Vector2> path = new LinkedList<Vector2>();
    private Vector2 targetPosition;

    [field:SerializeField]
    public AttackBehaviour AttackBehaviour {  get; private set; }
    [SerializeField] private ParticleSystem hitEffect;

	public Enemy TargetEnemy
    {
        get { return targetEnemy; }
        set 
        {
            targetEnemy = value; 
        }
    }
    public bool IsAlert{ get; set; }
    public bool IsTracking {  get; set; }
    public bool IsOnTheMove {  get; set; }
    public bool IsHold { get; set; }
    public Vector2 TargetPosition
    {
        get { return targetPosition; }
        set 
        {
            targetPosition = value; 
        }
    }
    public LinkedList<Vector2> Path
    {
        get
        {
            return path;
        }
    }
    private void Awake()
    {
        userUnit = GetComponent<UserUnit>();
        
        IsAlert = true;
        IsTracking = true;
        IsOnTheMove = false;
        targetEnemy = null;
        IsHold = false;

        stat = userUnit.Stat;
        agent = userUnit.Agent;
        animator = userUnit.Anim;
    }
    private void OnEnable()
    {
        AttackBehaviour.Setting(userUnit);
    }

    /// <summary>
    /// 공격 실행함수로 공격 애니메이션, 보는 방향 지정하고 공격 실행
    /// 영웅의 경우 확률적으로 스킬 발동
    /// </summary>
    public void Attack()
    {
        AttackBehaviour.Attack();
    }

    /// <summary>
    /// 적이 범위안에 들어왔을 때 처리
    /// 타겟이 없으면 이 적으로 타겟을 바꾸고
    /// 타겟을 추적중이었다면 무시
    /// </summary>
    /// <param name="enemy"></param>
    public void OnEnemyInRange(Enemy enemy)
    {
        if (targetEnemy == null)
        {
            targetEnemy = enemy;
        }
        if (!enemy.Dead)
        {
            userUnit.StateMachine.EnemyInRange();
        }
    }
    /// <summary>
    /// 적이 범위에서 나갔을 경우 처리
    /// 적을 추적중이면 쫓아가고
    /// 아니면 타겟을 null로 변경하고 새 타겟이 들어오길 기다림
    /// 현재 타겟인 적이 사거리를 벗어나면 현재 State의 콜백을 부른다.
    /// </summary>
    /// <param name="enemy"></param>
    public void OnEnemyExitRange(Enemy enemy)
    {
        if (targetEnemy != null && targetEnemy == enemy)
        {
            //Debug.Log("Target Enemy Out of Range");
            userUnit.StateMachine.EnemyExitRange();
        }
    }
    public void SetTarget(Enemy enemy)
    {
        targetEnemy = enemy;
    }
    public void PlayHitEffects()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySwordSound();
        }

        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect.gameObject, targetEnemy.transform.position, Quaternion.identity);
            Vector3 particlePosition = effect.transform.position;
            particlePosition.z -= 0.1f; // 타일맵보다 앞쪽으로 이동
            effect.transform.position = particlePosition;
            Destroy(effect, 0.5f);
        }
    }
}
