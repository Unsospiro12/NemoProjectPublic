using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUnit : MonoBehaviour
{
    void Start()
    {
        TutorialManager.Instance.tutorialUnits.Add(gameObject);
    }
}
