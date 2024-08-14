using System;
using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.AI;

public class StageLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] maps;
    [SerializeField] private int currentStageNum;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private EnemyRespawn enemyRespawn;
    public NavMeshData[] navMeshData;
    private NavMeshDataInstance navMeshDataInstance;
    private float enemySpawnTime = 2;
    private int enemySpawnAmount = 10;
    private int killReward = 7;
    [SerializeField] private Currency currency;

    private void OnEnable()
    {
        currentStageNum = DataManager.Instance.currentStage;
        StartSceneLoad(currentStageNum);
    }

    private void OnDisable()
    {
        // 씬이 비활성화될 때 NavMeshData 제거
        if (navMeshDataInstance.valid)
        {
            NavMesh.RemoveNavMeshData(navMeshDataInstance);
        }
    }

    public void StartSceneLoad(int stage)
    {
        // 스테이지에 따른 맵 생성
        int stageListNum = stage - 1;
        GameObject mapInstance = Instantiate(maps[stageListNum]);
        // 맵에 따른 NavMeshData 로드
        LoadNavMesh(stageListNum);
        // 생성된 맵에서 MovePoints 설정
        SetMovePoints(stageListNum,mapInstance);
        //스테이지에 따른 유닛 생성주기와 수 조절
        SetEnemySpawnLevel(stageListNum);

    }

    // 이미 Bake해놓은 NavMeshData를 불러와서 설정
    void LoadNavMesh(int stage)
    {
        if (navMeshData != null && stage >= 0 && stage < navMeshData.Length)
        {
            // 기존 NavMeshDataInstance가 있으면 제거합니다.
            if (navMeshDataInstance.valid)
            {
                NavMesh.RemoveNavMeshData(navMeshDataInstance);
            }
            // 새로운 NavMeshDataInstance를 생성하고 NavMesh에 추가합니다.
            navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData[stage], navMeshSurface.transform.position, navMeshSurface.transform.rotation);
        }
        else
        {
            // NavMeshData 오류 로그
            Debug.LogError("NavMeshData is not assigned or stage index is out of range.");
        }
    }

    // MovePoints 설정 메서드
    private void SetMovePoints(int stage,GameObject mapInstance)
    {
        MapMovePoint movepoint = maps[stage].GetComponent<MapMovePoint>();
        // 태그가 "MovePoint"인 모든 오브젝트 찾기       
        GameObject[] movePoints = movepoint.MovePoints;

        // EnemyRespawn 오브젝트에 MovePoints 설정
        if (movePoints.Length > 0)
        {
            enemyRespawn.movePoints = new Transform[movePoints.Length];
            for (int i = 0; i < movePoints.Length; i++)
            {
                enemyRespawn.movePoints[i] = movePoints[i].transform;
            }
        }
    }

    private void SetEnemySpawnLevel(int stage)
    {
        enemyRespawn.spawnTime = enemySpawnTime - (stage * 0.5f);
        enemyRespawn.perWaveMaxEnemies = enemySpawnAmount + (stage * enemySpawnAmount);
        currency.killCountThreshold = killReward - (stage * 2);
    }
}

