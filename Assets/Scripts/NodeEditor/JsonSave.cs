using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NodeEditor
{
    public class NodeItem
    {
        public string NodeId;
        public string PrefabName;
        public Vector3 Pos;
        public object NodeData;
    }

    public class MqttClientNodeData
    {
        public string Ip;
        public int Port;
        public string Username;
        public string Password;
    }
    
    public class JsonSave
    {
        public void Save()
        {
            var node = new NodeItem()
            {
                NodeId = "Node1001",
                PrefabName = "MQTTClientEditorNode",
                Pos = new Vector3(1, 1, 1),
                NodeData = new MqttClientNodeData
                {
                    Ip = "127.0.0.1",
                    Port = 1883,
                    Username = "test",
                    Password = ""
                }
            };
            var nodes = new List<NodeItem> { node };
            var json = JsonConvert.SerializeObject(nodes);
            var nodes_copy = JsonConvert.DeserializeObject<List<NodeItem>>(json);
        }
    }
}