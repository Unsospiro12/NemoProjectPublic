using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class CustomTest : EditorWindow
{
    // 테스트 툴 제작 (단축키 Shift + 1)
    //# -> Shift
    //% -> Ctrl
    //& -> Alt
    [MenuItem("CustomTool/Game Test #1")]
    static void CustomMenu()
    {
        GetWindow<CustomTest>();
    }

    //CustomTool제작
    private void OnGUI()
    {
        UITestTool();
        ChangeScene();
    }

    // 씬 이동
    // 사용전 Build Setting에 씬등록
    //사용시 씬 이름과 씬 번호 최신화 필요
    void ChangeScene()
    {
        EditorGUILayout.LabelField("씬 이동 버튼");
        if (GUILayout.Button("Enemy Test Scene"))
        {
            // 현재 게임을 실행중인지 판단 
            if (Application.isPlaying)
            {
                //현재 실행중이라면 SceneManager이용
                SceneManager.LoadScene(0);
            }
            else
            {
                // 현재 실행중이 아니라면 EditorSceneManager이용
                // EditorSceneManager는 씬경로를 받아와서 씬이동
                EditorSceneManager.OpenScene("Assets/Scenes/EnemyTest.unity");
            }
        }
        if (GUILayout.Button("Lih Scene"))
        {
            // 현재 게임을 실행중인지 판단 
            if (Application.isPlaying)
            {
                //현재 실행중이라면 SceneManager이용
                SceneManager.LoadScene(4);
            }
            else
            {
                // 현재 실행중이 아니라면 EditorSceneManager이용
                // EditorSceneManager는 씬경로를 받아와서 씬이동
                EditorSceneManager.OpenScene("Assets/Scenes/LihTestScene(Lss).unity");
            }
        }
        if (GUILayout.Button("LeeSangSooTest Scene"))
        {
            // 현재 게임을 실행중인지 판단 
            if (Application.isPlaying)
            {
                //현재 실행중이라면 SceneManager이용
                SceneManager.LoadScene(2);
            }
            else
            {
                // 현재 실행중이 아니라면 EditorSceneManager이용
                // EditorSceneManager는 씬경로를 받아와서 씬이동
                EditorSceneManager.OpenScene("Assets/Scenes/LeeSangSooTestScene.unity");
            }
        }
        if (GUILayout.Button("Main Scene"))
        {
            // 현재 게임을 실행중인지 판단 
            if (Application.isPlaying)
            {
                //현재 실행중이라면 SceneManager이용
                SceneManager.LoadScene(0);
            }
            else
            {
                // 현재 실행중이 아니라면 EditorSceneManager이용
                // EditorSceneManager는 씬경로를 받아와서 씬이동
                EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
            }
        }
        if (GUILayout.Button("Game Scene"))
        {
            // 현재 게임을 실행중인지 판단 
            if (Application.isPlaying)
            {
                //현재 실행중이라면 SceneManager이용
                SceneManager.LoadScene(2);
            }
            else
            {
                // 현재 실행중이 아니라면 EditorSceneManager이용
                // EditorSceneManager는 씬경로를 받아와서 씬이동
                EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
            }
        }
        if (GUILayout.Button("Test Tutorial Scene"))
        {
            // 현재 게임을 실행중인지 판단 
            if (Application.isPlaying)
            {
                //현재 실행중이라면 SceneManager이용
                SceneManager.LoadScene(1);
            }
            else
            {
                // 현재 실행중이 아니라면 EditorSceneManager이용
                // EditorSceneManager는 씬경로를 받아와서 씬이동
                EditorSceneManager.OpenScene("Assets/Scenes/TutorialScene.unity");
            }
        }
    }

    //UITest툴 제작
    //게임이 실행중이 아니라면 오류 발생
    void UITestTool()
    {
        EditorGUILayout.LabelField("UI Test");

        if (GUILayout.Button("키 지급"))
        {
            DataManager.Instance.KeyCount++;
        }
        
        if (GUILayout.Button("골드 지급"))
        {
            CurrencyEventHandler.Instance.CurGold += 500;
        }

        if (GUILayout.Button("다이아몬드 지급"))
        {
            CurrencyEventHandler.Instance.CurGold += 500;
        }
        
        if (GUILayout.Button("데이터 지우기"))
        {
            DataManager.Instance.OnDeleteData();
        }
        /*
        if (GUILayout.Button("CurAMinus"))
        {
            UITest.Instance.OnClickCurAminus();
        }

        if (GUILayout.Button("CurBMinus"))
        {
            UITest.Instance.OnClickCurBminus();
        }

        if (GUILayout.Button("유저 체력 회복"))
        {
            UserData.Instance.UserHealth += 10;
        }*/
    }

}
