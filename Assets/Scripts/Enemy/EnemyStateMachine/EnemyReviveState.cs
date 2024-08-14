public class EnemyReviveState : IState
{
    private Enemy enemy;

    public EnemyReviveState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() // Revive 상태에 진입했을 때 할 행동
    {
        enemy.Dead = false;
        enemy.hasRevived = true;
        enemy.Revive();
        enemy.StateMachine.ChangeState(new EnemyMoveState(enemy));
    }

    public void Update() // Revive 상태에서 매 프레임 마다 할 행동
    {

    }

    public void Exit() // Revive 상태에서 나올 때 할 행동
    {

    }
}

