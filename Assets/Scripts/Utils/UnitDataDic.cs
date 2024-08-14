using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class KeyValue
{
    public int ID;
    public Sprite UnitSprite;
    public string UnitWeponName;
}
public class UnitDataDic : InGameSingleton<UnitDataDic>
{
    public List<KeyValue> unitKeyValues = new List<KeyValue>();
    public List<KeyValue> enemyKeyValues = new List<KeyValue>();

    public Dictionary<int, Sprite> UnitSpriteDic = new Dictionary<int, Sprite>();
    public Dictionary<int, Sprite> EnemySpriteDic = new Dictionary<int, Sprite>();
    public Dictionary<int, string> UnitNameDic = new Dictionary<int, string>();
    public Dictionary<int, string> EnemyNameDic = new Dictionary<int, string>();

    protected override void Awake()
    {        
        base.Awake();

        foreach (KeyValue kv in unitKeyValues)
        {           
            UnitSpriteDic[kv.ID] = kv.UnitSprite;
            UnitNameDic[kv.ID] = kv.UnitWeponName;
        }
        foreach(KeyValue kv in enemyKeyValues)
        {
            EnemySpriteDic[kv.ID] = kv.UnitSprite;
            EnemyNameDic[kv.ID] = kv.UnitWeponName;
        }
    }

    public Sprite GetUnitSprite(int id)
    {
        if (UnitSpriteDic.TryGetValue(id,out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            //Debug.Log($"Sprite Error: No sprite found for ID {id}");
            return null;
        }
    }

    public Sprite GetEnemySprite(int id)
    {
        if (EnemySpriteDic.TryGetValue(id, out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            //Debug.Log($"Sprite Error: No sprite found for ID {id}");
            return null;
        }
    }

    public string GetUnitWeponName(int id)
    {
        if (UnitNameDic.TryGetValue(id, out string weponName))
        {
            return weponName;
        }
        else
        {
           // Debug.Log($"weponName Error: No weapon name found for ID {id}");
            return null;
        }
    }
    public string GetEnemyWeponName(int id)
    {
        if (EnemyNameDic.TryGetValue(id, out string weponName))
        {
            return weponName;
        }
        else
        {
            // Debug.Log($"weponName Error: No weapon name found for ID {id}");
            return null;
        }
    }
}
