using UnityEngine;

namespace Utils
{
    public static class GameObjectExt
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();
            if (component != null)
                return component;
            return go.AddComponent<T>();
        }
        
        public static T GetOrAddComponent<T>(this Component go) where T : Component
        {
            var component = go.GetComponent<T>();
            if (component != null)
                return component;
            return go.gameObject.AddComponent<T>();
        }
    }
}