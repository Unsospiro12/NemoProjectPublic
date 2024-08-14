using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUI
{
    public void OnClickPauseBtn()
    {
        UIManager.Instance.Show<PauseUI>();
    }
    public override void HideDirect()
    {
        UIManager.Instance.Hide<PauseUI>();
    }

    public override void OnOpened(object[] param)
    {
        Time.timeScale = 0;
    }

    public override void OnClosed(object[] param)
    {
        Time.timeScale = 1;
    }
}
