using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ListFor2D<T>
{
    public List<T> items;
}

public class StageChoice : MonoBehaviour
{
    [SerializeField] private Button[] stageBtns;
    [SerializeField] private GameObject[] stagePanel;
    [SerializeField] private GameObject[] stageScore;
    [SerializeField] private List<ListFor2D<GameObject>> stageMedal;
    [SerializeField] private List<ListFor2D<GameObject>> stageMedalFront;

    private void Start()
    {
        SetStagePanel();
    }

    public void OnClickStageBtn(int stage)
    {
        DataManager.Instance.currentStage = stage;
        SceneManager.LoadScene(Constants.Scenes.GameScene);
    }

    void SetStageScore(int stage, int score)
    {
        int index = stage - 1;

        stageScore[index].SetActive(true);
        for (int i = 0; i < stageMedal[index].items.Count; i++)
        {
            stageMedal[index].items[i].SetActive(i < score);
            stageMedalFront[index].items[i].SetActive(i >= score);
        }
    }

    void SetStagePanel()
    {      
        bool isStage1Cleared = DataManager.Instance.StageDatas[0].Clear;
        bool isStage2Cleared = DataManager.Instance.StageDatas[1].Clear;
        bool isStage3Cleared = DataManager.Instance.StageDatas[2].Clear;

        //스테이지 1 상태 설정
        SetStageScore(1, DataManager.Instance.StageDatas[0].Stars);

        //스테이지 2 상태 설정
        if (isStage1Cleared)
        {
            stagePanel[1].SetActive(false);
            stageBtns[1].interactable = true;
            SetStageScore(2, DataManager.Instance.StageDatas[1].Stars);
        }
        else
        {
            stagePanel[1].SetActive(true);
            stageBtns[1].interactable = false;
        }

        // Stage 3 상태 설정
        if (isStage2Cleared)
        {
            stagePanel[2].SetActive(false);
            stageBtns[2].interactable = true;
            SetStageScore(3, DataManager.Instance.StageDatas[2].Stars);
        }
        else
        {
            stagePanel[2].SetActive(true);
            stageBtns[2].interactable = false;
        }
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(Constants.Scenes.MainScene);
    }

}
