using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMObject : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;

    void Start()
    {
        SoundManager.Instance.PlayBGM(bgmClip);    
    }
}
