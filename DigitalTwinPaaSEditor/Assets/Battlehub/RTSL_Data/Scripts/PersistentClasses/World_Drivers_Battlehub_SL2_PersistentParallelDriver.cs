using System.Collections.Generic;
using ProtoBuf;
using Battlehub.RTSL;
using World.Drivers;
using World.Drivers.Battlehub.SL2;
using UnityEngine.Battlehub.SL2;

using UnityObject = UnityEngine.Object;
namespace World.Drivers.Battlehub.SL2
{
    [ProtoContract]
    public partial class PersistentParallelDriver<TID> : PersistentMonoBehaviour<TID>
    {
            }
}

