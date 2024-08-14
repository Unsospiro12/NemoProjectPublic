using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteBtn : MonoBehaviour
{
    //[SerializeField] private List<Sprite> soundImg;
    //[SerializeField] private Image muteBtnImg;
    //public bool IsMute = false;
    //private float Value;
    //[SerializeField] private SliderSetting sliderSetting;

    //private void Start()
    //{
    //    if(SoundManager.Instance.MuteBtn == null)
    //    {
    //        SoundManager.Instance.MuteBtn = this;
    //    }
    //}
    //public void OnClickMuteBtn()
    //{
    //    if(SoundManager.Instance.MasterVolumeSlider == null)
    //    {
    //        sliderSetting.SetSliders();
    //    }

    //    if(SoundManager.Instance.MuteBtn == null)
    //    {
    //        SoundManager.Instance.MuteBtn = this;
    //    }
    //    CheckMute();
    //}

    //void CheckMute()
    //{       
    //    if (!IsMute)
    //    {
    //        Value = SoundManager.Instance.MasterVolumeValue;
    //        SoundManager.Instance.SetMasterVolume(0);                     
    //    }
    //    else
    //    {
    //        SoundManager.Instance.SetMasterVolume(Value);            
    //    }
    //    CheckSprite();
    //    SoundManager.Instance.SetSlider();
    //}

    //public void CheckSprite()
    //{
    //    if (!IsMute)
    //    {
    //        muteBtnImg.sprite = soundImg[0];
    //    }
    //    else
    //    {
    //        muteBtnImg.sprite = soundImg[1];
    //    }
    // }
}
