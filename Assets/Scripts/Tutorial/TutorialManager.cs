using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : InGameSingleton<TutorialManager>
{
    [SerializeField] private TutorialPopUpUI tutorialPopUpUI;
    [SerializeField] private TutorialController tutorialController;
    public List<GameObject> tutorialScreen;
    public List<GameObject> tutorialUnits;

    public GameObject MissionBox;
    public TextMeshProUGUI MissionTxt;
    public TextMeshProUGUI CompleteTxt;
    public TextMeshProUGUI GoalTxt;

    private int[] OriginalUnitList;

    // 미션 박스 초기화
    public float disableTime = 0.5f;

    [Header("Tutorial Info")]
    public bool usingPopUpUI = true;
    public List<TutorialBase> tutorialInfo;

    [Header("Camera Tutorials")]
    public List<TutorialBase> cameraTutorials;

    [Header("Currency Tutorials")]
    public List<TutorialBase> currencyTutorials;

    [Header("UI Tutorials")]
    public List<TutorialBase> uiTutorials;

    [Header("Production Tutorials")]
    public List<TutorialBase> productionTutorials;

    [Header("Upgrade Tutorials")]
    public List<TutorialBase> upgradeTutorials;

    [Header("Control Tutorials")]
    public List<TutorialBase> controlTutorials;
    public UserUnitController unitController;

    [Header("Stage Tutorials")]
    public List<TutorialBase> stageTutorials;

    [Header("생산 가능 유닛 ID 목록")]
    public List<int> UsableUnitIDs;

    protected override void Awake()
    {
        base.Awake();

        // 생산 가능 유닛 일시적으로 제한
        OriginalUnitList = DataManager.Instance.UserUnitIDs.ToArray();
        DataManager.Instance.UserUnitIDs = UsableUnitIDs;
    }
    private void Start()
    {
        // 자원 초기화
        CurrencyEventHandler.Instance.CurGold = 0;
        CurrencyEventHandler.Instance.CurDiamond = 0;

        // 튜토리얼 초기화
        //unitController.isTutorial = true;

        // 첫 튜토리얼이면 게임 안내 튜토리얼 재생
        if (usingPopUpUI)
        {
            tutorialController.StartTutorials(tutorialInfo);
        }
    }

    

    public void ExitTutorial()
    {
        // 생산 가능 유닛 복구
        DataManager.Instance.UserUnitIDs = OriginalUnitList.ToList<int>(); 
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void EndCurrentTutorial()
    {
        if (!usingPopUpUI)
        {
            tutorialPopUpUI.ToggleTutorialPopUpUI();
        }
        else
        {
            usingPopUpUI = false;
        }
    }

    public void StartTutorialInfo()
    {
        usingPopUpUI = true;
        tutorialController.StartTutorials(tutorialInfo);
    }

    public void StartCameraTutorials()
    {
        tutorialPopUpUI.ToggleTutorialPopUpUI();
        tutorialController.StartTutorials(cameraTutorials);
    }

    public void StartCurrencyTutorials()
    {
        tutorialPopUpUI.ToggleTutorialPopUpUI();
        tutorialController.StartTutorials(currencyTutorials);
    }

    public void StartUITutorials()
    {
        tutorialPopUpUI.ToggleTutorialPopUpUI();
        tutorialController.StartTutorials(uiTutorials);
    }

    public void StartProductionTutorials()
    {
        tutorialPopUpUI.ToggleTutorialPopUpUI();
        tutorialController.StartTutorials(productionTutorials);
    }

    public void StartUpgradeTutorials()
    {
        tutorialPopUpUI.ToggleTutorialPopUpUI();
        tutorialController.StartTutorials(upgradeTutorials);
    }

    public void StartControlTutorials()
    {
        tutorialPopUpUI.ToggleTutorialPopUpUI();
        tutorialController.StartTutorials(controlTutorials);
    }

    public void StartStageTutorials()
    {
        usingPopUpUI = true;
        tutorialController.StartTutorials(stageTutorials);
    }

    public IEnumerator DisableMissionBox()
    {
        yield return new WaitForSeconds(disableTime);
        MissionBox.SetActive(false);
    }
}
