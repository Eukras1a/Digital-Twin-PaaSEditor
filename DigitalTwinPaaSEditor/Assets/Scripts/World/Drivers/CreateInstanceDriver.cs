using System.Collections.Generic;
using UnityEngine;

namespace World.Drivers
{
    public class CreateInstanceDriver : DriverBase, IDriver
    {
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private Transform _parent;

        public override void Do(Dictionary<string, string> args)
        {
            var nodeId = args["NodeId"];
            if (nodeId != NodeId)
            {
                return;
            }
                        
            Instantiate(_prefab, _parent, false);
        }

        public string GetNodeName()
        {
            return name;
        }
    }
}