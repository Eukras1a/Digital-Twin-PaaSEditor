using System.Collections.Generic;
using NodeEditor;
using UnityEngine;

namespace World.Drivers
{
    public abstract class DriverBase : MonoBehaviour
    {
        
        public string NodeId => Id;
        
        public string Id;
        public abstract void Do(Dictionary<string, string> args);

        public virtual void Reset(){}
    }
}