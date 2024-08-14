using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStat", menuName = "Stats/UnitStat")]
public class UnitStatData : BaseStatSO
{
    [Header("유닛 기본 스텟")]
    public int UnitCost;
    public float Atk;
    public int Mp;
    public float AttackSpeed;
    public float AtkRange;
    public float AtkArea;
    public bool EffectOnEnemy1;
    public bool DoubleAttack;

    [Header("업그레이드 수치")]
    public float AttackIncrement;
}
