using System.Collections.Generic;
using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine.Battlehub.SL2;

namespace World.Signals.Battlehub.SL2
{
    public partial class PersistentDeviceSignal<TID> : PersistentMonoBehaviour<TID>
    {
        [ProtoMember(280)] public string Nodes; 
        [ProtoMember(281)] public string Connections; 
        public override object WriteTo(object obj)
        {
            var uo = (DeviceSignal)obj;
            if (!string.IsNullOrWhiteSpace(Nodes))
            {
                uo.Nodes = JsonConvert.DeserializeObject<List<SignalNode>>(Nodes);
            }

            if (!string.IsNullOrWhiteSpace(Connections))
            {
                uo.Connections = JsonConvert.DeserializeObject<List<SignalConnection>>(Connections);
            }
            return base.WriteTo(obj);
        }
        
        public override void ReadFrom(object obj)
        {
            var uo = (DeviceSignal)obj;
            Nodes = JsonConvert.SerializeObject(uo.Nodes);
            Connections = JsonConvert.SerializeObject(uo.Connections);
            base.ReadFrom(obj);
        }
    }
}

