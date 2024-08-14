using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseUI : MonoBehaviour
{
    public UnityAction<object[]> opened;
    public UnityAction<object[]> closed;
 
    private void Awake()
    {
        opened = OnOpened;
        closed = OnClosed;
    }

    //SetActive함수를 GameObject가 아닌 UI에서 쓸수있도록 함수 선언
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    //UI를 여는 가상함수 구현
    public virtual void OnOpened(object[] param) { }

    //UI를 닫는 가상함수 구현
    public virtual void OnClosed(object[] param) { }
    //UI를 비활성화 하는 가상함수 구현
    public virtual void HideDirect() { }

}
