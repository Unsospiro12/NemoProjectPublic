using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BtnSound : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    //private List<Button> _buttons;

#if UNITY_EDITOR
    [ContextMenu("SetComponent")]
    public void SetComponent()
    {
        if (_buttons != null)
        {
            _buttons.Clear();
        }

        Button[] buttons = GameObject.FindObjectsOfType<Button>(true);
        _buttons.AddRange(buttons);
    }
#endif
    [SerializeField] private AudioClip btnClip;

    private void Start()
    {
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(PlayBtnClickSound);
        }
    }

    public void PlayBtnClickSound()
    { 
       SoundManager.Instance.PlayUnitSFX(btnClip);
    }
}
