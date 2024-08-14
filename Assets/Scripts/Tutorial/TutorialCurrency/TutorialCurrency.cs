using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TutorialCurrency : TutorialBase, IMissionBox
{
    [SerializeField] private int gambleCount;

    [Header("Mission")]
    [SerializeField] private Dialog[] mission;
    [SerializeField] private Dialog[] missionComplete;
    [SerializeField] private Dialog[] missionGoal;
    [SerializeField] TutorialManager TM;
    [SerializeField] private string missionTxt;
    [SerializeField] private string completeTxt;
    [SerializeField] private string goalTxt;

    public override void Enter()
    {
        CurrencyEventHandler.Instance.CurGold = 300;
        CurrencyEventHandler.Instance.CurDiamond = 0;
        gambleCount = 0;

        // 겜블 버튼 열어주기
        TutorialManager.Instance.tutorialScreen[(int)TutorialScreen.Gamble].SetActive(false);

        // 튜토리얼 미션 텍스트 제작해서 삽입
        TM = TutorialManager.Instance;
        CreateMissionText();
    }

    public override void Execute(TutorialController controller)
    {
        if (gambleCount >= 3) 
            controller.SetNextTutorial();

    }

    public override void Exit()
    {
        CurrencyEventHandler.Instance.CurGold = 0;
        CurrencyEventHandler.Instance.CurDiamond = 0;
        gambleCount = 0;

        UpdateMissionComplete();
        StartCoroutine(TM.DisableMissionBox());
    }

    public void ClickGamble()
    {
        gambleCount++;

        UpdateMissionComplete();
        CreateMissionText();
    }

    private void UpdateMissionComplete()
    {
         missionComplete[0].dialog = gambleCount.ToString();
    }

    public void CreateMissionText()
    {
        // 미션 텍스트 생성
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

        UpdateMissionText();
    }

    public void UpdateMissionText()
    {
        // 미션 텍스트 업데이트
        TM.MissionTxt.text = missionTxt;
        TM.CompleteTxt.text = completeTxt;
        TM.GoalTxt.text = goalTxt;
        TM.MissionBox.SetActive(true);
    }

    public void DisableMissionBox()
    {
        // 미션 텍스트 비활성화
        TM.MissionBox.SetActive(false);
    }
}
