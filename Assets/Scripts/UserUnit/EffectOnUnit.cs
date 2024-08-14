using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnUnit : MonoBehaviour
{
    #region Private Field
    #endregion

    #region MonoBehaviour Callbacks
    #endregion

    #region Public Methods
    public void EffectToEnemy(Enemy enemy)
    {
        StatusEffect statusEffect = enemy.GetComponent<StatusEffect>();
        if(statusEffect != null)
        {
            statusEffect.SetStatusEffect(StatusEffectType.Slow);
        }
    }
    #endregion

    #region Private Methods
    
    #endregion
}
