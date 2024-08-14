using UnityEngine;

public class EnemyMoveState : IState
{
    private Enemy enemy;

    public EnemyMoveState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() // Move 상태에 진입했을 때 할 행동
    {
        enemy.Anim.SetTrigger("Move");
    }

    public void Update() //Enemy 이동방향, 목표 지점을 향해 이동
    {
        Vector3 currentPosition = enemy.transform.position;
        Vector3 targetPosition = enemy.GetCurrentTargetPoint().position;

        if (Vector3.Distance(currentPosition, targetPosition) < 0.02f * enemy.enemyMovement.moveSpeed) // 현재 위치와 목표 지점 거리가 0.02f *  ~ 보다 작은 경우 실행 
        {                                                                                                                              // 0.02f는 속도가 빠를 때 한 프레임에 0.02가 오버하기 때문에 값을 넣어줌
            enemy.NextTargetPoint(); // 다음 목표 지점 이동
        }
        Vector3 direction = (targetPosition - currentPosition).normalized;
        enemy.enemyMovement.Move(direction);
    }

    public void Exit()
    {
        enemy.Anim.ResetTrigger("Move");
    }
}
