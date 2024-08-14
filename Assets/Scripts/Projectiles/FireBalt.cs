using UnityEngine;

public class FireBalt : GuidedProjectile
{
    #region Private Field
    private float explosionRadius;
    private string explosionAnimationName = "fire";
    private bool explosionEnabled = false;
    #endregion

    #region Serilize Field
    [SerializeField] private Animator explosionEffect;
    [SerializeField] private GameObject firebaltSprite;
    #endregion

    #region MonoBehaviour Callbacks
    private void Update()
    {
        if (isGoodToGo)
        {
            if (!FollowTarget())
            {
                DealDamage();
            }
        }
        else
        {
            if ((explosionEnabled && IsExplosionFinished()))
            {
                DestroyThis();
            }
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 적에게 데미지를 주고 폭발 이펙트 실행
    /// 기존에 화염볼트 이미지는 끄기
    /// </summary>
    protected override void DealDamage()
    {
        base.DealDamage();

        isGoodToGo = false;
        firebaltSprite.SetActive(false);

        explosionEffect.transform.rotation = Quaternion.identity;
        explosionEffect.gameObject.SetActive(true);
        explosionEnabled = true;
    }

    /// <summary>
    /// 폭발이 끝나면 폭발 애니메이션을 끄고 오브젝트 풀로 반환
    /// </summary>
    protected override void DestroyThis()
    {
        explosionEnabled = false;
        explosionEffect.gameObject.SetActive(false);
        firebaltSprite.SetActive(true);
        base.DestroyThis();
    }
    /// <summary>
    /// 폭발이 끝났는지 확인
    /// </summary>
    /// <returns></returns>
    private bool IsExplosionFinished()
    {
        AnimatorStateInfo animStateInfo = explosionEffect.GetCurrentAnimatorStateInfo(0);
        return animStateInfo.IsName(explosionAnimationName) && animStateInfo.normalizedTime >= 1.0f;
    }
    #endregion
}
