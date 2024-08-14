using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawnBtn : MonoBehaviour
{
    private TutorialProduction tutorialProduction;

    void Start()
    {
        tutorialProduction = FindObjectOfType<TutorialProduction>();
    }

    public void OnClickProduction()
    {
        tutorialProduction.ClickProduction();
    }
}
