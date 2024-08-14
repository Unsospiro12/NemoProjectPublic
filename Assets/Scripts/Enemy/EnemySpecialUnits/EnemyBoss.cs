using UnityEngine;

public class EnemyBoss : Enemy
{
    private float patternTimer = 5f;

    public override void Setting(Transform[] points, int waveLevel, bool isBoss = false, bool isSpecialEnemy = false, int bossTargetIndex = 0)
    {
        base.Setting(points, waveLevel, isBoss);
        patternTimer = 5f;
    }

    private void Update()
    {
        StateMachine.Update();

        if (isBoss && !Dead)
        {
            patternTimer -= Time.deltaTime;
            if (patternTimer <= 0)
            {
                StateMachine.ChangeState(new EnemyBossPatternState(this));
                patternTimer = 5f; // 패턴 타이머 초기화
            }
        }
    }
}
