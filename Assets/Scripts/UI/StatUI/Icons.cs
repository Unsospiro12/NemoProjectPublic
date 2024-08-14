using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icons : MonoBehaviour
{
    [SerializeField] private List<Image> iconList;
    
    public void IconUI(int idx,Sprite icon)
    {
        iconList[idx % 14].gameObject.SetActive(true);
        iconList[idx % 14].sprite = icon;
    }
    public void Delete(int idx)
    {
        iconList[idx % 14].gameObject.SetActive(false);
        iconList[idx % 14].sprite = null;
    }
}
