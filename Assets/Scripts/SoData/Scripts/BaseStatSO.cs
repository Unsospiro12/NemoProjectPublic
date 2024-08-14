using UnityEngine;

public class BaseStatSO : ScriptableObject
{
    [Header("유닛 이름 및 설명")]
    public string CharacterName;
    public string CharacterDescription;
    public int ID;

    [Header("기본 스텟")]
    public int Lv;
    public float MovementSpeed;
    public UnitType unitType;
}
