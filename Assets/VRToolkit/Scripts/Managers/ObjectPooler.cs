using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRToolkit.Managers
{
    public class ObjectPooler : MonoBehaviour
    {
        private static ObjectPooler objectPooler;

        public List<PooledObject> PooledObjects;

        private Dictionary<string, PooledObject> pools;

        [Serializable]
        public struct PooledObject
        {
            public GameObject objectToPool;
            public List<GameObject> Pool;
            public int PooledAmount;

            public void ObjectPooler()
            {
                Pool = new List<GameObject>();
            }
        }

        public static ObjectPooler Instance
        {
            get
            {
                if (!objectPooler)
                {
                    objectPooler = FindObjectOfType(typeof(ObjectPooler)) as ObjectPooler;

                    if (!objectPooler)
                    {
                        Debug.LogError("There needs to be one active ObjectPooler script on a GameObject in your scene.");
                    }
                }

                return objectPooler;
            }
        }

        // Use this for initialization
        void Awake()
        {
            // Initialize pools
            pools = new Dictionary<string, PooledObject>();
            int count = PooledObjects.Count;
            for (int i = 0; i < count; ++i)
            {
                for (int j = 0; j < PooledObjects[i].PooledAmount; ++j)
                {
                    GameObject go = Instantiate(PooledObjects[i].objectToPool, transform);
                    go.SetActive(false);
                    PooledObjects[i].Pool.Add(go);
                }
                pools.Add(PooledObjects[i].objectToPool.name, PooledObjects[i]);
            }
        }

        public GameObject GetPooledObject(string type, Transform parent = null)
        {
            if (pools == null) return null;

            for (int i = 0; i < pools[type].Pool.Count; ++i)
            {
                if (pools[type].Pool[i] != null && !pools[type].Pool[i].activeInHierarchy)
                {
                    GameObject obj = pools[type].Pool[i];
                    if (parent != null)
                    {
                        obj.transform.SetParent(parent, false);
                    }
                    return pools[type].Pool[i];
                }
            }

            GameObject go = Instantiate(pools[type].objectToPool, transform);

            go.SetActive(false);

            if (parent != null)
            {
                go.transform.SetParent(parent, false);
            }

            pools[type].Pool.Add(go);
            return go;
        }
    }
}