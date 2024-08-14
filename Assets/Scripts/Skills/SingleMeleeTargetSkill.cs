using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMeleeTargetSkill : ActiveSkill
{
    private Vector3 effectOriginPosition;
    private UnitType unitAttackType;
    [SerializeField] private bool effectPositionToTarget;
    [SerializeField] private float damageMultiplier;

    public override void Setting()
    {
        base.Setting();
        effectOriginPosition = SkillEffect.transform.position;
        unitAttackType = userUnit.Stat.UnitAttackType;
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        userUnit.Action.TargetEnemy.TakeDamage((int)(userUnit.Stat.Atk.TotalValule * damageMultiplier), unitAttackType);
    }

    protected override void TurnOnEffect()
    {
        if (effectPositionToTarget)
        {
            SkillEffect.transform.position = effectOriginPosition + userUnit.Action.TargetEnemy.transform.position;
        }
        base.TurnOnEffect();
    }
}
