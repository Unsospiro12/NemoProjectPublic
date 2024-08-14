using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitTextUI : MonoBehaviour
{
    public TextMeshProUGUI TextUI;
    public Animator Animator;
    private int hash;
    private ObjectPool<TextVariety> objectPool;

    private void Awake()
    {
        hash = Animator.StringToHash("ActiveText");
    }

    private void Start()
    {
        objectPool = UserData.Instance.TextObjectPool;
    }

    public void SetText(Notification noti)
    {
        switch (noti)
        {
            case Notification.Gold:
                TextUI.text = "골드가 부족합니다!";
                break;
            case Notification.Diamond:
                TextUI.text = "다이아몬드가 부족합니다!";
                break;
            case Notification.Area:
                TextUI.text = $"공간이 부족합니다!";
                break;
        }

        // 애니메이션 적용
        ActiveCurTextAnimation();
    }

    public void ActiveCurTextAnimation()
    {
        Animator.SetTrigger(hash);
    }

    public void DisableObject()
    {
        objectPool.ReturnObject(TextVariety.UnitText, this.gameObject);
    }
}
