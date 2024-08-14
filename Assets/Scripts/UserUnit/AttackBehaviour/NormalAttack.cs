using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 평범한 일반 근접 공격
/// 아무런 특성이 없음
/// </summary>
public class NormalAttack : AttackBehaviour
{
    public override void Attack()
    {
        base.Attack();
        if(skill != null)
        {
            if ((Time.time - lastSkillUseTime) > skillCoolDown)
            {
                ActivateSkill();
                return;
            }
        }

        AttackEffect();
        DealDamage();
    }
}
