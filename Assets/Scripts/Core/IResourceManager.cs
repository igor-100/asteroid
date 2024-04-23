using System;
using UnityEngine;
namespace Core
{
    public interface IResourceManager
    {
        T CreatePrefabInstance<T, E>(E item) where E : Enum;
        GameObject CreatePrefabInstance<E>(E item) where E : Enum;
        T GetAsset<T, E>(E item) where T : UnityEngine.Object where E : Enum;
        T GetPooledObject<T, E>(E item) where E : Enum where T : MonoBehaviour;
        GameObject GetPooledObject<T, E>(E item, int maximumSize) where E : Enum;
        void ResetPools();
    }
}
