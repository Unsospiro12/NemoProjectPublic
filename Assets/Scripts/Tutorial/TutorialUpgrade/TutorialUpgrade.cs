using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TutorialUpgrade : TutorialBase, IMissionBox
{
    public int upgradeCount = 0;
    public int upgradeCountGoal = 4;

    [SerializeField] private UpgradeUI upgradeUI;

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
        CurrencyEventHandler.Instance.CurGold = 0;
        CurrencyEventHandler.Instance.CurDiamond = 50;
        upgradeCount = 0;

        UnitUpgradeManager.Instance.ResetUpgrade();

        // 겜블 버튼 열어주기
        TutorialManager.Instance.tutorialScreen[(int)TutorialScreen.Upgrade].SetActive(false);

        // 튜토리얼 미션 텍스트 제작해서 삽입
        TM = TutorialManager.Instance;
        UpdateMissionComplete();
        CreateMissionText();
    }

    public override void Execute(TutorialController tutorialController)
    {
        if (upgradeCount >= upgradeCountGoal)
            tutorialController.SetNextTutorial();
    }

    public override void Exit()
    {
        CurrencyEventHandler.Instance.CurGold = 0;
        CurrencyEventHandler.Instance.CurDiamond = 0;
        upgradeCount = 0;

        upgradeUI.HideDirect();

        UnitUpgradeManager.Instance.ResetUpgrade();

        StartCoroutine(TM.DisableMissionBox());
    }

    public void OnClickUpgrade()
    {
       upgradeCount++;

        UpdateMissionComplete();
        CreateMissionText();
    }

    private void UpdateMissionComplete()
    {
        missionComplete[0].dialog = upgradeCount.ToString();
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
