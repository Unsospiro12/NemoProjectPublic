using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitIcon : MonoBehaviour
{
    [SerializeField] private List<GameObject> iconList;
    [SerializeField] private List<Toggle> toggles;
    private List<CanvasGroup> canvasGroupes;
    private int toggleIdx { get => toggles.FindIndex(obj => obj.isOn); }

    private void Awake()
    {
        toggles[0].isOn = true;
        Init();
        gameObject.SetActive(false);
    }
    //canvasGroupes에 CanvasGroup컴포넌트를 넣어주는 함수
    void Init()
    {
        canvasGroupes = new List<CanvasGroup>();
        for (int i = 0; i < toggles.Count; i++)
        {
            CanvasGroup canvasGroup = toggles[i].GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = toggles[i].gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroupes.Add(canvasGroup);
        }
    }
    //토글의 버튼에 따라 활성화 되는 페이지를 켜는 함수
    public void OnToggleChange()
    {
        if (toggleIdx == 0)
        {
            iconList[0].SetActive(true);
            iconList[1].SetActive(false);
            iconList[2].SetActive(false);
            iconList[3].SetActive(false);
        }
        else if (toggleIdx == 1)
        {
            iconList[1].SetActive(true);
            iconList[0].SetActive(false);
            iconList[2].SetActive(false);
            iconList[3].SetActive(false);
        }
        else if (toggleIdx == 2)
        {
            iconList[2].SetActive(true);
            iconList[0].SetActive(false);
            iconList[1].SetActive(false);
            iconList[3].SetActive(false);
        }
        else if (toggleIdx == 3)
        {
            iconList[3].SetActive(true);
            iconList[0].SetActive(false);
            iconList[1].SetActive(false);
            iconList[2].SetActive(false);
        }
    }
    //토글 켜기
    public void OnToggles(int index)
    {
        SetToggleVisible(index, true);
    }
    //토글 끄기
    public void OffToggles()
    {
        for(int i = 0; i < toggles.Count; i++)
        {
            SetToggleVisible(i,false);
        }
    }

    // 오브젝트가 비활성화 상태라면 알파값을 0으로 만들고 상호작용을 불가하게 만들고 레이캐스트에도 검출되지 않도록 변경
    void SetToggleVisible(int count, bool isVisible)
    {
        canvasGroupes[count].alpha = isVisible ? 1 : 0;
        canvasGroupes[count].interactable = isVisible;
        canvasGroupes[count].blocksRaycasts = isVisible;
    }
    //아이콘 설정
    public void SetIcon(int idx, Sprite unitIcon)
    {
        //구조 추후 변경 필요
        iconList[idx / 14].GetComponent<Icons>().IconUI(idx,unitIcon);
    }
    //아이콘 설정 해제
    public void DeleteIcon(int idx)
    {
        iconList[idx / 14].GetComponent<Icons>().Delete(idx);
    }
}
