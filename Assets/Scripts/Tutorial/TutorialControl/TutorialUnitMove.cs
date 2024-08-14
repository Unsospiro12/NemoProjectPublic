using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialUnitMove : TutorialBase
{
    [SerializeField] private GameObject tutorialUnit;
    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private bool isUnitMoved = false;

    private GameObject tutorialGo;
    [SerializeField] private GameObject goalArea;

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
        goalArea.SetActive(true);

        isUnitMoved = false;
        Vector3 spawnPos = spawnPoint.transform.position;
        tutorialGo = Instantiate(tutorialUnit, spawnPos, Quaternion.identity);

        // 조작 버튼 열어주기
        TutorialManager.Instance.tutorialScreen[(int)TutorialScreen.Control].SetActive(false);

        // 튜토리얼 미션 텍스트 제작해서 삽입
        completeCount = baseComplete;
        TM = TutorialManager.Instance;
        UpdateMissionComplete();
        CreateMissionText();
    }

    public override void Execute(TutorialController controller)
    {
        if (isUnitMoved)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        isUnitMoved = false;
        Destroy(tutorialGo);

        goalArea.SetActive(false);

        StartCoroutine(TM.DisableMissionBox());
    }

    public void OnUnitMove()
    {
        isUnitMoved = true;

        completeCount++;
        UpdateMissionComplete();
        CreateMissionText();
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