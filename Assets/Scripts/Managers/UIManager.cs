using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : InGameSingleton<UIManager>
{
    [SerializeField] private List<BaseUI> uiList;
    public bool isZoomLocked;

    //UI를 여는 함수를 제네릭으로 선언
    public void Show<T>(params object[] param) where T : BaseUI
    {
        var ui = uiList.Find(obj => obj.name == typeof(T).ToString());
        if(ui != null)
        {
            ui.SetActive(true);
            ui.opened.Invoke(param);
            isZoomLocked = true;
        }
    }

    //UI를 닫는 함수를 제네릭으로 선언
    public void Hide<T>(params object[] param) where T : BaseUI
    {
        
        var ui = uiList.Find(obj => obj.name == typeof(T).ToString());
        if (ui != null)
        {
            ui.SetActive(false);
            ui.closed.Invoke(param);
            isZoomLocked = false;
        }
    }

    
}
