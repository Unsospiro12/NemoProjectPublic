using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI: MonoBehaviour
{
    [SerializeField] private List<GameObject> scoreIcons;
    [SerializeField] private GameObject GotKeyEffect;
    [SerializeField] private TextMeshProUGUI clearedWaveText;

    private WaitForSeconds wait = new WaitForSeconds(1f);

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SetGameOverUI(int round, int score, int maxHealth, int curHealth)
    {
        int stageNum = DataManager.Instance.currentStage;

        clearedWaveText.SetText($"{GameManager.Instance.waveManager.WaveLevel + 1}");


        if (curHealth > 0)
        {
            if (!DataManager.Instance.StageDatas[stageNum-1].Clear)
            {
                // 첫 클리어 시에만 열쇠를 주고 클리어로 바꿔줌
                DataManager.Instance.KeyCount++;
                DataManager.Instance.StageDatas[stageNum-1].Clear = true;
                GotKeyEffect.SetActive(true);
            }

            if (maxHealth == curHealth)
            {
                for (int i = 0; i < scoreIcons.Count; i++)
                {
                    scoreIcons[i].SetActive(true);
                }
                if (DataManager.Instance.StageDatas[stageNum - 1].Stars <= 2)
                {
                    DataManager.Instance.StageDatas[stageNum - 1].Stars = scoreIcons.Count;
                }
            }
            else if (maxHealth * 0.5f <= curHealth)
            {
                for (int i = 0; i < scoreIcons.Count - 1; i++)
                {
                    scoreIcons[i].SetActive(true);
                }
                if (DataManager.Instance.StageDatas[stageNum - 1].Stars <= 1)
                {
                    DataManager.Instance.StageDatas[stageNum - 1].Stars = scoreIcons.Count - 1;
                }
            }
            else if (curHealth >= 1)
            {
                for (int i = 0; i < scoreIcons.Count - 2; i++)
                {
                    scoreIcons[i].SetActive(true);
                }
                if (DataManager.Instance.StageDatas[stageNum - 1].Stars <= 0)
                {
                    DataManager.Instance.StageDatas[stageNum - 1].Stars = scoreIcons.Count - 2;
                }
            }
        }

        // 저장
        DataManager.Instance.OnSaveData();
    }
}