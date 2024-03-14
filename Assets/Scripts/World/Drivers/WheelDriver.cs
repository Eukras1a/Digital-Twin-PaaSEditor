using System;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using UnityEngine;

namespace World.Drivers
{
    public class WheelDriver : DriverBase, IDriver
    {
        public float Target = 1f;
        public Axis Type = Axis.X;
        public float _doTime = 1.61f;
        
        public override void Do(Dictionary<string, string> args)
        {
            var nodeId = args["NodeId"];
            if (nodeId != NodeId)
            {
                return;
            }
        
            var target = float.Parse(args["Target"]);
            var endValue = Vector3.zero;
            switch (Type)
            {
                case Axis.X:
                    endValue = new Vector3(target, 0f);
                    break;
                case Axis.Y:
                    endValue = new Vector3(0f, target);
                    break;
                case Axis.Z:
                    endValue = new Vector3(0f, 0f, target);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            transform.DOLocalRotate(endValue, _doTime);
        }

        public void Reset()
        {
            Do(new Dictionary<string, string>()
            {
                {"Target", "0"},
                {"NodeId", NodeId}
            });
        }

        public string GetNodeName()
        {
            return Id;
        }

        [ContextMenu("Test")]
        public void Test()
        {
            Do(new Dictionary<string, string>
            {
                {"Target", Target.ToString(CultureInfo.InvariantCulture)}
            });
        }
    }
}