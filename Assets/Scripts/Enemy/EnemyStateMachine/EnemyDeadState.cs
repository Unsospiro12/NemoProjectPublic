using UnityEngine;

public class EnemyDeadState : IState
{
    private Enemy enemy;
    private float deadTime = 1f; // 애니메이션 실행시간
    public EnemyDeadState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() // Dead 상태에 진입했을 때 할 행동
    {
        enemy.Die();
    }

    public void Update() // Dead상태에서 매 프레임마다 할 행동
    {
        deadTime -= Time.deltaTime;
        if (deadTime <= 0)
        {
            if (enemy.revive && !enemy.hasRevived)
            {
                enemy.StateMachine.ChangeState(new EnemyReviveState(enemy));
                enemy.hasRevived = true;
            }
            else
                enemy.Destroy();
        }
    }

    public void Exit() // Dead상태에서 나올 때 할 행동
    {

    }
}
