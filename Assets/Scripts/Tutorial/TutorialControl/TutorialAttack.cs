using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class TutorialAttack : TutorialBase
{
    [SerializeField] private GameObject tutorialUnit;
    [SerializeField] private GameObject[] spawnPoints;

    [SerializeField] private bool isAttacked = false;

    [SerializeField] private List<GameObject> tutorialGos;

    [Header("Enemy")]
    [SerializeField] private int maxEnemyCount = 7;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTime = 1.0f;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private int stageLevel = 3;
    [SerializeField] private float endTime = 13f;

    [Header("Mission")]
    [SerializeField] private Dialog[] mission;
    [SerializeField] private Dialog[] missionComplete;
    [SerializeField] private Dialog[] missionGoal;
    [SerializeField] TutorialManager TM;
    [SerializeField] private string missionTxt;
    [SerializeField] private string completeTxt;
    [SerializeField] private string goalTxt;
    [SerializeField] private int baseComplete;
    [SerializeField] private int completeCount;


    public override void Enter()
    {
        isAttacked = false;
        tutorialGos = new List<GameObject>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Vector3 spawnPos = spawnPoints[i].transform.position;
            tutorialGos.Add(Instantiate(tutorialUnit, spawnPos, Quaternion.identity));
        }

        // 조작 버튼 열어주기
        TutorialManager.Instance.tutorialScreen[(int)TutorialScreen.Control].SetActive(false);

        // 튜토리얼 미션 텍스트 제작해서 삽입
        completeCount = baseComplete;
        TM = TutorialManager.Instance;
        UpdateMissionComplete();
        CreateMissionText();

        StartCoroutine(SpawnTutorialEnemy());

        GameManager.Instance.RecordBaseHealth();
    }

    public override void Execute(TutorialController controller)
    {
        if (isAttacked)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        isAttacked = false;
        foreach (var tutorialGo in tutorialGos)
        {
            Destroy(tutorialGo);
        }
        tutorialGos.Clear();

        // 자원 초기화
        CurrencyEventHandler.Instance.CurGold = 0;
        CurrencyEventHandler.Instance.CurDiamond = 0;

        StartCoroutine(TM.DisableMissionBox());
        UserData.Instance.UserHealth = GameManager.Instance.baseHealth;
    }

    private IEnumerator SpawnTutorialEnemy()
    {
        int spawnEnemies = 0;
        while (spawnEnemies < maxEnemyCount)
        {
            SpawnEnemyObj();
            spawnEnemies++;
            yield return new WaitForSeconds(spawnTime);
        }

        yield return new WaitForSeconds(endTime);

        UpdateMissionComplete();
        CreateMissionText();

        isAttacked = true;
    }

    private void SpawnEnemyObj()
    {
        GameObject obj = Instantiate(enemyPrefab);
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.Setting(movePoints, stageLevel);
    }

    private void UpdateMissionComplete()
    {
        missionComplete[0].dialog = completeCount.ToString();
    }

    private void CreateMissionText()
    {
        missionTxt = "";
        completeTxt = "";
        goalTxt = "";

        if (mission.Length == 1)
        {
            missionTxt = mission[0].dialog;
            completeTxt = missionComplete[0].dialog;
            goalTxt = missionGoal[0].dialog;
        }
        else
        {
            string tempText;

            for (int i = 1; i < mission.Length; i++)
            {
                tempText = "\n" + mission[i].dialog;
                missionTxt = missionTxt + tempText;
            }
            missionTxt = mission[0].dialog + missionTxt;

            for (int i = 1; i < missionComplete.Length; i++)
            {
                tempText = "\n" + missionComplete[i].dialog;
                completeTxt = completeTxt + tempText;
            }
            completeTxt = missionComplete[0].dialog + completeTxt;

            for (int i = 1; i < missionGoal.Length; i++)
            {
                tempText = "\n" + missionGoal[i].dialog;
                goalTxt = goalTxt + tempText;
            }
            goalTxt = missionGoal[0].dialog + goalTxt;
        }

        UpdateMissionTxt();
    }

    private void UpdateMissionTxt()
    {
        TM.MissionTxt.text = missionTxt;
        TM.CompleteTxt.text = completeTxt;
        TM.GoalTxt.text = goalTxt;
        TM.MissionBox.SetActive(true);
    }

    private void DisableMissionBox()
    {
        TM.MissionBox.SetActive(false);
    }
}
