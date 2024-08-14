using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : AttackBehaviour
{
    [SerializeField] private ProjectileNames projectileName;
    public override void Attack()
    {
        base.Attack();
        if (skill != null)
        {
            if ((Time.time - lastSkillUseTime) > skillCoolDown)
            {
                ActivateSkill();
                return;
            }
        }

        AttackEffect();
        if (targetEnemy != null)
        {
            GameObject projectileInstance = UserData.Instance.ProjectilePool.SpawnFromPool(projectileName);
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.SetTarget(targetEnemy, this);
        }
    }
}
