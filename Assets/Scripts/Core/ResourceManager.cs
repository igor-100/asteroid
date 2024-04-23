using System;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class ResourceManager : IResourceManager
    {
        private Dictionary<string, List<GameObject>> objectPools = new Dictionary<string, List<GameObject>>();

    public T CreatePrefabInstance<T, E>(E item) where E : Enum
    {
        var prefab = CreatePrefabInstance(item);
        var result = prefab.GetComponent<T>();

        return result;
    }

    public GameObject CreatePrefabInstance<E>(E item) where E : Enum
    {
        var path = string.Format("{0}/{1}", typeof(E).Name, item.ToString());
        var asset = Resources.Load<GameObject>(path);
        var result = GameObject.Instantiate(asset);

        return result;
    }

    public T GetAsset<T, E>(E item) where T : UnityEngine.Object where E : Enum
    {
        var path = string.Format("{0}/{1}", typeof(E).Name, item.ToString());
        var result = Resources.Load<T>(path);

        return result;
    }

    public T GetPooledObject<T, E>(E item) where E : Enum where T : MonoBehaviour
    {
        string type = item.ToString();
        if (!objectPools.ContainsKey(type))
        {
            objectPools.Add(type, new List<GameObject>());
        }

        objectPools.TryGetValue(type, out List<GameObject> pooledObjects);

        foreach (var obj in pooledObjects)
        {
            if (obj != null && !obj.activeSelf)
            {
                obj.SetActive(true);

                return obj.GetComponent<T>();
            }
        }

        var tmp = CreatePrefabInstance(item);
        pooledObjects.Add(tmp);

        tmp.SetActive(true);

        return tmp.GetComponent<T>();
    }

    // was created for testing purposes and should be implemented in a different way
    public GameObject GetPooledObject<T, E>(E item, int maximumSize) where E : Enum
    {
        string type = item.ToString();
        if (!objectPools.ContainsKey(type))
        {
            objectPools.Add(type, new List<GameObject>());
        }

        objectPools.TryGetValue(type, out List<GameObject> pooledObjects);

        foreach (var obj in pooledObjects)
        {
            if (!obj.activeSelf)
            {
                return obj;
            }
        }

        if (pooledObjects.Count <= maximumSize)
        {
            var tmp = CreatePrefabInstance(item);
            pooledObjects.Add(tmp);
            return tmp;
        }
        else
        {
            return null;
        }
    }

    public void ResetPools()
    {
        objectPools = new Dictionary<string, List<GameObject>>();
    }
    }
}
