using System;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace World.Drivers
{
    public class ParallelDriver : DriverBase, IDriver
    {
        public Axis Type = Axis.X;
        public float MinLimit = -100f;
        public float MaxLimit = 100f;
        [SerializeField]
        public float _speed = 10f;
    
        [SerializeField]
        private float _resetTarget = 0f;

        public override void Do(Dictionary<string, string> args)
        {
            var nodeId = args["NodeId"];
            if (nodeId != NodeId)
            {
                return;
            }

            var speed = _speed;
            if (args.TryGetValue("Speed", out var speedStr))
            {
                speed = float.Parse(speedStr);
            }
            var target = float.Parse(args["Target"]);
            target = Mathf.Clamp(target, MinLimit, MaxLimit);
            var endValue = Vector3.zero;
            var moveDistance = 0f;
            switch (Type)
            {
                case Axis.X:
                    moveDistance = Mathf.Abs(target - transform.localPosition.x);
                    endValue = new Vector3(target, 0f);
                    break;
                case Axis.Y:
                    moveDistance = Mathf.Abs(target - transform.localPosition.y);
                    endValue = new Vector3(0f, target);
                    break;
                case Axis.Z:
                    moveDistance = Mathf.Abs(target - transform.localPosition.z);
                    endValue = new Vector3(0f, 0f, target);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var time = moveDistance / speed;
            transform.DOLocalMove(endValue, time);
        }

        public override void Reset()
        {
            Do(new Dictionary<string, string>()
            {
                {"Target", _resetTarget.ToString(CultureInfo.InvariantCulture)},
                {"NodeId", NodeId}
            });
        }

        public string GetNodeName()
        {
            return name;
        }
    }
}
