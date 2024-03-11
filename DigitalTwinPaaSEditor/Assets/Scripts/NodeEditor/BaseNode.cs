using System.Collections.Generic;
using Battlehub.RTCommon;
using MeadowGames.UINodeConnect4;
using UnityEngine;

namespace NodeEditor
{
    public class BaseNode : MonoBehaviour
    {
        public virtual string GetNodeName()
        {
            return name;
        }

        public string PrefabName { get; set; }
        
        public bool TryGetTargetPortByOutPort(Port startPort, out List<TargetPort> ports)
        {
            ports = new List<TargetPort>();
            var graphManager = IOC.Resolve<GraphManager>();
            foreach (var connection in graphManager.localConnections)
            {
                var toPort = connection.port0;
                if (toPort != startPort)
                    continue;
                
                var editorNode = connection.port1.node.GetComponent<IEditorNode>();
                if (editorNode != null)
                {
                    ports.Add(new TargetPort
                    {
                        Port = toPort,
                        EditorNode = editorNode
                    });
                }
            };
            
            return ports.Count != 0;
        }
    }

    public class TargetPort
    {
        public IEditorNode EditorNode;
        public Port Port;
    }
}