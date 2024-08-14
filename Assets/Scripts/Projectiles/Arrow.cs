using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Arrow : GuidedProjectile
{
    #region MonoBehaviour Callbacks
    private void Update()
    {
        if (isGoodToGo)
        {
            // 목적지 도달했는지 확인
            if (!FollowTarget())
            {
                DealDamage();
                DestroyThis();
            }
        }
    }
    #endregion
}
