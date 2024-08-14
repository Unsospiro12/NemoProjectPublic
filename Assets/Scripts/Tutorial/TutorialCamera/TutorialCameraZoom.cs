using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCameraZoom : TutorialBase
{
    public CinemachineVirtualCamera virtualCamera;
    public bool zoomInCheck = false;
    public bool zoomOutCheck = false;
    public float minSize = 4.0f;
    public float maxSize = 6.6f;

    [Header("Mission")]
    [SerializeField] private Dialog[] mission;
    [SerializeField] private Dialog[] missionComplete;
    [SerializeField] private Dialog[] missionGoal;
    [SerializeField] TutorialManager TM;
    [SerializeField] private string missionTxt;
    [SerializeField] private string completeTxt;
    [SerializeField] private string goalTxt;
    [SerializeField] private int baseComplete = 0;
    [SerializeField] private int completeCount = 1;

    public override void Enter()
    {
        zoomInCheck = false;
        zoomOutCheck = false;



        // 튜토리얼 미션 텍스트 제작해서 삽입
        ResetMissionComplete();
        TM = TutorialManager.Instance;
        CreateMissionText();
    }

    public override void Execute(TutorialController controller)
    {
        if (virtualCamera.m_Lens.OrthographicSize <= minSize)
        {
            zoomInCheck = true;

            missionComplete[0].dialog = completeCount.ToString();
            CreateMissionText();
        }

        if ((virtualCamera.m_Lens.OrthographicSize >= maxSize) && zoomInCheck)
        {
            zoomOutCheck = true;

            missionComplete[1].dialog = completeCount.ToString();
            CreateMissionText();
        }

        if (zoomInCheck && zoomOutCheck)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        //Invoke("DisableMissionBox", TM.disableTime);
        StartCoroutine(TM.DisableMissionBox());
    }

    private void ResetMissionComplete()
    {
        for (int i = 0; i < missionComplete.Length; i++)
        {
            missionComplete[i].dialog = baseComplete.ToString();
        }
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
