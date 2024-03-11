using System.Collections.Generic;
using ProtoBuf;
using Battlehub.RTSL;
using World.Drivers;
using World.Drivers.Battlehub.SL2;
using UnityEngine.Battlehub.SL2;

using UnityObject = UnityEngine.Object;
namespace World.Drivers.Battlehub.SL2
{
    public partial class PersistentParallelDriver<TID> : PersistentMonoBehaviour<TID>
    {
        [ProtoMember(280)] public Axis Type;
        [ProtoMember(281)]public float MinLimit;
        [ProtoMember(282)]public float MaxLimit;
        [ProtoMember(283)]public float Speed;
        [ProtoMember(284)] public string NodeId;
        public override object WriteTo(object obj)
        {
            var uo = (ParallelDriver)obj;
            uo.Type = Type;
            uo.MinLimit = MinLimit;
            uo.MaxLimit = MaxLimit;
            uo._speed = Speed;
            uo.Id = NodeId;
            
            return base.WriteTo(obj);
        }

        public override void ReadFrom(object obj)
        {
            
            var uo = (ParallelDriver)obj;
            Type = uo.Type;
            MinLimit = uo.MinLimit;
            MaxLimit = uo.MaxLimit;
            Speed = uo._speed;
            NodeId = uo.Id;
            
            base.ReadFrom(obj);
        }
    }
}

