using System;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class WorldContext : MonoBehaviour
    {
        public static WorldContext Instance = new();
        
        private Queue<Action> _mainThreadQueue = new Queue<Action>();

        private void Awake()
        {
            Instance = this;
        }

        public WorldCamera MainCamera { get; private set; }
        public void SetMainCamera(WorldCamera worldCamera)
        {
            Debug.Log($"MainCamera set to {worldCamera.name}, {worldCamera.gameObject.GetInstanceID()}");
            MainCamera = worldCamera;
        }

        public void DoMainThread(Action action)
        {
            _mainThreadQueue.Enqueue(action);
        }

        public void Update()
        {
            while (_mainThreadQueue.Count > 0)
            {
                var action = _mainThreadQueue.Dequeue();
                action();
            }
        }
    }
}