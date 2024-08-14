using TMPro;
using UnityEngine;
using UnityEngine.UI;

//최대치가 있는 스탯
//최대값, 현재값이
//
[System.Serializable]
public class ResourceStat
{
    protected int maxValue;
    protected int currentValue;
    public Image Image;
    public TextMeshProUGUI Text;


    //최대값은 최소 1
    //최대값이 수정되면 비율에 맞춰 현재 값도 수정된다
    public int MaxValue
    {
        get { return maxValue; }
        set
        {
            if (maxValue <= 0)
            {
                currentValue = value;
            }
            else
            {
                currentValue = (currentValue * value) / maxValue;
            }
            maxValue = value <= 0 ? 1 : value;
        }
    }

    //현재값은 최소 0
    //현재값이 최대값을 넘으면 최대값으로 수정된다
    public int CurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = value < 0 ? 0 : value;
            if (currentValue > maxValue)
            {
                currentValue = maxValue;
            }
        }
    }
}

//기본값이 있는 스탯
//기본값, 추가값이 존재한다.
[System.Serializable]
public class AttributeStat
{
    protected float baseValue;
    protected float bonusValue;
    public TextMeshProUGUI Text;

    //기본 값은 기본 스탯
    //최소 0
    public float BaseValue
    {
        get { return baseValue; }
        set
        {
            baseValue = value < 0 ? 0 : value;
        }
    }

    //보너스 값은 장비, 물약 등으로 얻은 추가 수치
    //최소 0
    public float BonusValue
    {
        get { return bonusValue; }
        set
        {
            bonusValue = value < 0 ? 0 : value;
        }
    }

    //총합 계산
    public float TotalValule
    {
        get { return baseValue + bonusValue; }
    }
}

public class UnitStat : MonoBehaviour
{
    #region Serialize Field
    [SerializeField] protected BaseStatSO InitialStat;
    #endregion
    #region Protected Field
    protected string characterName;
    protected string description;
    protected int characterID;

    protected AttributeStat lv;
    protected AttributeStat movementSpeed;
    #endregion
    #region Public Properties
    public AttributeStat Lv
    {
        get
        {
            return lv;
        }
    }
    public AttributeStat MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }
    public string CharacterName
    {
        get
        {
            return characterName;
        }
        protected set
        {
            characterName = value;
        }
    }
    public string CharacterDescription
    {
        get
        {
            return description;
        }
    }
    public int CharaterID
    {
        get
        {
            return characterID;
        }
        protected set
        {
            characterID = value;
        }
    }

    #endregion
    #region MonoBehaviour Callbacks
    protected virtual void Awake()
    {

        movementSpeed = new AttributeStat();
        lv = new AttributeStat();

        characterName = InitialStat.CharacterName;
        description = InitialStat.CharacterDescription;
        movementSpeed.BaseValue = InitialStat.MovementSpeed;
        lv.BaseValue = InitialStat.Lv;
        characterID = InitialStat.ID;
    }
    #endregion
    #region Protected Methods
    /// <summary>
    /// 스탯별 이미지 바 설정
    /// </summary>
    /// <param name="statImage">스탯 이미지 넣는 필드</param>
    /// <param name="bar">체력바와 같은 이미지 바</param>
    protected void SetImage(Image statImage, Image bar)
    {
        if (bar != null)
        {
            statImage = bar;
        }
    }
    /// <summary>
    /// 스탯별 텍스트 자리 설정
    /// </summary>
    /// <param name="statText">스탯에 텍스트 추가</param>
    /// <param name="text"></param>
    protected void SetText(TextMeshProUGUI statText, TextMeshProUGUI text)
    {
        if(text != null)
        {
            statText = text;
        }
    }

    public BaseStatSO GetBaseStatSO()
    {
       return InitialStat;
    }
    #endregion
}

