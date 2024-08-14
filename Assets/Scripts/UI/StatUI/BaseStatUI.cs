using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStatUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text nameText;
    [SerializeField] private Image unitIcon;
    [SerializeField] private StatToolTipUI statToolTip;
    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void DeleteName()
    {
        nameText.text = "";
    }

    public void SetIcon(Sprite iconImg)
    {
        unitIcon.sprite = iconImg;
    }
    public void DeleteIcon()
    {
        unitIcon.sprite = null;
    }
    public void SetADStat(int id, float baseStat, float addStat)
    {
        statToolTip.SetADStatText(id, baseStat, addStat);
    }
    public void SetMDStat(int id, float baseStat, float addStat)
    {
        statToolTip.SetMDStatText(id, baseStat, addStat);
    }
    public void SetMoveSpeed(float stat)
    {
        statToolTip.SetMoveSpeedStatText(stat);
    }

    public virtual void DeleteStatData() 
    {
        statToolTip.DeleteStatToolTip();
    }

}
