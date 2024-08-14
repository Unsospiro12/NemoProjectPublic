using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    #region Private Field
    private bool[] onEffect;
    private float originalSpeed;
    private Coroutine slowCoroutine;
    #endregion
    #region Serilize Field
    [SerializeField] private GameObject[] statusPrefabList;
    [SerializeField] private Enemy unit;
    #endregion
    #region Public Properties
    public void SetStatusEffect(StatusEffectType type)
    {
        if(type == StatusEffectType.Slow)
        {
            if (!onEffect[0])
            {
                originalSpeed = unit.enemyMovement.moveSpeed;
                unit.enemyStat.MovementSpeed.BaseValue *= 0.5f;
                unit.Anim.speed *= 0.5f;
                onEffect[0] = true;
                statusPrefabList[0].SetActive(true);
                slowCoroutine = StartCoroutine(BringBackSpeed());
            }
            else
            {
                StopCoroutine(slowCoroutine);
                slowCoroutine = StartCoroutine(BringBackSpeed());
            }
        }
    }
    #endregion
    #region MonoBehaviour Callbacks
    private void Awake()
    {
        onEffect = new bool[statusPrefabList.Length];
    }
    #endregion
    #region Public Methods

    #endregion
    #region Private Methods
    private IEnumerator BringBackSpeed()
    {
        yield return new WaitForSeconds(2f);

        if (unit != null)
        {
            unit.enemyStat.MovementSpeed.BaseValue = originalSpeed;
            unit.Anim.speed = 1;
            statusPrefabList[0].SetActive(false);
            onEffect[0] = false;
        }
    }
    #endregion
}
