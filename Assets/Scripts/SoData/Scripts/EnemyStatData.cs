using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "Stats/EnemyStat")]
public class EnemyStatData : BaseStatSO
{
    [Header("Enemy 기본 스텟")]
    public int Hp;
    public float ADDef;
    public float APDef;
    public UnitDefType DefType;
}
