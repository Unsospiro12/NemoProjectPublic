using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TutorialGroupMove : TutorialBase, IMissionBox
{
    [SerializeField] private GameObject tutorialUnit;
    [SerializeField] private int tutorialUnitCount;
    [SerializeField] private GameObject[] spawnPoints;

    [SerializeField] private bool isGroupMoved = false;

    [SerializeField] private List<GameObject> tutorialGos;

    [SerializeField] private int goalCount;
    [SerializeField] private GameObject goalArea;

    [Header("Mission")]
    [SerializeField] private Dialog[] mission;
    [SerializeField] private Dialog[] missionComplete;
    [SerializeField] private Dialog[] missionGoal;
    [SerializeField] TutorialManager TM;
    [SerializeField] private string missionTxt;
    [SerializeField] private string completeTxt;
    [SerializeField] private string goalTxt;
    private float disableTime = 1f;
    private bool isGroupMission = false;

    public override void Enter()
    {
        isGroupMission = true;

        goalArea.SetActive(true);

        goalCount = 0;
        isGroupMoved = false;
        tutorialGos = new List<GameObject>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Vector3 spawnPos = spawnPoints[i].transform.position;
            tutorialGos.Add(Instantiate(tutorialUnit, spawnPos, Quaternion.identity));
        }

        // 조작 버튼 열어주기
        TutorialManager.Instance.tutorialScreen[(int)TutorialScreen.Control].SetActive(false);

        // 튜토리얼 미션 텍스트 제작해서 삽입
        TM = TutorialManager.Instance;
        TM.MissionBox.SetActive(true);
        UpdateMissionComplete();
        CreateMissionText();
    }

    public override void Execute(TutorialController controller)
    {
        if (isGroupMoved)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        StartCoroutine(TM.DisableMissionBox());
        StartCoroutine(ResetBaseInfoDelay());
    }

    private IEnumerator ResetBaseInfoDelay()
    {
        yield return new WaitForSeconds(disableTime);

        goalArea.SetActive(false);
        goalCount = 0;
        isGroupMoved = false;
        foreach (var tutorialGo in tutorialGos)
        {
            Destroy(tutorialGo);
        }
        tutorialGos.Clear();

        isGroupMission = false;
    }

    public void OnGroupMove()
    {
        if (isGroupMission)
        {
            goalCount++;

            UpdateMissionComplete();
            CreateMissionText();

            if (goalCount >= spawnPoints.Length)
            {
                isGroupMoved = true;
            }
        }

    }

    public void OnGroupOut()
    {
        if (isGroupMission)
        {
            goalCount--;

            UpdateMissionComplete();
            CreateMissionText();
        }
    }

    private void UpdateMissionComplete()
    {
        missionComplete[0].dialog = goalCount.ToString();
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
        //TM.MissionBox.SetActive(true);
    }

    private void DisableMissionBox()
    {
        TM.MissionBox.SetActive(false);
    }
}
