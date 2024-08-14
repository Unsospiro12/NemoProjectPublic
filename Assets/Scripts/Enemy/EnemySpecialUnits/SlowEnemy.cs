using UnityEngine;

public class SlowEnemy : Enemy
{
    private float debuffDuration = 2f;
    private float debuffRadius = 10000f; // 디버프 범위
    private LayerMask layerMask;

    public override void Die()
    {
        base.Die();
        int userUnitLayer = LayerMask.NameToLayer("UserUnit");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        layerMask = (1 << userUnitLayer) | (1 << enemyLayer);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, debuffRadius, layerMask);
        EnemyDebuff enemyDebuff = FindObjectOfType<EnemyDebuff>();

        foreach (var hitCollider in hitColliders)
        {
            UserUnitStat userUnitStat = hitCollider.GetComponent<UserUnitStat>();
            if (userUnitStat != null && enemyDebuff != null)
            {
                enemyDebuff.ApplyDebuff(userUnitStat, debuffDuration);
            }
        }
    }
}