using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public class PoolInfo
    {
        public string name;
        public GameObject objectType;
        public int size;
    }

    // string reference is bad, m'kay
    public static readonly string enemy = "EnemyWeak";
    public static readonly string bolt = "Bolt";
    public static readonly string powerUp = "PowerUp";
    public static readonly string healthPickup = "HealthPickup";
    public static readonly string special = "SpecialTarget";
    public static readonly string explosion = "Explosion";

    private static ObjectPool objectPool;
    public List<PoolInfo> Pools = new List<PoolInfo>();
    public bool fixedSize;

    private Dictionary<string, Queue<GameObject>> Objects;

    private void Awake()
    {
        objectPool = this;
        Objects = new Dictionary<string, Queue<GameObject>>();

        foreach(PoolInfo info in Pools)
        {
            Queue<GameObject> nextQueue = new Queue<GameObject>();
            for(int i = 0; i < info.size; ++i)
            {
                GameObject newObject = Instantiate(info.objectType);
                newObject.SetActive(false);

                nextQueue.Enqueue(newObject);
            }

            Objects.Add(info.name, nextQueue);
        }

    }

    public static GameObject SpawnObject(string name, Transform transform)
    {
        return objectPool.SpawnObjectInternal(name, transform);
    }

    public static GameObject SpawnObject(string name, Vector3 position)
    {
        objectPool.transform.position = position;
        return SpawnObject(name, objectPool.transform);
    }

    private GameObject SpawnObjectInternal(string name, Transform transform)
    {
        if(!Objects.ContainsKey(name))
        {
            Debug.LogError("unknown object pool: " + name);
            return null;
        }

        Queue<GameObject> q = Objects[name];
        if(q.Count == 0)
            return null;

        GameObject toReturn = q.Dequeue();

        IRestartable Restarter = toReturn.GetComponent<IRestartable>();
        if(Restarter != null)
        {
            Restarter.Restart();
        }

        toReturn.transform.position = transform.position;
        toReturn.transform.rotation = transform.rotation;
        toReturn.transform.localScale = transform.localScale;

        q.Enqueue(toReturn);

        return toReturn;
    }
}
