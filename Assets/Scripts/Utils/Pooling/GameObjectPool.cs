using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Pooling {
    public class GameObjectPool : MonoBehaviour {
        private static GameObjectPool s_Instance;
        private static Dictionary<int, Queue<PooledMonoBehaviour>> m_PooledObjects = new Dictionary<int, Queue<PooledMonoBehaviour>>();

        public static GameObjectPool Instance {
            get {
                if (s_Instance == null) {
                    s_Instance = FindObjectOfType<GameObjectPool>();
                    if (s_Instance == null) {
                        GameObject gameObject = new GameObject("GameObjectPool");
                        s_Instance = gameObject.AddComponent<GameObjectPool>();
                    }
                    s_Instance.gameObject.SetActive(false);
                }
                return s_Instance;
            }
        }
        
        public static TMonoBehaviour InstantiatePooled<TMonoBehaviour>(TMonoBehaviour prefab, Transform parent)
            where TMonoBehaviour : PooledMonoBehaviour {
            TMonoBehaviour instance = InstantiatePooledImpl(prefab);
            instance.transform.parent = parent;
            return instance;
        }
        
        public static TMonoBehaviour InstantiatePooled<TMonoBehaviour>(TMonoBehaviour prefab, Vector3 position, Quaternion rotation)
            where TMonoBehaviour : PooledMonoBehaviour {
            TMonoBehaviour instance = InstantiatePooledImpl(prefab);
            Transform instanceTransform = instance.transform;
            instanceTransform.parent = null;
            instanceTransform.position = position;
            instanceTransform.rotation = rotation;
            return instance;
        }

        public static TMonoBehaviour InstantiatePooled<TMonoBehaviour>(TMonoBehaviour prefab)
            where TMonoBehaviour : PooledMonoBehaviour {
            TMonoBehaviour instance = InstantiatePooledImpl(prefab);
            instance.transform.parent = null;
            return instance;
        }

        private static TMonoBehaviour InstantiatePooledImpl<TMonoBehaviour>(TMonoBehaviour prefab) where TMonoBehaviour : PooledMonoBehaviour {
            int id = prefab.GetInstanceID();
            TMonoBehaviour instance = null;

            if (m_PooledObjects.TryGetValue(id, out Queue<PooledMonoBehaviour> queue)) {
                if (queue.Count > 0) {
                    instance = queue.Peek() as TMonoBehaviour;
                    if (instance == null) {
                        throw new NullReferenceException();
                    }

                    queue.Dequeue();
                }
            }

            if (instance == null) {
                instance = Instantiate(prefab);
                instance.SetPrefabId(id);
            }

            instance.AwakePooled();

            return instance;
        }

        public static void ReturnObjectToPool(PooledMonoBehaviour instance) {
            int id = instance.PrefabId;

            if (m_PooledObjects.TryGetValue(id, out Queue<PooledMonoBehaviour> queue)) {
                queue.Enqueue(instance);
            } else {
                Queue<PooledMonoBehaviour> newQueue = new Queue<PooledMonoBehaviour>();
                newQueue.Enqueue(instance);
                m_PooledObjects.Add(id, newQueue);
            }

            instance.transform.parent = Instance.transform;
        }
    }
}