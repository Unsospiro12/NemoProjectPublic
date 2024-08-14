using System.Collections;
using System.Reflection.Emit;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    protected Animator animator;
    protected UserUnit userUnit;
    protected Enemy targetEnemy;
    protected UserUnitStat stat;

    protected Vector3 effectOriginPosition;
    protected WaitForSeconds waitForSec;

    protected int skillCoolDown;
    protected float lastSkillUseTime;

    [SerializeField] protected EffectOnUnit effectOnEnemy;
    [SerializeField] protected GameObject attackEffect;
    [SerializeField] protected ActiveSkill skill;
    [SerializeField] protected AudioClip audioClip;

    public void Setting(UserUnit userUnit)
    {
        this.animator = userUnit.Anim;
        this.userUnit = userUnit;
        this.stat = userUnit.Stat;

        if(attackEffect != null )
        {
            effectOriginPosition = attackEffect.transform.position;
        }
        waitForSec = new WaitForSeconds(0.6f);

        if(skill != null )
        {
            skill.Setting();
            skillCoolDown = skill.SkillCoolTime;
        }
    }

    public virtual void Attack()
    {
        SoundManager.Instance.PlayUnitSFX(audioClip, 1);
        targetEnemy = userUnit.Action.TargetEnemy;
        animator.SetTrigger(userUnit.AnimOnAttack);
        userUnit.FlipToRight(targetEnemy.transform.position.x > userUnit.transform.position.x);
    }
    public virtual void ActivateSkill()
    {
        lastSkillUseTime = Time.time;
        skill.ActivateSkill();
    }
    public virtual void AttackEffect()
    {
        if (attackEffect != null)
        {
            attackEffect.transform.position = effectOriginPosition + targetEnemy.transform.position;
            attackEffect.SetActive(true);

            StartCoroutine(TurnOffEffect());
        }
    }
    public virtual void DealDamage()
    {
        if(targetEnemy == null) return;
        if (stat.AttackArea.TotalValule <= 0) // 공격 범위가 없을 경우 단일 공격 
        {
            if (stat.IsEffectOnEnemy)
            {
                effectOnEnemy.EffectToEnemy(targetEnemy);
            }
            targetEnemy.TakeDamage((int)stat.Atk.TotalValule, stat.UnitAttackType);
        }
        else // 공격 범위가 있을 경우 범위 공격 실행
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(targetEnemy.transform.position, stat.AttackArea.TotalValule);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("Enemy"))
                {
                    Enemy enemy = colliders[i].GetComponent<Enemy>();
                    if (stat.IsEffectOnEnemy)
                    {
                        effectOnEnemy.EffectToEnemy(enemy);
                    }
                    enemy.TakeDamage((int)stat.Atk.TotalValule, stat.UnitAttackType);
                }
            }
        }
    }
    private IEnumerator TurnOffEffect()
    {
        yield return waitForSec;
        attackEffect.SetActive(false);
    }
}
