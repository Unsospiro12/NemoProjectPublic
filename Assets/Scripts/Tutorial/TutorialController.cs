using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField] 
    private List<TutorialBase> tutorials;
    //[SerializeField]
    //private string nextSceneName = "";

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;

    // 카메라
    public CameraControl cameraControl;

    // 게임 시간 초기화
    public GameTimeUI gameTimeUI;

    // 미션 박스 초기화
    public GameObject missionBox;



    private void Start()
    {
        //SetNextTutorial();
    }

    private void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        // 현재 튜토리얼의 Exit() 메서드 호출
        if (currentTutorial != null)
        {
            currentTutorial.Exit();

            // 스크린 초기화
            foreach (var screen in TutorialManager.Instance.tutorialScreen)
            {
                screen.SetActive(true);
            }

            // 카메라 초기화
            cameraControl.SetInitialCameraPos();

            // 유닛 초기화
            if (TutorialManager.Instance.tutorialUnits.Count > 0)
            {
                foreach (var unit in TutorialManager.Instance.tutorialUnits)
                {
                    Destroy(unit);
                }
                TutorialManager.Instance.tutorialUnits.Clear();
            }
        }

        // 마지막 튜토리얼을 진행했다면 CompletedAllTutorials() 메서드 호출
        if (currentIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }

        // 다음 튜토리얼 과정을 currentTutorial에 등록
        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        // 새로 바뀐 튜토리얼의 Enter() 메서드 호출
        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        // 현재 튜토리얼 초기화
        // 현재 튜토리얼 리스트 초기화
        currentTutorial = null;
        tutorials = null;
        currentIndex = -1;

        TutorialManager.Instance.EndCurrentTutorial();

        // 미션 박스 초기화
        missionBox.SetActive(false);
    }

    // 새 튜토리얼 부여하고 튜토리얼 시작
    public void StartTutorials(List<TutorialBase> newTutorials)
    {
        // 시간 초기화
        GameManager.Instance.ResetTime();

        tutorials = newTutorials;
        SetNextTutorial();
    }
}
