using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUpUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPopUp;
    
    public void ToggleTutorialPopUpUI()
    {
        tutorialPopUp.SetActive(!tutorialPopUp.activeSelf);
    }
}
