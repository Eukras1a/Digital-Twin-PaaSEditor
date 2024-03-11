using System.Collections.Generic;
using Battlehub.RTCommon;
using MeadowGames.UINodeConnect4;
using MeadowGames.UINodeConnect4.UICContextMenu;
using UnityEngine.UI;
using Utils;
using World.Signals;

namespace NodeEditor.Menu
{
    public class SaveSignalItem  : ContextItem
    {
        Button _button;

        public void GenerateDriveNodes()
        {
            var nodeEditorContext = IOC.Resolve<NodeEditorContext>();
           var deviceObj = nodeEditorContext.CurDevice;
            var deviceSignal = deviceObj.GetOrAddComponent<DeviceSignal>();
            var manager = FindObjectOfType<GraphManager>();
            deviceSignal.Nodes = new List<SignalNode>(manager.localNodes.Count);
            deviceSignal.Connections = new List<SignalConnection>();
            foreach (var node in manager.localNodes)
            {
                var editorNode = node.GetComponent<IEditorNode>();
                deviceSignal.Nodes.Add(new SignalNode
                {
                    NodeId = node.ID,
                    NodeType = editorNode.PrefabName,
                    Pos = new LVector2(node.transform.localPosition),
                    Data = editorNode?.Save(),
                    Color = new LColor(node.ElementColor)
                });
            }

            foreach (var connection in manager.localConnections)
            {
                var startPort = connection.port0;
                var toPort = connection.port1;
                deviceSignal.Connections.Add(new SignalConnection
                {
                    Id = connection.ID,
                    StartPort = FindConnectionPort(startPort),
                    ToPort = FindConnectionPort(toPort),
                    Color = new LColor(connection.ElementColor)
                });
            }
        }
        
        private ConnectionPort FindConnectionPort(Port port)
        {
            if (port == null)
                return null;
            var portNode = port.node;
            if (portNode == null)
                return null;
            return new ConnectionPort
            {
                NodeId = portNode.ID,
                PortId = port.ID
            };
        }

        protected override void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
        }

        void OnEnable()
        {
            _button.onClick.AddListener(GenerateDriveNodes);
        }

        void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}