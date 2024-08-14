using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected ProjectileNames projectileName;
    public AudioClip audioClip;

    protected Enemy targetEnemy;
    protected AttackBehaviour attackBehaviour;

    protected bool isGoodToGo = false;
    /// <summary>
    /// 미사일 초기화 함수
    /// 적을 세팅하고 데미지 양을 세팅한다
    /// </summary>
    /// <param name="targetEnemy"></param>
    /// <param name="attackBehaviour"></param>
    public virtual void SetTarget(Enemy targetEnemy, AttackBehaviour attackBehaviour)
    {
        this.targetEnemy = targetEnemy;
        this.attackBehaviour = attackBehaviour;
        isGoodToGo = true;
    }
    /// <summary>
    /// 적 유닛에게 데미지를 가함
    /// </summary>
    protected virtual void DealDamage()
    {
        attackBehaviour.DealDamage();
    }
    /// <summary>
    /// 오브젝트 풀에 이 오브젝트를 돌려줌
    /// </summary>
    protected virtual void DestroyThis()
    {
        UserData.Instance.ProjectilePool.ReturnObject(projectileName, gameObject);
    }
}

