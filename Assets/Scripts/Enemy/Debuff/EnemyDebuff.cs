using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebuff : MonoBehaviour
{
    private class DebuffInfo
    {
        public Coroutine coroutine;
        public GameObject debuffEffectInstance;
        public float originalAttackSpeed;
        public float originalMovementSpeed;
    }

    private Dictionary<UserUnitStat, DebuffInfo> debuffedUnits = new Dictionary<UserUnitStat, DebuffInfo>();

    [SerializeField] private GameObject speedDebuffEffect;

    public void ApplyDebuff(UserUnitStat userUnitStat, float duration)
    {
        if (userUnitStat == null) return;

        // 기존 디버프가 있는 경우, 디버프를 제거
        if (debuffedUnits.TryGetValue(userUnitStat, out var debuffInfo))
        {
            StopDebuff(userUnitStat);
        }

        // 기존 값 저장
        var newDebuffInfo = new DebuffInfo
        {
            originalAttackSpeed = userUnitStat.AttackSpeed.BaseValue,
            originalMovementSpeed = userUnitStat.MovementSpeed.BaseValue
        };

        // 디버프 적용
        userUnitStat.AttackSpeed.BaseValue += 0.5f;
        userUnitStat.MovementSpeed.BaseValue *= 0.5f;

        // 디버프 효과 생성
        if (speedDebuffEffect != null)
        {
            newDebuffInfo.debuffEffectInstance = Instantiate(speedDebuffEffect, userUnitStat.transform);
            newDebuffInfo.debuffEffectInstance.SetActive(true);
        }

        newDebuffInfo.coroutine = StartCoroutine(DebuffCoroutine(userUnitStat, duration));

        debuffedUnits[userUnitStat] = newDebuffInfo;
    }

    private IEnumerator DebuffCoroutine(UserUnitStat userUnitStat, float duration)
    {
        yield return new WaitForSeconds(duration);
        StopDebuff(userUnitStat);
    }

    private void StopDebuff(UserUnitStat userUnitStat)
    {
        if (userUnitStat != null)
        {
            if (debuffedUnits.TryGetValue(userUnitStat, out var debuffInfo))
            {
                // 디버프 제거
                userUnitStat.AttackSpeed.BaseValue = debuffInfo.originalAttackSpeed;
                userUnitStat.MovementSpeed.BaseValue = debuffInfo.originalMovementSpeed;

                if (debuffInfo.debuffEffectInstance != null)
                {
                    Destroy(debuffInfo.debuffEffectInstance);
                }

                if (debuffInfo.coroutine != null)
                {
                    StopCoroutine(debuffInfo.coroutine);
                }

                debuffedUnits.Remove(userUnitStat);
            }
        }
    }
}
