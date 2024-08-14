using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject statToolTipUI;
    [SerializeField] GameObject bg;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 mousePosition = Input.mousePosition;
        statToolTipUI.transform.position = mousePosition;
        statToolTipUI.gameObject.SetActive(true);
        bg.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statToolTipUI.gameObject.SetActive(false);
    }

}
