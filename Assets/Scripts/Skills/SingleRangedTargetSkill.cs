using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRangedTargetSkill : ActiveSkill
{
    private UnitType unitAttackType;
    [SerializeField] private ProjectileNames projectileName;

    public override void Setting()
    {
        base.Setting();
        unitAttackType = userUnit.Stat.UnitAttackType;
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        Enemy enemy = userUnit.Action.TargetEnemy;
        if (enemy != null)
        {
            GameObject projectileInstance = UserData.Instance.ProjectilePool.SpawnFromPool(projectileName);
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.SetTarget(enemy, userUnit.Action.AttackBehaviour);
        }
    }
}
