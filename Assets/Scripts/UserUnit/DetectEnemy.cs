using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DetectEnemy : MonoBehaviour
{
    #region Private Field

    #endregion
    #region Serilize Field
    [SerializeField] private UserUnitAction action;
    #endregion
    #region Public Properties

    #endregion
    #region MonoBehaviour Callbacks

    #endregion
    #region Public Methods

    #endregion
    #region Private Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy;
        if ((enemy = collision.GetComponent<Enemy>()) != null)
        {
            action.OnEnemyInRange(enemy);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy enemy;
        if ((enemy = collision.GetComponent<Enemy>()) != null)
        {
            action.OnEnemyInRange(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy;
        if ((enemy = collision.GetComponent<Enemy>()) != null)
        {
            action.OnEnemyExitRange(enemy);
        }
            
    }
    
    #endregion
}
