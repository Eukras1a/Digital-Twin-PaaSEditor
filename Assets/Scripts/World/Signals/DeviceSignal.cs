using System;
using System.Collections.Generic;
using UnityEngine;

namespace World.Signals
{
    public class DeviceSignal : MonoBehaviour
    {
        public List<SignalNode> Nodes;
        public List<SignalConnection> Connections;
    }

    [Serializable]
    public class SignalNode
    {
        public string NodeId;
        public string NodeType;
        public LVector2 Pos;
        public Dictionary<string, object> Data;
        public LColor Color;
    }
    
    [Serializable]
    public class SignalConnection
    {
        public ConnectionPort StartPort;
        public ConnectionPort ToPort;
        public string Id;
        public LColor Color;
    }
    
    [Serializable]
    public class ConnectionPort
    {
        public string NodeId;
        public string PortId;
    }

    [Serializable]
    public class LVector2
    {
        public float X;
        public float Y;

        public LVector2(Vector3 v3)
        {
            X = v3.x;
            Y = v3.y;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y);
        }
    }
    
    [Serializable]
    public class LColor
    {
        public float R;
        public float G;
        public float B;
        public float A;

        public LColor(Color color)
        {
            R = color.r;
            G = color.g;
            B = color.b;
            A = color.a;
        }

        public Color ToColor()
        {
            return new Color(R, G, B, A);
        }
    }
}