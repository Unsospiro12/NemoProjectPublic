using System.Collections;
using UnityEngine;

public class ActiveSkill : MonoBehaviour
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int SkillCoolTime;

    [SerializeField] protected GameObject SkillEffect;
    [SerializeField] protected UserUnit userUnit;
    [SerializeField] private AudioClip audioClip;

    protected WaitForSeconds turnOffTime = new WaitForSeconds(0.6f);

    public virtual void Setting()
    {

    }
    public virtual void ActivateSkill()
    {
        SoundManager.Instance.PlayUnitSFX(audioClip, 1);
        TurnOnEffect();
        StartCoroutine(TurnOffEffect());
    }
    protected virtual void TurnOnEffect()
    {
        if (SkillEffect != null)
        {
            SkillEffect.SetActive(true);
        }
    }

    protected virtual IEnumerator TurnOffEffect()
    {
        yield return turnOffTime;
        if (SkillEffect != null)
        {
            SkillEffect.SetActive(false);
        }
        EndSkillEffect();
    }

    protected virtual void EndSkillEffect()
    {

    }
}
