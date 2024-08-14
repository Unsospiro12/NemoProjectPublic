using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInPool
{
    public GameObject prefab;
    public int size;

    public GameObject GetNewObject()
    {
        return GameObject.Instantiate(prefab);
    }
}

[System.Serializable]
public class TagObjectPair<T>
{
    public T Tag;
    public ObjectInPool Object;
}

public class ObjectPool<T> where T : System.Enum
{
    private Dictionary<T, ObjectInPool> objectsInPool;
    private Dictionary<T, Queue<GameObject>> poolDictionary;
    private Dictionary<T, List<GameObject>> temporaryPool;

    public void InitializePool(TagObjectPair<T>[] tagobpair)
    {
        objectsInPool = new Dictionary<T, ObjectInPool>();
        poolDictionary = new Dictionary<T, Queue<GameObject>>();
        temporaryPool = new Dictionary<T, List<GameObject>>();

        foreach (TagObjectPair<T> op in tagobpair)
        {
            this.objectsInPool[op.Tag] = op.Object;
        }

        foreach (var o in objectsInPool)
        {
            T key = o.Key;

            Queue<GameObject> poolObjects = new Queue<GameObject>();
            for (int i = 0; i < o.Value.size; i++)
            {
                GameObject obj = GameObject.Instantiate(o.Value.prefab, UserData.Instance.transform);
                obj.SetActive(false);
                poolObjects.Enqueue(obj);
            }
            poolDictionary.Add(key, poolObjects);
            List<GameObject> gameObjects = new List<GameObject>();
            temporaryPool.Add(key, gameObjects);
        }
    }

    // 오브젝트 풀 불러오기
    public GameObject SpawnFromPool(T tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Tag {tag} does not exist in the pool.");
            return null;
        }

        GameObject obj = null;
        if (poolDictionary[tag].Count > 0)
        {
            obj = poolDictionary[tag].Dequeue();
        }
        else
        {
            obj = objectsInPool[tag].GetNewObject();
        }

        temporaryPool[tag].Add(obj);
        obj.SetActive(true);
        return obj;
    }

    public GameObject SpawnFromPool(T tag, Vector3 position)
    {
        GameObject obj = SpawnFromPool(tag);
        if (obj != null)
        {
            obj.transform.position = position;
        }
        return obj;
    }

    // 오브젝트풀 추가용
    public void AddObjectPool(T tag, GameObject prefab, int size)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag] = new Queue<GameObject>();
        }

        for (int i = 0; i < size; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
    }

    public void ReturnObject(T tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
        temporaryPool[tag].Remove(obj);
    }

    public void DisableAllObjects()
    {
        // 모든 태그 순회
        foreach (var entry in temporaryPool)
        {
            T tag = entry.Key;
            List<GameObject> poolList = entry.Value;

            // 비활성화할 오브젝트를 수집할 임시 리스트
            List<GameObject> objectsToDisable = new List<GameObject>();

            foreach (GameObject obj in poolList)
            {
                obj.SetActive(false);
                poolDictionary[tag].Enqueue(obj);
                objectsToDisable.Add(obj);
            }

            // foreach 루프가 끝난 후에 삭제
            foreach (GameObject obj in objectsToDisable)
            {
                temporaryPool[tag].Remove(obj);
            }
        }
    }
}

public class ObjectPool
{
    private Queue<GameObject> objectQueue;
    private GameObject prefab;

    public Queue<GameObject> PoolContents { get { return objectQueue; } }

    public ObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;
        objectQueue = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(false);
            objectQueue.Enqueue(obj);
        }
    }

    // 오브젝트 풀에서 오브젝트 가져오기
    public GameObject SpawnFromPool()
    {
        GameObject obj;
        if (objectQueue.Count > 0)
        {
            obj = objectQueue.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate(prefab);
        }

        obj.SetActive(true);
        return obj;
    }

    // 오브젝트 풀에서 오브젝트 가져오기 (위치 지정)
    public GameObject SpawnFromPool(Vector3 position)
    {
        GameObject obj = SpawnFromPool();
        if (obj != null)
        {
            obj.transform.position = position;
        }
        return obj;
    }

    // 오브젝트를 풀에 반환
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        objectQueue.Enqueue(obj);
    }

    // 모든 오브젝트 비활성화
    public void DisableAllObjects()
    {
        foreach (GameObject obj in objectQueue)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
            }
        }
    }
}
