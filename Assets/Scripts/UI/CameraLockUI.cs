using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLockUI : MonoBehaviour
{
    [SerializeField] Image cameraLockImg;
    [SerializeField] Sprite[] cameraIcons;
    [SerializeField] CameraControl cameraCtrl;

    private void FixedUpdate()
    {
        if (cameraCtrl.isLock)
        {
            cameraLockImg.sprite = cameraIcons[0];
        }
        else
        {
            cameraLockImg.sprite = cameraIcons[1];
        }
    }
}
