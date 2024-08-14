using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private float waitRespawnTimes; // 다음 웨이브까지 인터벌 시간
    [SerializeField] private int waveLevel;
    [SerializeField] private GameObject skipUI;
    [SerializeField] private GameObject startUI;
    [SerializeField] private Button skipWaveButton;
    [SerializeField] private Button startWaveButton;
    [SerializeField] private CurrentWaveUI curWaveUI; // 메인 웨이브 UI

    public EnemyRespawn enemyRespawn;
    public int maxWaveLevel; // 최종 웨이브
    public int bossWave; // 보스 웨이브
    public int WaveLevel
    {
        get
        {
            return waveLevel;
        }
        set
        {
            waveLevel = value;
            curWaveUI.SetWaveText(waveLevel);
        }
    }
    public bool bossAlive = false; // 보스가 생존 체크
    public bool slowEnemyAlive = false; // 보스 소환 유닛 생존 체크
    private WaitForSeconds waitRespawnTime;
    private Coroutine waveCoroutine;

    private void Awake()
    {
        waitRespawnTime = new WaitForSeconds(waitRespawnTimes);
        skipWaveButton.onClick.AddListener(OnSkipWaveButtonClicked);
        startWaveButton.onClick.AddListener(OnStartWaveButtonClicked);
    }

    private void OnSkipWaveButtonClicked()
    {
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
            waveCoroutine = null;
        }
        NextWave();
    }

    private void OnStartWaveButtonClicked()
    {
        StartWaveManagement();
        skipUI.SetActive(true);
        startUI.SetActive(false);
    }

    private void StartWaveManagement()
    {
        if (waveCoroutine == null)
            waveCoroutine = StartCoroutine(ManageWave());
    }

    private IEnumerator ManageWave()
    {
        while (true)
        {
            if (IsGameClear())
            {
                GameClear();
                yield break;
            }

            if (IsBossStage())
            {
                yield return HandleBossWave();
            }
            else
            {
                yield return HandleNormalStage();
            }
        }
    }

    private IEnumerator HandleNormalStage()
    {
        enemyRespawn.StartSpawningEnemies(enemyRespawn.perWaveMaxEnemies);

        while (enemyRespawn.currentEnemyCount > 0 || enemyRespawn.currentWaveEnemies.Count < enemyRespawn.perWaveMaxEnemies) // 적이 모두 죽을때 까지 대기
        {
            yield return null;
        }
        yield return waitRespawnTime;
        NextWave();
    }

    private IEnumerator HandleBossWave()
    {
        enemyRespawn.SpawnBoss();
        bossAlive = true;
        slowEnemyAlive = true;
        skipWaveButton.interactable = false;

        while (bossAlive && slowEnemyAlive)
        {
            yield return null; // 보스가 살아있는 동안 대기
        }
        StopCoroutine(waveCoroutine);
    }

    private bool IsGameClear()
    {
        return WaveLevel >= maxWaveLevel;
    }

    public bool IsBossStage()
    {
        return (WaveLevel + 1) % bossWave == 0;
    }

    public void GameClear()
    {
        skipWaveButton.interactable = false;
        GameManager.Instance.SetGameOverUI();
        GameManager.Instance.gameOverUI.SetActive(true);
        GameManager.Instance.GameOver();
    }

    public void NextWave()
    {
        WaveLevel++;
        bossAlive = false; // 보스 생존 체크 초기화
        enemyRespawn.NextWave();

        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
            waveCoroutine = null;
        }

        waveCoroutine = StartCoroutine(ManageWave());
    }

    public void BossDied()
    {
        bossAlive = false; // 보스 생존 체크
    }

    public void SlowEnemyDied()
    {
        slowEnemyAlive = false; // 생존 체크
    }

    public void ResetWaveManager()
    {
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
            waveCoroutine = null;
        }
        skipUI.SetActive(false);
        startUI.SetActive(true);
        WaveLevel = 0;
        bossAlive = false;
        enemyRespawn.ResetRespawn();
    }
}
