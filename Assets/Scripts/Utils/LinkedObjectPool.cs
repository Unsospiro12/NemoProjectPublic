using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public UnitName tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;
    public Dictionary<string, LinkedList<GameObject>> PoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, LinkedList<GameObject>>();
        foreach (var pool in Pools)
        {
            LinkedList<GameObject> objectPool = new LinkedList<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, gameObject.transform);
                obj.SetActive(false);
                objectPool.AddLast(obj);
            }
            PoolDictionary.Add(pool.tag.ToString(), objectPool);
        }
    }

    // 오브젝트 풀 불러오기
    public GameObject SpawnFromPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].First.Value;
        PoolDictionary[tag].RemoveFirst();
        PoolDictionary[tag].AddLast(obj);
        obj.SetActive(true);
        return obj;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].First.Value;
        PoolDictionary[tag].RemoveFirst();
        PoolDictionary[tag].AddLast(obj);
        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    // 오브젝트풀 추가용
    public void AddObjectPool(string tag, GameObject prefab, int size)
    {
        LinkedList<GameObject> objectPool = new LinkedList<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, gameObject.transform);
            obj.SetActive(false);
            objectPool.AddLast(obj);
        }
        PoolDictionary.Add(tag, objectPool);
    }

    public void AddObjectPools(GameObject[] prefabs, int size)
    {
        foreach (var prefab in prefabs)
        {
            string tag = prefab.name;
            if (!PoolDictionary.ContainsKey(tag))
            {
                AddObjectPool(tag, prefab, size);
            }
        }
    }
}
