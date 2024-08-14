using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        // 저장
        DataManager.Instance.OnSaveData();
        SceneManager.LoadScene(Constants.Scenes.StageChoiceScene);
    }

    public void OnClickQuitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
    public void OnClickTutorialBtn()
    {
        SceneManager.LoadScene(Constants.Scenes.TutorialScene);
    }
    public void OnClickDictionaryBtn()
    {
        SceneManager.LoadScene(Constants.Scenes.UnitDictionary);
    }
}
