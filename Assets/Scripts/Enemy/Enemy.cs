using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : Unit, IDamagable, ISelectable
{
    [SerializeField] private AudioClip audioClip;
    private int movePointCount; // 이동 경로 갯수
    private int killCount; // Enemy 처치했을 때 카운트
    private bool isSelected = false;
    public int targetPointIndex; // 목표 지점 인덱스
    public WaveManager waveManager;
    public EnemyStat enemyStat;
    public EnemyMovement enemyMovement; // Enemy 이동제어
    public EnemyRespawn respawn;
    public Transform[] movePoints; // 이동 경로 모음
    public GameObject reviveStateEffect;
    public GameObject splitStateEffect;
    public bool revive = false; // 부활 상태인지 아닌지 체크
    public bool hasRevived = false; // 중복 부활을 막기 위한 부활 여부 추적
    public int currentStageLevel; // 현재 웨이브 레벨

    private SelectableType selectableType = SelectableType.Enemy;
    public bool isBoss;
    public bool isSlowEnemy;
    public bool canRevive;
    public bool hasSplit;
    public bool Dead { get; set; }

    public virtual void Setting(Transform[] points, int stageLevel, bool isBoss = false, bool isSlowEnemy = false, int bossTargetIndex = 0) // Enemy 생성할때 설정 (이동 경로,스탯,상태 등)
    {
        string enemyName = gameObject.name.Replace("(Clone)", "");
        if (enemyName == "Skeleton" || enemyName == "SkeletonArcher" || enemyName == "SkeletonWarrior")
        {
            canRevive = true;
        }
        else
        {
            canRevive = false;
        }

        StateMachine = new StateMachine();
        Anim = GetComponent<Animator>();
        respawn = FindObjectOfType<EnemyRespawn>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyStat = GetComponent<EnemyStat>();
        waveManager = FindObjectOfType<WaveManager>();
        movePointCount = points.Length; // 이동 경로 갯수
        movePoints = points; // 이동 경로 갯수 모음
        currentStageLevel = stageLevel; // 현재 tmxpdlwl 레벨
        targetPointIndex = isSlowEnemy ? bossTargetIndex : 0; // 보스의 targetPointIndex를 적용
        if (!isSlowEnemy)
        {
            transform.position = points[targetPointIndex].position; // 첫번째 생성장소 지정
        }
        enemyStat.InitiallizeStats(stageLevel); // Enemy 스탯변동
        hasSplit = false;
        Dead = false;
        revive = false; // 부활 상태인지 아닌지 체크
        StateMachine.ChangeState(new EnemyMoveState(this));
    }

    public void Revive()
    {
        enemyStat.HP.Image.fillAmount = 1f;
        enemyStat.HP.CurrentValue = enemyStat.HP.MaxValue;
    }
    public void NextTargetPoint() // 다음 목표 포인트로 이동하게 하는 함수
    {
        if (targetPointIndex < movePointCount - 1) // 목표 지점 인덱스가 총 이동 경로 갯수 보다 작은 경우 실행 ( -1은 마지막 포인트에서 적 유닛이 파괴될 것이기 때문에)
        {
            Vector3 dir = (movePoints[targetPointIndex].position - transform.position).normalized; // 목표지점 - 현재지점 = 움직여야할 방향,거리(좌표)가 나옴

            if (Vector3.Distance(transform.position, movePoints[targetPointIndex].position) < 0.1f) // 목표지점에 도달했는 지 여부를 확인
            {
                targetPointIndex++; // 목표지점 변경
                dir = (movePoints[targetPointIndex].position - transform.position).normalized;
            }

            if (dir.x < 0) // 방향 전환 로직
            {
                transform.rotation = Quaternion.identity;
                enemyStat.healthBarTransform.rotation = Quaternion.identity;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                enemyStat.healthBarTransform.rotation = Quaternion.identity;
            }
            enemyMovement.Move(dir); // 방향,거리(좌표) 벡터값을 Move함수에 넘기고 Enemy를 움직이게 함
        }
        else if (targetPointIndex == movePointCount - 1) // 마지막 포인트에 도달했다면 실행 (현재 목표포인트 인덱스의 최대수치는 4 , 이동경로는 총 5개가 존재)
        {
            if (Vector3.Distance(transform.position, movePoints[targetPointIndex].position) < 0.1f) // 목표 지점에 도달했는 지 여부 확인
            {
                Dead = true;
                Destroy(); // Enemy가 마지막 목표지점에 도달했으면 Enemy 삭제
                UserData.Instance.UserHealth--; // 유저 라이프 감소
                respawn.DeadEnemyCount(gameObject); //  보스 여부 체크

                if (isBoss)
                {
                    waveManager.BossDied();
                }

                if (isSlowEnemy)
                {
                    waveManager.SlowEnemyDied();
                }
            }
        }
    }

    public void TakeDamage(int damage, UnitType attackType) // 데미지 받았을 때 호출되는 함수
    {
        if (!Dead)
        {
            UnitDefType unitDefType = enemyStat.EnemyDefType;
            float adDef = enemyStat.ADDef.TotalValule;
            float apDef = enemyStat.APDef.TotalValule;
            float addDamage = AdditionalDamageLogic(unitDefType, attackType);
            //데미지만큼 현재 체력 감소
            if (attackType == UnitType.Magic)
            {
                //Debug.Log($"APDamage : {(int)MathF.Round((damage - apDef) * addDamage, 0)}");
                enemyStat.HP.CurrentValue -= (int)MathF.Round((damage - apDef) * addDamage, 0); // 공격타입이 마법이라면 마법 방어력으로 계산
            }
            else
            {
                //Debug.Log($"ADDamage : {(int)MathF.Round((damage - adDef) * addDamage, 0)}");
                enemyStat.HP.CurrentValue -= (int)MathF.Round((damage - adDef) * addDamage, 0); // 공격타입이 물리라면 물리 방어력으로 계산
            }


            if (enemyStat.HP.CurrentValue < 0)
            {
                enemyStat.HP.CurrentValue = 0;
            }
            enemyStat.SetImage(enemyStat.HP.Image, (float)enemyStat.HP.CurrentValue / enemyStat.HP.MaxValue);


            if (enemyStat.HP.CurrentValue <= 0) // 체력이 0보다 작거나 같으면 죽음 상태로 바뀜
            {
                StateMachine.ChangeState(new EnemyDeadState(this));

                if (isBoss)
                {
                    waveManager.bossAlive = false;
                }

                if (isSlowEnemy)
                {
                    waveManager.slowEnemyAlive = false;
                }
            }
        }
    }

    //데미지 배율을 저장하는 배열
    float[,] damageMatrix =
    {
        //LightArmor HeavyArmor
        {1.3f , 0.85f }, // Melee
        {1.1f, 0.7f },   // Ranged
        {1.15f, 0.8f }   // Magic
    };
    float AdditionalDamageLogic(UnitDefType defType, UnitType attackType)
    {
        return damageMatrix[(int)attackType, (int)defType];
    }

    public void SelectThis()
    {
        isSelected = true;
    }

    public void DeSelectThis()
    {
        isSelected = false;
    }

    public SelectableType SelectType()
    {
        return selectableType;
    }

    public virtual void Die() // Enemy가 Player Unit에게 죽었을 때 호출되는 함수
    {
        Dead = true;
        enemyMovement.Move(Vector3.zero); // 죽었을 때 움직임을 멈추게 함
        Anim.SetTrigger("Die"); // Die 애니메이션 실행
        SoundManager.Instance.PlayEnemySFX(audioClip, 1);
        Currency currency = FindObjectOfType<Currency>();
        killCount++; // 킬 카운트 증가 
        currency.GiveKillCountRewardToPlayer(killCount); // 킬 카운트를 매개변수로 보내 조건이 충족하면 보상을 지급하는 함수
        int TutorialSceneIndexCheck = SceneManager.GetActiveScene().buildIndex;

        if (TutorialSceneIndexCheck == 1)
            return;

        if (isBoss)
        {
            waveManager.bossAlive = false;
        }

        if (isSlowEnemy)
        {
            waveManager.slowEnemyAlive = false;
        }

        if (canRevive && !revive && !hasRevived) // 스테이지 레벨이 2고 , 부활 상태가 아니고 , 중복부활을 하지 않았다면
        {
            ReviveStateEffect();
            revive = true; // 부활 상태로 체크
        }
        else
        {
            if (waveManager.WaveLevel == 5 && !hasSplit) // 6웨이브 체크
            {
                Vector3 deathPosition = transform.position; // 죽은 위치
                int deathTargetIndex = targetPointIndex; // 죽은 적의 TargetPointIndex

                // 분열된 적 생성
                respawn.SpawnSplitEnemies(deathPosition, deathTargetIndex);

                hasSplit = true; // 분열 완료로 상태 업데이트
            }
            waveManager.enemyRespawn.DeadEnemyCount(gameObject); // 보스 여부 체크
        }
    }

    public void Destroy() // 사용된 Enemy 오브젝트 제거하는 함수
    {
        Destroy(gameObject);
    }

    public Transform GetCurrentTargetPoint() // 목표 지점을 반환하는 함수
    {
        return movePoints[targetPointIndex];
    }

    public void ReviveStateEffect()
    {
        GameObject stateEffect = Instantiate(reviveStateEffect, gameObject.transform);
        Vector3 newPosition = stateEffect.transform.position;
        newPosition.y += 0.5f;
        stateEffect.transform.position = newPosition;
    }

    public void SplitStateEffect()
    {
        GameObject stateEffect = Instantiate(splitStateEffect, gameObject.transform);
        Vector3 newPosition = stateEffect.transform.position;
        newPosition.y += 0.5f;
        stateEffect.transform.position = newPosition;
    }
}
