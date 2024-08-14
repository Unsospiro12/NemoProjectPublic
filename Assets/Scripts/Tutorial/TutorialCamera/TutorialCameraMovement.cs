using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialCameraMovement : TutorialBase
{
    public CinemachineVirtualCamera virtualCamera;
    public CameraControl cameraControl;
    public float movementThreshold = 3f;
    public bool cameraLockCheck = false;
    public bool movementCheck = false;
    public bool resetCheck = false;

    public InputAction cameraReset;

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
        cameraLockCheck = false;
        movementCheck = false;
        resetCheck = false;

        // 카메라 위치 초기화
        Vector3 basePos = new Vector3(0, 0, virtualCamera.transform.position.z);
        virtualCamera.transform.position = basePos;
        cameraControl.isLock = true;

        cameraReset = cameraControl.cameraReset;
        cameraReset.performed += CheckCameraReset;

        // 튜토리얼 미션 텍스트 제작해서 삽입
        ResetMissionComplete();
        TM = TutorialManager.Instance;
        CreateMissionText();
    }

    public override void Execute(TutorialController controller)
    {
        // 카메라 이동이 풀리면
        if (!cameraControl.isLock)
        {
            cameraLockCheck = true;

            missionComplete[0].dialog = completeCount.ToString();
            CreateMissionText();
        }

        // 카메라가 일정 이상 이동하면
        if (CheckCameraMove())
        {
            movementCheck = true;

            missionComplete[1].dialog = completeCount.ToString();
            CreateMissionText();
        }

        // 모든 조건이 달성되면
        if (movementCheck && cameraLockCheck && resetCheck)
        {
            controller.SetNextTutorial();

            missionComplete[2].dialog = completeCount.ToString();
            CreateMissionText();
        }
    }

    // 카메라가 Threshold 이상 이동했는지 체크
    public bool CheckCameraMove()
    {
        if (virtualCamera.transform.position.x > movementThreshold ||
            virtualCamera.transform.position.x < -movementThreshold ||
            virtualCamera.transform.position.y > movementThreshold ||
            virtualCamera.transform.position.y < -movementThreshold)
        {
            return true;
        }
        return false;
    }

    // 카메라가 이동했을 때, 카메라 초기화 체크
    public void CheckCameraReset(InputAction.CallbackContext context)
    {
        if (context.performed && movementCheck)
        {
            resetCheck = true;
        }
    }

    public override void Exit()
    {
        // 카메라 위치 초기화
        Vector3 basePos = new Vector3(0, 0, virtualCamera.transform.position.z);
        virtualCamera.transform.position = basePos;
        cameraControl.isLock = true;

        cameraReset.performed -= CheckCameraReset;

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

        EnterMissionTxt();
    }

    private void EnterMissionTxt()
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
