using UnityEngine;

public class EnemyBossPatternState : IState
{
    public Enemy boss;
    private float patternDuration = 1f; // 패턴 실행 시간
    private bool patternExecuted = false;

    public EnemyBossPatternState(Enemy boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Anim.SetTrigger("Pattern"); // 패턴 애니메이션 실행
        patternExecuted = false;
        boss.enemyMovement.Move(Vector3.zero);
    }

    public void Update()
    {
        patternDuration -= Time.deltaTime;

        if (patternDuration <= 0 && !patternExecuted)
        {
            // 특수한 Enemy를 소환하는 로직
            SpawnSlowEnemy();
            patternExecuted = true;
        }

        if (patternDuration <= 0)
        {
            boss.StateMachine.ChangeState(new EnemyMoveState(boss)); // 패턴 종료 후 이동 상태로 전환
        }
    }

    public void Exit()
    {
        boss.Anim.ResetTrigger("Pattern");
        patternDuration = 1f; // 패턴 지속 시간을 초기화
    }

    private void SpawnSlowEnemy()
    {
        EnemyRespawn respawn = GameObject.FindObjectOfType<EnemyRespawn>();
        if (respawn != null)
        {
            respawn.SpawnSlowEnemy(boss.transform.position, boss.targetPointIndex);
        }
    }
}
