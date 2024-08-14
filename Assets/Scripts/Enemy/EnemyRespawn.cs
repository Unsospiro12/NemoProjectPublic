using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs; //Enemy 종류
    [SerializeField] private GameObject splitEnemyPrefab;
    [SerializeField] private GameObject bossPrefab; // Boss 종류
    [SerializeField] private GameObject slowEnemyPrefab;
    [SerializeField] private Button skipWaveButton;
    [SerializeField] GameManager gm;
    private WaitForSeconds waitSpawnTime;
    private Coroutine spawningCoroutine;
    public Transform[] movePoints; // 이동포인트
    public WaveManager waveManager;
    public float spawnTime; // 생성시간
    public int perWaveMaxEnemies; // Enemy 생성 수
    public int currentEnemyCount = 0; // 현재 Enemy 수
    public List<GameObject> currentWaveEnemies = new List<GameObject>(); // 현재 웨이브의 적을 추적

    private void Awake()
    {
        waitSpawnTime = new WaitForSeconds(spawnTime);
        skipWaveButton.interactable = false;
    }

    public void StartSpawningEnemies(int perWaveMaxEnemies)
    {
        skipWaveButton.interactable = false;
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
        }

        spawningCoroutine = StartCoroutine(SpawnEnemies(perWaveMaxEnemies));
    }

    private IEnumerator SpawnEnemies(int perWaveMaxEnemies)
    {
        int spawnEnemies = 0;
        while (spawnEnemies < perWaveMaxEnemies)
        {
            if (currentEnemyCount < perWaveMaxEnemies)
            {
                SpawnEnemyPrefab();
                spawnEnemies++;
                yield return waitSpawnTime;
            }
            else
            {
                yield return null;
            }
        }
        if (waveManager.WaveLevel != waveManager.maxWaveLevel - 1)
        {
            skipWaveButton.interactable = true; // 마지막 웨이브가 아닐 때만 스킵 버튼 활성화
            GiveRoundReward();
        }
        spawningCoroutine = null;
    }

    public void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, gm.DummyObject.transform);
        if (boss != null)
        {
            Enemy enemy = boss.GetComponent<Enemy>();
            enemy.Setting(movePoints, DataManager.Instance.currentStage);
            enemy.isBoss = true;
            currentEnemyCount++; // 보스는 하나만 생성
            currentWaveEnemies.Add(boss);
        }
    }

    private void SpawnEnemyPrefab()
    {
        GameObject enemyPrefab = GetCurrentStageEnemyPrefab();
        GameObject copy = Instantiate(enemyPrefab, gm.DummyObject.transform);
        if (copy != null)
        {
            Enemy enemy = copy.GetComponent<Enemy>();
            enemy.Setting(movePoints, DataManager.Instance.currentStage);
            currentEnemyCount++;
            currentWaveEnemies.Add(copy); // 현재 웨이브의 적에 추가

            if (waveManager.WaveLevel == 5)
            {
                enemy.SplitStateEffect();
            }
        }
    }

    public void SpawnSplitEnemies(Vector3 position, int targetIndex)
    {

        // 분열된 적의 생성 위치를 조금 다르게 설정
        Vector3 offset1 = new Vector3(-0.5f, 0, 0); // 첫 번째 적의 위치 오프셋
        Vector3 offset2 = new Vector3(0.5f, 0, 0);  // 두 번째 적의 위치 오프셋

        Vector3 spawnPosition1 = position + offset1;
        Vector3 spawnPosition2 = position + offset2;

        // 분열된 적을 각각 다른 위치에 생성
        GameObject splitEnemy1 = Instantiate(splitEnemyPrefab, spawnPosition1, Quaternion.identity, gm.DummyObject.transform);
        GameObject splitEnemy2 = Instantiate(splitEnemyPrefab, spawnPosition2, Quaternion.identity, gm.DummyObject.transform);

        if (splitEnemy1 != null && splitEnemy2 != null)
        {
            Enemy enemy1 = splitEnemy1.GetComponent<Enemy>();
            Enemy enemy2 = splitEnemy2.GetComponent<Enemy>();

            // Setting 메서드 호출 시 죽은 적의 targetPointIndex를 전달
            enemy1.Setting(movePoints, DataManager.Instance.currentStage, false, false, targetIndex);
            enemy2.Setting(movePoints, DataManager.Instance.currentStage, false, false, targetIndex);

            // 위치를 다시 초기화하지 않도록 합니다.
            enemy1.transform.position = spawnPosition1;
            enemy2.transform.position = spawnPosition2;

            // 분열된 적들에게 죽은 적의 targetPointIndex를 설정
            enemy1.targetPointIndex = targetIndex;
            enemy2.targetPointIndex = targetIndex;

            enemy1.hasSplit = true; // 분열된 Enemy는 다시 분열되지 않도록 설정
            enemy2.hasSplit = true; // 분열된 Enemy는 다시 분열되지 않도록 설정

            // 방향 전환 로직 적용
            ApplyDirectionLogic(enemy1, movePoints[targetIndex].position);
            ApplyDirectionLogic(enemy2, movePoints[targetIndex].position);

            currentEnemyCount += 2; // 적 2마리 추가
            currentWaveEnemies.Add(splitEnemy1);
            currentWaveEnemies.Add(splitEnemy2);
        }
    }

    private void ApplyDirectionLogic(Enemy enemy, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - enemy.transform.position;

        if (direction.x < 0) // 왼쪽으로 향하는 경우
        {
            enemy.transform.rotation = Quaternion.identity;
            enemy.enemyStat.healthBarTransform.rotation = Quaternion.identity;
        }
        else // 오른쪽으로 향하는 경우
        {
            enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            enemy.enemyStat.healthBarTransform.rotation = Quaternion.identity;
        }
    }



    private GameObject GetCurrentStageEnemyPrefab() // 라운드마다 다른 종류의 Enemy프리펩 생성
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            return null;
        }

        int index = waveManager.WaveLevel % enemyPrefabs.Length;
        if (index < 0 || index >= enemyPrefabs.Length)
        {
            return null;
        }
        return enemyPrefabs[index];
    }

    public void SpawnSlowEnemy(Vector3 position, int bossTargetIndex)
    {
        GameObject slowEnemy = Instantiate(slowEnemyPrefab, position, Quaternion.identity, gm.DummyObject.transform);
        if (slowEnemy != null)
        {
            Enemy enemy = slowEnemy.GetComponent<Enemy>();
            enemy.Setting(movePoints, DataManager.Instance.currentStage, false, true, bossTargetIndex);
            enemy.isSlowEnemy = true;
            Transform nextTargetPoint = movePoints[bossTargetIndex];
            Vector3 direction = nextTargetPoint.position - position;
            if (direction.x < 0) // 방향 전환 로직
            {
                slowEnemy.transform.rotation = Quaternion.identity;
                enemy.enemyStat.healthBarTransform.rotation = Quaternion.identity;
            }
            else
            {
                slowEnemy.transform.rotation = Quaternion.Euler(0, 180, 0);
                enemy.enemyStat.healthBarTransform.rotation = Quaternion.identity;
            }
            currentEnemyCount++;
            currentWaveEnemies.Add(slowEnemy); // 현재 웨이브의 적에 추가
        }
    }

    public void DeadEnemyCount(GameObject enemy) // 적이 죽었을 때 호출되는 함수
    {
        if (currentWaveEnemies.Contains(enemy))
        {
            currentEnemyCount--;
            currentWaveEnemies.Remove(enemy); // 현재 웨이브의 적에서 제거

            if (currentEnemyCount < 0)
            {
                currentEnemyCount = 0;
            }

            if (waveManager.WaveLevel == waveManager.maxWaveLevel - 1 && currentEnemyCount == 0)
            {
                StartCoroutine(DelayedGameClear(2f));
            }

            if (waveManager.IsBossStage() && !waveManager.bossAlive && !waveManager.slowEnemyAlive)
            {
                waveManager.BossDied();
                waveManager.SlowEnemyDied();
            }
        }
    }

    public void ResetRespawn()
    {
        currentEnemyCount = 0;
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            StopCoroutine("DelayGameClear");
            spawningCoroutine = null;
        }
    }

    internal void NextWave()
    {
        currentEnemyCount = 0;
        currentWaveEnemies.Clear();
    }

    public void GiveRoundReward()
    {
        Currency currency = FindObjectOfType<Currency>();
        currency.GiveRoundRewardToPlayer();
    }

    private IEnumerator DelayedGameClear(float delay)
    {
        yield return new WaitForSeconds(delay);
        waveManager.GameClear(); // 지연 후 게임 클리어
    }
}