using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectileBehaviour : FireBalt
{
    [SerializeField] private SkillEffect skillEffect;
    protected override void DealDamage()
    {
        base.DealDamage();
        GameObject skillEffectObject = UserData.Instance.SkillObjectPool.SpawnFromPool(skillEffect);
        skillEffectObject.transform.position = transform.position;
        skillEffectObject.SetActive(true);
    }
}
