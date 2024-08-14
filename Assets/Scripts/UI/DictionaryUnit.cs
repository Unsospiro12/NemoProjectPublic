using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryUnit : MonoBehaviour
{
    public Image Icon; // 유닛 이미지 아이콘
    public TextMeshProUGUI UnitName; // 유닛 이름
    public TextMeshProUGUI UnitDescription; // 유닛 설명
    public int UnitID; // 유닛 ID
    public AudioClip audioClip;

    [SerializeField] private GameObject locked; // 유닛 잠금, 해제 UI
    [SerializeField] private Button button; // 유닛 설명 버튼
    private bool isLocked; // 유닛 잠금 유무

    public void Lock()
    {
        isLocked = true;
        locked.SetActive(true);
    }

    public void UnLock()
    {
        isLocked = false;
        locked.SetActive(false);

        // 저장
        DataManager.Instance.OnSaveData();
    }

    /// <summary>
    /// 유닛 클릭
    /// 잠금 상태가 아닐 경우 유닛 상세 데이터 표시
    /// 잠금 상태일 경우 열쇠 갯수를 1개 써서 잠금 해제
    /// </summary>
    public void OnClickThis()
    {
        if (isLocked)
        {
            UnitDictionaryManager.Instance.UnLockUnit(UnitID);
        }
        else
        {
            UnitDictionaryManager.Instance.ShowUnitDetail(UnitID);
        }
    }
}
