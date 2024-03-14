using System.Collections.Generic;
using ProtoBuf;
using Battlehub.RTSL;
using World.Signals;
using World.Signals.Battlehub.SL2;
using UnityEngine.Battlehub.SL2;

using UnityObject = UnityEngine.Object;
namespace World.Signals.Battlehub.SL2
{
    [ProtoContract]
    public partial class PersistentDeviceSignal<TID> : PersistentMonoBehaviour<TID>
    {
            }
}

