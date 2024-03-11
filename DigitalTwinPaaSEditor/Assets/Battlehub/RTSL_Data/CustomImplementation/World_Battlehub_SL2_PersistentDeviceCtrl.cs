using System.Collections.Generic;
using ProtoBuf;
using Battlehub.RTSL;
using UnityEngine;
using World;
using World.Battlehub.SL2;
using UnityEngine.Battlehub.SL2;

using UnityObject = UnityEngine.Object;
namespace World.Battlehub.SL2
{
    public partial class PersistentDeviceCtrl<TID> : PersistentMonoBehaviour<TID>
    {
        [ProtoMember(280)] public string DeviceId;

        protected override object WriteToImpl(object obj)
        {
            var ctrl = (DeviceCtrl)obj;
            ctrl.DeviceId = DeviceId;
            return base.WriteToImpl(obj);
        }

        protected override void ReadFromImpl(object obj)
        {
            var ctrl = (DeviceCtrl)obj;
            DeviceId = ctrl.DeviceId;
            base.ReadFromImpl(obj);
        }
    }
}

