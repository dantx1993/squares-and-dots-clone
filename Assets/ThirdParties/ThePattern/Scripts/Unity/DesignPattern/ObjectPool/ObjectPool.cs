using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThePattern.Unity
{
    public sealed class ObjectPool : Singleton<ObjectPool>
    {
        public enum StartupPoolMode
        {
            Awake,
            Start,
            CallManually
        }
        [System.Serializable]
        public class StartupPool
        {
            public int size;
            public GameObject prefab;
        }
        private static List<GameObject> _tempList = new List<GameObject>();
        private readonly Dictionary<GameObject, List<GameObject>> _pooledObjects = new Dictionary<GameObject, List<GameObject>>();
        private readonly Dictionary<GameObject, GameObject> _spawnedObjects = new Dictionary<GameObject, GameObject>();
        private ObjectPool.StartupPoolMode _startupPoolMode;
        private ObjectPool.StartupPool[] _startupPools;
        private bool _startupPoolCreated;

        public ObjectPool.StartupPoolMode StartupMode => _startupPoolMode;

        private void Awake()
        {
            if (this._startupPoolMode != ObjectPool.StartupPoolMode.Awake)
            {
                return;
            }
            ObjectPool.CreateStartupPools();
        }

        private void Start()
        {
            if (this._startupPoolMode != ObjectPool.StartupPoolMode.Start)
            {
                return;
            }
            ObjectPool.CreateStartupPools();
        }

        private static void CreateStartupPools()
        {
            if (ObjectPool.Instance._startupPoolCreated)
            {
                return;
            }
            ObjectPool.Instance._startupPoolCreated = true;
            ObjectPool.StartupPool[] startupPools = ObjectPool.Instance._startupPools;
            if (startupPools != null && (uint)startupPools.Length > 0U)
            {
                for (int i = 0; i < startupPools.Length; ++i)
                {
                    ObjectPool.CreatePool(startupPools[i].prefab, startupPools[i].size);
                }
            }
        }

        internal static void SetDefaultStartUpPools(ObjectPool.StartupPool[] pools)
        {
            ObjectPool.Instance._startupPools = pools;
        }
        public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
        {
            ObjectPool.CreatePool(((Component)(object)prefab).gameObject, initialPoolSize);
        }
        public static void CreatePool(GameObject prefab, int initialPoolSize)
        {
            if (UnityEngine.Object.ReferenceEquals(prefab, (UnityEngine.Object)null) || ObjectPool.Instance._pooledObjects.ContainsKey(prefab))
            {
                return;
            }
            List<GameObject> gameObjectList = new List<GameObject>();
            ObjectPool.Instance._pooledObjects.Add(prefab, gameObjectList);
            if (initialPoolSize > 0)
            {
                bool activeSelf = prefab.activeSelf;
                prefab.SetActive(false);
                Transform transform = ObjectPool.Instance.transform;
                while (gameObjectList.Count < initialPoolSize)
                {
                    GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate<GameObject>(prefab);
                    gameObject.transform.SetParent(transform);
                    gameObjectList.Add(gameObject);
                }
                prefab.SetActive(activeSelf);
            }
        }

        public static GameObject Spawn(string resourcePath, Transform parent, Vector3 position, Quaternion rotation)
        {
            return (Resources.Load(resourcePath) as GameObject).Spawn(parent, position, rotation);
        }
        public static GameObject Spawn(string resourcePath, Transform parent, Vector3 position)
        {
            return ObjectPool.Spawn(resourcePath, parent, position, Quaternion.identity);
        }
        public static GameObject Spawn(string resourcePath, Vector3 position)
        {
            return ObjectPool.Spawn(resourcePath, null, position, Quaternion.identity);
        }
        public static GameObject Spawn(string resourcePath, Transform parent)
        {
            return ObjectPool.Spawn(resourcePath, parent, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(string resourcePath)
        {
            return ObjectPool.Spawn(resourcePath, null, Vector3.zero, Quaternion.identity);
        }

        public static T Spawn<T>(string resourcePath, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(resourcePath, parent, position, rotation).GetComponent<T>();
        }
        public static T Spawn<T>(string resourcePath, Transform parent, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn<T>(resourcePath, parent, position, Quaternion.identity);
        }
        public static T Spawn<T>(string resourcePath, Transform parent) where T : Component
        {
            return ObjectPool.Spawn<T>(resourcePath, parent, Vector3.zero, Quaternion.identity);
        }
        public static T Spawn<T>(string resourcePath, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn<T>(resourcePath, null, position, Quaternion.identity);
        }
        public static T Spawn<T>(string resourcePath) where T : Component
        {
            return ObjectPool.Spawn<T>(resourcePath, null, Vector3.zero, Quaternion.identity);
        }


        public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(((Component)(object)prefab).gameObject, parent, position, rotation).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(((Component)(object)prefab).gameObject, null, position, rotation).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(((Component)(object)prefab).gameObject, parent, position, Quaternion.identity).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(((Component)(object)prefab).gameObject, null, position, Quaternion.identity).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return ObjectPool.Spawn(((Component)(object)prefab).gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab) where T : Component
        {
            return ObjectPool.Spawn(((Component)(object)prefab).gameObject, (Transform)null, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            List<GameObject> gameObjectList;
            if (ObjectPool.Instance._pooledObjects.TryGetValue(prefab, out gameObjectList))
            {
                GameObject key1 = null;
                if (gameObjectList.Count > 0)
                {
                    while (UnityEngine.Object.ReferenceEquals(key1, (UnityEngine.Object)null) && gameObjectList.Count > 0)
                    {
                        key1 = gameObjectList[0];
                        gameObjectList.RemoveAt(0);
                    }
                    if (!UnityEngine.Object.ReferenceEquals(key1, (UnityEngine.Object)null))
                    {
                        Transform transform1 = key1.transform;
                        transform1.SetParent(parent);
                        transform1.localPosition = position;
                        transform1.localRotation = rotation;
                        key1.SetActive(true);
                        ObjectPool.Instance._spawnedObjects.Add(key1, prefab);
                        return key1;
                    }
                }
                GameObject key2 = (GameObject)UnityEngine.GameObject.Instantiate<GameObject>(prefab);
                Transform transform2 = key2.transform;
                transform2.SetParent(parent);
                transform2.localPosition = position;
                transform2.localRotation = rotation;
                ObjectPool.Instance._spawnedObjects.Add(key2, prefab);
                return key2;
            }
            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate<GameObject>(prefab);
            Transform component = (Transform)gameObject.GetComponent<Transform>();
            component.SetParent(parent);
            component.localPosition = position;
            component.localRotation = rotation;
            return gameObject;
        }
        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
        {
            return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }
        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return ObjectPool.Spawn(prefab, (Transform)null, position, rotation);
        }
        public static GameObject Spawn(GameObject prefab, Transform parent)
        {
            return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return ObjectPool.Spawn(prefab, (Transform)null, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab)
        {
            return ObjectPool.Spawn(prefab, (Transform)null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(T obj) where T : Component
        {
            ObjectPool.Recycle(((Component)(object)obj).gameObject);
        }
        public static void Recycle(GameObject obj)
        {
            GameObject prefab;
            if (ObjectPool.Instance._spawnedObjects.TryGetValue(obj, out prefab))
            {
                ObjectPool.Recycle(obj, prefab);
            }
            else
            {
                Destroy(obj);
            }
        }
        private static void Recycle(GameObject obj, GameObject prefab)
        {
            ObjectPool.Instance._pooledObjects[prefab].Add(obj);
            ObjectPool.Instance._spawnedObjects.Remove(obj);
            obj.transform.SetParent(((Component)ObjectPool.Instance).transform);
            obj.SetActive(false);
        }

        public static void RecycleAll<T>(T prefab) where T : Component
        {
            ObjectPool.RecycleAll(((Component)(object)prefab).gameObject);
        }
        public static void RecycleAll(GameObject prefab)
        {
            using (Dictionary<GameObject, GameObject>.Enumerator enumerator = ObjectPool.Instance._spawnedObjects.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<GameObject, GameObject> current = enumerator.Current;
                    if (UnityEngine.Object.ReferenceEquals((UnityEngine.Object)current.Value, (UnityEngine.Object)prefab))
                    {
                        ObjectPool._tempList.Add(current.Key);
                    }

                }
            }
            for (int index = 0; index < ObjectPool._tempList.Count; ++index)
            {
                ObjectPool.Recycle(ObjectPool._tempList[index]);
            }
            ObjectPool._tempList.Clear();
        }
        public static void RecycleAll()
        {
            ObjectPool._tempList.AddRange((IEnumerable<GameObject>)ObjectPool.Instance._spawnedObjects.Keys);
            for (int index = 0; index < ObjectPool._tempList.Count; ++index)
            {
                ObjectPool.Recycle(ObjectPool._tempList[index]);
            }
            ObjectPool._tempList.Clear();
        }

        public static bool IsSpawned(GameObject obj)
        {
            return ObjectPool.Instance._spawnedObjects.ContainsKey(obj);
        }

        public static int CountPooled<T>(T prefab) where T : Component
        {
            return ObjectPool.CountPooled(((Component)(object)prefab).gameObject);
        }
        public static int CountPooled(GameObject prefab)
        {
            List<GameObject> gameObjectList;
            return ObjectPool.Instance._pooledObjects.TryGetValue(prefab, out gameObjectList) ? gameObjectList.Count : 0;
        }

        public static int CountSpawned<T>(T prefab) where T : Component
        {
            return ObjectPool.CountSpawned(((Component)(object)prefab).gameObject);
        }
        public static int CountSpawned(GameObject prefab)
        {
            int num = 0;
            using (Dictionary<GameObject, GameObject>.ValueCollection.Enumerator enumerator = ObjectPool.Instance._spawnedObjects.Values.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    GameObject current = enumerator.Current;
                    if (UnityEngine.Object.ReferenceEquals((UnityEngine.Object)prefab, (UnityEngine.Object)current))
                    {
                        ++num;
                    }
                }
            }
            return num;
        }

        public static int CountAllPooled()
        {
            int num = 0;
            using (Dictionary<GameObject, List<GameObject>>.ValueCollection.Enumerator enumerator = ObjectPool.Instance._pooledObjects.Values.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    List<GameObject> current = enumerator.Current;
                    num += current.Count;
                }
            }
            return num;
        }

        public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null)
            {
                list = new List<GameObject>();
            }
            if (!appendList)
            {
                list.Clear();
            }
            List<GameObject> gameObjectList;
            if (ObjectPool.Instance._pooledObjects.TryGetValue(prefab, out gameObjectList))
            {
                list.AddRange((IEnumerable<GameObject>)gameObjectList);
            }
            return list;
        }
        public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null)
            {
                list = new List<T>();
            }
            if (!appendList)
            {
                list.Clear();
            }
            List<GameObject> gameObjectList;
            if (ObjectPool.Instance._pooledObjects.TryGetValue(((Component)(object)prefab).gameObject, out gameObjectList))
            {
                for (int i = 0; i < gameObjectList.Count; ++i)
                {
                    list.Add(gameObjectList[i].GetComponent<T>());
                }

            }
            return list;
        }

        public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null)
            {
                list = new List<GameObject>();
            }
            if (!appendList)
            {
                list.Clear();
            }
            using (Dictionary<GameObject, GameObject>.Enumerator enumerator = ObjectPool.Instance._spawnedObjects.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<GameObject, GameObject> current = enumerator.Current;
                    if (UnityEngine.Object.ReferenceEquals((UnityEngine.Object)current.Value, (UnityEngine.Object)prefab))
                    {
                        list.Add(current.Key);
                    }
                }
            }
            return list;
        }
        public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null)
            {
                list = new List<T>();
            }
            if (!appendList)
            {
                list.Clear();
            }

            GameObject gameObject = ((Component)(object)prefab).gameObject;
            using (Dictionary<GameObject, GameObject>.Enumerator enumerator = ObjectPool.Instance._spawnedObjects.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<GameObject, GameObject> current = enumerator.Current;
                    if (UnityEngine.Object.ReferenceEquals((UnityEngine.Object)current.Value, (UnityEngine.Object)prefab))
                    {
                        list.Add(current.Key.GetComponent<T>());
                    }
                }
            }
            return list;
        }

        public static void DestroyPooled(GameObject prefab)
        {
            List<GameObject> gameObjectList;
            if (!ObjectPool.Instance._pooledObjects.TryGetValue(prefab, out gameObjectList))
            {
                return;
            }
            for (int i = 0; i < gameObjectList.Count; ++i)
            {
                UnityEngine.Object.Destroy((UnityEngine.Object)gameObjectList[i]);
            }
            gameObjectList.Clear();
        }
        public static void DestroyPooled<T>(T prefab) where T : Component
        {
            ObjectPool.DestroyPooled(((Component)(object)prefab).gameObject);
        }
        public static void DestroyAll(GameObject prefab)
        {
            ObjectPool.RecycleAll(prefab);
            ObjectPool.DestroyPooled(prefab);
        }
        public static void DestroyAll<T>(T prefab) where T : Component
        {
            ObjectPool.DestroyAll(((Component)(object)prefab).gameObject);
        }
    }
}

