using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPauseUI : BaseUI
{
    public void OnClickPauseBtn()
    {
        UIManager.Instance.Show<TutorialPauseUI>();
    }
    public override void HideDirect()
    {
        UIManager.Instance.Hide<TutorialPauseUI>();
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
