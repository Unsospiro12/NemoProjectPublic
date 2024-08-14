using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBuffSkill : ActiveSkill
{
    [SerializeField] private float skillDuration;
    [SerializeField] private StatType buffType;
    [SerializeField] private float buffAmount;
    private float originalStat;
    private AttributeStat buffStat;

    public override void Setting()
    {
        base.Setting();
        turnOffTime = new WaitForSeconds(skillDuration);
        switch (buffType)
        {
            case StatType.AttackSpeed:
                buffStat = userUnit.Stat.AttackSpeed;
                break;
        }
        
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        originalStat = buffStat.BaseValue;
        buffStat.BaseValue = originalStat / buffAmount;
        userUnit.Anim.speed *= buffAmount;
    }

    protected override void EndSkillEffect()
    {
        base.EndSkillEffect();
        buffStat.BaseValue = originalStat;
        userUnit.Anim.speed = 1;
    }
}
