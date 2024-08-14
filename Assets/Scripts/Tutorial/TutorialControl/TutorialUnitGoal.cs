using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUnitGoal : MonoBehaviour
{
    [SerializeField] private TutorialUnitMove tutorialUnitMove;
    [SerializeField] private TutorialGroupMove tutorialGroupMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorialUnitMove.OnUnitMove();
            tutorialGroupMove.OnGroupMove();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorialGroupMove.OnGroupOut();
        }
    }
}
