using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoveSpeedSetting : MonoBehaviour
{
    [SerializeField] CameraControl cameraCtrl;
    [SerializeField] private Slider cameraMoveSpeedSlider;

    private void Start()
    {
        cameraMoveSpeedSlider.value = (DataManager.Instance.CameraMovementSpeed - 5f) / 30f;
    }
    public void SetCameraMoveSpeed(float value)
    {
        DataManager.Instance.CameraMovementSpeed = (value * 30f) + 5f;
    }
}
