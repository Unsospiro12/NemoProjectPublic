using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{
    [SerializeField] private Slider masetVolumeSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        SetSliders();
    }

    public void SetSliders()
    {
        if (SoundManager.Instance.MasterVolumeSlider == null)
        {
            SoundManager.Instance.MasterVolumeSlider = masetVolumeSlider;
        }
        if (SoundManager.Instance.BgmVolumeSlider == null)
        {
            SoundManager.Instance.BgmVolumeSlider = bgmSlider;
        }
        if (SoundManager.Instance.SfxVolumeSlider == null)
        {
            SoundManager.Instance.SfxVolumeSlider = sfxVolumeSlider;
        }

        SoundManager.Instance.SetSlider();
    }

    private void OnDisable()
    {
        DataManager.Instance.OnSaveData();
    }
}
