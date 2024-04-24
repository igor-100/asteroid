using UnityEngine;
namespace Utils
{
    public static class MonoExtensions
    {
        public static T CreateComponent<T>() where T : Component
        {
            var name = typeof(T).Name;
            var go = new GameObject(name);
            var result = go.AddComponent<T>();

            return result;
        }
    }
}
