using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : InGameSingleton<EnemyManager>
{
    [SerializeField] private List<EnemyStat> enemyStats;
    [SerializeField] private EnemyRespawn enemyRespawn;
    [SerializeField] private WaveManager waveManager;

    public void RegisterEnemy(EnemyStat enemyStat)
    {
        enemyStats.Add(enemyStat);
    }

    public void ResetStage()
    {
        ResetEnemyStat();
        waveManager.ResetWaveManager();
    }

    public void ResetEnemyStat()
    {
        foreach (var enemyStat in enemyStats)
        {
            enemyStat.EnemyBaseStats();
        }
    }
}
