using UnityEngine;

public class GuidedProjectile : Projectile
{
    #region Private Field
    protected Vector3 startPosition;
    protected Vector3 targetPosition;
    protected float flightDuration;
    protected float elapsedTime;
    protected float progress;

    protected Vector3 direction;
    #endregion
    #region Public Methods
    public override void SetTarget(Enemy targetEnemy, AttackBehaviour attackBehaviour)
    {
        base.SetTarget(targetEnemy, attackBehaviour);

        startPosition = transform.position;
        targetPosition = targetEnemy.transform.position;

        float distance = Vector3.Distance(startPosition, targetPosition);
        flightDuration = distance / speed;
        elapsedTime = 0f;

        // 초기 방향 계산
        direction = (targetPosition - startPosition).normalized;
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// 적의 위치를 추격하는 함수
    /// 추격중에는 true, 목적지에 도달하면 false를 돌려준다
    /// </summary>
    /// <returns>목적지 도달 여부</returns>
    protected virtual bool FollowTarget()
    {
        if (targetEnemy != null)
        {
            targetPosition = targetEnemy.transform.position;
        }

        elapsedTime += Time.deltaTime;
        progress = elapsedTime / flightDuration;

        // 현재 위치 계산
        Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, progress);
        transform.position = currentPosition;

        // 방향 계산: 속도 벡터를 기반으로 화살의 방향 설정
        transform.right = Quaternion.Euler(0, 0, 90) * direction;

        // 목적지 도달했는지 확인
        if (progress >= 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion
}
