using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace FG
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public struct PoolableObjects
        {
            public ObjectType objectType;
            public GameObject objectPrefab;
            public int amountToPool;
            public int sizeIncreaseAmount;
            
            PoolableObjects(ObjectType k, GameObject v, int a, int s)
            {
                objectType = k;
                objectPrefab = v;
                amountToPool = a;
                sizeIncreaseAmount = s;
            }
        }

        public static ObjectPooler instance;

        [SerializeField] private bool dynamicExpandingPool = true;
        [SerializeField] private PoolableObjects[] objectsToPool;

        private Dictionary<ObjectType, List<GameObject>> pooledObjects = new Dictionary<ObjectType, List<GameObject>>();
        
        public enum  ObjectType
        {
            BulletExplosion,
            RocketExplosion,
            Bullet,
            HeavyBullet,
            Rocket,
            SwarmEnemy,
            SwarmEnemyBoss
        }

        private void Awake()
        {
            instance = this;

            for (int i = 0; i < objectsToPool.Length; i++)
            {
                pooledObjects.Add(objectsToPool[i].objectType, new List<GameObject>());
                Debug.Log("Adding " + objectsToPool[i].objectType + " to object pool");
                GameObject prefab = objectsToPool[i].objectPrefab;
                
                for (int j = 0; j < objectsToPool[i].amountToPool; j++)
                {
                    GameObject obj = Instantiate(prefab, transform);
                    pooledObjects[objectsToPool[i].objectType].Add(obj);
                    obj.SetActive(false);
                }
            }
        }

        private void AddObjectOfType(ObjectType typeOfObjectToAdd)
        {
            if (pooledObjects.ContainsKey(typeOfObjectToAdd))
            {
                PoolableObjects poolableObj = objectsToPool.Where(e => e.objectType == typeOfObjectToAdd).ToArray()[0];
                
                for (int i = 0; i < poolableObj.sizeIncreaseAmount; i++)
                {
                    GameObject obj = Instantiate(poolableObj.objectPrefab, transform);
                    pooledObjects[typeOfObjectToAdd].Add(obj);
                    obj.SetActive(false);
                }
            }
        }

        private GameObject FindDisabledObjectOfType(ObjectType objectTypeToFind)
        {
            while (true)
            {
                if (!pooledObjects.ContainsKey(objectTypeToFind)) return null;

                for (int i = 0; i < pooledObjects[objectTypeToFind].Count; i++)
                {
                    if (pooledObjects[objectTypeToFind][i].activeSelf) continue;

                    return pooledObjects[objectTypeToFind][i];
                }

                if (!dynamicExpandingPool) return null;
                AddObjectOfType(objectTypeToFind);
            }
        }

        public GameObject GetPooledObject(ObjectType objectTypeToGet)
        {
            GameObject obj = FindDisabledObjectOfType(objectTypeToGet);
            if (!obj) return null;
            obj.SetActive(true);
            return obj;

        }
    }
}
