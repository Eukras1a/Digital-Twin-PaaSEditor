using System;
using System.Linq;
using Battlehub.RTCommon;
using MeadowGames.UINodeConnect4;
using UnityEngine;
using World;
using World.Signals;

namespace NodeEditor
{
    public class NodeEditorContext : MonoBehaviour
    {
        private DeviceCtrl _curDevice;
        private GraphManager _graphManager;
        public DeviceCtrl CurDevice
        {
            set
            {
                _curDevice = value;
                UpdateGraph();
            }
            get => _curDevice;
        }

        // public static NodeEditorContext Instance;
        public void Awake()
        {
            // Instance = this;
            IOC.Register(this);
            _graphManager = GetComponentInChildren<GraphManager>();
            IOC.Register(_graphManager);
        }

        public void Start()
        {
            gameObject.SetActive(false);
        }

        public bool Enable
        {
            set
            {
                gameObject.SetActive(value);
                var runtimeEditor = IOC.Resolve<IRTE>();
                runtimeEditor.IsBusy = value;
            }
            
            get => gameObject.activeSelf;
        }

        public Node AddNode(Node template, Vector3 position)
        {
           var newNode = _graphManager.InstantiateNode(template, Vector3.zero);
           newNode.transform.localPosition = position;
           var editorNode = newNode.GetComponent<IEditorNode>();
           if (editorNode != null)
           {
               editorNode.PrefabName = template.name;
           }
           var index = 0;
           while (true)
           {
               var nodeId = $"{template.ID}-{index}";
               index++;
               if (HasNode(nodeId))
                   continue;
               newNode.ID = nodeId;
               break;
           }
           
           return newNode;
        }
        
        public Connection AddConnection(Port start, Port to)
        {
            var newConnection = Connection.NewConnection(start, to);
            var index = 0;
            while (true)
            {
                var nodeId = $"connection-{index}";
                index++;
                if (HasConnection(nodeId))
                    continue;
                newConnection.ID = nodeId;
                break;
            }
            return newConnection;
        }

        public Port FindPort(ConnectionPort portData)
        {
            return FindPort(portData.NodeId, portData.PortId);
        }

        public Port FindPort(string nodeId, string portId)
        {
            foreach (var node in _graphManager.localNodes)
            {
                if (node.ID != nodeId)
                    continue;
                foreach (var port in node.ports)
                {
                    if (port.ID == portId)
                        return port;
                }
            }

            return null;
        }
        
        public bool HasNode(string nodeId)
        {
            return _graphManager.localNodes.Any(c => c.ID == nodeId);
        }
        
        public bool HasConnection(string nodeId)
        {
            return _graphManager.localConnections.Any(c => c.ID == nodeId);
        }

        public void UpdateGraph()
        {
            ClearGraph();

            if (_curDevice == null)
                return;
            var signal = _curDevice.GetComponent<DeviceSignal>();
            if (signal == null)
                return;
            if (signal.Nodes == null)
                return;
            
            foreach (var nodeData in signal.Nodes)
            {
                CreateNewNode(nodeData);
            }

            if (signal.Connections == null)
                return;
            foreach (var connectionData in signal.Connections)
            {
                CreateConnection(connectionData);
            }
        }

        private void CreateConnection(SignalConnection connectionData)
        {
            var startPort = FindPort(connectionData.StartPort);
            if (startPort == null)
                return;
            var toPort = FindPort(connectionData.ToPort);
            if (toPort == null)
                return;

            var newConnection = AddConnection(startPort, toPort);
            newConnection.ID = connectionData.Id;
            newConnection.ElementColor = connectionData.Color.ToColor();
        }

        private Node CreateNewNode(SignalNode nodeData)
        {
            var prefab = Resources.Load<GameObject>($"NodeEditor/Nodes/{nodeData.NodeType}");
            if (prefab == null)
                return null;
            
            var node = prefab.GetComponent<Node>();
            var newNode = _graphManager.InstantiateNode(node, Vector3.zero);
            newNode.transform.localPosition = nodeData.Pos.ToVector3();
            newNode.ElementColor = nodeData.Color.ToColor();
            _graphManager.UnselectAllElements();

            var editorNode = newNode.GetComponent<IEditorNode>();
            if (editorNode != null)
            {
                editorNode.PrefabName = nodeData.NodeType;
                editorNode.Load(nodeData.Data);
            }
            newNode.ID = nodeData.NodeId;
            return newNode;
        }

        public void ClearGraph()
        {
            foreach (var connection in _graphManager.localConnections.ToArray())
            {
                connection.Remove();
            }

            foreach (var node in _graphManager.localNodes.ToArray())
            {
                node.Remove();
            }
        }
    }
}