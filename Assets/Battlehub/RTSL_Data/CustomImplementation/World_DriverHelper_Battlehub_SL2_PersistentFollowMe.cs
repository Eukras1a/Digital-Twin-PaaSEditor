using System.Collections.Generic;
using ProtoBuf;
using Battlehub.RTSL;
using World.DriverHelper;
using World.DriverHelper.Battlehub.SL2;
using UnityEngine.Battlehub.SL2;

using UnityObject = UnityEngine.Object;
namespace World.DriverHelper.Battlehub.SL2
{
    public partial class PersistentFollowMe<TID> : PersistentMonoBehaviour<TID>
    {
        [ProtoMember(280)] public bool EnableRotation;
        [ProtoMember(281)] public TID Target;
        
        public override object WriteTo(object obj)
        {
            var uo = (FollowMe)obj;
            uo.Target = FromID(Target, uo.Target);
            uo.EnableRotation = EnableRotation;
            return base.WriteTo(obj);
        }

        public override void ReadFrom(object obj)
        {
            var uo = (FollowMe)obj;
            Target = ToID(uo.Target);
            EnableRotation = uo.EnableRotation;
            base.ReadFrom(obj);
        }
    }
}

